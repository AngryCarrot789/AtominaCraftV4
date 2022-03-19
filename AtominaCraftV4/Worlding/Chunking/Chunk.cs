using System;
using System.Runtime.CompilerServices;
using AtominaCraftV4.Utils;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace AtominaCraftV4.Worlding.Chunking {
    public class Chunk {
        public const int CHUNK_WIDTH = 16;
        public const int CHUNK_HEIGHT = 16;
        public const int BITS_X = 0b1111;
        public const int BITS_Z = 0b1111;

        public readonly World world;

        // this should rarely be used, apart from visibility updates,
        // which should be done internally anyway :)
        public readonly ChunkBlockStorage storage;
        public readonly ChunkLocation location;
        public readonly int x;
        public readonly int z;

        public Chunk(World world, int x, int z) {
            this.world = world;
            this.storage = new ChunkBlockStorage(this);
            this.location = new ChunkLocation(x, z);
            this.x = x;
            this.z = z;
        }

        public BlockState GetBlockAt(int x, int y, int z) {
            if (y < 0 || y > 255) {
                return default;
            }

            return this.storage.GetBlockAt(x & 15, y & 255, z & 15);
        }

        public bool TryGetBlockAt(int x, int y, int z, out BlockState state) {
            if (y < 0 || y > 255) {
                state = default;
                return false;
            }

            return this.storage.TryGetBlockAt(x & 15, y & 255, z & 15, out state);
        }

        public BlockState SetBlockIdMeta(int x, int y, int z, int id, int meta, int visibility = 0b111111) {
            if (y < 0 || y > 255) {
                return default;
            }

            BlockState state = this.storage.SetBlockAt(x & 15, y & 255, z & 15, id, meta, visibility);
            this.world.OnBlockChanged((this.x << 4) + (x & 15), y & 255, (this.z << 4) + (z & 15));
            return state;
        }

        public bool IsBlockSpaceEmpty(int x, int y, int z) {
            if (y < 0 || y > 255) {
                return true;
            }

            return this.storage.IsEmpty(x & 15, y, z & 15);
        }

        public void OnLoaded() {

        }

        public override bool Equals(object obj) {
            return obj == this || (obj is Chunk chunk && chunk.Equals(this));
        }

        public bool Equals(Chunk chunk) {
            return chunk.x == this.x && chunk.z == this.z && chunk.world.Equals(this.world);
        }

        public override int GetHashCode() {
            return this.x + (this.z << 15);
        }

        public static long HashI64(int x, int z) {
            return ((long)x << 32) + (long)z - -2147483648L;
        }

        public override string ToString() {
            return $"Chunk({this.world} -> {this.x}, {this.z})";
        }
    }
}