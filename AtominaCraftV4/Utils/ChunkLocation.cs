using AtominaCraftV4.Worlding.Chunking;

namespace AtominaCraftV4.Utils {
    /// <summary>
    /// The coordinates of a chunk
    /// </summary>
    public readonly struct ChunkLocation {
        public readonly int x;
        public readonly int z;

        public ChunkLocation(int x, int z) {
            this.x = x;
            this.z = z;
        }

        /// <summary>
        /// Converts a block location (within a chunk) into a world coordinate, based on this chunk's coordinates
        /// </summary>
        /// <param name="chunkCoord">The coordinate of a block within a chunk</param>
        /// <returns></returns>
        public BlockWorldCoord ToWorldCoord(in BlockChunkCoord chunkCoord) {
            return new BlockWorldCoord((this.x << 4) + chunkCoord.x, chunkCoord.y, (this.z << 4) + chunkCoord.z);
        }

        /// <summary>
        /// Converts a block location (within a chunk) into a world coordinate, based on this chunk's coordinates
        /// </summary>
        /// <param name="x">Block X within a chunk (0-15)</param>
        /// <param name="y">Block Y coordinate (0-255)</param>
        /// <param name="z">Block Z within a chunk (0-15)</param>
        /// <returns></returns>
        public BlockWorldCoord ToWorldCoord(int x, int y, int z) {
            return new BlockWorldCoord((this.x << 4) + x, y, (this.z << 4) + z);
        }

        public override bool Equals(object obj) {
            return obj is ChunkLocation coord && coord.x == this.x && coord.z == this.z;
        }

        public override int GetHashCode() {
            return this.x + (this.z << 15);
        }

        public long GetLongHash() {
            return Chunk.HashI64(this.x, this.z);
        }

        public override string ToString() {
            return $"ChunkLocation({this.x}, {this.z} ({this.x << 4}, {this.z << 4}))";
        }
    }
}