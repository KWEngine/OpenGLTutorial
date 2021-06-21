using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using System.IO;
using OpenTK.Mathematics;
using OpenGLTutorial.GameCore;
using OpenGLTutorial.Primitives;
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
        private static int _textureID = -1;

        public static void Init()
        {
            _viewProjectionMatrix = Matrix4.LookAt(0, 0, 1, 0, 0, 0, 0, 1, 0) * Matrix4.CreateOrthographic(ApplicationWindow.CurrentWindow.Size.X, ApplicationWindow.CurrentWindow.Size.Y, 0.1f, 10f);
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

            Stream ddsStream = a.GetManifestResourceStream("OpenGLTutorial.Textures.font.dds");
            byte[] streamData = new byte[ddsStream.Length];
            ddsStream.Read(streamData, 0, streamData.Length);
            TextureLoaderDDS.TryLoadDDS(streamData, false, out _textureID, out int tWidth, out int tHeight);
            ddsStream.Close();
            streamData = null;

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
        }

        private static float CalculateOffsetFor(string input)
        {
            return 1;
        }

        public static void Draw(string input, int x, int y)
        {
            if (input.Length == 1)
            {
                GL.UseProgram(_shaderId);

                Matrix4 modelMatrix = Matrix4.CreateScale(32) * Matrix4.CreateTranslation(x, y, 0);
                // model-view-projection matrix erstellen:
                Matrix4 mvp = modelMatrix * _viewProjectionMatrix;
                GL.UniformMatrix4(_uniformMatrix, false, ref mvp);

                float offsetX = CalculateOffsetFor(input);

                GL.Uniform1(_uniformCharacterOffset, offsetX);

                // Texture an den Shader übertragen:
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, _textureID);
                GL.Uniform1(_uniformTexture, 0);

                GL.BindVertexArray(PrimitiveQuad.GetVAOId());
                GL.DrawArrays(PrimitiveType.Triangles, 0, PrimitiveQuad.GetPointCount());
                GL.BindVertexArray(0);

                GL.BindTexture(TextureTarget.Texture2D, 0);

                GL.UseProgram(0);
            }
        }
    }
}
