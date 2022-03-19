using AtominaCraftV4.REghZy.MathsF;
using AtominaCraftV4.Rendering.Meshes;
using AtominaCraftV4.Rendering.Shaders;
using OpenTK.Graphics.OpenGL;

namespace AtominaCraftV4.Rendering {
    public class Sky {
        public readonly MeshWithIndexBuffer meshWithIndexBuffer;
        public readonly Shader shader;

        public Sky() {
            this.meshWithIndexBuffer = CubeGen.GeneratePlaneAlongZ();
            this.shader = Shader.shaders["sky"];
        }

        public void Draw(Camera cam) {
            GL.DepthMask(false);
            Matrix4 mvp = cam.proj.Inversed;
            Matrix4 mv = cam.view.Inversed;
            this.shader.Use();
            this.shader.SetUniformMatrix4("mvp", mvp);
            this.shader.SetUniformMatrix4("mv", mv);
            this.meshWithIndexBuffer.Draw();
            GL.DepthMask(true);
        }
    }
}