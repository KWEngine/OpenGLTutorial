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
using OpenGLTutorial.Textures;
using OpenGLTutorial.GameCore;

namespace OpenGLTutorial
{
    class ApplicationWindow : GameWindow
    {
        private GameWorld _currentWorld = new GameWorld();

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
            PrimitiveQuad.Init();
            ShaderStandard.Init();

            _viewMatrix = Matrix4.LookAt(0, 0, 1, 0, 0, 0, 0, 1, 0);

           

            GameObject g1 = new GameObject();
            g1.Position = new Vector3(100, -50, 0);
            g1.SetScale(300, 300, 1);
            g1.SetTexture("OpenGLTutorial.Textures.crate.jpg");
            _currentWorld.AddGameObject(g1);

            GameObject g2 = new GameObject();
            g2.Position = new Vector3(-100, +50, 0);
            g2.SetScale(50, 50, 1);
            g2.SetTexture("OpenGLTutorial.Textures.crate.jpg");
            _currentWorld.AddGameObject(g2);
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

            // Shader-Programm wählen:
            GL.UseProgram(ShaderStandard.GetProgramId());

            foreach (GameObject g in _currentWorld.GetGameObjects())
            {
                Matrix4 modelMatrix = 
                    Matrix4.CreateScale(g.GetScale()) 
                    * Matrix4.CreateFromQuaternion(g.GetRotation()) 
                    * Matrix4.CreateTranslation(g.Position);

                // model-view-projection matrix erstellen:
                Matrix4 mvp = modelMatrix * viewProjection;

                GL.UniformMatrix4(ShaderStandard.GetMatrixId(), false, ref mvp);

                // Texture an den Shader übertragen:
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, g.GetTextureId());
                GL.Uniform1(ShaderStandard.GetTextureId(), 0);


                GL.BindVertexArray(PrimitiveQuad.GetVAOId());
                GL.DrawArrays(PrimitiveType.Triangles, 0, PrimitiveQuad.GetPointCount());
                GL.BindVertexArray(0);

                GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            GL.UseProgram(0);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            // Objekte richten sich je nach Benutzereingaben neu in der Welt aus

            foreach (GameObject g in _currentWorld.GetGameObjects())
            {
                g.Update(KeyboardState, MouseState);
            }
        }
    }
}
