using System;
using System.Collections.Generic;
using System.Reflection;
using OpenTK.Graphics.OpenGL4;
using System.IO;
using OpenTK.Mathematics;
using OpenGLTutorial.GameCore;
using OpenGLTutorial.OpenGLCore;
using OpenGLTutorial.OpenGLCore.Primitives;

namespace OpenGLTutorial.ShaderProgram
{
    public static class ShaderStandard
    {
        private static int _shaderId = -1;

        private static int _vertexShaderId = -1;
        private static int _fragmentShaderId = -1;

        private static int _uniformMatrix = -1;
        private static int _uniformModelMatrix = -1;
        private static int _uniformNormalMatrix = -1;

        private static int _uniformTexture = -1;
        private static int _uniformTextureNormalMap = -1;
        private static int _uniformTextureNormalMapUse = -1;

        private static int _uniformLightCount = -1;
        private static int _uniformLightPositions = -1;
        private static int _uniformAmbientLight = -1;

        /// <summary>
        /// Initialisiert das Hauptrenderprogramm (Shader)
        /// </summary>
        public static void Init()
        {
            _shaderId = GL.CreateProgram();

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

            _vertexShaderId = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(_vertexShaderId, sVertexCode);

            _fragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(_fragmentShaderId, sFragmentCode);

            GL.CompileShader(_vertexShaderId);
            GL.AttachShader(_shaderId, _vertexShaderId);

            GL.CompileShader(_fragmentShaderId);
            GL.AttachShader(_shaderId, _fragmentShaderId);

            GL.LinkProgram(_shaderId);

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
