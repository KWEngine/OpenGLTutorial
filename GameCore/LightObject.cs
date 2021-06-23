using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGLTutorial.GameCore
{
    /// <summary>
    /// Lichtobjekte haben bisher nur eine Position.
    /// Denkbar wären hier noch Felder für Intensität und Farbe.
    /// </summary>
    public class LightObject
    {
        public Vector3 Position = new Vector3(0, 0, 0);
    }
}
