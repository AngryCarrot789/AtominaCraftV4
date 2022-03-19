using System;

namespace AtominaCraftV4.REghZy.MathsF {
    public struct Vector2f {
        public float x;
        public float y;

        public static Vector2f Zero => new Vector2f(0.0f);
        public static Vector2f One  => new Vector2f(1.0f);
        public static Vector2f UnitX => new Vector2f(1.0f, 0.0f);
        public static Vector2f UnitY => new Vector2f(0.0f, 1.0f);

        public float this[int i] {
            get {
                switch (i) {
                    case 0: return this.x;
                    case 1: return this.y;
                    default: throw new ArgumentOutOfRangeException(nameof(i), "Index must be be 0 or 1");
                }
            }
            set {
                switch (i) {
                    case 0: this.x = value; return;
                    case 1: this.y = value; return;
                    default: throw new ArgumentOutOfRangeException(nameof(i), "Index must be 0 or 1");
                }
            }
        }

        public Vector2f(float all) {
            this.x = all;
            this.y = all;
        }


        public Vector2f(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public Vector2f(in Vector2f vector) {
            this.x = vector.x;
            this.y = vector.y;
        }

        public float Dot(in Vector2f b) {
            return this.x * b.x + this.y * b.y;
        }

        public float Mag() {
            return (float) Math.Sqrt(this.x * this.x + this.y * this.y);
        }

        public float MagSq() {
            return this.x * this.x + this.y * this.y;
        }

        public float Distance(in Vector2f o) {
            return (float) Math.Sqrt(FMath.Square(this.x - o.x) + FMath.Square(this.y - o.y));
        }

        public float DistanceSquared(in Vector2f o) {
            return FMath.Square(this.x - o.x) + FMath.Square(this.y - o.y);
        }

        public double Angle(in Vector2f vec) {
            return FMath.Acos(Normalise().Dot(vec.Normalise()));
        }

        public Vector2f Midpoint(in Vector2f other) {
            return new Vector2f((this.x + other.x) / 2, (this.y + other.y) / 2);
        }

        public float MagnitudeSquare() {
            return this.x * this.x + this.y * this.y;
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
        }

        public Vector2f GetNonNAN() {
            Vector2f vec = this;
            vec.RemoveNAN();
            return vec;
        }

        public Vector2f Normalise() {
            float mag = FMath.Sqrt(this.x * this.x + this.y * this.y);
            return new Vector2f(this.x / mag, this.y / mag);
        }

        public Vector2f ClipMagnitude(float max) {
            if (max <= 0.0f) {
                return new Vector2f(0.0f);
            }

            float r = MagnitudeSquare() / (max * max);
            if (r > 1.0f) {
                Vector2f v = this;
                v /= FMath.Sqrt(r);
                return new Vector2f(v.x, v.y);
            }

            return this;
        }

        public Vector2f Negate() {
            return new Vector2f(-this.x, -this.y);
        }

        public bool IsNDC() {
            return this.x > -1.0f && this.x < 1.0f && this.y > -1.0f && this.y < 1.0f;
        }

        public static Vector2f operator +(in Vector2f a, in Vector2f b) {
            return new Vector2f(a.x + b.x, a.y + b.y);
        }

        public static Vector2f operator +(in Vector2f a, float b) {
            return new Vector2f(a.x + b, a.y + b);
        }

        public static Vector2f operator +(float a, in Vector2f b) {
            return new Vector2f(a + b.x, a + b.y);
        }

        public static Vector2f operator -(in Vector2f a, in Vector2f b) {
            return new Vector2f(a.x - b.x, a.y - b.y);
        }

        public static Vector2f operator -(in Vector2f a, float b) {
            return new Vector2f(a.x - b, a.y - b);
        }

        public static Vector2f operator -(float b, in Vector2f a) {
            return new Vector2f(b - a.x, b - a.y);
        }

        public static Vector2f operator -(in Vector2f v) {
            return new Vector2f(-v.x, -v.y);
        }

        public static Vector2f operator *(in Vector2f a, in Vector2f b) {
            return new Vector2f(a.x * b.x, a.y * b.y);
        }

        public static Vector2f operator *(float b, in Vector2f a) {
            return new Vector2f(b * a.x, b * a.y);
        }

        public static Vector2f operator *(in Vector2f a, float b) {
            return new Vector2f(a.x * b, a.y * b);
        }

        public static Vector2f operator /(in Vector2f a, in Vector2f b) {
            return new Vector2f(a.x / b.x, a.y / b.y);
        }

        public static Vector2f operator /(float a, in Vector2f v) {
            return new Vector2f(a / v.x, a / v.y);
        }

        public static Vector2f operator /(in Vector2f a, float b) {
            return new Vector2f(a.x / b, a.y / b);
        }

        public static bool operator ==(in Vector2f a, in Vector2f b) {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(in Vector2f a, in Vector2f b) {
            return a.x != b.x || a.y != b.y;
        }

        public void Set(float x, float y, float z) {
            this.x = x;
            this.y = y;
        }

        public bool Equals(in Vector2f other, float tolerance = 0.0001f) {
            return FMath.Abs(this.x - other.x) < tolerance && FMath.Abs(this.y - other.y) < tolerance;
        }

        public override bool Equals(object obj) {
            return obj is Vector2f other && this == other;
        }

        public Vector2f Copy() {
            // IL code will load "this", then execute ldobj (which performs a value-type copy), and returns that copy
            return this;
        }

        public override string ToString() {
            return $"Vector2f({FMath.Round(this.x, 2)}, {FMath.Round(this.y, 2)})";
        }
    }
}