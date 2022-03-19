using AtominaCraftV4.Blocks;
using AtominaCraftV4.Worlding;
using AtominaCraftV4.Worlding.Chunking;

namespace AtominaCraftV4.Utils {
    public class Direction {
        public static readonly Direction UP;
        public static readonly Direction DOWN;
        public static readonly Direction NORTH;
        public static readonly Direction EAST;
        public static readonly Direction SOUTH;
        public static readonly Direction WEST;
        public static readonly Direction UNKNOWN;

        public static readonly Direction[] AllDirections;
        public static readonly Direction[] ValidDirections;
        public static readonly Direction[] AllOpposites;
        public static readonly Direction[] ValidOpposites;

        public readonly int BitMask;
        public readonly int Index;
        public readonly int x;
        public readonly int y;
        public readonly int z;
        public readonly BlockChunkCoord ChunkCoord;
        public readonly BlockWorldCoord WorldCoord;

        public Direction Opposite { get; private set; }

        static Direction() {
            UP      = new Direction(0b0000001, 0,  0,  1,  0);
            DOWN    = new Direction(0b0000010, 1,  0, -1,  0);
            NORTH   = new Direction(0b0000100, 2,  0,  0, -1);
            EAST    = new Direction(0b0001000, 3,  1,  0,  0);
            SOUTH   = new Direction(0b0010000, 4,  0,  0,  1);
            WEST    = new Direction(0b0100000, 5, -1,  0,  0);
            UNKNOWN = new Direction(0b1000000, 6,  0,  0,  0);
            AllDirections   = new Direction[] { UP,   DOWN, NORTH, EAST, SOUTH, WEST, UNKNOWN };
            ValidDirections = new Direction[] { UP,   DOWN, NORTH, EAST, SOUTH, WEST };
            AllOpposites    = new Direction[] { DOWN, UP,   SOUTH, WEST, NORTH, EAST, UNKNOWN };
            ValidOpposites  = new Direction[] { DOWN, UP,   SOUTH, WEST, NORTH, EAST };

            DOWN.Opposite = UP;
            UP.Opposite = DOWN;
            NORTH.Opposite = SOUTH;
            SOUTH.Opposite = NORTH;
            WEST.Opposite = EAST;
            EAST.Opposite = WEST;
            UNKNOWN.Opposite = UNKNOWN;
        }

        public static int operator +(Direction a, Direction b) {
            return a.BitMask | b.BitMask;
        }

        public static int operator +(int a, Direction b) {
            return a | b.BitMask;
        }

        public static int operator +(Direction a, int b) {
            return a.BitMask | b;
        }

        public static int operator -(int a, Direction b) {
            return a & ~b.BitMask;
        }

        public BlockData GetBlockAt(World world, int x, int y, int z) {
            return world.GetBlockAt(this.x + x, this.y + y, this.z + z);
        }

        public BlockData GetBlockAt(World world, BlockWorldCoord coord) {
            return world.GetBlockAt(this.x + coord.x, this.y + coord.y, this.z + coord.z);
        }

        private Direction(int bitMask, int index, int x, int y, int z) {
            this.BitMask = bitMask;
            this.Index = index;
            this.x = x;
            this.y = y;
            this.z = z;
            this.ChunkCoord = new BlockChunkCoord(x, y, z);
            this.WorldCoord = new BlockWorldCoord(x, y, z);
        }
    }
}