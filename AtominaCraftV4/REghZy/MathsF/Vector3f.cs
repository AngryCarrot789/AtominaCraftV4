using System;

namespace AtominaCraftV4.REghZy.MathsF {
    public struct Vector3f {
        public float x;
        public float y;
        public float z;

        public static Vector3f Zero => new Vector3f(0.0f);
        public static Vector3f One  => new Vector3f(1.0f);
        public static Vector3f UnitX => new Vector3f(1.0f, 0.0f, 0.0f);
        public static Vector3f UnitY => new Vector3f(0.0f, 1.0f, 0.0f);
        public static Vector3f UnitZ => new Vector3f(0.0f, 0.0f, 1.0f);

        // Right-handed cartesian coordinate system, which OpenGL uses
        public static Vector3f Up       => new Vector3f( 0.0f,  1.0f,  0.0f);
        public static Vector3f Down     => new Vector3f( 0.0f, -1.0f,  0.0f);
        public static Vector3f Left     => new Vector3f( 1.0f,  0.0f,  0.0f);
        public static Vector3f Right    => new Vector3f(-1.0f,  0.0f,  0.0f);
        public static Vector3f Backward => new Vector3f( 0.0f,  0.0f,  1.0f);
        public static Vector3f Forward  => new Vector3f( 0.0f,  0.0f, -1.0f);

        public float this[int i] {
            get {
                switch (i) {
                    case 0: return this.x;
                    case 1: return this.y;
                    case 2: return this.z;
                    default: throw new ArgumentOutOfRangeException(nameof(i), "Index must be between 0 and 2");
                }
            }
            set {
                switch (i) {
                    case 0: this.x = value; return;
                    case 1: this.y = value; return;
                    case 2: this.z = value; return;
                    default: throw new ArgumentOutOfRangeException(nameof(i), "Index must be between 0 and 2");
                }
            }
        }

        public Vector3f(float all) {
            this.x = all;
            this.y = all;
            this.z = all;
        }


        public Vector3f(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3f(in Vector3f vector) {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }

        public float Dot(in Vector3f b) {
            return this.x * b.x + this.y * b.y + this.z * b.z;
        }

        public float Mag() {
            return (float) Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
        }

        public float MagSq() {
            return this.x * this.x + this.y * this.y + this.z * this.z;
        }

        public float Distance(in Vector3f o) {
            return (float) Math.Sqrt(FMath.Square(this.x - o.x) + FMath.Square(this.y - o.y) + FMath.Square(this.z - o.z));
        }

        public float DistanceSquared(in Vector3f o) {
            return FMath.Square(this.x - o.x) + FMath.Square(this.y - o.y) + FMath.Square(this.z - o.z);
        }

        public double Angle(in Vector3f vec) {
            return FMath.Acos(Normalise().Dot(vec.Normalise()));
        }

        public Vector3f Midpoint(in Vector3f other) {
            return new Vector3f((this.x + other.x) / 2, (this.y + other.y) / 2, (this.z + other.z) / 2);
        }

        public Vector3f Cross(in Vector3f b) {
            return new Vector3f(this.y * b.z - this.z * b.y, this.z * b.x - this.x * b.z, this.x * b.y - this.y * b.x);
        }

        public static Vector3f Cross(Vector3f a, Vector3f b) {
            return new Vector3f(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        public float MagnitudeSquare() {
            return this.x * this.x + this.y * this.y + this.z * this.z;
        }

        public float Magnitude() {
            return FMath.Sqrt(MagnitudeSquare());
        }

        public void RemoveNAN() {
            if (float.IsNaN(this.x)) {
                this.x = 0.0f;
            }

            if (float.IsNaN(this.y)) {
                this.y = 0.0f;
            }

            if (float.IsNaN(this.z)) {
                this.z = 0.0f;
            }
        }

        public Vector3f GetNonNAN() {
            Vector3f vec = this;
            vec.RemoveNAN();
            return vec;
        }

        public Vector3f Normalise() {
            float mag = FMath.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            return new Vector3f(this.x / mag, this.y / mag, this.z / mag);
        }

        public Vector3f ClipMagnitude(float max) {
            if (max <= 0.0f) {
                return new Vector3f(0.0f);
            }

            float r = MagnitudeSquare() / (max * max);
            if (r > 1.0f) {
                Vector3f v = this;
                v /= FMath.Sqrt(r);
                return new Vector3f(v.x, v.y, v.z);
            }

            return this;
        }

        public Vector3f Negate() {
            return new Vector3f(-this.x, -this.y, -this.z);
        }

        public bool IsNDC() {
            return this.x > -1.0f && this.x < 1.0f && this.y > -1.0f && this.y < 1.0f && this.z > -1.0f && this.z < 1.0f;
        }

        public static Vector3f operator +(in Vector3f a, in Vector3f b) {
            return new Vector3f(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3f operator +(in Vector3f a, float b) {
            return new Vector3f(a.x + b, a.y + b, a.z + b);
        }

        public static Vector3f operator +(float a, in Vector3f b) {
            return new Vector3f(a + b.x, a + b.y, a + b.z);
        }

        public static Vector3f operator -(in Vector3f a, in Vector3f b) {
            return new Vector3f(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3f operator -(in Vector3f a, float b) {
            return new Vector3f(a.x - b, a.y - b, a.z - b);
        }

        public static Vector3f operator -(float b, in Vector3f a) {
            return new Vector3f(b - a.x, b - a.y, b - a.z);
        }

        public static Vector3f operator -(in Vector3f v) {
            return new Vector3f(-v.x, -v.y, -v.z);
        }

        public static Vector3f operator *(in Vector3f a, in Vector3f b) {
            return new Vector3f(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3f operator *(float b, in Vector3f a) {
            return new Vector3f(b * a.x, b * a.y, b * a.z);
        }

        public static Vector3f operator *(in Vector3f a, float b) {
            return new Vector3f(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3f operator /(in Vector3f a, in Vector3f b) {
            return new Vector3f(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static Vector3f operator /(float a, in Vector3f v) {
            return new Vector3f(a / v.x, a / v.y, a / v.z);
        }

        public static Vector3f operator /(in Vector3f a, float b) {
            return new Vector3f(a.x / b, a.y / b, a.z / b);
        }

        public static bool operator ==(in Vector3f a, in Vector3f b) {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(in Vector3f a, in Vector3f b) {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public void Set(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public bool Equals(in Vector3f other, float tolerance = 0.0001f) {
            return FMath.Abs(this.x - other.x) < tolerance && FMath.Abs(this.y - other.y) < tolerance && FMath.Abs(this.z - other.z) < tolerance;
        }

        public override bool Equals(object obj) {
            return obj is Vector3f other && this == other;
        }

        public Vector3f Copy() {
            // IL code will load "this", then execute ldobj (which performs a value-type copy), and returns that copy
            return this;
        }

        public override string ToString() {
            return $"Vector3f({FMath.Round(this.x, 2)}, {FMath.Round(this.y, 2)}, {FMath.Round(this.z, 2)})";
        }
    }
}