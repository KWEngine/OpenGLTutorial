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
            -0.5f, -0.5f, 0, // 1. Punkt des Dreiecks
            +0.5f, -0.5f, 0, // 2. Punkt des Dreiecks
            +0.0f, +0.5f, 0  // 3. Punkt des Dreiecks
        };

        private static float[] _uvs = new float[]
        {
           0, 0,    // 1. Punkt (Texturkoordinate)
           1, 0,    // 2. Punkt (Texturkoordinate)
           0.5f, 1  // 3. Punkt (Texturkoordinate)
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

            int vboUVs = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboUVs);
            GL.BufferData(BufferTarget.ArrayBuffer, 6 * 4, _uvs, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindVertexArray(0);
        }

        public static int GetVAOId()
        {
            return _vao;
        }

    }
}
