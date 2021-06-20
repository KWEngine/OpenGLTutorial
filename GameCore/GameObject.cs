using System;
using System.Collections.Generic;
using System.Text;
using OpenGLTutorial.Textures;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLTutorial.GameCore
{
    public class GameObject
    {
        private Vector2 Position = new Vector2(0, 0);
        private Quaternion Orientation = new Quaternion(0, 0, 0, 1);
        private Vector2 Scale = new Vector2(1, 1);
        private int _textureId = -1;
        private int _textureNormalMapId = -1;
        private int _number = -1;
        private bool _hasPotentialCollisions = false;

        public void MarkAsCollisionCandidate()
        {
            _hasPotentialCollisions = true;
        }

        public bool IsCollisionCandidate()
        {
            return _hasPotentialCollisions;
        }

        public int GetLeft()
        {
            return GetCenterX() - GetWidth() / 2;
        }

        public int GetRight()
        {
            return GetCenterX() + GetWidth() / 2;
        }

        public int GetTop()
        {
            return GetCenterY() + GetHeight() / 2;
        }

        public int GetBottom()
        {
            return GetCenterY() - GetHeight() / 2;
        }

        public void SetNumber(int n)
        {
            if (_number >= 0)
                _number = n;
        }

        public int GetNumber()
        {
            return _number;
        }

        public int GetTextureId()
        {
            return _textureId;
        }

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

        public int GetTextureNormalMap()
        {
            return _textureNormalMapId;
        }

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

        public void SetScale(int x, int y)
        {
            if(x > 0 && y > 0)
            {
                Scale = new Vector2(x, y);
            }
            else
            {
                // Fehlerfall: entweder Exception oder Fehlermeldung ausgeben!
                Scale = new Vector2(1, 1);
            }
        }

        public int GetWidth()
        {
            return (int)Scale.X;
        }

        public int GetHeight()
        {
            return (int)Scale.Y;
        }

        public Vector2 GetScale()
        {
            return Scale;
        }

        public void SetPosition(int x, int y)
        {
            Position = new Vector2(x, y);
        }

        public int GetCenterX()
        {
            return (int)Position.X;
        }

        public int GetCenterY()
        {
            return (int)Position.Y;
        }

        public Vector2 GetPosition()
        {
            return Position;
        }

        public void AddRotation(Axis a, float degrees)
        {
            Quaternion newrotation;
            if (a == Axis.X)
            {
                newrotation = Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians(degrees));
            }
            else if(a == Axis.Y)
            {
                newrotation = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(degrees));
            }
            else
            {
                newrotation = Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(degrees));
            }

            // Multipliziert man 2 Quaternions miteinander, addieren sich ihre Rotationswerte:
            // (Man könnte das * also eher wie ein + verstehen)
            Orientation = Orientation * newrotation;
        }

        public Quaternion GetRotation()
        {
            return Orientation;
        }

        public virtual void Update(KeyboardState k, MouseState s)
        {
            // TODO: Diese Methode soll später von Unterklassen überschrieben werden können. 
            //       Deshalb wird sie als 'virtual' markiert. Dies ermöglicht es einer Unterklasse
            //       die Methode selbst anzulegen und mit Hilfe des 'override'-Keywords dann als
            //       Überschreibung zu deklarieren!
        }
    }
}
