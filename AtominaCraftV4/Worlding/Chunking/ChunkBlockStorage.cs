using System;
using System.Runtime.CompilerServices;
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

        public bool TryGetBlockAt(int x, int y, int z, out BlockState state) {
            StorageSection section = this.sections[y >> 4];
            if (section == null) {
                state = default;
                return false;
            }

            return section.TryGetBlockAt(x, y & 15, z, out state);
        }

        public BlockState GetBlockAt(int x, int y, int z) {
            StorageSection section = this.sections[y >> 4];
            if (section == null) {
                return default;
            }
            
            return section.GetBlockAt(x, y & 15, z);
        }

        public BlockState SetBlockAt(int x, int y, int z, int id, int meta, int visibility = 0b111111) {
            return (this.sections[y >> 4] ?? (this.sections[y >> 4] = new StorageSection())).SetBlockAt(x, y & 15, z, id, meta, visibility);
        }

        public void UpdateBlockVisibility(int x, int y, int z, int visibility, VisibilityUpdateFlag flag) {
            (this.sections[y >> 4] ?? (this.sections[y >> 4] = new StorageSection())).UpdateBlockVisibility(x, y & 15, z, visibility, flag);
        }

        public void UpdateBlockVisibility(int x, int y, int z, bool visibility, Direction direction) {
            (this.sections[y >> 4] ?? (this.sections[y >> 4] = new StorageSection())).UpdateBlockVisibility(x, y & 15, z, direction, visibility);
        }

        public bool IsEmpty(int x, int y, int z) {
            StorageSection section = this.sections[y >> 4];
            return section == null || section.IsEmpty(x, y & 15, z);
        }

        /// <summary>
        /// A 16x16x16 section of a chunk
        /// </summary>
        public class StorageSection {
            public const int LAYER_COUNT = 16;
            public const int LAYER_SIZE = 256;
            public readonly BlockState[][] layers;

            public StorageSection() {
                this.layers = new BlockState[LAYER_COUNT][];
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public BlockState GetBlockAt(int x, int y, int z) {
                BlockState[] layer = this.layers[y];
                if (layer == null) {
                    return default;
                }

                return layer[x + (z << 4)];
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public bool TryGetBlockAt(int x, int y, int z, out BlockState state) {
                BlockState[] a = this.layers[y];
                if (a == null) {
                    state = default;
                    return false;
                }

                state = a[x + (z << 4)];
                return state.Real;
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public BlockState SetBlockAt(int x, int y, int z, int id, int meta, int visibility = 0b111111) {
                return (this.layers[y] ?? (this.layers[y] = new BlockState[LAYER_SIZE]))[x + (z << 4)] = new BlockState(id, meta, visibility);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public void UpdateBlockVisibility(int x, int y, int z, int visibility, VisibilityUpdateFlag flag) {
                BlockState state = GetBlockAt(x, y, z);
                switch (flag) {
                    case VisibilityUpdateFlag.ADD: SetBlockAt(x, y, z, state.id, state.meta, state.Visibility | visibility);
                        break;
                    case VisibilityUpdateFlag.SUB: SetBlockAt(x, y, z, state.id, state.meta, state.Visibility & ~visibility);
                        break;
                    case VisibilityUpdateFlag.SET: SetBlockAt(x, y, z, state.id, state.meta, visibility);
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(flag), flag, "VisibilityUpdateFlag was invalid");
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public void UpdateBlockVisibility(int x, int y, int z, Direction direction, bool visible) {
                UpdateBlockVisibility(x, y, z, direction.BitMask, visible ? VisibilityUpdateFlag.ADD : VisibilityUpdateFlag.SUB);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public bool IsEmpty(int x, int y, int z) {
                BlockState[] layer = this.layers[y];
                return layer == null || !layer[x + (z << 4)].Real;
            }
        }
    }
}