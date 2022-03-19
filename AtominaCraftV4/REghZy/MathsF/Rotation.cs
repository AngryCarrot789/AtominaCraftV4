namespace AtominaCraftV4.REghZy.MathsF {
    public static class Rotation {
        private const float R2D = 180.0f / Maths.PI;
        private const float D2R = Maths.PI / 180.0f;

        public static float RadiansToDegrees(float radians) => radians * R2D;
        public static float DegreesToRadians(float degrees) => degrees * D2R;
        
        public static float Angle2D(float anchorX, float anchorZ, float pointX, float pointZ) {
            if (anchorX == anchorZ) {
                if (pointZ > anchorZ) {
                    return 180;
                }
                else if (pointZ < anchorZ) {
                    return 0;
                }
                else {
                    return 0; // throw exception maybe?
                }
            }
            else if (pointZ == anchorZ) {
                if (pointX > anchorX) {
                    return 90;
                }
                else if (pointX < anchorX) {
                    return 270;
                }
                else {
                    return 0; // throw exception maybe?
                }
            }
            else {
                float theta = FMath.Atan2(pointX - anchorX, -(pointZ - anchorZ));
                if (theta < 0.0) {
                    theta += Maths.PI_DOUBLE;
                }

                return R2D * theta;
            }
        }

        public static float Angle2DFast(float anchorX, float anchorZ, float pointX, float pointZ) {
            float theta = FMath.Atan2(pointX - anchorX, -(pointZ - anchorZ));
            if (theta < 0.0) {
                theta += Maths.PI_DOUBLE;
            }

            return R2D * theta;
        }

        public static float opposite(float angle) {
            return angle < 180.0f ? angle + 180.0f : angle - 180.0f;
        }

        public static float dist(float x1, float z1, float x2, float z2) {
            return FMath.Sqrt(FMath.Pow(x2 - x1, 2) + FMath.Pow(z2 - z1, 2));
        }
    }
}