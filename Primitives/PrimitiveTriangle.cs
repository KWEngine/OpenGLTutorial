using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLTutorial.Primitives
{
    public static class PrimitiveTriangle
    {
        private static int _vao = -1;

        private static float[] _vertices = new float[]
        {
            -0.5f, -0.5f, 0,
            +0.5f, -0.5f, 0,
            +0.0f, +0.5f, 0
        };

        public static void Init()
        {
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            int vboVertices = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboVertices);
            GL.BufferData(BufferTarget.ArrayBuffer, 9 * 4, _vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindVertexArray(0);
        }

        public static int GetVAOId()
        {
            return _vao;
        }

    }
}
