using AtominaCraftV4.Worlding.Chunking;

namespace AtominaCraftV4.Utils {
    /// <summary>
    /// The coordinate of a block within a chunk, NOT WORLD COORDINATES
    /// </summary>
    public readonly struct BlockChunkCoord {
        public readonly int x;
        public readonly int y;
        public readonly int z;

        public BlockChunkCoord(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public BlockWorldCoord ToWorldCoord(Chunk chunk) {
            return chunk.location.ToWorldCoord(this);
        }

        public BlockChunkCoord Translate(Direction direction) {
            return this + direction.ChunkCoord;
        }

        public static BlockChunkCoord operator +(in BlockChunkCoord a, in BlockChunkCoord b) {
            return new BlockChunkCoord(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static BlockChunkCoord operator +(in BlockChunkCoord a, Direction direction) {
            return a + direction.ChunkCoord;
        }

        public static BlockChunkCoord operator -(in BlockChunkCoord a, in BlockChunkCoord b) {
            return new BlockChunkCoord(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static BlockChunkCoord operator -(in BlockChunkCoord a, Direction direction) {
            return a - direction.ChunkCoord;
        }

        public static BlockChunkCoord operator *(in BlockChunkCoord a, in BlockChunkCoord b) {
            return new BlockChunkCoord(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static BlockChunkCoord operator /(in BlockChunkCoord a, in BlockChunkCoord b) {
            return new BlockChunkCoord(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        /// <summary>
        /// Whether the object is equal to this block coordinate
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            return obj is BlockChunkCoord coord && coord.GetHashCode() == GetHashCode();
        }

        /// <summary>
        /// Hash code of a chunk block coordinate
        /// <para>
        /// Byte layout: yyyy yyyy zzzz xxxx (16 bits, 2 bytes, fills an unsigned short)
        /// </para>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return this.x | (this.z << 4) | (this.y << 8);
        }

        public override string ToString() {
            return $"BlockChunkCoord({this.x}, {this.y}, {this.z})";
        }
    }
}