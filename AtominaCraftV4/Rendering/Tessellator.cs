using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using AtominaCraftV4.Blocks;
using AtominaCraftV4.Entities;
using AtominaCraftV4.REghZy.MathsF;
using AtominaCraftV4.Rendering.Meshes;
using AtominaCraftV4.Rendering.Meshes.Generator;
using AtominaCraftV4.Rendering.Shaders;
using AtominaCraftV4.Utils;
using AtominaCraftV4.Worlding.Chunking;
using Microsoft.Win32.SafeHandles;
using OpenTK.Graphics.OpenGL;
using REghZy.Streams;
using Buffer = System.Buffer;

namespace AtominaCraftV4.Rendering {
    public static class Tessellator {
        private static readonly int vao;
        private static readonly int vbo;
        private static readonly Shader Shader;

        public const int MASK_UP    = 0b000001;
        public const int MASK_DOWN  = 0b000010;
        public const int MASK_NORTH = 0b000100;
        public const int MASK_EAST  = 0b001000;
        public const int MASK_SOUTH = 0b010000;
        public const int MASK_WEST  = 0b100000;
        public const int MASK_ALL   = 0b111111;

        static Tessellator() {
            unsafe {
                List<Vertex> vertices = new List<Vertex>();
                vertices.AddRange(BlockFace.U.GenerateVertices());
                vertices.AddRange(BlockFace.D.GenerateVertices());
                vertices.AddRange(BlockFace.N.GenerateVertices());
                vertices.AddRange(BlockFace.E.GenerateVertices());
                vertices.AddRange(BlockFace.S.GenerateVertices());
                vertices.AddRange(BlockFace.W.GenerateVertices());

                vao = GL.GenVertexArray();
                GL.BindVertexArray(vao);

                // Generate vertex buffer
                vbo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(Vertex), vertices.ToArray(), BufferUsageHint.StaticDraw);

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

                Shader = Shader.shaders["textureatlas"];
            }
        }

        public static void DrawChunk(Chunk chunk) {
            ChunkBlockStorage storage = chunk.storage;
            for (int i = 0; i < 16; i++) {
                ChunkBlockStorage.StorageSection a = storage.sections[i];
                if (a != null) {
                    for (int j = 0; j < 16; j++) {
                        BlockData[] layer = a.layers[j];
                        if (layer != null) {
                            DrawLayer(chunk, layer, (i << 4) + j);
                        }
                    }
                }
            }
        }

        public static void DrawLayer(Chunk chunk, BlockData[] layer, int y) {
            int cx = chunk.x << 4;
            int cz = chunk.z << 4;
            for (int i = 0; i < layer.Length; i++) {
                BlockData data = layer[i];
                if (data.IsEmpty) {
                    continue;
                }

                int x = cx + (i & 15);
                int z = cz + ((i >> 4) & 15);
                DrawCube(chunk, x, y, z, data);
            }
        }

        public static void DrawCube(Chunk chunk, int x, int y, int z, in BlockData data) {
            Vector3f pos = new Vector3f(x, y, z);
            Block block = Block.Blocks[data.id];
            if (block == null) {
                return;
            }

            Vector3f scale = new Vector3f(0.5f);
            Matrix4 mv = Matrix4.WorldToLocal(pos, Vector3f.Zero, scale).Transposed;
            Matrix4 mvp = EntityPlayer.MainCamera.matrix * Matrix4.LocalToWorld(pos, Vector3f.Zero, scale);
            Shader.Use();

            // speed boost to draw all faces at once if they all match, rather than individuals
            bool useUvs = !block.AllTextureAreTheSame;
            if (!useUvs) {
                Shader.SetUniformVec2("uv_offset", new Vector2f(block.topTexture, 0));
            }

            Shader.SetUniformMatrix4("mv", mv);
            Shader.SetUniformMatrix4("mvp", mvp);

            GL.BindVertexArray(vao);
            int visibility = data.visibility;
            if ((visibility & MASK_ALL) == MASK_ALL && !useUvs) {
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }
            else {
                if ((visibility & MASK_UP) != 0) {
                    if (useUvs) {
                        Shader.SetUniformVec2("uv_offset", new Vector2f(block.topTexture, 0));
                    }

                    GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                }

                if ((visibility & MASK_DOWN) != 0) {
                    if (useUvs) {
                        Shader.SetUniformVec2("uv_offset", new Vector2f(block.bottomTexture, 0));
                    }

                    GL.DrawArrays(PrimitiveType.Triangles, 6, 6);
                }

                if ((visibility & MASK_NORTH) != 0) {
                    if (useUvs) {
                        Shader.SetUniformVec2("uv_offset", new Vector2f(block.northTexture, 0));
                    }

                    GL.DrawArrays(PrimitiveType.Triangles, 12, 6);
                }

                if ((visibility & MASK_EAST) != 0) {
                    if (useUvs) {
                        Shader.SetUniformVec2("uv_offset", new Vector2f(block.eastTexture, 0));
                    }

                    GL.DrawArrays(PrimitiveType.Triangles, 18, 6);
                }

                if ((visibility & MASK_SOUTH) != 0) {
                    if (useUvs) {
                        Shader.SetUniformVec2("uv_offset", new Vector2f(block.southTexture, 0));
                    }

                    GL.DrawArrays(PrimitiveType.Triangles, 24, 6);
                }

                if ((visibility & MASK_WEST) != 0) {
                    if (useUvs) {
                        Shader.SetUniformVec2("uv_offset", new Vector2f(block.westTexture, 0));
                    }

                    GL.DrawArrays(PrimitiveType.Triangles, 30, 6);
                }
            }
        }
    }
}