using System;
using System.Collections.Generic;
using System.Reflection;
using OpenTK.Graphics.OpenGL4;
using System.IO;
using OpenTK.Mathematics;
using OpenGLTutorial.GameCore;
using OpenGLTutorial.OpenGLCore;
using OpenGLTutorial.OpenGLCore.Primitives;
using OpenGLTutorial.Textures;

namespace OpenGLTutorial.ShaderProgram
{
    /// <summary>
    /// ShaderStandard repräsentiert die CPU-Seite des GPU-Shaders.
    /// Die Klasse kümmert sich um die Kommunikation zwischen C# (CPU) und Grafikkarte.
    /// </summary>
    public static class ShaderStandard
    {
        // Jedes dieser int-Felder speichert eine OpenGL-ID.
        // OpenGL arbeitet intern mit diesen IDs und weiß genau, 
        // in welchen Speicherbereich der Grafikkarte die dazugehörigen
        // Informationen liegen.
        // Jeden Frame werden die zum Rendern benötigten Infos mit Hilfe
        // dieser IDs an die GPU geschickt.
        private static int _shaderId = -1;                      // ID des Shader-Programms (es kann mehrere Programme geben, die unterschiedlich rendern)                     

        private static int _vertexShaderId = -1;                // Der Vertex-Shader ist der Teil des Renderprogramms, der die Objekte auf dem Bildschirm positioniert
        private static int _fragmentShaderId = -1;              // Der Fragment-Shader füllt die Objekte nach der Positionierung mit Farbe, Licht, Texturen

        private static int _uniformMatrix = -1;                 // Hinter dieser ID steckt die Matrix (Model-View-Projection-Matrix), die jeden Ecktpunkt eines Objekt neu positioniert
        private static int _uniformModelMatrix = -1;            // Die Model-Matrix ist der 'Model'-Teil der Model-View-Projection-Matrix. Sie enthält nur die puren Positionsangaben - keine Angaben zu Kamera oder Bildschirmseitenverhältnis
        private static int _uniformNormalMatrix = -1;           // Die Normal-Matrix ist eine abgewandelte Form der Model-Matrix. Sie ist für die Beleuchtung wichtig.

        private static int _uniformTexture = -1;                // Link zur Textureinheit auf der GPU
        private static int _uniformTextureNormalMap = -1;       // Link zur Textureinheit der Normal-Map (Belichtungstextur) auf der GPU
        private static int _uniformTextureNormalMapUse = -1;    // Link zu einem Schalter, der angibt, ob eine Normal-Map existiert oder nicht

        private static int _uniformLightCount = -1;             // Link zur Anzahl der zu berechnenden Lichter
        private static int _uniformLightPositions = -1;         // Link zum Array mit den Lichterpositionen
        private static int _uniformAmbientLight = -1;           // Link zur Angabe des Umgebungslichts (R, G, B jeweils zwischen 0 und 1)

        private static float[] _dummyPositions = new float[0];  // Dummy-Array mit 0 Lichtpositionen
        private static int _textureIdColorOutline = TextureLoader.LoadTexture("OpenGLTutorial.Textures.color_red_outline.bmp");

        /// <summary>
        /// Initialisiert das Hauptrenderprogramm (Shader)
        /// </summary>
        public static void Init()
        {
            // Erstelle ein Shader-Programm (bzw. eine ID dazu):
            _shaderId = GL.CreateProgram(); 

            // Ermittle, wo im Arbeitsspeicher gerade unser C#-Programm liegt:
            Assembly a = Assembly.GetExecutingAssembly();

            // Vertex Shader auslesen:
            Stream sVertex = a.GetManifestResourceStream("OpenGLTutorial.ShaderProgram.shaderStandard_vertex.glsl");
            StreamReader sReaderVertex = new StreamReader(sVertex);
            string sVertexCode = sReaderVertex.ReadToEnd();
            sReaderVertex.Dispose();
            sVertex.Close();

            // Fragment Shader auslesen:
            Stream sFragment = a.GetManifestResourceStream("OpenGLTutorial.ShaderProgram.shaderStandard_fragment.glsl");
            StreamReader sReaderFragment = new StreamReader(sFragment);
            string sFragmentCode = sReaderFragment.ReadToEnd();
            sReaderFragment.Dispose();
            sFragment.Close();

            // Erstelle den Vertex-Shader-Teil des Renderprogramms:
            _vertexShaderId = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(_vertexShaderId, sVertexCode);

            // Erstelle den Fragment-Shader-Teil des Renderprogramms:
            _fragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(_fragmentShaderId, sFragmentCode);

            // Kompiliere den Vertex-Shader auf der GPU:
            GL.CompileShader(_vertexShaderId);
            GL.AttachShader(_shaderId, _vertexShaderId);

            // Kompiliere den Fragment-Shader auf der GPU:
            GL.CompileShader(_fragmentShaderId);
            GL.AttachShader(_shaderId, _fragmentShaderId);

            // Vereine beide Shader-Teile in einem gemeinsamem Renderprogramm (Shader):
            GL.LinkProgram(_shaderId);

            // Zum Schluss wird in den folgenden 9 Befehlen die Verbindung zwischen CPU und GPU
            // gesetzt. GL.GetUniformLocation() ermittelt für ein Programm (erster Parameter),
            // wo auf der GPU die dazugehörige Variable zu finden ist.
            // In der ersten Zeile wird z.B. nachgeschaut, wo in der GPU nun die Stelle für
            // die Shader-Variable 'uMatrix' liegt. Die Stelle wird in _uniformMatrix gespeichert, 
            // so dass zu späterem Zeitpunkt die Infos an genau diese Stelle hochgeladen werden können:
            _uniformMatrix = GL.GetUniformLocation(_shaderId, "uMatrix");
            _uniformModelMatrix = GL.GetUniformLocation(_shaderId, "uModelMatrix");
            _uniformNormalMatrix = GL.GetUniformLocation(_shaderId, "uNormalMatrix");

            _uniformTexture = GL.GetUniformLocation(_shaderId, "uTexture");
            _uniformTextureNormalMap = GL.GetUniformLocation(_shaderId, "uTextureNormalMap");
            _uniformTextureNormalMapUse = GL.GetUniformLocation(_shaderId, "uTextureNormalMapUse");

            _uniformLightCount = GL.GetUniformLocation(_shaderId, "uLightCount");
            _uniformLightPositions = GL.GetUniformLocation(_shaderId, "uLightPositions");
            _uniformAmbientLight = GL.GetUniformLocation(_shaderId, "uAmbientLight");

        }

        /// <summary>
        /// Zeichnet die aktuelle Szene, indem sie die Lichtdaten, Kamera- und Bildschirmeinstellungen, sowie die aktuell zu zeichnenden Objekte übergeben bekommt.
        /// </summary>
        /// <param name="lightpositions">Lichtdaten (Lichtpositionen [x][y][z][x][y][z]...)</param>
        /// <param name="viewProjectionMatrix">Matrix, die Kameraposition und FOV enthält</param>
        /// <param name="objectList">Array mit den zu zeichnenden Objekten</param>
        public static void Draw(float[] lightpositions, Matrix4 viewProjectionMatrix, GameObject[] objectList)
        {
            // Aktiviere auf der GPU das richtige Renderprogramm:
            GL.UseProgram(GetProgramId());

            // Übertrage Lichtpositionen und die Farbe des Umgebungslichts an die GPU:
            GL.Uniform3(GetLightPositionsId(), lightpositions.Length / 3, lightpositions);
            GL.Uniform1(GetLightCountId(), lightpositions.Length / 3);
            GL.Uniform3(GetAmbientLightId(), 1f, 1f, 1f);

            // Durchlaufe jedes zu zeichnende Objekt:
            foreach (GameObject g in objectList)
            {
                if (g != null)
                {
                    // Erstelle aus Skalierung, Rotation und Position des aktuellen Objekts
                    // eine Matrix:
                    Matrix4 modelMatrix =
                        Matrix4.CreateScale(new Vector3(g.GetScale().X, g.GetScale().Y, 1))
                        * Matrix4.CreateFromQuaternion(g.GetRotation())
                        * Matrix4.CreateTranslation(g.GetPosition().X, g.GetPosition().Y, 0);

                    // Für Lichtreflektionen muss aus der Model-Matrix die Normal-Matrix berechnet werden:
                    Matrix4 normalMatrix = Matrix4.Invert(Matrix4.Transpose(modelMatrix));

                    // Model-View-Projection matrix erstellen (Kombi aus Model-Matrix und View-Projection-Matrix):
                    Matrix4 mvp = modelMatrix * viewProjectionMatrix;

                    // Übertrage die Matrizen an die GPU:
                    GL.UniformMatrix4(GetMatrixId(), false, ref mvp);
                    GL.UniformMatrix4(GetModelMatrixId(), false, ref modelMatrix);
                    GL.UniformMatrix4(GetNormalMatrixId(), false, ref normalMatrix);

                    // Textur an die GPU übertragen:
                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.Texture2D, g.GetTextureId());
                    GL.Uniform1(GetTextureId(), 0);

                    // Normal-Map-Textur an die GPU übertragen:
                    GL.ActiveTexture(TextureUnit.Texture1);
                    int normalMapId = g.GetTextureNormalMap();
                    GL.BindTexture(TextureTarget.Texture2D, normalMapId > 0 ? normalMapId : ApplicationWindow.TextureDefault);
                    GL.Uniform1(GetTextureNormalMapId(), 1);
                    // Die GPU kann nicht entscheiden, ob eine Textur valide ist oder nicht. Daher muss man ihr
                    // für die optionale Normal-Map noch mit 1 oder 0 mitteilen, ob es eine gibt. Der Shader-Code
                    // fragt diesen Wert dann ab, bevor auf diese Textur zugegriffen wird:
                    GL.Uniform1(GetTextureNormalMapUseId(), normalMapId > 0 ? 1 : 0);

                    // Übertrage die Geometriedaten (Eckpunkte, etc.) des Quadrats an die GPU:
                    GL.BindVertexArray(PrimitiveQuad.GetVAOId());
                    GL.DrawArrays(PrimitiveType.Triangles, 0, PrimitiveQuad.GetPointCount());

                    // Teile der GPU mit, dass die Geometrie- und Texturdaten nicht mehr benötigt werden:
                    GL.BindVertexArray(0);
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                }
            }

            // Deaktiviere das aktuell gewählte Renderprogramm (0 ist fast immer eine ungültige ID in OpenGL):
            GL.UseProgram(0);
        }

        /// <summary>
        /// Zeichnet die Objekte um 8 Pixel vergrößert in rot.
        /// </summary>
        /// <param name="viewProjectionMatrix">Matrix, die Kameraposition und FOV enthält</param>
        /// <param name="objectList">Array mit den zu zeichnenden Objekten</param>
        public static void DrawOutline(Matrix4 viewProjectionMatrix, GameObject[] objectList)
        {
            GL.UseProgram(GetProgramId());

            GL.Uniform3(GetLightPositionsId(), 0, _dummyPositions);
            GL.Uniform1(GetLightCountId(), 0);
            GL.Uniform3(GetAmbientLightId(), 1f, 1f, 1f);

            // Durchlaufe jedes zu zeichnende Objekt:
            foreach (GameObject g in objectList)
            {
                if (g != null && g.IsCollisionCandidate() == true)
                {
                    Matrix4 modelMatrix =
                        Matrix4.CreateScale(new Vector3(g.GetScale().X + 8, g.GetScale().Y + 8, 1))
                        * Matrix4.CreateFromQuaternion(g.GetRotation())
                        * Matrix4.CreateTranslation(g.GetPosition().X, g.GetPosition().Y, 0);

                    Matrix4 normalMatrix = Matrix4.Identity;
                    Matrix4 mvp = modelMatrix * viewProjectionMatrix;

                    GL.UniformMatrix4(GetMatrixId(), false, ref mvp);
                    GL.UniformMatrix4(GetModelMatrixId(), false, ref modelMatrix);
                    GL.UniformMatrix4(GetNormalMatrixId(), false, ref normalMatrix);

                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.Texture2D, _textureIdColorOutline);
                    GL.Uniform1(GetTextureId(), 0);

                    GL.ActiveTexture(TextureUnit.Texture1);
                    int normalMapId = g.GetTextureNormalMap();
                    GL.BindTexture(TextureTarget.Texture2D, normalMapId > 0 ? normalMapId : ApplicationWindow.TextureDefault);
                    GL.Uniform1(GetTextureNormalMapId(), 1);
                    GL.Uniform1(GetTextureNormalMapUseId(), 0);

                    GL.BindVertexArray(PrimitiveQuad.GetVAOId());
                    GL.DrawArrays(PrimitiveType.Triangles, 0, PrimitiveQuad.GetPointCount());

                    GL.BindVertexArray(0);
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                }
            }

            // Deaktiviere das aktuell gewählte Renderprogramm (0 ist fast immer eine ungültige ID in OpenGL):
            GL.UseProgram(0);
        }

        public static int GetProgramId()
        {
            return _shaderId;
        }

        public static int GetMatrixId()
        {
            return _uniformMatrix;
        }

        public static int GetModelMatrixId()
        {
            return _uniformModelMatrix;
        }

        public static int GetNormalMatrixId()
        {
            return _uniformNormalMatrix;
        }

        public static int GetTextureId()
        {
            return _uniformTexture;
        }

        public static int GetLightCountId()
        {
            return _uniformLightCount;
        }

        public static int GetLightPositionsId()
        {
            return _uniformLightPositions;
        }

        public static int GetAmbientLightId()
        {
            return _uniformAmbientLight;
        }

        public static int GetTextureNormalMapId()
        {
            return _uniformTextureNormalMap;
        }

        public static int GetTextureNormalMapUseId()
        {
            return _uniformTextureNormalMapUse;
        }
    }
}
