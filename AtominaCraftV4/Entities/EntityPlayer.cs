using AtominaCraftV4.REghZy.MathsF;
using AtominaCraftV4.Rendering;
using AtominaCraftV4.Windowing;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace AtominaCraftV4.Entities {
    public class EntityPlayer : Entity {
        public static Camera MainCamera;

        public readonly Camera camera;

        public Vector3f moveSpeed;

        public EntityPlayer() {
            this.camera = MainCamera = new Camera();
            this.moveSpeed = new Vector3f(10.0f);
        }

        public override void Update() {
            base.Update();

            float moveB = 0.0f, moveR = 0.0f, moveU = 0.0f;
            if (Keyboard.Instance[Keys.W]) {
                moveB -= 1.0f;
            }

            if (Keyboard.Instance[Keys.S]) {
                moveB += 1.0f;
            }

            if (Keyboard.Instance[Keys.A]) {
                moveR -= 1.0f;
            }

            if (Keyboard.Instance[Keys.D]) {
                moveR += 1.0f;
            }

            if (Keyboard.Instance[Keys.Space]) {
                moveU += 1.0f;
            }

            if (Mouse.Instance[MouseButton.Button5]) {
                moveU -= 1.0f;
            }

            if (Keyboard.Instance[Keys.LeftShift]) {
                this.moveSpeed = new Vector3f(25.0f);
            }
            else {
                this.moveSpeed = new Vector3f(10.0f);
            }

            Move(moveB, moveR, moveU);
            UpdateCamera();
        }

        public void Move(float back, float right, float up) {
            Matrix4 camToWorld = Matrix4.LocalToWorld(this.pos, this.euler, this.scale) * Matrix4.RotY(this.camera.rotateY);
            Vector3f lookDirection = camToWorld.MultiplyDirection(new Vector3f(right, up, back)).Normalise();
            Vector3f movement = lookDirection.GetNonNAN() * (this.moveSpeed * Delta.time);
            this.velocity += movement;
        }

        public void UpdateCamera() {
            this.camera.pos = this.pos;
            this.camera.Update();
        }
    }
}