using System;
using OpenTK.Windowing.Desktop;

namespace OpenGLTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            GameWindowSettings gws = new GameWindowSettings();
            gws.IsMultiThreaded = false;
            gws.RenderFrequency = 0;
            gws.UpdateFrequency = 0;
            NativeWindowSettings nws = new NativeWindowSettings();
            nws.Flags = OpenTK.Windowing.Common.ContextFlags.Debug;
            nws.Size = new OpenTK.Mathematics.Vector2i(1280, 720);
            nws.IsFullscreen = false;
            nws.NumberOfSamples = 0; // FSAA
            nws.Title = "EF_SI_AE Broadphase Collision Detection";
            nws.WindowBorder = OpenTK.Windowing.Common.WindowBorder.Resizable;

            ApplicationWindow w = new ApplicationWindow(gws, nws);
            w.Run();
            w.Dispose();
        }
    }
}
