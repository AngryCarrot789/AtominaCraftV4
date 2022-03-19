using AtominaCraftV4.Utils;
using REghZy.Collections.Dictionaries;

namespace AtominaCraftV4.Worlding.Chunking {
    public class Chunk {
        public const int CHUNK_WIDTH = 16;
        public const int CHUNK_HEIGHT = 16;
        public const int CHUNK_WIDTH_BITS = 0b1111;
        public const int CHUNK_HEIGHT_BITS = 0b1111;

        public readonly ChunkBlockStorage storage;
        public readonly World world;
        public readonly int x;
        public readonly int z;
        public readonly ChunkLocation location;

        public Chunk(World world, int x, int z) {
            this.storage = new ChunkBlockStorage(this);
            this.world = world;
            this.x = x;
            this.z = z;
            this.location = new ChunkLocation(x, z);
        }

        public BlockData GetBlockAt(int x, int y, int z) {
            return this.storage.GetBlockAt(x & 15, y & 255, z & 15);
        }

        public BlockData SetBlockIdMeta(int x, int y, int z, int id, int meta, int visibility = 0b111111) {
            x &= 15;
            z &= 15;
            BlockData data = this.storage.SetBlockAt(x, y & 255, z, id, meta, visibility);
            this.world.DoBlockUpdateVisibility((this.x << 4) + x, y & 255, (this.z << 4) + z);
            return data;
        }

        public BlockData SetBlockId(int x, int y, int z, int id, int visibility = 0b111111) {
            x &= 15;
            z &= 15;
            BlockData data = this.storage.SetBlockAt(x, y & 255, z, id, 0, visibility);
            this.world.DoBlockUpdateVisibility((this.x << 4) + x, y & 255, (this.z << 4) + z);
            return data;
        }

        public bool IsBlockSpaceEmpty(int x, int y, int z) {
            return this.storage.IsEmpty(x & 15, y & 255, z & 15);
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