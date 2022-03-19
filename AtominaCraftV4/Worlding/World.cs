using System.Collections.Generic;
using AtominaCraftV4.Blocks;
using AtominaCraftV4.Entities;
using AtominaCraftV4.Utils;
using AtominaCraftV4.Worlding.Chunking;
using OpenTK.Windowing.GraphicsLibraryFramework;
using REghZy.Collections.Dictionaries;

namespace AtominaCraftV4.Worlding {
    public class World {
        public readonly string name;

        public readonly LongKeyDictionary<Chunk> chunks;

        public readonly List<Entity> entities;

        public World(string name) {
            this.name = name;
            this.chunks = new LongKeyDictionary<Chunk>();
            this.entities = new List<Entity>();
        }

        public void Update() {
            foreach (Entity entity in this.entities) {
                entity.Update();
            }
        }

        public BlockData GetBlockAt(int x, int y, int z) {
            return GetChunkAt(x >> 4, z >> 4).GetBlockAt(x, y, z);
        }

        public BlockData GetBlockAt(in BlockWorldCoord coord) {
            return GetChunkAt(coord.x >> 4, coord.z >> 4).GetBlockAt(coord.x, coord.y, coord.z);
        }

        public void SetBlockAt(int x, int y, int z, int id, int meta = 0, int visibility = 0b111111) {
            GetChunkAt(x >> 4, z >> 4).SetBlockIdMeta(x, y, z, id, meta, visibility);
        }

        public void DoBlockUpdateVisibility(int x, int y, int z) {
            UpdateBlockVisibility(x, y, z);
            UpdateBlockVisibility(x, y + 1, z, Direction.DOWN);
            UpdateBlockVisibility(x, y - 1, z, Direction.UP);
            UpdateBlockVisibility(x, y, z - 1, Direction.SOUTH);
            UpdateBlockVisibility(x + 1, y, z, Direction.NORTH);
            UpdateBlockVisibility(x, y, z + 1, Direction.WEST);
            UpdateBlockVisibility(x - 1, y, z, Direction.EAST);
        }

        /// <summary>
        /// Fully re-calculates the block's face visibilities at the given coords
        /// </summary>
        public void UpdateBlockVisibility(int x, int y, int z) {
            UpdateBlockVisibility(x, y, z, Direction.UP);
            UpdateBlockVisibility(x, y, z, Direction.DOWN);
            UpdateBlockVisibility(x, y, z, Direction.NORTH);
            UpdateBlockVisibility(x, y, z, Direction.EAST);
            UpdateBlockVisibility(x, y, z, Direction.SOUTH);
            UpdateBlockVisibility(x, y, z, Direction.WEST);
        }

        /// <summary>
        /// Recalculates a block's visibily in a specific direction
        /// </summary>
        private void UpdateBlockVisibility(int x, int y, int z, Direction direction) {
            bool visible;
            if (GetChunkAt(x >> 4, z >> 4).IsBlockSpaceEmpty(x + direction.x, y + direction.y, z + direction.z)) {
                visible = true;
            }
            else {
                Block block = Block.Blocks[GetBlockAt(x, y, z).id];
                if (block == null) {
                    return;
                }

                visible = block.IsFaceVisible(this, new BlockWorldCoord(x, y, z), direction);
            }

            GetChunkAt(x >> 4, z >> 4).storage.UpdateBlockVisibility(x & 15, y & 255, z & 15, visible, direction);
        }

        public Chunk GetChunkAt(int x, int z) {
            return this.chunks.TryGet(Chunk.HashI64(x, z), out Chunk chunk) ? chunk : LoadChunk(x, z);
        }

        private Chunk LoadChunk(int x, int z) {
            Chunk chunk = this.chunks[Chunk.HashI64(x, z)] = new Chunk(this, x, z);
            chunk.OnLoaded();
            return chunk;
        }

        public bool Equals(World world) {
            return this.name == world.name;
        }

        public override int GetHashCode() {
            return this.name.GetHashCode();
        }

        public override string ToString() {
            return $"World({this.name})";
        }
    }
}