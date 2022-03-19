using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
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

        public BlockState GetBlockAt(int x, int y, int z) {
            return GetChunkAt(x >> 4, z >> 4).GetBlockAt(x, y, z);
        }

        public BlockState GetBlockAt(in BlockWorldCoord coord) {
            return GetChunkAt(coord.x >> 4, coord.z >> 4).GetBlockAt(coord.x, coord.y, coord.z);
        }

        public bool TryGetBlockAt(int x, int y, int z, out BlockState state) {
            return GetChunkAt(x >> 4, z >> 4).TryGetBlockAt(x, y, z, out state);
        }

        /// <summary>
        /// Tries to get a block at the specific world coordinates
        /// </summary>
        /// <returns>
        /// True if a block existed at those coordinates, otherwise false
        /// </returns>
        public bool TryGetBlockAt(in BlockWorldCoord coord, out BlockState state) {
            return GetChunkAt(coord.x >> 4, coord.z >> 4).TryGetBlockAt(coord.x, coord.y, coord.z, out state);
        }

        public void SetBlockAt(int x, int y, int z, int id, int meta = 0, int visibility = 0b111111) {
            GetChunkAt(x >> 4, z >> 4).SetBlockIdMeta(x, y, z, id, meta, visibility);
        }

        public void OnBlockChanged(int x, int y, int z) {
            UpdateAllBlockFaces(x, y, z);
            UpdateOppositeFaceVisibility(x, y, z, Direction.UP);
            UpdateOppositeFaceVisibility(x, y, z, Direction.DOWN);
            UpdateOppositeFaceVisibility(x, y, z, Direction.NORTH);
            UpdateOppositeFaceVisibility(x, y, z, Direction.EAST);
            UpdateOppositeFaceVisibility(x, y, z, Direction.SOUTH);
            UpdateOppositeFaceVisibility(x, y, z, Direction.WEST);
        }

        public void UpdateOppositeFaceVisibility(int x, int y, int z, Direction direction) {
            UpdateFaceVisibility(x + direction.x, y + direction.y, z + direction.z, direction.Opposite);
        }

        /// <summary>
        /// Fully re-calculates the block's face visibilities at the given coords
        /// </summary>
        public void UpdateAllBlockFaces(int x, int y, int z) {
            UpdateFaceVisibility(x, y, z, Direction.UP);
            UpdateFaceVisibility(x, y, z, Direction.DOWN);
            UpdateFaceVisibility(x, y, z, Direction.NORTH);
            UpdateFaceVisibility(x, y, z, Direction.EAST);
            UpdateFaceVisibility(x, y, z, Direction.SOUTH);
            UpdateFaceVisibility(x, y, z, Direction.WEST);
        }

        /// <summary>
        /// Recalculates a block's visibily in a specific direction
        /// </summary>
        public void UpdateFaceVisibility(int x, int y, int z, Direction direction) {
            if (y < 0 || y > 255) {
                return;
            }

            int oX = x + direction.x;
            int oY = y + direction.y;
            int oZ = z + direction.z;
            bool visible;
            if (TryGetChunk(x >> 4, z >> 4, out Chunk chunk)) {
                BlockState state;
                if (TryGetBlockAt(x, y, z, out state)) {
                    Block block = Block.Blocks[state.id];
                    if (block == null) {
                        return;
                    }

                    visible = block.IsFaceVisible(this, x, y, z, direction);
                }
                else {
                    visible = true;
                }
            }
            else {
                visible = true;
            }

            chunk.storage.UpdateBlockVisibility(x & 15, y & 255, z & 15, visible, direction);
        }

        public bool IsChunkLoaded(int x, int z) {
            return this.chunks.ContainsKey(Chunk.HashI64(x, z));
        }

        public Chunk GetChunkAt(int x, int z) {
            return this.chunks.TryGet(Chunk.HashI64(x, z), out Chunk chunk) ? chunk : LoadChunk(x, z);
        }

        public bool TryGetChunk(int x, int z, out Chunk loadedChunk) {
            return this.chunks.TryGet(Chunk.HashI64(x, z), out loadedChunk);
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

        private static void CheckY(int y) {
            if (y < 0) {
                throw new ArgumentOutOfRangeException(nameof(y), y, "Y cannot be below 0");
            }
            else if (y > 255) {
                throw new ArgumentOutOfRangeException(nameof(y), y, "Y cannot be above 255");
            }
        }
    }
}