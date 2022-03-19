using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using AtominaCraftV4.Blocks;
using AtominaCraftV4.Entities;
using AtominaCraftV4.REghZy.MathsF;
using AtominaCraftV4.Rendering.Meshes;
using AtominaCraftV4.Rendering.Meshes.Generator;
using AtominaCraftV4.Rendering.Shaders;
using AtominaCraftV4.Worlding.Chunking;
using OpenTK.Graphics.OpenGL;

namespace AtominaCraftV4.Rendering {
    public static class Tessellator {
        public static readonly int vao;
        public static readonly int vbo;
        public static readonly Shader TextureShader;

        public static readonly int UVOFF_LOC;
        public static readonly int MVP_LOC;
        public static readonly int MV_LOC;

        public const int MASK_UP = 0b000001; // Direction.UP   .BitMask
        public const int MASK_DOWN = 0b000010; // Direction.DOWN .BitMask
        public const int MASK_NORTH = 0b000100; // Direction.NORTH.BitMask
        public const int MASK_EAST = 0b001000; // Direction.EAST .BitMask
        public const int MASK_SOUTH = 0b010000; // Direction.SOUTH.BitMask
        public const int MASK_WEST = 0b100000; // Direction.WEST .BitMask
        public const int MASK_ALL = 0b111111;

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

                TextureShader = Shader.shaders["textureatlas"];
                UVOFF_LOC = GL.GetUniformLocation(TextureShader.ProgramId, "uv_offset");
                MVP_LOC = GL.GetUniformLocation(TextureShader.ProgramId, "mvp");
                MV_LOC = GL.GetUniformLocation(TextureShader.ProgramId, "mv");
            }
        }

        public static void DrawChunk(Chunk chunk) {
            ChunkBlockStorage storage = chunk.storage;
            for (int i = 0; i < 16; i++) {
                int y = i << 4;
                ChunkBlockStorage.StorageSection a = storage.sections[i];
                if (a != null) {
                    for (int j = 0; j < 16; j++) {
                        BlockState[] layer = a.layers[j];
                        if (layer != null) {
                            DrawLayer(chunk, layer, y + j);
                        }
                    }
                }
            }
        }

        public static void DrawLayer(Chunk chunk, BlockState[] layer, int y) {
            int cx = chunk.x << 4;
            int cz = chunk.z << 4;
            for (int i = 0; i < 256; i++) {
                BlockState state = layer[i];
                if (state.IsEmpty) {
                    continue;
                }

                int x = cx + (i & 15);
                int z = cz + ((i >> 4) & 15);
                DrawCube(x, y, z, state);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static void DrawCube(int x, int y, int z, in BlockState state) {
            Block block = Block.Blocks[state.id];
            if (block == null) {
                return;
            }

            Vector3f pos = new Vector3f(x + 0.5f, y + 0.5f, z + 0.5f);
            Vector3f scale = new Vector3f(0.5f);
            Matrix4 mv = Matrix4.WorldToLocal(pos, Vector3f.Zero, scale).Transposed;
            Matrix4 mvp = EntityPlayer.MainCamera.matrix * Matrix4.LocalToWorld(pos, Vector3f.Zero, scale);

            // reduce ldsfld... i hope
            Shader shader = TextureShader;
            // speed boost to draw all faces at once if they all match, rather than individuals
            shader.SetUniformMatrix4(MV_LOC, mv);
            shader.SetUniformMatrix4(MVP_LOC, mvp);

            int visibility = state.Visibility;
            if ((visibility & MASK_UP) != 0) {
                shader.SetUniformVec2(UVOFF_LOC, GetUVOffsetForBlock(block, 0));
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            }

            if ((visibility & MASK_DOWN) != 0) {
                shader.SetUniformVec2(UVOFF_LOC, GetUVOffsetForBlock(block, 1));
                GL.DrawArrays(PrimitiveType.Triangles, 6, 6);
            }

            if ((visibility & MASK_NORTH) != 0) {
                shader.SetUniformVec2(UVOFF_LOC, GetUVOffsetForBlock(block, 2));
                GL.DrawArrays(PrimitiveType.Triangles, 12, 6);
            }

            if ((visibility & MASK_EAST) != 0) {
                shader.SetUniformVec2(UVOFF_LOC, GetUVOffsetForBlock(block, 3));
                GL.DrawArrays(PrimitiveType.Triangles, 18, 6);
            }

            if ((visibility & MASK_SOUTH) != 0) {
                shader.SetUniformVec2(UVOFF_LOC, GetUVOffsetForBlock(block, 4));
                GL.DrawArrays(PrimitiveType.Triangles, 24, 6);
            }

            if ((visibility & MASK_WEST) != 0) {
                shader.SetUniformVec2(UVOFF_LOC, GetUVOffsetForBlock(block, 5));
                GL.DrawArrays(PrimitiveType.Triangles, 30, 6);
            }
        }

        // [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        // public static void DrawCube(int x, int y, int z, in BlockState state) {
        //     Block block = Block.Blocks[state.id];
        //     if (block == null) {
        //         return;
        //     }
        // 
        //     Vector3f pos = new Vector3f(x + 0.5f, y + 0.5f, z + 0.5f);
        //     Vector3f scale = new Vector3f(0.5f);
        //     Matrix4 mv = Matrix4.WorldToLocal(pos, Vector3f.Zero, scale).Transposed;
        //     Matrix4 mvp = EntityPlayer.MainCamera.matrix * Matrix4.LocalToWorld(pos, Vector3f.Zero, scale);
        // 
        //     // reduce ldsfld... i hope
        //     Shader shader = TextureShader;
        //     // speed boost to draw all faces at once if they all match, rather than individuals
        //     bool useUvs = !block.AllTextureAreTheSame;
        //     if (!useUvs) {
        //         shader.SetUniformVec2(UVOFF_LOC, new Vector2f(block.topTexture, 0));
        //     }
        // 
        //     shader.SetUniformMatrix4(MV_LOC, mv);
        //     shader.SetUniformMatrix4(MVP_LOC, mvp);
        // 
        //     int visibility = state.Visibility;
        //     if ((visibility & MASK_ALL) == MASK_ALL && !useUvs) {
        //         GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        //     }
        //     else {
        //         if ((visibility & MASK_UP) != 0) {
        //             if (useUvs) {
        //                 shader.SetUniformVec2(UVOFF_LOC, GetUVOffsetForBlock(block, 0));
        //             }
        // 
        //             GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        //         }
        // 
        //         if ((visibility & MASK_DOWN) != 0) {
        //             if (useUvs) {
        //                 shader.SetUniformVec2(UVOFF_LOC, GetUVOffsetForBlock(block, 1));
        //             }
        // 
        //             GL.DrawArrays(PrimitiveType.Triangles, 6, 6);
        //         }
        // 
        //         if ((visibility & MASK_NORTH) != 0) {
        //             if (useUvs) {
        //                 shader.SetUniformVec2(UVOFF_LOC, GetUVOffsetForBlock(block, 2));
        //             }
        // 
        //             GL.DrawArrays(PrimitiveType.Triangles, 12, 6);
        //         }
        // 
        //         if ((visibility & MASK_EAST) != 0) {
        //             if (useUvs) {
        //                 shader.SetUniformVec2(UVOFF_LOC, GetUVOffsetForBlock(block, 3));
        //             }
        // 
        //             GL.DrawArrays(PrimitiveType.Triangles, 18, 6);
        //         }
        // 
        //         if ((visibility & MASK_SOUTH) != 0) {
        //             if (useUvs) {
        //                 shader.SetUniformVec2(UVOFF_LOC, GetUVOffsetForBlock(block, 4));
        //             }
        // 
        //             GL.DrawArrays(PrimitiveType.Triangles, 24, 6);
        //         }
        // 
        //         if ((visibility & MASK_WEST) != 0) {
        //             if (useUvs) {
        //                 shader.SetUniformVec2(UVOFF_LOC, GetUVOffsetForBlock(block, 5));
        //             }
        // 
        //             GL.DrawArrays(PrimitiveType.Triangles, 30, 6);
        //         }
        //     }
        // }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static Vector2f GetUVOffsetForBlock(Block block, int direction) {
            switch (direction) {
                case 0: return new Vector2f(block.topTexture, 0);
                case 1: return new Vector2f(block.bottomTexture, 0);
                case 2: return new Vector2f(block.northTexture, 0);
                case 3: return new Vector2f(block.eastTexture, 0);
                case 4: return new Vector2f(block.southTexture, 0);
                case 5: return new Vector2f(block.westTexture, 0);
                default: throw new Exception($"Invalid direction: {direction}");
            }
        }
    }
}