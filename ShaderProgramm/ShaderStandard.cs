﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using System.IO;

namespace OpenGLTutorial.ShaderProgramm
{
    public static class ShaderStandard
    {
        private static int _shaderId = -1;

        private static int _vertexShaderId = -1;
        private static int _fragmentShaderId = -1;

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

        }

        public static int GetProgramId()
        {
            return _shaderId;
        }
    }
}
