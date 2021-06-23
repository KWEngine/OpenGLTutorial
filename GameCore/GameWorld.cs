using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGLTutorial.GameCore
{
    /// <summary>
    /// Eine GameWorld-Instanz enthält eine Liste aller Objekte (auch Lichter).
    /// Statt in Arrays werden die Objekte in Listen vorgehalten, weil Listen
    /// in ihrer Größe dynamisch verkleinert oder vergrößert werden können.
    /// Intern sind Listen kaum von Arrays zu unterscheiden.
    /// </summary>
    public class GameWorld
    {
        private List<GameObject> _gameObjects = new List<GameObject>();     // Liste aller GameObject-Instanzen
        private List<LightObject> _lightObjects = new List<LightObject>();  // Liste aller Lichtinstanzen

        /// <summary>
        /// Fügt der Welt ein GameObject hinzu
        /// </summary>
        /// <param name="g">hinzuzufügendes GameObject</param>
        public void AddGameObject(GameObject g)
        {
            _gameObjects.Add(g);
        }

        /// <summary>
        /// Entfernt ein GameObject aus der Welt
        /// </summary>
        /// <param name="g">zu entfernendes GameObject</param>
        /// <returns>true, wenn das Entfernen geklappt hat</returns>
        public bool RemoveGameObject(GameObject g)
        {
            return _gameObjects.Remove(g);
        }

        /// <summary>
        /// Fügt ein Lichtobjekt hinzu
        /// </summary>
        /// <param name="l">Hinzuzufügendes Lichtobjekt</param>
        public void AddLightObject(LightObject l)
        {
            _lightObjects.Add(l);
        }

        /// <summary>
        /// Entfernt ein Lichtobjekt aus der Welt
        /// </summary>
        /// <param name="l">zu entfernendes Lichtobjekt</param>
        /// <returns>true, wenn das Entfernen geklappt hat</returns>
        public bool RemoveLightObject(LightObject l)
        {
            return _lightObjects.Remove(l);
        }

        /// <summary>
        /// Erfragt die Liste der aktuellen GameObjekt-Instanzen
        /// </summary>
        /// <returns>Liste der Instanzen</returns>
        public List<GameObject> GetGameObjects()
        {
            return _gameObjects;
        }

        /// <summary>
        /// Erfragt die Positionen aller Lichtobjekte als float[]-Array für das Renderprogramm.
        /// (Diese Methode hat keine Auswirkung auf das Spielgeschehen)
        /// </summary>
        /// <returns>Array mit Lichtpositionen [x][y][z][x][y][z]... </returns>
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

        /// <summary>
        /// Erfragt die Anzahl der aktuell in der Welt verorteten Lichter
        /// </summary>
        /// <returns>Anzahl Lichter</returns>
        public int GetLightCount()
        {
            return _lightObjects.Count;
        }
    }
}
