using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AtominaCraftV4.Worlding.Chunking {
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct BlockState {
        public readonly int id;
        public readonly int meta;

        // Private so that a bit can be used as a cheap "nullable" alternative
        private readonly int visibility;

        public bool IsEmpty => this.id == 0;

        public bool TopVisible => (this.visibility & 0b000001) != 0;

        public bool BottomVisible => (this.visibility & 0b000010) != 0;

        public bool NorthVisible => (this.visibility & 0b000100) != 0;

        public bool SouthVisible => (this.visibility & 0b001000) != 0;

        public bool EastVisible => (this.visibility & 0b010000) != 0;

        public bool WestVisible => (this.visibility & 0b100000) != 0;

        public bool AllFacesVisible => (this.visibility & 0b111111) == 0b111111;

        public int Visibility => this.visibility & 0b111111;

        public bool Real {
            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            get => (this.visibility & 0b1000000) != 0;
        }

        public BlockState(int id, int meta, int visibility = 0b111111) {
            this.id = id;
            this.meta = meta;
            this.visibility = (0b111111 & visibility) | 0b1000000;
        }
    }
}