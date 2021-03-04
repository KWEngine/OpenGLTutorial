using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLTutorial.Textures
{
    public static class TextureLoader
    {
        public static int LoadTexture(string file)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Stream s = a.GetManifestResourceStream(file);

            Bitmap b = new Bitmap(s);
            if(b == null)
            {
                return -1; // Fehlerfall
            }
            else
            {
                BitmapData bmd = b.LockBits(
                    new Rectangle(0, 0, b.Width, b.Height), 
                    ImageLockMode.ReadOnly, 
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb
                    );

                int textureId = GL.GenTexture();

                GL.BindTexture(TextureTarget.Texture2D, textureId);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, 
                    b.Width, b.Height, 0, 
                    OpenTK.Graphics.OpenGL4.PixelFormat.Bgr, PixelType.UnsignedByte, bmd.Scan0);

                GL.TexParameter(
                    TextureTarget.Texture2D, 
                    (TextureParameterName)OpenTK.Graphics.OpenGL.ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, 
                    8
                    );
                GL.TexParameter(
                    TextureTarget.Texture2D, 
                    TextureParameterName.TextureMinFilter, 
                    (int)TextureMinFilter.LinearMipmapLinear
                    );
                GL.TexParameter(
                    TextureTarget.Texture2D, 
                    TextureParameterName.TextureMagFilter, 
                    (int)TextureMagFilter.Linear
                    );
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

                GL.BindTexture(TextureTarget.Texture2D, 0);

                b.Dispose();
                s.Close();

                return textureId;
            }
        }
    }
}
