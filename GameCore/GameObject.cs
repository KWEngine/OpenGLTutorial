using System;
using System.Collections.Generic;
using System.Text;
using OpenGLTutorial.Textures;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLTutorial.GameCore
{
    /// <summary>
    /// Die GameObject-Klasse beschreibt ein Objekt, das vom Renderprogramm gezeichnet werden soll.
    /// Alle Instanzen verfügen über ihre eigenen Angaben zu Position, Rotation und Skalierung (Größe).
    /// </summary>
    public class GameObject
    {
        private Vector2 Position = new Vector2(0, 0);                   // Speichert die Objektposition (Mitte des Objekts)
        private Quaternion Orientation = new Quaternion(0, 0, 0, 1);    // Speichert die Rotation (Orientierung) des Objekts
        private Vector2 Scale = new Vector2(1, 1);                      // Speichert die Größe (Breite, Höhe) des Objekts
        private int _textureId = -1;                                    // Speichert die OpenGL-ID der Textur, die für das Objekt verwendet werden soll
        private int _textureNormalMapId = -1;                           // s.o., nur für die Normal-Map-Textur (optional)
        private bool _hasPotentialCollisions = false;                   // Speichert, ob das Objekt ein Kollisionskandidat ist

        /// <summary>
        ///  Markiert das Objekt als Kollisionskandidat
        /// </summary>
        public void MarkAsCollisionCandidate()                         
        {
            _hasPotentialCollisions = true;
        }

        /// <summary>
        /// Fragt das Objekt, ob es ein Kollisionskandidat ist.
        /// </summary>
        /// <returns>true, wenn es potenziell kollidiert, sonst false</returns>
        public bool IsCollisionCandidate()                           
        {
            return _hasPotentialCollisions;
        }

        /// <summary>
        /// Erfragt die Position des linken Objektrands
        /// </summary>
        /// <returns>Linker Objektrand (in Pixeln)</returns>
        public int GetLeft()                                            
        {
            return GetCenterX() - GetWidth() / 2;
        }

        /// <summary>
        /// Erfragt die Position des rechten Objektrands
        /// </summary>
        /// <returns>Rechter Objektrand (in Pixeln)</returns>
        public int GetRight()
        {
            return GetCenterX() + GetWidth() / 2;
        }

        /// <summary>
        /// Erfragt die Position des oberen Objektrands
        /// </summary>
        /// <returns>Oberer Objektrand (in Pixeln)</returns>
        public int GetTop()
        {
            return GetCenterY() + GetHeight() / 2;
        }

        /// <summary>
        /// Erfragt die Position des unteren Objektrands
        /// </summary>
        /// <returns>Unterer Objektrand (in Pixeln)</returns>
        public int GetBottom()
        {
            return GetCenterY() - GetHeight() / 2;
        }

        /// <summary>
        /// Erfragt die ID der aktuell dem Objekt zugewiesenen Textur
        /// </summary>
        /// <returns>OpenGL-Textur-ID</returns>
        public int GetTextureId()
        {
            return _textureId;
        }

        /// <summary>
        /// Setzt die Textur des Objekts auf die im Parameter angegebene Datei.
        /// Die Datei muss sich im Ordner 'Textures' befinden und als 'Eingebettete Ressource' markiert sein.
        /// </summary>
        /// <param name="filename">Dateiname (ohne Pfad)</param>
        public void SetTexture(string filename)
        {
            if(TextureLoader.IsTextureAlreadyDefined(filename))
            {
                _textureId = TextureLoader.GetTexture(filename);
            }
            else
            {
                _textureId = TextureLoader.LoadTexture(filename);
            }
        }

        /// <summary>
        /// Erfragt die ID der aktuell dem Objekt zugewiesenen Normal-Map-Textur
        /// </summary>
        /// <returns>OpenGL-Textur-ID</returns>
        public int GetTextureNormalMap()
        {
            return _textureNormalMapId;
        }

        /// <summary>
        /// Setzt die Normal-Map-Textur des Objekts auf die im Parameter angegebene Datei.
        /// Die Datei muss sich im Ordner 'Textures' befinden und als 'Eingebettete Ressource' markiert sein.
        /// </summary>
        /// <param name="filename">Dateiname (ohne Pfad)</param>
        public void SetNormalMap(string filename)
        {
            if (TextureLoader.IsTextureAlreadyDefined(filename))
            {
                _textureNormalMapId = TextureLoader.GetTexture(filename);
            }
            else
            {
                _textureNormalMapId = TextureLoader.LoadTexture(filename);
            }
        }

        /// <summary>
        /// Setzt die Größe des Objekts in Pixeln
        /// </summary>
        /// <param name="x">Pixelbreite</param>
        /// <param name="y">Pixelhöhe</param>
        public void SetScale(int x, int y)
        {
            if(x > 0 && y > 0)
            {
                Scale = new Vector2(x, y);
            }
            else
            {
                // Fehlerfall: hier vielleicht Exception werfen oder Fehlermeldung ausgeben!
                Scale = new Vector2(1, 1);
            }
        }

        /// <summary>
        /// Erfragt die aktuelle Breite des Objekts
        /// </summary>
        /// <returns>Pixelbreite</returns>
        public int GetWidth()
        {
            return (int)Scale.X;
        }

        /// <summary>
        /// Erfragt die aktuelle Höhe des Objekts
        /// </summary>
        /// <returns>Pixelhöhe</returns>
        public int GetHeight()
        {
            return (int)Scale.Y;
        }

        /// <summary>
        /// Erfragt die aktuelle Größe des Objekts als Vector2-Instanz
        /// </summary>
        /// <returns>Objektgröße (in Vector2 floats)</returns>
        public Vector2 GetScale()
        {
            return Scale;
        }

        /// <summary>
        /// Setzt die Position des Objekts (bzw. der Objektmitte)
        /// </summary>
        /// <param name="x">x-Position</param>
        /// <param name="y">y-Position</param>
        public void SetPosition(int x, int y)
        {
            Position = new Vector2(x, y);
        }

        /// <summary>
        /// Erfragt die X-Koordinate der Objektmitte
        /// </summary>
        /// <returns>X-Koordinate der Objektmitte in Pixeln</returns>
        public int GetCenterX()
        {
            return (int)Position.X;
        }

        /// <summary>
        /// Erfragt die Y-Koordinate der Objektmitte
        /// </summary>
        /// <returns>Y-Koordinate der Objektmitte in Pixeln</returns>
        public int GetCenterY()
        {
            return (int)Position.Y;
        }

        /// <summary>
        /// Erfragt die Position der Objektmitte 
        /// </summary>
        /// <returns>Objektmitte als Vector2 floats</returns>
        public Vector2 GetPosition()
        {
            return Position;
        }

        /// <summary>
        /// Rotiert das Objekt um die angegebenen Grad
        /// </summary>
        /// <param name="degrees">Anzahl der Grad (0 bis 359)</param>
        /// <param name="a">Achse, um die rotiert wird (Standard: Z)</param>
        public void AddRotation(float degrees, Axis a = Axis.Z)
        {
            Quaternion newrotation;
            if (a == Axis.Z)
            {
                newrotation = Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(degrees));
            }
            else if(a == Axis.Y)
            {
                newrotation = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(degrees));
            }
            else
            {
                newrotation = Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians(degrees));
            }

            // Multipliziert man 2 Quaternions miteinander, addieren sich ihre Rotationswerte:
            // (Man könnte das * also eher wie ein + verstehen)
            Orientation = Orientation * newrotation;
        }

        /// <summary>
        /// Erfragt die aktuelle Rotation des Objekts
        /// </summary>
        /// <returns>Rotation des Objekts als Quaternion (floats)</returns>
        public Quaternion GetRotation()
        {
            return Orientation;
        }

        /// <summary>
        /// Diese Methode soll später von Unterklassen überschrieben werden können. 
        /// Deshalb wird sie als 'virtual' markiert. Dies ermöglicht es einer Unterklasse
        /// die Methode selbst anzulegen und mit Hilfe des 'override'-Keywords dann als
        /// Überschreibung zu deklarieren!
        /// </summary>
        /// <param name="k">Aktueller Keyboardstatus (welche Tasten sind gedrückt?)</param>
        /// <param name="s">Aktueller Mausstatus (welche Tasten sind gedrückt und wo ist der Cursor?)</param>
        public virtual void Update(KeyboardState k, MouseState s)
        {
            // Ich enthalte nichts, weil ich erst von den erbenden Klassen implementiert werde!
        }
    }
}
