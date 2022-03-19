using System;

namespace AtominaCraftV4.REghZy.MathsF {
    public struct Quaternion {
        private const float SlerpEpsilon = 1e-6f;

        public static Quaternion Zero =>        new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        public static Quaternion UnitX =>       new Quaternion(1.0f, 0.0f, 0.0f, 0.0f);
        public static Quaternion UnitY =>       new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);
        public static Quaternion UnitZ =>       new Quaternion(0.0f, 0.0f, 1.0f, 0.0f);
        public static Quaternion UnitW =>       new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        public static Quaternion UnitXYZ =>     new Quaternion(1.0f, 1.0f, 1.0f, 0.0f);
        public static Quaternion UnitXYZW =>    new Quaternion(1.0f, 1.0f, 1.0f, 1.0f);

        public float x;
        public float y;
        public float z;
        public float w;

        public Vector3f XYZ {
            get => new Vector3f(this.x, this.y, this.z);
            set {
                this.x = value.x;
                this.y = value.y;
                this.z = value.z;
            }
        }

        public Vector4f XYZW {
            get => new Vector4f(this.x, this.y, this.z, this.w);
            set {
                this.x = value.x;
                this.y = value.y;
                this.z = value.z;
                this.w = value.w;
            }
        }

        public float this[int i] {
            get {
                switch (i) {
                    case 0: return this.x;
                    case 1: return this.y;
                    case 2: return this.z;
                    case 3: return this.w;
                    default: throw new ArgumentOutOfRangeException(nameof(i), "Index must be between 0 and 3");
                }
            }
            set {
                switch (i) {
                    case 0: this.x = value; return;
                    case 1: this.y = value; return;
                    case 2: this.z = value; return;
                    case 3: this.w = value; return;
                    default: throw new ArgumentOutOfRangeException(nameof(i), "Index must be between 0 and 3");
                }
            }
        }

        public Quaternion(float all) {
            this.x = all;
            this.y = all;
            this.z = all;
            this.w = 0.0f;
        }

        public Quaternion(float all, float w) {
            this.x = all;
            this.y = all;
            this.z = all;
            this.w = w;
        }

        public Quaternion(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0.0f;
        }

        public Quaternion(float x, float y, float z, float w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Quaternion(in Quaternion q) {
            this.x = q.x;
            this.y = q.y;
            this.z = q.z;
            this.w = q.w;
        }

        public Quaternion(in Vector3f xyz) {
            this.x = xyz.x;
            this.y = xyz.y;
            this.z = xyz.z;
            this.w = 0.0f;
        }

        public Quaternion(in Vector3f xyz, float w) {
            this.x = xyz.x;
            this.y = xyz.y;
            this.z = xyz.z;
            this.w = w;
        }

        public Quaternion(in Vector4f xyzw) {
            this.x = xyzw.x;
            this.y = xyzw.y;
            this.z = xyzw.z;
            this.w = xyzw.w;
        }

        public static Quaternion AngleAxis(float rotationRadians, in Vector3f axis) {
            return new Quaternion(axis * FMath.Sin(rotationRadians * 0.5f), FMath.Cos(rotationRadians * 0.5f));
        }

        public static Quaternion ToQuaternion(in Vector3f euler) {
            float cy = FMath.Cos(euler.y / 2.0f); // yaw / 2.0f
            float sy = FMath.Sin(euler.y / 2.0f); // yaw / 2.0f
            float cp = FMath.Cos(euler.x / 2.0f); // pitch / 2.0f
            float sp = FMath.Sin(euler.x / 2.0f); // pitch / 2.0f
            float cr = FMath.Cos(euler.z / 2.0f); // roll / 2.0f
            float sr = FMath.Sin(euler.z / 2.0f); // roll / 2.0f
            return new Quaternion(
                sr * cp * cy - cr * sp * sy,
                cr * sp * cy + sr * cp * sy,
                cr * cp * sy - sr * sp * cy,
                cr * cp * cy + sr * sp * sy);
        }

        public float MagnitudeSquared() {
            return this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w;
        }

        public float Magnitude() {
            return FMath.Sqrt(MagnitudeSquared());
        }

        // idk the differences between this... maybe i should learn how they work instead of copying from the internet :)
        // well not exactly copy and paste considering these didn't originally use the operators nor helper functions and stuff... but still

        public Vector3f RotateVector0(in Vector3f coordinate) {
            return (this * new Quaternion(coordinate) * MultiplicativeInverse()).XYZ;
        }

        public Vector3f RotateVector1(in Vector3f v) {
            Vector3f qv = this.XYZ;
            Vector3f t = qv.Cross(v) * 2.0f;
            return v + t * this.w + qv.Cross(t);
        }

        public Quaternion MultiplicativeInverse() {
            return Conjungate() * (1.0f / MagnitudeSquared());
        }

        public Quaternion Normalised() {
            float a = 1.0f / FMath.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w);
            return new Quaternion(this.x * a, this.y * a, this.z * a, this.w * a);
        }

        public Quaternion Conjungate() {
            return new Quaternion(-this.x, -this.y, -this.z, this.w);
        }

        public Vector3f ToEuler() {
            Vector3f angles = new Vector3f();

            // roll (x-axis rotation)
            float sr_cp = 2.0f * (this.w * this.x + this.y * this.z);
            float cr_cp = 1.0f - 2.0f * (this.x * this.x + this.y * this.y);
            angles.z = FMath.Atan2(sr_cp, cr_cp);

            // pitch (y-axis rotation)
            float sp = 2.0f * (this.w * this.y - this.z * this.x);
            if (FMath.Abs(sp) >= 1.0f) {
                // use 90 degrees if out of range
                angles.x = FMath.CopySign(Maths.PI_HALF, sp);
            }
            else {
                angles.x = FMath.Asin(sp);
            }

            // yaw (z-axis rotation)
            float sy_cp = 2.0f * (this.w * this.z + this.x * this.y);
            float cy_cp = 1.0f - 2 * (this.y * this.y + this.z * this.z);
            angles.y = FMath.Atan2(sy_cp, cy_cp);

            return angles;
        }

        /// <summary>
        /// Interpolates between two quaternions, using spherical linear interpolation.
        /// </summary>
        /// <param name="quaternion1">The first source Quaternion.</param>
        /// <param name="quaternion2">The second source Quaternion.</param>
        /// <param name="amount">The relative weight of the second source Quaternion in the interpolation.</param>
        /// <returns>The interpolated Quaternion.</returns>
        public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, float amount) {
            float t = amount;
            float cosOmega = quaternion1.x * quaternion2.x + quaternion1.y * quaternion2.y + quaternion1.z * quaternion2.z + quaternion1.w * quaternion2.w;
            bool flip = false;
            if (cosOmega < 0.0f) {
                flip = true;
                cosOmega = -cosOmega;
            }

            float s1, s2;
            if (cosOmega > (1.0f - SlerpEpsilon)) {
                s1 = 1.0f - t;
                s2 = (flip) ? -t : t;
            }
            else {
                float omega = FMath.Acos(cosOmega);
                float invSinOmega = 1 / FMath.Sin(omega);
                s1 = FMath.Sin((1.0f - t) * omega) * invSinOmega;
                s2 = (flip) ? -FMath.Sin(t * omega) * invSinOmega : FMath.Sin(t * omega) * invSinOmega;
            }

            return new Quaternion(
                s1 * quaternion1.x + s2 * quaternion2.x,
                s1 * quaternion1.y + s2 * quaternion2.y,
                s1 * quaternion1.z + s2 * quaternion2.z,
                s1 * quaternion1.w + s2 * quaternion2.w
            );
        }

        /// <summary>
        ///  Linearly interpolates between two quaternions.
        /// </summary>
        /// <param name="a">The first source Quaternion.</param>
        /// <param name="b">The second source Quaternion.</param>
        /// <param name="amount">The relative weight of the second source Quaternion in the interpolation.</param>
        /// <returns>The interpolated Quaternion.</returns>
        public static Quaternion Lerp(Quaternion a, Quaternion b, float amount) {
            float t = amount;
            float t1 = 1.0f - t;
            float x;
            float y;
            float z;
            float w;
            float dot = a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
            if (dot >= 0.0f) {
                x = t1 * a.x + t * b.x;
                y = t1 * a.y + t * b.y;
                z = t1 * a.z + t * b.z;
                w = t1 * a.w + t * b.w;
            }
            else {
                x = t1 * a.x - t * b.x;
                y = t1 * a.y - t * b.y;
                z = t1 * a.z - t * b.z;
                w = t1 * a.w - t * b.w;
            }

            float invNorm = 1.0f / FMath.Sqrt(x * x + y * y + z * z + w * w);
            return new Quaternion(x * invNorm, y * invNorm, z * invNorm, w * invNorm);
        }

        public static Quaternion operator *(in Quaternion a, in Vector3f b) {
            return new Quaternion(
                a.w * b.x + a.y * b.z - a.z * b.y,
                a.w * b.y - a.x * b.z + a.z * b.x,
                a.w * b.z + a.x * b.y - a.y * b.x,
                a.x * b.x - a.y * b.y - a.z * b.z
            );
        }

        public static Quaternion operator *(in Quaternion a, in Quaternion b) {
            return new Quaternion(
                a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
                a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
                a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
                a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z
            );
        }

        public static Quaternion operator *(in Quaternion a, float scalar) {
            return new Quaternion(a.x * scalar, a.y * scalar, a.z * scalar, a.w * scalar);
        }

        public static Quaternion operator /(in Quaternion a, float scalar) {
            return new Quaternion(a.x / scalar, a.y / scalar, a.z / scalar, a.w / scalar);
        }

        public static Quaternion operator *(float scalar, in Quaternion a) {
            return new Quaternion(a.x * scalar, a.y * scalar, a.z * scalar, a.w * scalar);
        }

        public static Quaternion operator /(float scalar, in Quaternion a) {
            return new Quaternion(scalar / a.x, scalar / a.y, scalar / a.z, scalar / a.w);
        }

        public static Quaternion operator +(in Quaternion a, in Quaternion b) {
            return new Quaternion(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static Quaternion operator -(in Quaternion a, in Quaternion b) {
            return new Quaternion(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public static bool operator ==(in Quaternion a, in Quaternion b) {
            return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
        }

        public static bool operator !=(in Quaternion a, in Quaternion b) {
            return a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;
        }

        public bool Equals(in Quaternion other, float tolerance = 0.0001f) {
            return FMath.Abs(this.x - other.x) < tolerance && FMath.Abs(this.y - other.y) < tolerance &&
                   FMath.Abs(this.z - other.z) < tolerance && FMath.Abs(this.w - other.w) < tolerance;
        }

        public override bool Equals(object obj) {
            return obj is Quaternion quaternion && quaternion == this;
        }

        public override string ToString() {
            return $"Quaternion({FMath.Round(this.x, 3)}, {FMath.Round(this.y, 3)}, {FMath.Round(this.z, 3)}, {FMath.Round(this.w, 3)})";
        }

        public Quaternion Copy() {
            return this;
        }
    }
}