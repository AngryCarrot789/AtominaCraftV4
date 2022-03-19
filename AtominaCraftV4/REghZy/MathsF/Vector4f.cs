using System;

namespace AtominaCraftV4.REghZy.MathsF {
    public struct Vector4f {
        public float x;
        public float y;
        public float z;
        public float w;

        public static Vector4f Zero => new Vector4f(0.0f);
        public static Vector4f One => new Vector4f(1.0f);
        public static Vector4f UnitX => new Vector4f(1.0f, 0.0f, 0.0f);
        public static Vector4f UnitY => new Vector4f(0.0f, 1.0f, 0.0f);
        public static Vector4f UnitZ => new Vector4f(0.0f, 0.0f, 1.0f);

        // Right-handed cartesian coordinate system, which OpenGL uses
        public static Vector4f Up       => new Vector4f( 0.0f,  1.0f,  0.0f);
        public static Vector4f Down     => new Vector4f( 0.0f, -1.0f,  0.0f);
        public static Vector4f Left     => new Vector4f( 1.0f,  0.0f,  0.0f);
        public static Vector4f Right    => new Vector4f(-1.0f,  0.0f,  0.0f);
        public static Vector4f Backward => new Vector4f( 0.0f,  0.0f,  1.0f);
        public static Vector4f Forward  => new Vector4f( 0.0f,  0.0f, -1.0f);

        public Vector3f Homogenised => new Vector3f(this.x / this.w, this.y/ this.w, this.z/ this.w);

        public float MagnitudeSquared => this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w;

        public float Magnitude => (float) Math.Sqrt(this.MagnitudeSquared);

        public Vector3f XYZ {
            get => new Vector3f(this.x, this.y, this.z);
            set {
                this.x = value.x;
                this.y = value.y;
                this.z = value.z;
            }
        }

        public float this[int i] {
            get {
                switch (i) {
                    case 0: return this.x;
                    case 1: return this.y;
                    case 2: return this.z;
                    case 3: return this.w;
                    default: throw new ArgumentOutOfRangeException(nameof(i), $"Index must be between 0 and 3: {i}");
                }
            }
            set {
                switch (i) {
                    case 0: this.x = value; return;
                    case 1: this.y = value; return;
                    case 2: this.z = value; return;
                    case 3: this.w = value; return;
                    default: throw new ArgumentOutOfRangeException(nameof(i), $"Index must be between 0 and 3: {i}");
                }
            }
        }

        public Vector4f(Vector3f vector) {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
            this.w = 0.0f;
        }

        public Vector4f(Vector3f vector, float w) {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
            this.w = w;
        }

        public Vector4f(Vector4f vector) {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
            this.w = vector.w;
        }

        public Vector4f(float all) {
            this.x = all;
            this.y = all;
            this.z = all;
            this.w = all;
        }

        public Vector4f(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0.0f;
        }

        public Vector4f(float x, float y, float z, float w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public float Dot(Vector4f a) {
            return this.x * a.x + this.y * a.y + this.z * a.z + this.w * a.w;
        }

        public static Vector4f operator +(Vector4f a, Vector4f b) {
            return new Vector4f(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static Vector4f operator +(Vector4f a, float b) {
            return new Vector4f(a.x + b, a.y + b, a.z + b, a.w + b);
        }

        public static Vector4f operator +(float a, Vector4f b) {
            return new Vector4f(a + b.x, a + b.y, a + b.z, a + b.w);
        }

        public static Vector4f operator -(Vector4f a, Vector4f b) {
            return new Vector4f(a.x - b.x, a.y - b.y, a.z - b.z, a.z - b.w);
        }

        public static Vector4f operator -(Vector4f a, float b) {
            return new Vector4f(a.x - b, a.y - b, a.z - b);
        }

        public static Vector4f operator -(float b, Vector4f a) {
            return new Vector4f(b - a.x, b - a.y, b - a.z);
        }

        public static Vector4f operator -(Vector4f v) {
            return new Vector4f(-v.x, -v.y, -v.z);
        }

        public static Vector4f operator *(Vector4f a, Vector4f b) {
            return new Vector4f(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector4f operator *(float b, Vector4f a) {
            return new Vector4f(b * a.x, b * a.y, b * a.z);
        }

        public static Vector4f operator *(Vector4f a, float b) {
            return new Vector4f(a.x * b, a.y * b, a.z * b);
        }

        public static Vector4f operator /(Vector4f a, Vector4f b) {
            return new Vector4f(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static Vector4f operator /(float a, Vector4f v) {
            return new Vector4f(a / v.x, a / v.y, a / v.z);
        }

        public static Vector4f operator /(Vector4f a, float b) {
            return new Vector4f(a.x / b, a.y / b, a.z / b);
        }

        public static bool operator ==(in Vector4f a, in Vector4f b) {
            return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
        }

        public static bool operator !=(in Vector4f a, in Vector4f b) {
            return a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;
        }

        public void Set(float x, float y, float z, float w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public bool Equals(Vector4f other, float tolerance = 0.0001f) {
            return FMath.Abs(this.x - other.x) < tolerance && FMath.Abs(this.y - other.y) < tolerance &&
                   FMath.Abs(this.z - other.z) < tolerance && FMath.Abs(this.w - other.w) < tolerance;
        }

        public override bool Equals(object obj) {
            return obj is Vector4f other && this == other;
        }

        public Vector4f Copy() {
            return new Vector4f(this.x, this.y, this.z, this.w);
        }

        public override string ToString() {
            return $"Vector4f({FMath.Round(this.x, 3)}, {FMath.Round(this.y, 3)}, {FMath.Round(this.z, 3)}, {FMath.Round(this.w, 3)})";
        }
    }
}