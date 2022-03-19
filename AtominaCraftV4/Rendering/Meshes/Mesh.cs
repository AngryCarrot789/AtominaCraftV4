using System.Collections.Generic;
using System.Security;
using AtominaCraftV4.REghZy.MathsF;
using OpenTK.Graphics.OpenGL;

namespace AtominaCraftV4.Rendering.Meshes {
    public class Mesh {
        public readonly int vertexCount;
        public readonly int faceCount;
        public readonly List<Vertex>  vertices;

        public readonly int vao; // vertex array object ID
        public readonly int[] vbos;

        public BeginMode beginMode;

        public Mesh(List<Vertex> vertices) {
            unsafe {
                this.vertices = vertices;
                this.vertexCount = vertices.Count;
                this.faceCount = vertices.Count / 3;
                this.vbos = new int[3];

                this.vao = GL.GenVertexArray();
                GL.BindVertexArray(this.vao);

                // Generate vertex buffer
                this.vbos[0] = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbos[0]);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(Vertex), this.vertices.ToArray(), BufferUsageHint.StaticDraw);

                // // Generate texture buffer
                // this.vbos[1] = GL.GenBuffer();
                // GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbos[1]);
                // GL.BufferData(BufferTarget.ArrayBuffer, uvs.Count * sizeof(float), uvs.ToArray(), BufferUsageHint.StaticDraw);
                // GL.EnableVertexAttribArray(1);
                // GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

                // vertex positions
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), 0);

                // vertex normals
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), sizeof(Vector3f));

                // texture coordinates
                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), sizeof(Vector3f) * 2);

                GL.BindVertexArray(0);
            }

            this.beginMode = BeginMode.Triangles;
        }

        public void Draw() {
            GL.BindVertexArray(this.vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, this.vertexCount);
        }
    }
}