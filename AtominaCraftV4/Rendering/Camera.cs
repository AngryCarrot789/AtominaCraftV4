using System;
using AtominaCraftV4.REghZy.MathsF;
using AtominaCraftV4.Windowing;
using OpenTK.Mathematics;
using Matrix4 = AtominaCraftV4.REghZy.MathsF.Matrix4;

namespace AtominaCraftV4.Rendering {
    public class Camera {
        public Vector3f pos;
        public Vector3f rot;

        public Matrix4 proj;
        public Matrix4 view;

        public Matrix4 matrix;

        public float near;
        public float far;

        public float rotateX;
        public float rotateY;

        public Camera() {
            this.near = 0.01f;
            this.far = 1000.0f;
            UpdateProjection();
        }

        public void UpdateProjection() {
            float fovRads = 1.0f / FMath.Tanh(90 * Maths.PI / 360.0f);
            float aspect = ((float) ACWindow.MainWindow.Height) / ((float) ACWindow.MainWindow.Width);
            float distance = this.near - this.far;

            this.proj[0] = fovRads * aspect;
            this.proj[1] = 0.0f;
            this.proj[2] = 0.0f;
            this.proj[3] = 0.0f;

            this.proj[4] = 0.0f;
            this.proj[5] = fovRads;
            this.proj[6] = 0.0f;
            this.proj[7] = 0.0f;

            this.proj[8] = 0.0f;
            this.proj[9] = 0.0f;
            this.proj[10] = (this.near + this.far) / distance;
            this.proj[11] = 2 * this.near * this.far / distance;

            this.proj[12] = 0.0f;
            this.proj[13] = 0.0f;
            this.proj[14] = -1.0f;
            this.proj[15] = 0.0f;
        }

        public void SetRotation(float rotateX, float rotateY) {
            this.rotateX = rotateX;
            this.rotateY = rotateY;
        }

        public void Update() {
            // const float sensitivity = 2.75f;
            const float sensitivity = 0.001f;
            // const float sensitivity = 0.01f;
            // rotateX uses mouseY because it's rotation along the Y axis
            // this.rotateX -= (y * sensitivity * Delta.time);
            // if (this.rotateX > Maths.PI_HALF)
            //     this.rotateX = Maths.PI_HALF;
            // else if (this.rotateX < Maths.PI_NEG / 2)
            //     this.rotateX = Maths.PI_NEG / 2;
            // this.rotateY -= (x * sensitivity * Delta.time);
            // if (this.rotateY > Maths.PI)
            //     this.rotateY -= Maths.PI_NEG_DOUBLE;
            // else if (this.rotateY < Maths.PI_NEG)
            //     this.rotateY += Maths.PI_DOUBLE;
            // this.rotateY = Math.Clamp(this.rotateY, Maths.PI_NEG_DOUBLE + 0.0001f, Maths.PI_DOUBLE - 0.0001f);
            // this.view = Matrix4.RotX(-this.rotateX) * Matrix4.RotY(-this.rotateY) * Matrix4.WorldToLocal(this.pos, Vector3f.Zero, Vector3f.One);

            this.rotateX -= (Mouse.Instance.ChangeY * sensitivity);
            if (this.rotateX > Maths.PI_HALF) {
                this.rotateX = Maths.PI_HALF;
            }
            else if (this.rotateX < Maths.PI_NEG / 2) {
                this.rotateX = Maths.PI_NEG_HALF;
            }

            this.rotateY -= (Mouse.Instance.ChangeX * sensitivity);
            if (this.rotateY > Maths.PI) {
                this.rotateY = Maths.PI_NEG + 0.0001f;
            }
            else if (this.rotateY < Maths.PI_NEG) {
                this.rotateY = Maths.PI - 0.0001f;
            }

            this.view = Matrix4.RotX(-this.rotateX) * Matrix4.RotY(-this.rotateY) * Matrix4.Scale(1.0f / Vector3f.One) * Matrix4.Translation(-this.pos);
            this.matrix = this.proj * this.view;
        }
    }
}