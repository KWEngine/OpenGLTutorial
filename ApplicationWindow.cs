using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenGLTutorial.Primitives;
using OpenGLTutorial.ShaderProgramm;

namespace OpenGLTutorial
{
    class ApplicationWindow : GameWindow
    {
        private Matrix4 _projectionMatrix = Matrix4.Identity;   // Gleicht das Bildschirmverhältnis (z.B. 16:9) aus
        private Matrix4 _viewMatrix = Matrix4.Identity;         // Simuliert eine Kamera

        public ApplicationWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) 
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // Basis-OpenGL-Aktionen (wie z.B. grundlegende Einstellungen) ausführen!
            GL.ClearColor(0, 0, 0, 1); // Farbe des gelöschten Bildschirms wählen

            GL.Enable(EnableCap.DepthTest); // Tiefenpuffer aktivieren

            GL.Enable(EnableCap.CullFace); // Zeichnen von verdeckten Teilen eines Objekts verhindern
            GL.CullFace(CullFaceMode.Back); // Der Kamera abgewandte Flächen werden ignoriert
            GL.FrontFace(FrontFaceDirection.Ccw);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            PrimitiveTriangle.Init();
            ShaderStandard.Init();

            _viewMatrix = Matrix4.LookAt(0, 0, 1, 0, 0, 0, 0, 1, 0);

        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            // Anpassung der 3D-Instanzen an die neue Fenstergröße

             GL.Viewport(0, 0, Size.X, Size.Y);
            _projectionMatrix = Matrix4.CreateOrthographic(Size.X, Size.Y, 0.1f, 1000f);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            // Hier passiert das tatsächliche Zeichnen von Formen (z.B. Dreiecke)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // view-projection matrix:
            Matrix4 viewProjection = _viewMatrix * _projectionMatrix;

            // model matrix (zum Testen):
            Matrix4 modelMatrix = Matrix4.CreateScale(200) * Matrix4.CreateTranslation(-100, 0, 0);

            // model-view-projection matrix erstellen:
            Matrix4 mvp = modelMatrix * viewProjection;

            // Shader-Programm wählen:
            GL.UseProgram(ShaderStandard.GetProgramId());
            GL.UniformMatrix4(ShaderStandard.GetMatrixId(), false, ref mvp);

            GL.BindVertexArray(PrimitiveTriangle.GetVAOId());
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.BindVertexArray(0);

            GL.UseProgram(0);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            // Objekte richten sich je nach Benutzereingaben neu in der Welt aus
        }
    }
}
