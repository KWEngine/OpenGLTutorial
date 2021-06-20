using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenGLTutorial.Primitives;
using OpenGLTutorial.ShaderProgram;
using OpenGLTutorial.Textures;
using OpenGLTutorial.GameCore;
using OpenGLTutorial.Schueler;

namespace OpenGLTutorial
{
    class ApplicationWindow : GameWindow
    {
        private GameWorld _currentWorld = new GameWorld();
        public static ApplicationWindow CurrentWindow;

        private Matrix4 _projectionMatrix = Matrix4.Identity;   // Gleicht das Bildschirmverhältnis (z.B. 16:9) aus
        private Matrix4 _viewMatrix = Matrix4.Identity;         // Simuliert eine Kamera

        public ApplicationWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) 
            : base(gameWindowSettings, nativeWindowSettings)
        {
            VSync = VSyncMode.Adaptive;
            CurrentWindow = this;
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
            ShaderHUD.Init();

            _viewMatrix = Matrix4.LookAt(0, 0, 1, 0, 0, 0, 0, 1, 0);

            GameObject[] list = PrepareGameObjectsForTask();
            int i = 0;
            while (i < list.Length)
            {
                _currentWorld.AddGameObject(list[i]);
                i++;
            }
        }

        private GameObject[] PrepareGameObjectsForTask()
        {
            GameObject[] gameObjectList = new GameObject[10];

            GameObject g1 = new GameObject();
            g1.SetPosition(-500, 250);
            g1.SetScale(100, 50);
            g1.SetTexture("OpenGLTutorial.Textures.color_blue.bmp");
            
            GameObject g2 = new GameObject();
            g2.SetPosition(-350, 150);
            g2.SetScale(250, 50);
            g2.SetTexture("OpenGLTutorial.Textures.color_green.bmp");

            GameObject g3 = new GameObject();
            g3.SetPosition(-100, 50);
            g3.SetScale(50, 50);
            g3.SetTexture("OpenGLTutorial.Textures.color_orange.bmp");

            GameObject g4 = new GameObject();
            g4.SetPosition(100, 50);
            g4.SetScale(200, 50);
            g4.SetTexture("OpenGLTutorial.Textures.color_red.bmp");

            GameObject g5 = new GameObject();
            g5.SetPosition(150, -50);
            g5.SetScale(50, 50);
            g5.SetTexture("OpenGLTutorial.Textures.color_yellow.bmp");

            GameObject g6 = new GameObject();
            g6.SetPosition(300, -150);
            g6.SetScale(300, 50);
            g6.SetTexture("OpenGLTutorial.Textures.color_pink.bmp");

            gameObjectList[0] = g1;
            gameObjectList[1] = g2;
            gameObjectList[2] = g3;
            gameObjectList[3] = g4;
            gameObjectList[4] = g5;
            gameObjectList[5] = g6;

            return gameObjectList;
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

            // Berechne die aktuellen Positionen aller Lichtobjekte und speichere sie in einem Array:
            float[] lightpositions = _currentWorld.GetLightPositions();

            // Hole die aktuellen Objekte, die gezeichnet werden sollen:
            GameObject[] aktuelleObjektliste = _currentWorld.GetGameObjects().ToArray();

            // Sortiere sie aufsteigend nach der Position ihrer linken Seite:
            Schuelermethoden.SortiereDieObjekte(aktuelleObjektliste);

            // Durchlaufe alle Objekte und markiere diejenigen, die potenziell Teil einer Kollision sind:
            Schuelermethoden.MarkierePotenzielleKollisionskandidaten(aktuelleObjektliste);

            // Shader-Programm wählen:
            ShaderStandard.Draw(lightpositions, _viewMatrix * _projectionMatrix, aktuelleObjektliste);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }
    }
}
