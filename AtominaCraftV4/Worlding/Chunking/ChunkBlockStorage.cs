using System;
using AtominaCraftV4.Utils;

namespace AtominaCraftV4.Worlding.Chunking {
    /// <summary>
    /// A class for storing block data within chunks
    /// </summary>
    public class ChunkBlockStorage {
        public readonly StorageSection[] sections;
        public readonly Chunk chunk;

        public ChunkBlockStorage(Chunk chunk) {
            this.sections = new StorageSection[16];
            this.chunk = chunk;
        }

        public StorageSection this[int index] {
            get => this.sections[index >> 4];
        }

        public BlockData GetBlockAt(int x, int y, int z) {
            return this.sections[y >> 4]?.GetBlockAt(x, y, z) ?? default;
        }

        public BlockData SetBlockAt(int x, int y, int z, int id, int meta, int visibility = 0b111111) {
            return (this.sections[y >> 4] ?? (this.sections[y >> 4] = new StorageSection())).SetBlockAt(x, y, z, id, meta, visibility);
        }

        public BlockData SetBlockAt(int x, int y, int z, BlockData data) {
            return (this.sections[y >> 4] ?? (this.sections[y >> 4] = new StorageSection())).SetBlockAt(x, y, z, data);
        }

        public void UpdateBlockVisibility(int x, int y, int z, int visibility, VisibilityUpdateFlag flag) {
            (this.sections[y >> 4] ?? (this.sections[y >> 4] = new StorageSection())).UpdateBlockVisibility(x, y, z, visibility, flag);
        }

        public void UpdateBlockVisibility(int x, int y, int z, bool visibility, Direction direction) {
            (this.sections[y >> 4] ?? (this.sections[y >> 4] = new StorageSection())).UpdateBlockVisibility(x, y, z, visibility, direction);
        }

        public bool IsEmpty(int x, int y, int z) {
            StorageSection section = this.sections[y >> 4];
            return section == null || section.IsEmpty(x, y, z);
        }

        /// <summary>
        /// A 16x16x16 section of a chunk
        /// </summary>
        public class StorageSection {
            public readonly BlockData[][] layers;

            public StorageSection() {
                this.layers = new BlockData[16][];
            }

            public BlockData GetBlockAt(int x, int y, int z) {
                return this.layers[y & 15]?[x + (z << 4)] ?? default;
            }

            public BlockData SetBlockAt(int x, int y, int z, int id, int meta, int visibility = 0b111111) {
                return (this.layers[y & 15] ?? (this.layers[y & 15] = new BlockData[256]))[x + (z << 4)] = new BlockData(id, meta, visibility);
            }

            public BlockData SetBlockAt(int x, int y, int z, BlockData data) {
                return (this.layers[y & 15] ?? (this.layers[y & 15] = new BlockData[256]))[x + (z << 4)] = data;
            }

            public void UpdateBlockVisibility(int x, int y, int z, int visibility, VisibilityUpdateFlag flag) {
                BlockData data = GetBlockAt(x, y, z);
                switch (flag) {
                    case VisibilityUpdateFlag.ADD: SetBlockAt(x, y, z, data.id, data.meta, data.visibility | visibility);
                        break;
                    case VisibilityUpdateFlag.SUB: SetBlockAt(x, y, z, data.id, data.meta, data.visibility & ~visibility);
                        break;
                    case VisibilityUpdateFlag.SET: SetBlockAt(x, y, z, data.id, data.meta, visibility);
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(flag), flag, null);
                }
            }

            public void UpdateBlockVisibility(int x, int y, int z, bool visibility, Direction direction) {
                UpdateBlockVisibility(x, y, z, direction.BitMask, visibility ? VisibilityUpdateFlag.ADD : VisibilityUpdateFlag.SUB);
            }

            public bool IsEmpty(int x, int y, int z) {
                BlockData[] layer = this.layers[y & 15];
                return layer == null || layer[x + (z << 4)].id == 0;
            }
        }
    }
}