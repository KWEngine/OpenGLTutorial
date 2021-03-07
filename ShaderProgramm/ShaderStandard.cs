using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using System.IO;
using OpenTK.Mathematics;

namespace OpenGLTutorial.ShaderProgramm
{
    public static class ShaderStandard
    {
        private static int _shaderId = -1;

        private static int _vertexShaderId = -1;
        private static int _fragmentShaderId = -1;

        private static int _uniformMatrix = -1;
        private static int _uniformModelMatrix = -1;
        private static int _uniformNormalMatrix = -1;

        private static int _uniformTexture = -1;
        private static int _uniformTextureNormalMap = -1;
        private static int _uniformTextureNormalMapUse = -1;

        private static int _uniformLightCount = -1;
        private static int _uniformLightPositions = -1;
        private static int _uniformAmbientLight = -1;

        public static void Init()
        {
            _shaderId = GL.CreateProgram();

            Assembly a = Assembly.GetExecutingAssembly();

            // Vertex Shader auslesen:
            Stream sVertex = a.GetManifestResourceStream("OpenGLTutorial.ShaderProgramm.shaderStandard_vertex.glsl");
            StreamReader sReaderVertex = new StreamReader(sVertex);
            string sVertexCode = sReaderVertex.ReadToEnd();
            sReaderVertex.Dispose();
            sVertex.Close();

            // Fragment Shader auslesen:
            Stream sFragment = a.GetManifestResourceStream("OpenGLTutorial.ShaderProgramm.shaderStandard_fragment.glsl");
            StreamReader sReaderFragment = new StreamReader(sFragment);
            string sFragmentCode = sReaderFragment.ReadToEnd();
            sReaderFragment.Dispose();
            sFragment.Close();

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
            _uniformModelMatrix = GL.GetUniformLocation(_shaderId, "uModelMatrix");
            _uniformNormalMatrix = GL.GetUniformLocation(_shaderId, "uNormalMatrix");

            _uniformTexture = GL.GetUniformLocation(_shaderId, "uTexture");
            _uniformTextureNormalMap = GL.GetUniformLocation(_shaderId, "uTextureNormalMap");
            _uniformTextureNormalMapUse = GL.GetUniformLocation(_shaderId, "uTextureNormalMapUse");

            _uniformLightCount = GL.GetUniformLocation(_shaderId, "uLightCount");
            _uniformLightPositions = GL.GetUniformLocation(_shaderId, "uLightPositions");
            _uniformAmbientLight = GL.GetUniformLocation(_shaderId, "uAmbientLight");

        }

        public static int GetProgramId()
        {
            return _shaderId;
        }

        public static int GetMatrixId()
        {
            return _uniformMatrix;
        }

        public static int GetModelMatrixId()
        {
            return _uniformModelMatrix;
        }

        public static int GetNormalMatrixId()
        {
            return _uniformNormalMatrix;
        }

        public static int GetTextureId()
        {
            return _uniformTexture;
        }

        public static int GetLightCountId()
        {
            return _uniformLightCount;
        }

        public static int GetLightPositionsId()
        {
            return _uniformLightPositions;
        }

        public static int GetAmbientLightId()
        {
            return _uniformAmbientLight;
        }

        public static int GetTextureNormalMapId()
        {
            return _uniformTextureNormalMap;
        }

        public static int GetTextureNormalMapUseId()
        {
            return _uniformTextureNormalMapUse;
        }
    }
}
