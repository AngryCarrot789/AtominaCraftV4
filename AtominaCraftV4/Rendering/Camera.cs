using System;
using System.Net.Http.Headers;
using AtominaCraftV4.REghZy.MathsF;
using AtominaCraftV4.Windowing;
using OpenTK.Mathematics;
using REghZy.Utils;
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

        public float pitch;
        public float yaw;

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
            this.pitch = rotateX;
            this.yaw = rotateY;
        }

        public void Update() {
            const float sensitivity = 0.0012f;
            float y = Mouse.Instance.ChangeX * sensitivity;
            float p = Mouse.Instance.ChangeY * sensitivity;

            // static string ek(string value, int len = 10, char fill = ' ') {
            //     if (value.Length >= len) {
            //         return value.Substring(0, len);
            //     }
            //     else {
            //         return value + StringUtils.Repeat(fill, len - value.Length);
            //     }
            // }
            // Console.WriteLine($"{ek(Math.Round(yaw, 5).ToString())} {ek(Math.Round(pitch, 5).ToString())} ({ek(Math.Round(Mouse.Instance.ChangeX, 5).ToString())} {ek(Math.Round(Mouse.Instance.ChangeY, 5).ToString())})");

            this.yaw -= y;
            if (this.yaw > Maths.PI) {
                this.yaw = Maths.PI_NEG + 0.0001f;
            }
            else if (this.yaw < Maths.PI_NEG) {
                this.yaw = Maths.PI - 0.0001f;
            }

            this.pitch -= p;
            if (this.pitch > Maths.PI_HALF) {
                this.pitch = Maths.PI_HALF;
            }
            else if (this.pitch < Maths.PI_NEG / 2) {
                this.pitch = Maths.PI_NEG_HALF;
            }

            this.view = Matrix4.RotX(-this.pitch) * Matrix4.RotY(-this.yaw) * Matrix4.Scale(1.0f / Vector3f.One) * Matrix4.Translation(-this.pos);
            this.matrix = this.proj * this.view;
        }
    }
}