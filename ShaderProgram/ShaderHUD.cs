using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenGLTutorial.OpenGLCore;
using OpenGLTutorial.GameCore;
using OpenGLTutorial.OpenGLCore.Primitives;
using OpenGLTutorial.Textures;

namespace OpenGLTutorial.ShaderProgram
{
    public static class ShaderHUD
    {
        private static Matrix4 _viewProjectionMatrix = Matrix4.Identity;

        private static int _shaderId = -1;

        private static int _vertexShaderId = -1;
        private static int _fragmentShaderId = -1;

        private static int _uniformMatrix = -1;
        private static int _uniformCharacterOffset = -1;

        private static int _uniformTexture = -1;
        private static int _uniformCollider = -1;
        private static int _textureID = -1;

        public static void Init()
        {
            //_viewProjectionMatrix = Matrix4.LookAt(0, 0, 1, 0, 0, 0, 0, 1, 0) * Matrix4.CreateOrthographic(ApplicationWindow.CurrentWindow.Size.X, ApplicationWindow.CurrentWindow.Size.Y, 0.1f, 10f);
            _viewProjectionMatrix = Matrix4.LookAt(0, 0, 1, 0, 0, 0, 0, 1, 0) * Matrix4.CreateOrthographicOffCenter(0, ApplicationWindow.CurrentWindow.Size.X, ApplicationWindow.CurrentWindow.Size.Y, 0, 0.1f, 1000f);

            _shaderId = GL.CreateProgram();

            Assembly a = Assembly.GetExecutingAssembly();

            // Vertex Shader auslesen:
            Stream sVertex = a.GetManifestResourceStream("OpenGLTutorial.ShaderProgram.shaderHUD_vertex.glsl");
            StreamReader sReaderVertex = new StreamReader(sVertex);
            string sVertexCode = sReaderVertex.ReadToEnd();
            sReaderVertex.Dispose();
            sVertex.Close();

            // Fragment Shader auslesen:
            Stream sFragment = a.GetManifestResourceStream("OpenGLTutorial.ShaderProgram.shaderHUD_fragment.glsl");
            StreamReader sReaderFragment = new StreamReader(sFragment);
            string sFragmentCode = sReaderFragment.ReadToEnd();
            sReaderFragment.Dispose();
            sFragment.Close();

            using (Stream s = a.GetManifestResourceStream("OpenGLTutorial.Textures.font.dds"))
            {
                _textureID = TextureLoaderDDS.LoadFont(s);
            }

            _vertexShaderId = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(_vertexShaderId, sVertexCode);

            _fragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(_fragmentShaderId, sFragmentCode);

            GL.CompileShader(_vertexShaderId);
            GL.AttachShader(_shaderId, _vertexShaderId);

            GL.CompileShader(_fragmentShaderId);
            GL.AttachShader(_shaderId, _fragmentShaderId);

            GL.LinkProgram(_shaderId);

            _uniformMatrix = GL.GetUniformLocation(_shaderId, "uMatrix");
            _uniformCharacterOffset = GL.GetUniformLocation(_shaderId, "uOffset");
            _uniformTexture = GL.GetUniformLocation(_shaderId, "uTexture");
            _uniformCollider = GL.GetUniformLocation(_shaderId, "uIsCollider");
        }

        private static int CalculateOffsetFor(string input)
        {
            return (int)input[0] - 32;
        }

        private static void DrawInternal(string input, int x, int y, bool isCollisionCandidate)
        {
            if (input.Length == 1)
            {
                Matrix4 modelMatrix = Matrix4.CreateScale(32) * Matrix4.CreateTranslation(x, y, 0);
                Matrix4 mvp = modelMatrix * _viewProjectionMatrix;
                GL.UniformMatrix4(_uniformMatrix, false, ref mvp);

                int offsetX = CalculateOffsetFor(input);
                GL.Uniform1(_uniformCharacterOffset, offsetX);

                GL.Uniform1(_uniformCollider, isCollisionCandidate ? 1 : 0);

                GL.BindVertexArray(PrimitiveQuad.GetVAOId());
                GL.DrawArrays(PrimitiveType.Triangles, 0, PrimitiveQuad.GetPointCount());
                GL.BindVertexArray(0);
            }
        }
        public static void Draw(GameObject[] objektliste)
        {
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);

            GL.UseProgram(_shaderId);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _textureID);
            GL.Uniform1(_uniformTexture, 0);

            for (int i = 0; i < objektliste.Length; i++)
            {
                GameObject g = objektliste[i];
                if (g != null)
                {
                    DrawInternal("" + i, g.GetCenterX(), g.GetCenterY(), g.IsCollisionCandidate());
                }
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.UseProgram(0);

            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
        }
    }
}
