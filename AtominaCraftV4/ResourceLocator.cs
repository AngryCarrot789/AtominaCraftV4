using System;
using System.IO;

namespace AtominaCraftV4 {
    public class ResourceLocator {
        private static readonly string assetFolderPath;

        public static string AssetFolderPath => assetFolderPath;

        public static string TextureFolderPath => Path.Combine(assetFolderPath, @"Textures");

        public static string ShaderFolderPath => Path.Combine(assetFolderPath, @"Shaders");

        static ResourceLocator() {
            if ((assetFolderPath = FindDirectory(Directory.GetCurrentDirectory(), "Assets")) == null) {
                throw new Exception("Failed to find resources directory");
            }
        }

        private static string FindDirectory(string path, string directoryName) {
            if (path == null) {
                return null;
            }

            DirectoryInfo info = new DirectoryInfo(path);
            foreach (DirectoryInfo directory in info.EnumerateDirectories()) {
                if (directory.Name == directoryName) {
                    return directory.FullName;
                }
            }

            DirectoryInfo parent = info.Parent;
            if (parent == null) {
                return null;
            }

            return FindDirectory(parent.FullName, directoryName);
        }

        public static string GetAssetPath(string asset) {
            return Path.Combine(assetFolderPath, asset);
        }
    }
}