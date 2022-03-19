using System.Runtime.CompilerServices;
using AtominaCraftV4.Utils;
using AtominaCraftV4.Worlding;
using AtominaCraftV4.Worlding.Chunking;

namespace AtominaCraftV4.Blocks {
    public class Block {
        public static readonly Block[] Blocks = new Block[4096];

        public static readonly Block DIRT;
        public static readonly Block GRASS;
        public static readonly Block ELECTROMAGNET;
        public static readonly Block WOOD_PLANK;
        public static readonly Block STEEL;

        public readonly int id;
        public int topTexture;
        public int bottomTexture;
        public int northTexture;
        public int eastTexture;
        public int southTexture;
        public int westTexture;

        public string displayName;

        public bool isTransparent;

        public int SidesTextureId {
            set => this.northTexture = this.eastTexture = this.southTexture = this.westTexture = value;
        }

        public int TopBottomTextureId {
            set => this.topTexture = this.bottomTexture = value;
        }

        public bool AllTextureAreTheSame {
            get {
                int a = this.topTexture;
                return a == this.bottomTexture && a == this.northTexture && a == this.eastTexture && a == this.southTexture && a == this.westTexture;
            }
        }

        public Block(int id) {
            this.id = id;
        }

        static Block() {
            Blocks[0] = null;
            Blocks[1] = DIRT = new Block(1).SetName("Dirt").SetTextureAll(1);
            Blocks[2] = GRASS = new Block(2).SetName("Grass").SetTextureSides(2).SetTopTexture(1).SetBottomTexture(1);
            Blocks[3] = ELECTROMAGNET = new Block(3).SetName("Electromagnet").SetTextureAll(3);
            Blocks[4] = WOOD_PLANK = new Block(4).SetName("Wood Planks").SetTextureAll(4);
            Blocks[5] = STEEL = new Block(5).SetName("Steel").SetTextureAll(5);
        }

        public Block SetTextureAll(int textureId) {
            this.topTexture = this.bottomTexture = this.northTexture = this.eastTexture = this.southTexture = this.westTexture = textureId;
            return this;
        }

        public Block SetTextureSides(int textureId) {
            this.northTexture = this.eastTexture = this.southTexture = this.westTexture = textureId;
            return this;
        }

        public Block SetTopTexture(int textureId) {
            this.topTexture = textureId;
            return this;
        }

        public Block SetBottomTexture(int i) {
            this.bottomTexture = i;
            return this;
        }

        public Block SetName(string dirt) {
            this.displayName = dirt;
            return this;
        }

        /// <summary>
        /// Whether this block's face (in the given normal direction) is visible
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual bool IsFaceVisible(World world, int x, int y, int z, Direction direction) {
            if (world.TryGetBlockAt(x + direction.x, y + direction.y, z + direction.z, out BlockState state)) {
                Block block = Blocks[state.id];
                return block == null || block.isTransparent;
                // return block == null || !block.CanObscureOppositeBlock(world, x + direction.x, y + direction.y, z + direction.z, direction.Opposite);
            }

            return true;
        }

        /// <summary>
        /// Whether this block can obscure the face of another block (offset by direction)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual bool CanObscureOppositeBlock(World world, int x, int y, int z, Direction direction) {
            if (this.isTransparent) {
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual bool CanRender(World world, in BlockWorldCoord coord) {
            return true;
        }
    }
}