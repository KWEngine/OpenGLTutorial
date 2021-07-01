using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace OpenGLTutorial.GameCore
{
    /// <summary>
    /// Helferklasse zum Anzeigen von Fehlern
    /// </summary>
    public static class ErrorChecker
    {
        /// <summary>
        /// Prüft, ob OpenGL Fehlercodes produziert hat und gibt diese auf der Debug-Konsole aus.
        /// </summary>
        /// <returns>true, wenn es einen Fehler gab</returns>
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
