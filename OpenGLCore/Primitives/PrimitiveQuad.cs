using OpenTK.Graphics.OpenGL4;

namespace OpenGLTutorial.OpenGLCore.Primitives
{
    /// <summary>
    /// Enthält die Geometriedaten eines Quadrats
    /// </summary>
    public static class PrimitiveQuad
    {
        // Ein VAO ist in OpenGL ein Vertex-Array-Object.
        // Es ist eine Art Container für diverse Geometrieinformationen.
        // Wenn ein Objekt gerendert werden soll, gibt man einfach nur die VAO-ID an
        // und OpenGL holt sich dazu dann die entsprechenden Geometriedaten:
        private static int _vao = -1;

        // Liste mit den Eckpunkten des Quadrats (es wird aus zwei rechtwinkligen Dreiecken gebildet).
        // Dreiecke sind die bevorzugte Form in der 3D-Grafikverarbeitung.
        // Jedes primitive Geometrieobjekt sollte sein Zentrum im Ursprung des Koordinatensystems haben
        // (bei einer Größe von 1x1 sind die äußeren Ränder also jeweils -0.5 und +0.5).
        private static float[] _vertices = new float[]
        {
            // Dreieck 1:
            -0.5f, -0.5f, 0,
            +0.5f, -0.5f, 0,
            -0.5f, +0.5f, 0,

            // Dreieck 2:
            +0.5f, -0.5f, 0,
            +0.5f, +0.5f, 0,
            -0.5f, +0.5f, 0

        };

        // UVs sind die Texturkoordinaten. Sie bestimmen, wie sich eine Textur (z.B. JPG)
        // über die Quadratfläche verteilt. Dies geschieht anhand der Eckpunkte (siehe _vertices).
        // Man nennt es UV, weil XY bereits für die Breite/Höhe steht und sonst Verwechslungsgefahr besteht.
        private static float[] _uvs = new float[]
        {
           0, 0,
           1, 0,
           0, 1,

           1, 0,
           1, 1,
           0, 1
        };

        // Normalvektoren sind nur für die Lichtberechnung wichtig. Sie geben an
        // wohin die Fläche des Quadrats zeigt, wenn es nicht gedreht/geneigt wird.
        // Da es in OpenGL keine 'Fläche' gibt, wird auf jedem Eckpunkt eine solcher
        // Normalenvektor platziert. Deshalb gibt es auch 6 Vektoren [jeder (0|0|1)].
        private static float[] _normals = new float[]
        {
            0, 0, 1,
            0, 0, 1,
            0, 0, 1,

            0, 0, 1,
            0, 0, 1,
            0, 0, 1
        };

        // Tangenten liegen im 90°-Winkel zu den Normalvektoren.
        private static float[] _tangents = new float[]
        {
            1, 0, 0,
            1, 0, 0,
            1, 0, 0,

            1, 0, 0,
            1, 0, 0,
            1, 0, 0
        };

        // Bitangenten liegen im 90°-Winkel zu den Tangenten.
        private static float[] _bitangents = new float[]
        {
            0, 1, 0,
            0, 1, 0,
            0, 1, 0,

            0, 1, 0,
            0, 1, 0,
            0, 1, 0
        };

        /// <summary>
        /// Initialisiert die Geometriedaten des Quadrats
        /// </summary>
        public static void Init()
        {
            // Generiere und binde das Container-Objekt (VAO):
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            // VBOs sind spezielle OpenGL-Puffer (Arrays), die Informationen über die Geometrie enthalten.
            // Das erste VBO ist für die Eckpunkte der 2 Dreiecke.
            // Im Prinzip kopieren wir den Inhalt des float-Arrays '_vertices' (siehe weiter oben)
            // in das VBO-Objekt.
            int vboVertices = GL.GenBuffer();                                                                       // Lege Puffer an
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboVertices);                                                   // Binde den Puffer als aktuelles Objekt
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * 4, _vertices, BufferUsageHint.StaticDraw);   // Kopiere _vertices hinein
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);                               // Das VBO liegt an Position 0 im VAO
            GL.EnableVertexAttribArray(0);                                                                          // Markiere die Position als 'aktiv'
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);                                                             // Löse die Bindung

            int vboUVs = GL.GenBuffer();                                                                            // Ähnlich läuft es für die anderen Daten!
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboUVs);
            GL.BufferData(BufferTarget.ArrayBuffer, _uvs.Length * 4, _uvs, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            int vboNormals = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboNormals);
            GL.BufferData(BufferTarget.ArrayBuffer, _normals.Length * 4, _normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            int vboTangents = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboTangents);
            GL.BufferData(BufferTarget.ArrayBuffer, _tangents.Length * 4, _tangents, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(3);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            int vboBiTangents = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboBiTangents);
            GL.BufferData(BufferTarget.ArrayBuffer, _bitangents.Length * 4, _bitangents, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(4);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindVertexArray(0);  // Wenn alle VBOs in das VAO gepackt wurden, wird die Bindung zum VAO aufgehoben. Es ist fertig konfiguriert.
        }

        /// <summary>
        /// Gibt die VAO-ID des Quadrats zurück.
        /// </summary>
        /// <returns>VAO-ID als int32</returns>
        public static int GetVAOId()
        {
            return _vao;
        }

        /// <summary>
        /// Gibt die Anzahl der zu zeichnenden Punkte (2 Dreiecke = 6 Eckpunkte) an.
        /// </summary>
        /// <returns>Für Quadrat immer 6</returns>
        public static int GetPointCount()
        {
            return 6;
        }
    }
}
