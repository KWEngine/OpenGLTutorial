using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace OpenGLTutorial.GameCore
{
    public static class ErrorChecker
    {
        public static bool Check()
        {
            bool hasError = false;
            ErrorCode c;
            while ((c = GL.GetError()) != ErrorCode.NoError)
            {
                hasError = true;
                Debug.WriteLine(c.ToString());
            }
            return hasError;
        }
    }
}
