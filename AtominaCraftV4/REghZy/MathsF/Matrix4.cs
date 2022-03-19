using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace AtominaCraftV4.REghZy.MathsF {
    /// <summary>
    /// A 4x4 float matrix, in row-major order
    /// <code>
    ///      _____________________
    ///      |  __X___Y___Z___W__|
    ///      |X| M00 M01 M02 M03 |
    /// Rows |Y| M10 M11 M12 M13 |
    ///      |Z| M20 M21 M22 M23 |
    ///      |W| M30 M31 M32 M33 |
    ///      |_|____ Columns ____|
    ///
    ///
    /// Column content goes down, Row content goes right
    /// </code>
    /// </summary>
    public struct Matrix4 {
        public float M00; public float M01; public float M02; public float M03;
        public float M10; public float M11; public float M12; public float M13;
        public float M20; public float M21; public float M22; public float M23;
        public float M30; public float M31; public float M32; public float M33;


        // Columns; their content goes top to bottom

        public Vector4f ColumnX4 {
            get => new Vector4f(this.M00, this.M10, this.M20, this.M30);
            set {
                this.M00 = value.x;
                this.M10 = value.y;
                this.M20 = value.z;
                this.M30 = value.w;
            }
        }

        public Vector4f ColumnY4 {
            get => new Vector4f(this.M01, this.M11, this.M21, this.M31);
            set {
                this.M01 = value.x;
                this.M11 = value.y;
                this.M21 = value.z;
                this.M31 = value.w;
            }
        }

        public Vector4f ColumnZ4 {
            get => new Vector4f(this.M02, this.M12, this.M22, this.M32);
            set {
                this.M02 = value.x;
                this.M12 = value.y;
                this.M22 = value.z;
                this.M32 = value.w;
            }
        }

        public Vector4f ColumnW4 {
            get => new Vector4f(this.M03, this.M13, this.M23, this.M33);
            set {
                this.M03 = value.x;
                this.M13 = value.y;
                this.M23 = value.z;
                this.M33 = value.w;
            }
        }

        public Vector3f ColumnX {
            get => new Vector3f(this.M00, this.M10, this.M20);
            set {
                this.M00 = value.x;
                this.M10 = value.y;
                this.M20 = value.z;
            }
        }

        public Vector3f ColumnY {
            get => new Vector3f(this.M01, this.M11, this.M21);
            set {
                this.M01 = value.x;
                this.M11 = value.y;
                this.M21 = value.z;
            }
        }

        public Vector3f ColumnZ {
            get => new Vector3f(this.M02, this.M12, this.M22);
            set {
                this.M02 = value.x;
                this.M12 = value.y;
                this.M22 = value.z;
            }
        }

        public Vector3f ColumnW {
            get => new Vector3f(this.M03, this.M13, this.M23);
            set {
                this.M03 = value.x;
                this.M13 = value.y;
                this.M23 = value.z;
            }
        }

        public Vector4f RowX4 {
            get => new Vector4f(this.M00, this.M01, this.M02, this.M03);
            set {
                this.M00 = value.x;
                this.M01 = value.y;
                this.M02 = value.z;
                this.M03 = value.w;
            }
        }

        public Vector4f RowY4 {
            get => new Vector4f(this.M10, this.M11, this.M12, this.M13);
            set {
                this.M10 = value.x;
                this.M11 = value.y;
                this.M12 = value.z;
                this.M13 = value.w;
            }
        }

        public Vector4f RowZ4 {
            get => new Vector4f(this.M20, this.M21, this.M22, this.M23);
            set {
                this.M20 = value.x;
                this.M21 = value.y;
                this.M22 = value.z;
                this.M23 = value.w;
            }
        }

        public Vector4f RowW4 {
            get => new Vector4f(this.M30, this.M31, this.M32, this.M33);
            set {
                this.M30 = value.x;
                this.M31 = value.y;
                this.M32 = value.z;
                this.M33 = value.w;
            }
        }

        public Vector3f RowX {
            get => new Vector3f(this.M00, this.M01, this.M02);
            set {
                this.M00 = value.x;
                this.M01 = value.y;
                this.M02 = value.z;
            }
        }

        public Vector3f RowY {
            get => new Vector3f(this.M10, this.M11, this.M12);
            set {
                this.M10 = value.x;
                this.M11 = value.y;
                this.M12 = value.z;
            }
        }

        public Vector3f RowZ {
            get => new Vector3f(this.M20, this.M21, this.M22);
            set {
                this.M20 = value.x;
                this.M21 = value.y;
                this.M22 = value.z;
            }
        }

        public Vector3f RowW {
            get => new Vector3f(this.M30, this.M31, this.M32);
            set {
                this.M30 = value.x;
                this.M31 = value.y;
                this.M32 = value.z;
            }
        }

        public Vector3f TranslationPart {
            get => new Vector3f(this.M03, this.M13, this.M23);
            set {
                this.M03 = value.x;
                this.M13 = value.y;
                this.M23 = value.z;
            }
        }

        public Vector3f ScalePart {
            get => new Vector3f(this.M00, this.M11, this.M22);
            set {
                this.M00 = value.x;
                this.M11 = value.y;
                this.M22 = value.z;
            }
        }

        /// <summary>
        /// Returns a matrix where the entire matrix is flipped along the diagonal
        /// </summary>
        public Matrix4 Transposed => new Matrix4(
            this.M00, this.M10, this.M20, this.M30,
            this.M01, this.M11, this.M21, this.M31,
            this.M02, this.M12, this.M22, this.M32,
            this.M03, this.M13, this.M23, this.M33);

        public Matrix4 Inversed => Inverse(this);

        /// <summary>
        /// Gets or sets a specific element at the given index. This is row-major
        /// </summary>
        /// <param name="index">The index (0-15)</param>
        /// <exception cref="ArgumentOutOfRangeException">The index is below 0 or above 15</exception>
        public float this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                switch (index) {
                    case 0x0: return this.M00;
                    case 0x1: return this.M01;
                    case 0x2: return this.M02;
                    case 0x3: return this.M03;
                    case 0x4: return this.M10;
                    case 0x5: return this.M11;
                    case 0x6: return this.M12;
                    case 0x7: return this.M13;
                    case 0x8: return this.M20;
                    case 0x9: return this.M21;
                    case 0xA: return this.M22;
                    case 0xB: return this.M23;
                    case 0xC: return this.M30;
                    case 0xD: return this.M31;
                    case 0xE: return this.M32;
                    case 0xF: return this.M33;
                    default: throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 15");
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                switch (index) {
                    case 0x0: this.M00 = value; return;
                    case 0x1: this.M01 = value; return;
                    case 0x2: this.M02 = value; return;
                    case 0x3: this.M03 = value; return;
                    case 0x4: this.M10 = value; return;
                    case 0x5: this.M11 = value; return;
                    case 0x6: this.M12 = value; return;
                    case 0x7: this.M13 = value; return;
                    case 0x8: this.M20 = value; return;
                    case 0x9: this.M21 = value; return;
                    case 0xA: this.M22 = value; return;
                    case 0xB: this.M23 = value; return;
                    case 0xC: this.M30 = value; return;
                    case 0xD: this.M31 = value; return;
                    case 0xE: this.M32 = value; return;
                    case 0xF: this.M33 = value; return;
                    default: throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 15");
                }
            }
        }

        /// <summary>
        /// Gets or sets an element intersecting the specific row and column indexes
        /// </summary>
        /// <param name="row">The row (top to bottom)</param>
        /// <param name="column">The column (left to right)</param>
        public float this[int row, int column] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this[row * 4 + column];

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this[row * 4 + column] = value;
        }

        public Matrix4(in Matrix4 matrix4) {
            this.M00 = matrix4.M00; this.M01 = matrix4.M01; this.M02 = matrix4.M02; this.M03 = matrix4.M03;
            this.M10 = matrix4.M10; this.M11 = matrix4.M11; this.M12 = matrix4.M12; this.M13 = matrix4.M13;
            this.M20 = matrix4.M20; this.M21 = matrix4.M21; this.M22 = matrix4.M22; this.M23 = matrix4.M23;
            this.M30 = matrix4.M30; this.M31 = matrix4.M31; this.M32 = matrix4.M32; this.M33 = matrix4.M33;
        }

        public Matrix4x4 SysNumericMatrix => new Matrix4x4(
            this.M00, this.M01, this.M02, this.M03,
            this.M10, this.M11, this.M12, this.M13,
            this.M20, this.M21, this.M22, this.M23,
            this.M30, this.M31, this.M32, this.M33);

        public Matrix4(float m00, float m01, float m02, float m03,
                       float m10, float m11, float m12, float m13,
                       float m20, float m21, float mA, float mB,
                       float mC, float mD, float mE, float mF) {
            this.M00 = m00; this.M01 = m01; this.M02 = m02; this.M03 = m03;
            this.M10 = m10; this.M11 = m11; this.M12 = m12; this.M13 = m13;
            this.M20 = m20; this.M21 = m21; this.M22 = mA; this.M23 = mB;
            this.M30 = mC; this.M31 = mD; this.M32 = mE; this.M33 = mF;
        }

        public Matrix4(Matrix4x4 matrix4) {
            this.M00 = matrix4.M11;
            this.M01 = matrix4.M12;
            this.M02 = matrix4.M13;
            this.M03 = matrix4.M14;
            this.M10 = matrix4.M21;
            this.M11 = matrix4.M22;
            this.M12 = matrix4.M23;
            this.M13 = matrix4.M24;
            this.M20 = matrix4.M31;
            this.M21 = matrix4.M32;
            this.M22 = matrix4.M33;
            this.M23 = matrix4.M34;
            this.M30 = matrix4.M41;
            this.M31 = matrix4.M42;
            this.M32 = matrix4.M43;
            this.M33 = matrix4.M44;
        }

        public void MakeIdentity() {
            this.M00 = 1.0f; this.M01 = 0.0f; this.M02 = 0.0f; this.M03 = 0.0f;
            this.M10 = 0.0f; this.M11 = 1.0f; this.M12 = 0.0f; this.M13 = 0.0f;
            this.M20 = 0.0f; this.M21 = 0.0f; this.M22 = 1.0f; this.M23 = 0.0f;
            this.M30 = 0.0f; this.M31 = 0.0f; this.M32 = 0.0f; this.M33 = 1.0f;
        }

        public void MakeRotationX(float r) {
            float cosR = (float) Math.Cos(r);
            float sinR = (float) Math.Sin(r);
            this.M00 = 1.0f; this.M01 = 0.0f; this.M02 =  0.0f; this.M03 = 0.0f;
            this.M10 = 0.0f; this.M11 = cosR; this.M12 = -sinR; this.M13 = 0.0f;
            this.M20 = 0.0f; this.M21 = sinR; this.M22 =  cosR; this.M23 = 0.0f;
            this.M30 = 0.0f; this.M31 = 0.0f; this.M32 =  0.0f; this.M33 = 1.0f;
        }
        public void MakeRotationY(float r) {
            float cosR = (float) Math.Cos(r);
            float sinR = (float) Math.Sin(r);
            this.M00 = cosR;  this.M01 = 0.0f; this.M02 = sinR; this.M03 = 0.0f;
            this.M10 = 0.0f;  this.M11 = 1.0f; this.M12 = 0.0f; this.M13 = 0.0f;
            this.M20 = -sinR; this.M21 = 0.0f; this.M22 = cosR; this.M23 = 0.0f;
            this.M30 = 0.0f;  this.M31 = 0.0f; this.M32 = 0.0f; this.M33 = 1.0f;
        }

        public void MakeRotationZ(float r) {
            float cosR = (float) Math.Cos(r);
            float sinR = (float) Math.Sin(r);
            this.M00 = cosR; this.M01 = -sinR; this.M02 = 0.0f; this.M03 = 0.0f;
            this.M10 = sinR; this.M11 = cosR;  this.M12 = 0.0f; this.M13 = 0.0f;
            this.M20 = 0.0f; this.M21 = 0.0f;  this.M22 = 1.0f; this.M23 = 0.0f;
            this.M30 = 0.0f; this.M31 = 0.0f;  this.M32 = 0.0f; this.M33 = 1.0f;
        }

        public void MakeTranslation(float x, float y, float z) {
            this.M00 = 1.0f; this.M01 = 0.0f; this.M02 = 0.0f; this.M03 = x;
            this.M10 = 0.0f; this.M11 = 1.0f; this.M12 = 0.0f; this.M13 = y;
            this.M20 = 0.0f; this.M21 = 0.0f; this.M22 = 1.0f; this.M23 = z;
            this.M30 = 0.0f; this.M31 = 0.0f; this.M32 = 0.0f; this.M33 = 1.0f;
        }

        public void MakeScale(float x, float y, float z) {
            this.M00 = x;
            this.M11 = y;
            this.M22 = z;
            this.M33 = 1.0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4 Zero() {
            return new Matrix4();
        }

        public static Matrix4 Identity() {
            Matrix4 matrix4 = new Matrix4();
            matrix4.MakeIdentity();
            return matrix4;
        }

        public static Matrix4 RotXYZ(in Vector3f v) {
            return RotX(v.x) * RotY(v.y) * RotZ(v.z);
        }

        public static Matrix4 RotZYX(in Vector3f v) {
            return RotZ(v.z) * RotY(v.y) * RotX(v.x);
        }

        public static Matrix4 RotXYZ(float x, float y, float z) {
            return RotX(x) * RotY(y) * RotZ(z);
        }

        public static Matrix4 RotZYX(float x, float y, float z) {
            return RotZ(z) * RotY(y) * RotX(x);
        }

        public static Matrix4 RotX(in Vector3f v) {
            Matrix4 matrix4 = new Matrix4();
            matrix4.MakeRotationX(v.x);
            return matrix4;
        }

        public static Matrix4 RotX(float r) {
            Matrix4 matrix4 = new Matrix4();
            matrix4.MakeRotationX(r);
            return matrix4;
        }

        public static Matrix4 RotY(in Vector3f v) {
            Matrix4 matrix4 = new Matrix4();
            matrix4.MakeRotationY(v.y);
            return matrix4;
        }

        public static Matrix4 RotY(float r) {
            Matrix4 matrix4 = new Matrix4();
            matrix4.MakeRotationY(r);
            return matrix4;
        }

        public static Matrix4 RotZ(in Vector3f v) {
            Matrix4 matrix4 = new Matrix4();
            matrix4.MakeRotationZ(v.z);
            return matrix4;
        }

        public static Matrix4 RotZ(float r) {
            Matrix4 matrix4 = new Matrix4();
            matrix4.MakeRotationZ(r);
            return matrix4;
        }

        public static Matrix4 Translation(in Vector3f v) {
            Matrix4 matrix4 = new Matrix4();
            matrix4.MakeTranslation(v.x, v.y, v.z);
            return matrix4;
        }

        public static Matrix4 Translation(float x, float y, float z) {
            Matrix4 matrix4 = new Matrix4();
            matrix4.MakeTranslation(x, y, z);
            return matrix4;
        }

        public static Matrix4 Scale(in Vector3f v) {
            Matrix4 matrix4 = new Matrix4();
            matrix4.MakeScale(v.x, v.y, v.z);
            return matrix4;
        }

        public static Matrix4 Scale(float x, float y, float z) {
            Matrix4 matrix4 = new Matrix4();
            matrix4.MakeScale(x, y, z);
            return matrix4;
        }

        public Vector3f MultiplyPoint(in Vector3f v) {
            float w = this.M30 * v.x + this.M31 * v.y + this.M32 * v.z + this.M33;
            return new Vector3f(
                this.M00 * v.x + this.M01 * v.y + this.M02 * v.z + this.M03,
                this.M10 * v.x + this.M11 * v.y + this.M12 * v.z + this.M13,
                this.M20 * v.x + this.M21 * v.y + this.M22 * v.z + this.M23) * w;
        }

        public Vector3f MultiplyDirection(in Vector3f v) {
            return new Vector3f(
                this.M00 * v.x + this.M01 * v.y + this.M02 * v.z,
                this.M10 * v.x + this.M11 * v.y + this.M12 * v.z,
                this.M20 * v.x + this.M21 * v.y + this.M22 * v.z);
        }

        public static Vector4f operator *(in Matrix4 m, in Vector4f v) {
            return new Vector4f(
                m.M00 * v.x + m.M01 * v.y + m.M02 * v.z + m.M03 * v.w,
                m.M10 * v.x + m.M11 * v.y + m.M12 * v.z + m.M13 * v.w,
                m.M20 * v.x + m.M21 * v.y + m.M22 * v.z + m.M23 * v.w,
                m.M30 * v.x + m.M31 * v.y + m.M32 * v.z + m.M33 * v.w);
        }

        public static Vector4f operator *(in Matrix4 m, in Vector3f v) {
            return new Vector4f(
                m.M00 * v.x + m.M01 * v.y + m.M02 * v.z + m.M03,
                m.M10 * v.x + m.M11 * v.y + m.M12 * v.z + m.M13,
                m.M20 * v.x + m.M21 * v.y + m.M22 * v.z + m.M23,
                m.M30 * v.x + m.M31 * v.y + m.M32 * v.z + m.M33);
        }

        public static Matrix4 operator /(in Matrix4 m, float a) {
            Matrix4 n = new Matrix4();
            for (int i = 0; i < 16; i++) {
                n[i] = m[i] / a;
            }

            return n;
        }

        public static Matrix4 operator *(in Matrix4 m1, in Matrix4 m2) {
            return new Matrix4(
                m2.M00 * m1.M00 + m2.M10 * m1.M01 + m2.M20 * m1.M02 + m2.M30 * m1.M03,
                m2.M01 * m1.M00 + m2.M11 * m1.M01 + m2.M21 * m1.M02 + m2.M31 * m1.M03,
                m2.M02 * m1.M00 + m2.M12 * m1.M01 + m2.M22 * m1.M02 + m2.M32 * m1.M03,
                m2.M03 * m1.M00 + m2.M13 * m1.M01 + m2.M23 * m1.M02 + m2.M33 * m1.M03,
                m2.M00 * m1.M10 + m2.M10 * m1.M11 + m2.M20 * m1.M12 + m2.M30 * m1.M13,
                m2.M01 * m1.M10 + m2.M11 * m1.M11 + m2.M21 * m1.M12 + m2.M31 * m1.M13,
                m2.M02 * m1.M10 + m2.M12 * m1.M11 + m2.M22 * m1.M12 + m2.M32 * m1.M13,
                m2.M03 * m1.M10 + m2.M13 * m1.M11 + m2.M23 * m1.M12 + m2.M33 * m1.M13,
                m2.M00 * m1.M20 + m2.M10 * m1.M21 + m2.M20 * m1.M22 + m2.M30 * m1.M23,
                m2.M01 * m1.M20 + m2.M11 * m1.M21 + m2.M21 * m1.M22 + m2.M31 * m1.M23,
                m2.M02 * m1.M20 + m2.M12 * m1.M21 + m2.M22 * m1.M22 + m2.M32 * m1.M23,
                m2.M03 * m1.M20 + m2.M13 * m1.M21 + m2.M23 * m1.M22 + m2.M33 * m1.M23,
                m2.M00 * m1.M30 + m2.M10 * m1.M31 + m2.M20 * m1.M32 + m2.M30 * m1.M33,
                m2.M01 * m1.M30 + m2.M11 * m1.M31 + m2.M21 * m1.M32 + m2.M31 * m1.M33,
                m2.M02 * m1.M30 + m2.M12 * m1.M31 + m2.M22 * m1.M32 + m2.M32 * m1.M33,
                m2.M03 * m1.M30 + m2.M13 * m1.M31 + m2.M23 * m1.M32 + m2.M33 * m1.M33
            );
        }

        public static Matrix4 Inverse(in Matrix4 m) {
            Matrix4 inv = new Matrix4();
            inv.M00 =  m.M11 * m.M22 * m.M33 - m.M11 * m.M23 * m.M32 - m.M21 * m.M12 * m.M33 + m.M21 * m.M13 * m.M32 + m.M31 * m.M12 * m.M23 - m.M31 * m.M13 * m.M22;
            inv.M01 = -m.M01 * m.M22 * m.M33 + m.M01 * m.M23 * m.M32 + m.M21 * m.M02 * m.M33 - m.M21 * m.M03 * m.M32 - m.M31 * m.M02 * m.M23 + m.M31 * m.M03 * m.M22;
            inv.M02 =  m.M01 * m.M12 * m.M33 - m.M01 * m.M13 * m.M32 - m.M11 * m.M02 * m.M33 + m.M11 * m.M03 * m.M32 + m.M31 * m.M02 * m.M13 - m.M31 * m.M03 * m.M12;
            inv.M03 = -m.M01 * m.M12 * m.M23 + m.M01 * m.M13 * m.M22 + m.M11 * m.M02 * m.M23 - m.M11 * m.M03 * m.M22 - m.M21 * m.M02 * m.M13 + m.M21 * m.M03 * m.M12;
            inv.M10 = -m.M10 * m.M22 * m.M33 + m.M10 * m.M23 * m.M32 + m.M20 * m.M12 * m.M33 - m.M20 * m.M13 * m.M32 - m.M30 * m.M12 * m.M23 + m.M30 * m.M13 * m.M22;
            inv.M11 =  m.M00 * m.M22 * m.M33 - m.M00 * m.M23 * m.M32 - m.M20 * m.M02 * m.M33 + m.M20 * m.M03 * m.M32 + m.M30 * m.M02 * m.M23 - m.M30 * m.M03 * m.M22;
            inv.M12 = -m.M00 * m.M12 * m.M33 + m.M00 * m.M13 * m.M32 + m.M10 * m.M02 * m.M33 - m.M10 * m.M03 * m.M32 - m.M30 * m.M02 * m.M13 + m.M30 * m.M03 * m.M12;
            inv.M13 =  m.M00 * m.M12 * m.M23 - m.M00 * m.M13 * m.M22 - m.M10 * m.M02 * m.M23 + m.M10 * m.M03 * m.M22 + m.M20 * m.M02 * m.M13 - m.M20 * m.M03 * m.M12;
            inv.M20 =  m.M10 * m.M21 * m.M33 - m.M10 * m.M23 * m.M31 - m.M20 * m.M11 * m.M33 + m.M20 * m.M13 * m.M31 + m.M30 * m.M11 * m.M23 - m.M30 * m.M13 * m.M21;
            inv.M21 = -m.M00 * m.M21 * m.M33 + m.M00 * m.M23 * m.M31 + m.M20 * m.M01 * m.M33 - m.M20 * m.M03 * m.M31 - m.M30 * m.M01 * m.M23 + m.M30 * m.M03 * m.M21;
            inv.M22 =  m.M00 * m.M11 * m.M33 - m.M00 * m.M13 * m.M31 - m.M10 * m.M01 * m.M33 + m.M10 * m.M03 * m.M31 + m.M30 * m.M01 * m.M13 - m.M30 * m.M03 * m.M11;
            inv.M23 = -m.M00 * m.M11 * m.M23 + m.M00 * m.M13 * m.M21 + m.M10 * m.M01 * m.M23 - m.M10 * m.M03 * m.M21 - m.M20 * m.M01 * m.M13 + m.M20 * m.M03 * m.M11;
            inv.M30 = -m.M10 * m.M21 * m.M32 + m.M10 * m.M22 * m.M31 + m.M20 * m.M11 * m.M32 - m.M20 * m.M12 * m.M31 - m.M30 * m.M11 * m.M22 + m.M30 * m.M12 * m.M21;
            inv.M31 =  m.M00 * m.M21 * m.M32 - m.M00 * m.M22 * m.M31 - m.M20 * m.M01 * m.M32 + m.M20 * m.M02 * m.M31 + m.M30 * m.M01 * m.M22 - m.M30 * m.M02 * m.M21;
            inv.M32 = -m.M00 * m.M11 * m.M32 + m.M00 * m.M12 * m.M31 + m.M10 * m.M01 * m.M32 - m.M10 * m.M02 * m.M31 - m.M30 * m.M01 * m.M12 + m.M30 * m.M02 * m.M11;
            inv.M33 =  m.M00 * m.M11 * m.M22 - m.M00 * m.M12 * m.M21 - m.M10 * m.M01 * m.M22 + m.M10 * m.M02 * m.M21 + m.M20 * m.M01 * m.M12 - m.M20 * m.M02 * m.M11;
            float det = m[0] * inv[0] + m[1] * inv[4] + m[2] * inv[8] + m[3] * inv[12];
            return inv / det;
        }

        public static Matrix4 Projection(float vpWidth, float vpHeight, float n, float f, float fov) {
            float fr = (float) (1.0f / Math.Tan(fov * Math.PI / 360.0f)); // fov in radians
            float a = vpHeight / vpWidth; // aspect ratio
            float d = n - f; // distance between near and far
            Matrix4 mat = new Matrix4();
            mat.M00 = fr * a; mat.M01 = 0.0f; mat.M02 = 0.0f;        mat.M03 = 0.0f;
            mat.M10 = 0.0f;   mat.M11 = fr;   mat.M12 = 0.0f;        mat.M13 = 0.0f;
            mat.M20 = 0.0f;   mat.M21 = 0.0f; mat.M22 = (n + f) / d; mat.M23 = (2 * n * f) / d;
            mat.M30 = 0.0f;   mat.M31 = 0.0f; mat.M32 = -1.0f;       mat.M33 = 0.0f;
            return mat;
        }

        /// <summary>
        /// Orthographic matrix
        /// </summary>
        /// <param name="l">Left</param>
        /// <param name="t">Top</param>
        /// <param name="r">Right</param>
        /// <param name="b">Bottom</param>
        /// <param name="n">Near</param>
        /// <param name="f">Far</param>
        public static Matrix4 Orthographic(float l, float t, float r, float b, float n, float f) {
            Matrix4 mat = new Matrix4();
            mat.M00 = 2 / (r - l); mat.M01 = 0.0f;        mat.M02 = 0.0f;         mat.M03 = -(r + l) / (r - l);
            mat.M10 = 0.0f;        mat.M11 = 2 / (t - b); mat.M12 = 0.0f;         mat.M13 = -(t + b) / (t - b);
            mat.M20 = 0.0f;        mat.M21 = 0.0f;        mat.M22 = -2 / (f - n); mat.M23 = -(f + n) / (f - n);
            mat.M30 = 0.0f;        mat.M31 = 0.0f;        mat.M32 = 0.0f;         mat.M33 = 1.0f;
            return mat;
        }

        public static Matrix4 WorldToLocal(in Vector3f position, in Vector3f rotation, in Vector3f scale) {
            return Scale(1.0f / scale.x, 1.0f / scale.y, 1.0f / scale.z) *
                   RotZ(-rotation.z) *
                   RotX(-rotation.x) *
                   RotY(-rotation.y) *
                   Translation(-position.x, -position.y, -position.z);
        }

        public static Matrix4 LocalToWorld(in Vector3f position, in Vector3f rotation, in Vector3f scale) {
            return Translation(position) *
                   RotY(rotation.y) *
                   RotX(rotation.x) *
                   RotZ(rotation.z) *
                   Scale(scale);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix4 Copy() {
            return new Matrix4(this);
        }

        public unsafe delegate void PtrCallback(float* ptr);

        /// <summary>
        /// A helper for accessing a pointer to the first element
        /// </summary>
        /// <param name="callback"></param>
        public unsafe void Pointer(PtrCallback callback) {
            fixed (float* m = &this.M00) {
                callback(m);
            }
        }
    }
}