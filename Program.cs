using System;
using OpenGLTutorial.OpenGLCore;
using OpenTK.Windowing.Desktop;

namespace OpenGLTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            // Um das Anwendungsfenster auf dem Bildschirm zentrieren zu können,
            // müssen die Bildschirmbreite und -höhe ermittelt werden.
            // Leider bietet .NET Core 3 dazu bisher keine gute Komfortmethode an.
            // Also muss die Windows-API (in C++ geschriebene DLL user32.dll) 
            // befragt werden (siehe Win32API.cs):
            int screenWidth = Win32API.GetScreenWidth();
            int screenHeight = Win32API.GetScreenHeight();

            // Im GameWindowSettings-Objekt werden Render-Einstellungen
            // vorgenommen (Aktualisierungsrate, etc.):
            GameWindowSettings gws = new GameWindowSettings();
            gws.IsMultiThreaded = false;
            gws.RenderFrequency = 0;
            gws.UpdateFrequency = 0;

            // In den NativeWindowSettings werden Einstellungen vorgenommen
            // die nicht den Fensterinhalt, sondern eher den Rahmen des Fensters
            // (was beim Betriebssystem dann registriert wird) betreffen:
            NativeWindowSettings nws = new NativeWindowSettings();              
            nws.Flags = OpenTK.Windowing.Common.ContextFlags.Debug;             // nur während der Entwicklung benötigt
            nws.Size = new OpenTK.Mathematics.Vector2i(1280, 720);              // Auflösung des Fensterinhalts
            nws.Location = new OpenTK.Mathematics.Vector2i(                     // Position des Fensters
                screenWidth / 2 - nws.Size.X / 2, 
                screenHeight / 2 - nws.Size.Y / 2
                );
            nws.IsFullscreen = false;                                           // Vollbild?
            nws.NumberOfSamples = 0; // FSAA                                    // FSAA-Stufe (0 oder 1 heißt: deaktiviert)
            nws.Title = "EF_SI_AE Broadphase Collision Detection";              // Fenstertitel
            nws.WindowBorder = OpenTK.Windowing.Common.WindowBorder.Resizable;  // Ist das Fenster in seiner Größe anpassbar?

            // Dem ApplicationWindow werden dann diese Settings mitgegeben.
            // Nach diesen Vorgaben wird es erstellt.
            ApplicationWindow w = new ApplicationWindow(gws, nws);
            w.Run();                                                            // Starte den Render-Loop
            w.Dispose();                                                        // Wird der Loop gestoppt (z.B. durch break;)
                                                                                // wird das Fenster bei Windows abgemeldet.
        }
    }
}
