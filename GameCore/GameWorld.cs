using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGLTutorial.GameCore
{
    public class GameWorld
    {
        private List<GameObject> _gameObjects = new List<GameObject>();
        private List<LightObject> _lightObjects = new List<LightObject>();

        public void AddGameObject(GameObject g)
        {
            _gameObjects.Add(g);
        }

        public bool RemoveGameObject(GameObject g)
        {
            return _gameObjects.Remove(g);
        }

        public void AddLightObject(LightObject l)
        {
            _lightObjects.Add(l);
        }

        public bool RemoveLightObject(LightObject l)
        {
            return _lightObjects.Remove(l);
        }

        public List<GameObject> GetGameObjects()
        {
            return _gameObjects;
        }

        public float[] GetLightPositions()
        {
            float[] positions = new float[_lightObjects.Count * 3];
            for(int i = 0, j = 0; i < _lightObjects.Count; i++, j += 3)
            {
                LightObject currentLight = _lightObjects[i];
                positions[j] = currentLight.Position.X;
                positions[j+1] = currentLight.Position.Y;
                positions[j+2] = currentLight.Position.Z;
            }

            return positions;
        }

        public int GetLightCount()
        {
            return _lightObjects.Count;
        }
    }
}
