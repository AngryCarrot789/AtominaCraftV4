using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace AtominaCraftV4.Rendering.Meshes {
    public class Texture : IDisposable {
        public bool Is3D;

        public static readonly Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        static Texture() {
            textures["checker_gray"] = Load(Path.Combine(ResourceLocator.TextureFolderPath, "checker_gray.bmp"), 1, 1);
            textures["checker_green"] = Load(Path.Combine(ResourceLocator.TextureFolderPath, "checker_green.bmp"), 1, 1);
            textures["dirt"] = Load(Path.Combine(ResourceLocator.TextureFolderPath, "dirt.png"), 1, 1);
            textures["electromagnet"] = Load(Path.Combine(ResourceLocator.TextureFolderPath, "electromagnet.png"), 1, 1);
            textures["gold"] = Load(Path.Combine(ResourceLocator.TextureFolderPath, "gold.bmp"), 1, 1);
            textures["grass_side"] = Load(Path.Combine(ResourceLocator.TextureFolderPath, "grass_side.png"), 1, 1);
            textures["white"] = Load(Path.Combine(ResourceLocator.TextureFolderPath, "white.bmp"), 1, 1);
            textures["floorplan"] = Load(Path.Combine(ResourceLocator.TextureFolderPath, "floorplan_textures.bmp"), 4, 4);
        }

        /// <summary>
        ///     ID to the location of the texture in video memory
        /// </summary>
        public int TextureID;

        public static Texture Load(string path, int rows, int columns) {
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

            Texture texture = new Texture(data, rows, columns);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            return texture;
        }

        public Texture(BitmapData data, int rows, int columns) {
            this.Is3D = rows > 1 || columns > 1;
            this.TextureID = GL.GenTexture();
            if (this.Is3D) {
                GL.BindTexture(TextureTarget.Texture2DArray, this.TextureID);
                GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.LinearMipmapNearest);
                GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (int) TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.GenerateMipmap, 1);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                GL.TexImage3D(
                    TextureTarget.Texture2DArray,
                    0,
                    PixelInternalFormat.Rgba,
                    data.Width / rows,
                    data.Height / columns,
                    rows * columns,
                    0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0);
            }
            else {
                GL.BindTexture(TextureTarget.Texture2D, this.TextureID);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
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
            }
        }

        public void Dispose() {
            GL.DeleteTexture(this.TextureID);
        }

        public void Use() {
            if (this.Is3D) {
                GL.BindTexture(TextureTarget.Texture2DArray, this.TextureID);
            }
            else {
                GL.BindTexture(TextureTarget.Texture2D, this.TextureID);
            }
        }
    }
}