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
    }
}
