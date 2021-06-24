using OpenGLTutorial.GameCore;
using OpenGLTutorial.OpenGLCore.Primitives;
using OpenGLTutorial.Schueler;
using OpenGLTutorial.ShaderProgram;
using OpenGLTutorial.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace OpenGLTutorial.OpenGLCore
{
    /// <summary>
    /// Das ApplicationWindow ist das Herzstück der Anwendung. Hier werden alle nötigen Einstellungen
    /// vorgenommen und das Grundgerüst der OpenGL-API initialisiert.
    /// Hier finden sich auch die beiden Methoden OnRenderFrame() und OnUpdateFrame(), die für das
    /// Rendern und Bewegen der Objekte zuständig sind.
    /// </summary>
    class ApplicationWindow : GameWindow
    {
        private GameWorld _currentWorld = new GameWorld();      // Erstellt eine Welt, die vom Fenster gezeigt wird
        public static ApplicationWindow CurrentWindow;          // Globales Feld, das von allen Objekten genutzt werden kann, um das Fenster anzusprechen
        public static int TextureDefault;                       // ID einer Textureinheit, die verwendet wird, wenn eine andere Textur nicht gefunden werden kann

        private Matrix4 _projectionMatrix = Matrix4.Identity;   // Diese Matrix speichert das Bildschirmseitenverhältnis (z.B. 16:9)
        private Matrix4 _viewMatrix = Matrix4.Identity;         // Simuliert eine Kamera (Position, Neigung, etc.)

        private string _windowTitle = "";
        private double _sumOfFrameTime = 0;
        private uint _sumOfFrames = 0;

        /// <summary>
        /// Konstruktormethode des OpenGL-Fensters
        /// </summary>
        /// <param name="gameWindowSettings">Spieleinstellungen</param>
        /// <param name="nativeWindowSettings">Fenstereinstellungen</param>
        public ApplicationWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) 
            : base(gameWindowSettings, nativeWindowSettings)
        {
            VSync = VSyncMode.Off;                              // Deaktiviere VSync
            CurrentWindow = this;                               // Setze den globalen Fensterzeiger auf das aktuelle Fenster
            TextureDefault = TextureLoader.LoadTexture("OpenGLTutorial.Textures.color_white.bmp");  // Lade die Standardtextur (weiße Farbe)
            _windowTitle = nativeWindowSettings.Title;
        }

        /// <summary>
        /// Diese Methode wird automatisch aufgerufen, sobald das Fenster erstellt wird (im Anschluss an die Konstruktormethode).
        /// Sie initialisiert die Geometriedaten, stellt grundlegende OpenGL-Parameter ein und fügt ein paar Beispielobjekte 
        /// in die aktuelle Spielwelt ein.
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();                                          // Aufruf der OnLoad()-Methode der Oberklasse (i.d.R. sollte diese Methode leer sein)

            GL.ClearColor(0, 0, 0, 1);                              // Farbe des leeren Bildschirms wählen

            GL.Enable(EnableCap.DepthTest);                         // Tiefenpuffer (welches Objekt liegt vor welchem?) aktivieren

            GL.Enable(EnableCap.CullFace);                          // Zeichnen von bestimmten Seiten eines Objekts verhindern:
            GL.CullFace(CullFaceMode.Back);                         // Der Kamera abgewandte Seiten werden ignoriert
            GL.FrontFace(FrontFaceDirection.Cw);                    // Ob eine Seite eine Vor- oder Rückseite ist, entscheidet die Aufzählung ihrer Eckpunkte (im Uhrzeigersinn oder nicht)
            GL.BlendFunc(                                           // Wenn ein Objekt transparent ist, wird hier festgelegt, wie genau es mit anderen
                BlendingFactor.SrcAlpha,                            // Objekten verrechnet werden soll
                BlendingFactor.OneMinusSrcAlpha
                );

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);   // Aktiviert den Hauptbildschirm (ID 0) als Renderziel

            PrimitiveQuad.Init();                                   // Initialisiert die Geometrie für Quadrate
            ShaderStandard.Init();                                  // Initialisiert das Hauptrenderprogramm (Shader)
            ShaderHUD.Init();                                       // Initialisiert das HUD-Renderprogramm  (Shader)

            _viewMatrix = Matrix4.LookAt(0, 0, 1, 0, 0, 0, 0, 1, 0);// Initialisiert die Kameraeinstellungen (Blick entlang der Z-Achse)

            GameObject[] list = PrepareGameObjectsForTask();        // Erstellt eine Liste von Beispielobjekten...
            int i = 0;
            while (i < list.Length)
            {
                _currentWorld.AddGameObject(list[i]);               // ...und fügt jedes Objekt der Spielwelt hinzu
                i++;
            }
        }


        /// <summary>
        /// Erstellt Beispielobjekte, die später in der Welt zu finden sind (für die Aufgabe auf 6 reduziert)
        /// </summary>
        /// <returns>Array mit den erstellten GameObject-Instanzen</returns>
        private GameObject[] PrepareGameObjectsForTask()
        {
            GameObject[] gameObjectList = new GameObject[6];

            GameObject g0 = new GameObject();
            g0.SetPosition(75, 50);
            g0.SetScale(100, 50);
            g0.SetTexture("OpenGLTutorial.Textures.color_blue.bmp");
            
            GameObject g1 = new GameObject();
            g1.SetPosition(200, 150);
            g1.SetScale(250, 50);
            g1.SetTexture("OpenGLTutorial.Textures.color_green.bmp");

            GameObject g2 = new GameObject();
            g2.SetPosition(500, 250);
            g2.SetScale(50, 50);
            g2.SetTexture("OpenGLTutorial.Textures.color_orange.bmp");

            GameObject g3 = new GameObject();
            g3.SetPosition(700, 350);
            g3.SetScale(200, 50);
            g3.SetTexture("OpenGLTutorial.Textures.color_red.bmp");

            GameObject g4 = new GameObject();
            g4.SetPosition(750, 450);
            g4.SetScale(50, 50);
            g4.SetTexture("OpenGLTutorial.Textures.color_yellow.bmp");

            GameObject g5 = new GameObject();
            g5.SetPosition(900, 550);
            g5.SetScale(300, 50);
            g5.SetTexture("OpenGLTutorial.Textures.color_pink.bmp");

            // Die Reihenfolge im Array ist absichtlich 'durcheinander',
            // damit die Sortieraufgabe überhaupt sinnvoll ist:
            gameObjectList[0] = g4;
            gameObjectList[1] = g1;
            gameObjectList[2] = g5;
            gameObjectList[3] = g0;
            gameObjectList[4] = g3;
            gameObjectList[5] = g2;

            return gameObjectList;
        }

        /// <summary>
        /// Wird immer dann ausgeführt, wenn der Benutzer das Fenster in seiner Größe verändert
        /// </summary>
        /// <param name="e">Zusätzliche Informationen zum Resize-Event</param>
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            // Anpassung der OpenGL-Einstellungen an die neue Fenstergröße:
             GL.Viewport(0, 0, Size.X, Size.Y);
            //_projectionMatrix = Matrix4.CreateOrthographic(Size.X, Size.Y, 0.1f, 1000f);
            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Size.X, Size.Y, 0, 0.1f, 1000f);
        }

        /// <summary>
        /// Die Methode wird periodisch aufgerufen und zeichnet die aktuelle Spielszene komplett neu. 
        /// Bei einem 60Hz-Monitor 60x pro Sekunde. Bei einem 140Hz-Monitor 140x pro Sekunde usw.!
        /// Für 60fps darf die Ausführung des Codes von OnRenderFrame() und OnUpdateFrame() zusammen nicht länger als 16ms dauern.
        /// </summary>
        /// <param name="args">Zusätzliche Informationen zum Render-Event</param>
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

            // Shader-Programm für Nummerierung wählen:
            ShaderHUD.Draw(aktuelleObjektliste);

            // Nach dem Zeichnen wird mit SwapBuffers() das gerade gezeichnete Bild an den Monitor geschickt.
            // Der Puffer, der vorher an den Monitor geschickt wurde, ist jetzt der Puffer, in der für den
            // nächsten Durchgang gezeichnet wird:
            SwapBuffers();

            // Die FPS-Anzeige wird nur einmal pro Sekunde aktualisiert, weil zu häufiges
            // Aktualisieren des Fenstertitels das System ausbremst und somit die
            // gemessenen FPS verfälschen kann:
            _sumOfFrameTime += args.Time;               // In args.Time ist die seit dem letzten Frame verstrichene Zeit (in Sekunden) enthalten
            _sumOfFrames++;                             // Jeder Frame wird (genau wie die frame time eine Zeile weiter oben) aufsummiert.
            if (_sumOfFrameTime > 1)                    // Wenn insgesamt mehr als eine Sekunde vergangen ist, dann...
            {
                // ...teile die Summe der vergangenen Sekunden durch die Summe der bis dahin gezeichneten Frames.
                // Das Ergebnis dieser Division ist dann die durchschnittliche frame time (z.B. 0.016 für 16ms).
                // Indem man z.B. 1 / 0.016 teilt, bekommt man die "frames per second":
                Title = _windowTitle + " (" + Math.Round(1 / (_sumOfFrameTime / _sumOfFrames)) + " fps)";

                // Reset der Summen für die nächste Hochrechnung:
                _sumOfFrameTime = 0;
                _sumOfFrames = 0;
            }
        }

        /// <summary>
        /// Die Methode wird periodisch aufgerufen und aktualisiert (später) alle Objekte der aktuellen Spielszene.
        /// Bei einem 60Hz-Monitor 60x pro Sekunde. Bei einem 140Hz-Monitor 140x pro Sekunde usw.!
        /// Für 60fps darf die Ausführung des Codes von OnRenderFrame() und OnUpdateFrame() zusammen nicht länger als 16ms dauern.
        /// </summary>
        /// <param name="args">Zusätzliche Informationen zum Update-Event</param>
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            // Hier ist aktuell noch nichts enthalten. Später würde man hier in einer Schleife alle GameObject-Instanzen
            // fragen, ob sie sich bewegen 'möchten' und sie entsprechend der Benutzereingaben oder ihrer AI versetzen/rotieren/skalieren:
            base.OnUpdateFrame(args);
        }
    }
}
