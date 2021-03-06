﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLTutorial.Primitives
{
    public static class PrimitiveQuad
    {
        private static int _vao = -1;

        private static float[] _vertices = new float[]
        {
            -0.5f, -0.5f, 0,
            +0.5f, -0.5f, 0,
            -0.5f, +0.5f, 0,

            +0.5f, -0.5f, 0,
            +0.5f, +0.5f, 0,
            -0.5f, +0.5f, 0

        };

        private static float[] _uvs = new float[]
        {
           0, 1,
           1, 1,
           0, 0,

           1, 1,
           1, 0,
           0, 0
        };

        private static float[] _normals = new float[]
        {
            0, 0, 1,
            0, 0, 1,
            0, 0, 1,

            0, 0, 1,
            0, 0, 1,
            0, 0, 1
        };

        private static float[] _tangents = new float[]
        {
            1, 0, 0,
            1, 0, 0,
            1, 0, 0,

            1, 0, 0,
            1, 0, 0,
            1, 0, 0
        };

        private static float[] _bitangents = new float[]
        {
            0, 1, 0,
            0, 1, 0,
            0, 1, 0,

            0, 1, 0,
            0, 1, 0,
            0, 1, 0
        };

        public static void Init()
        {
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            int vboVertices = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboVertices);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * 4, _vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            int vboUVs = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboUVs);
            GL.BufferData(BufferTarget.ArrayBuffer, _uvs.Length * 4, _uvs, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            int vboNormals = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboNormals);
            GL.BufferData(BufferTarget.ArrayBuffer, _normals.Length * 4, _normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            int vboTangents = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboTangents);
            GL.BufferData(BufferTarget.ArrayBuffer, _tangents.Length * 4, _tangents, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(3);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            int vboBiTangents = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboBiTangents);
            GL.BufferData(BufferTarget.ArrayBuffer, _bitangents.Length * 4, _bitangents, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(4);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindVertexArray(0);
        }

        public static int GetVAOId()
        {
            return _vao;
        }

        public static int GetPointCount()
        {
            return 6;
        }
    }
}
