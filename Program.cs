using System;
using OpenTK.Windowing.Desktop;

namespace OpenGLTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            GameWindowSettings gws = new GameWindowSettings();
            gws.UpdateFrequency = 0;
            NativeWindowSettings nws = new NativeWindowSettings();
            nws.Flags = OpenTK.Windowing.Common.ContextFlags.Debug;
            nws.NumberOfSamples = 0; // FSAA
            nws.Title = "Mein OpenGL-Projekt";
            nws.WindowBorder = OpenTK.Windowing.Common.WindowBorder.Resizable;

            ApplicationWindow w = new ApplicationWindow(gws, nws);
            w.Run();
            w.Dispose();
        }
    }
}
