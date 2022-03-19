using System.Runtime.CompilerServices;

namespace AtominaCraftV4.REghZy.MathsF {
    public static class Bits {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int FloatBitsToI32(float value) {
            return *(int*) &value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe float I32ToFloatBits(int value) {
            return *(float*) &value;
        }
    }
}