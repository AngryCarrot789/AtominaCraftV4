using System.Collections.Generic;
using System.Numerics;
using AtominaCraftV4.REghZy.MathsF;
using OpenTK.Graphics.OpenGL;

namespace AtominaCraftV4.Rendering.Meshes {
    public class MeshWithIndexBuffer {
        public readonly List<Vertex>  vertices;
        public readonly List<uint>    indices;
        public readonly List<Texture> textures;

        public readonly int vao; // vertex array object ID
        public readonly int vbo; // vertex buffer object ID
        public readonly int ebo; // indices buffer object ID

        public BeginMode beginMode;

        public MeshWithIndexBuffer(List<Vertex> vertices, List<uint> indices, List<Texture> textures) {
            unsafe {
                this.vertices = vertices;
                this.indices = indices;
                this.textures = textures;

                this.vao = GL.GenVertexArray();
                this.vbo = GL.GenBuffer();
                this.ebo = GL.GenBuffer();

                GL.BindVertexArray(this.vao);

                GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(Vertex), this.vertices.ToArray(), BufferUsageHint.StaticDraw);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), this.indices.ToArray(), BufferUsageHint.StaticDraw);

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
            // uint diffuseNr = 1;
            // uint specularNr = 1;
            // for (uint i = 0; i < this.textures.Count; i++) {
            //     glActiveTexture(GL_TEXTURE0 + i); // activate proper texture unit before binding
            //     // retrieve texture number (the N in diffuse_textureN)
            //     string number;
            //     string name = textures[i].type;
            //     if (name == "texture_diffuse")
            //         number = std::to_string(diffuseNr++);
            //     else if (name == "texture_specular")
            //         number = std::to_string(specularNr++);
            //     shader.setFloat(("material." + name + number).c_str(), i);
            //     glBindTexture(GL_TEXTURE_2D, textures[i].id);
            // }
            // glActiveTexture(GL_TEXTURE0);

            GL.BindVertexArray(this.vao);
            GL.DrawElements(this.beginMode, this.indices.Count, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }
}