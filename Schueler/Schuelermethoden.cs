using System;
using System.Collections.Generic;
using System.Text;
using OpenGLTutorial.GameCore;

namespace OpenGLTutorial.Schueler
{
    public static class Schuelermethoden
    {
        /// <summary>
        /// Diese Methode sortiert die übergebenen Objekte des Arrays 'objektArray' aufsteigend nach der Position ihrer linken Seite.
        /// </summary>
        /// <param name="objekte">Zu sortierendes Array mit GameObject-Instanzen</param>
        public static void SortiereDieObjekte(GameObject[] objektArray)
        {
            // TODO: Hier die Objekte aufsteigend anhand des Position ihrer linken Seite sortieren!
            //       Die Sortierung erfolgt im via Parameter übergebenen Array (ohne Kopie) mit Hilfe
            //       von 'BubbleSort'.
        }

        /// <summary>
        /// Durchläuft das mit GameObject-Instanzen gefüllte Array und markiert die Objekte, die potenziell kollidieren.
        /// </summary>
        /// <param name="objektArray">Die zu untersuchenden GameObject-Instanzen in einem Array</param>
        public static void MarkierePotenzielleKollisionskandidaten (GameObject[] objektArray)
        {
           // TODO: Hier müssen alle Objekte des Arrays durchlaufen werden.
           //       Für jedes Objekt im Array wird geprüft, ob eines der nachfolgenden Objekte im Array
           //       mit ihm kollidieren könnte. Ist dies der Fall, werden beide Objekte als potenzielle
           //       Kollisionskandidaten markiert. Ist ein dem zu testenden Objekt nachfolgendes Objekt
           //       kein Kollisionskandidat, so kann dank der vorherigen Sortierung angenommen werden,
           //       dass auch alle weiteren Objekte rechts davon nicht mit dem Objekt kollidieren können.
           //       Anschließend wird das nächste Objekt betrachtet.
        }
    }
}
