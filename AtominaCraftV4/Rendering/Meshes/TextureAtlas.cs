using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace AtominaCraftV4.Rendering.Meshes {
    public static class TextureAtlas {
        private static int textureId;
        
        static TextureAtlas() {

        }

        public static void Load() {
            string path = Path.Combine(ResourceLocator.TextureFolderPath, "texture-atlas.png");
            if (!File.Exists(path)) {
                throw new FileNotFoundException("File does not exist", path);
            }

            Bitmap bitmap = new Bitmap(path);
            BitmapData data = bitmap.LockBits(
                new Rectangle(
                    0,
                    0,
                    bitmap.Width,
                    bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            try {
                LoadAtlas(data);
            }
            catch (Exception e) {
                throw new Exception("Failed to load texture atlas", e);
            }

            bitmap.UnlockBits(data);
            bitmap.Dispose();
        }

        private static void LoadAtlas(BitmapData data) {
            // 2048x2048 image,
            // 64x64 textures,
            // 32 cols and rows
            textureId = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            // GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            // GL.GenerateTextureMipmap(textureId);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                data.Width,
                data.Height,
                0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMinFilter.Nearest);
            // GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
            // GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
        }

        public static void Use() {
            GL.BindTexture(TextureTarget.Texture2D, textureId);
        }
    }
}