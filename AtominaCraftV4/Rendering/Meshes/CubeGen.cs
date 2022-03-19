using System.Collections.Generic;
using System.Numerics;
using System.Security;
using AtominaCraftV4.REghZy.MathsF;
using AtominaCraftV4.Rendering.Meshes.Generator;

namespace AtominaCraftV4.Rendering.Meshes {
    public class CubeGen {
        public static MeshWithIndexBuffer GenerateCube(bool top, bool bottom, bool north, bool east, bool south, bool west) {
            List<Vertex> vertices = new List<Vertex>();
            vertices.Add(new Vertex(new Vector3f( 1.0f,  1.0f, -1.0f), new Vector3f( 0.0f,  0.0f, -1.0f), new Vector2f(0.0f, 0.0f)));
            vertices.Add(new Vertex(new Vector3f( 1.0f, -1.0f, -1.0f), new Vector3f( 0.0f,  0.0f, -1.0f), new Vector2f(0.0f, 0.0f)));
            vertices.Add(new Vertex(new Vector3f( 1.0f,  1.0f,  1.0f), new Vector3f( 0.0f,  0.0f,  1.0f), new Vector2f(0.0f, 0.0f)));
            vertices.Add(new Vertex(new Vector3f( 1.0f, -1.0f,  1.0f), new Vector3f( 0.0f,  0.0f,  1.0f), new Vector2f(0.0f, 0.0f)));
            vertices.Add(new Vertex(new Vector3f(-1.0f,  1.0f, -1.0f), new Vector3f( 0.0f,  0.0f, -1.0f), new Vector2f(0.0f, 0.0f)));
            vertices.Add(new Vertex(new Vector3f(-1.0f, -1.0f, -1.0f), new Vector3f( 0.0f,  0.0f, -1.0f), new Vector2f(0.0f, 0.0f)));
            vertices.Add(new Vertex(new Vector3f(-1.0f,  1.0f,  1.0f), new Vector3f( 0.0f,  0.0f,  1.0f), new Vector2f(0.0f, 0.0f)));
            vertices.Add(new Vertex(new Vector3f(-1.0f, -1.0f,  1.0f), new Vector3f( 0.0f,  0.0f,  1.0f), new Vector2f(0.0f, 0.0f)));

            List<uint> indices = new List<uint>();
            void AddVertex(params uint[] indexes) {
                foreach (uint index in indexes) {
                    indices.Add(index - 1);
                }
            }

            if (top) {
                AddVertex(1, 5, 7, 1, 7, 3);
            }

            if (north) {
                AddVertex(5, 1, 6, 1, 2, 6);
            }

            if (east) {
                AddVertex(1, 3, 4, 1, 4, 2);
            }

            if (south) {
                AddVertex(3, 7, 8, 3, 8, 4);
            }

            if (west) {
                AddVertex(7, 5, 6, 7, 6, 8);
            }

            if (bottom) {
                AddVertex(6, 2, 8, 2, 4, 8);
            }

            return new MeshWithIndexBuffer(vertices, indices, null);
        }

        public static MeshWithIndexBuffer GeneratePlane() {
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();

            vertices.Add(new Vertex(new Vector3f(-1.0f, 0.0f, -1.0f), new Vector3f(0.0f, 1.0f, 0.0f)));
            vertices.Add(new Vertex(new Vector3f(1.0f, 0.0f, -1.0f), new Vector3f(0.0f, 1.0f, 0.0f)));
            vertices.Add(new Vertex(new Vector3f(1.0f, 0.0f, 1.0f), new Vector3f(0.0f, 1.0f, 0.0f)));
            vertices.Add(new Vertex(new Vector3f(-1.0f, 0.0f, 1.0f), new Vector3f(0.0f, 1.0f, 0.0f)));

            void add(params uint[] indexes) {
                foreach (uint index in indexes) {
                    indices.Add(index);
                }
            }

            add(1, 0, 3, 1, 3, 2);
            return new MeshWithIndexBuffer(vertices, indices, null);
        }

        public static MeshWithIndexBuffer GeneratePlaneAlongZ() {
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();

            vertices.Add(new Vertex(new Vector3f(-1.0f,  1.0f, 0.0f), new Vector3f(0.0f, 0.0f, 1.0f)));
            vertices.Add(new Vertex(new Vector3f( 1.0f,  1.0f, 0.0f), new Vector3f(0.0f, 0.0f, 1.0f)));
            vertices.Add(new Vertex(new Vector3f( 1.0f, -1.0f, 0.0f), new Vector3f(0.0f, 0.0f, 1.0f)));
            vertices.Add(new Vertex(new Vector3f(-1.0f, -1.0f, 0.0f), new Vector3f(0.0f, 0.0f, 1.0f)));

            void add(params uint[] indexes) {
                foreach (uint index in indexes) {
                    indices.Add(index);
                }
            }

            add(1, 0, 3, 1, 3, 2);
            return new MeshWithIndexBuffer(vertices, indices, null);
        }

        public static Mesh GenerateCubeWithoutEBO(bool top, bool bottom, bool north, bool east, bool south, bool west, int u = 1, int v = 1) {
            List<Vertex> vertices = new List<Vertex>();

            void LoadVertices(IEnumerable<Vertex> verts) {
                foreach (Vertex vertex in verts) {
                    vertices.Add(new Vertex(vertex.pos, vertex.normal, new Vector2f(vertex.uv.x * u, vertex.uv.y * v)));
                }
            }

            if (top) {
                LoadVertices(BlockFace.U.GenerateVertices());
            }

            if (bottom) {
                LoadVertices(BlockFace.D.GenerateVertices());
            }

            if (north) {
                LoadVertices(BlockFace.N.GenerateVertices());
            }

            if (east) {
                LoadVertices(BlockFace.E.GenerateVertices());
            }

            if (south) {
                LoadVertices(BlockFace.S.GenerateVertices());
            }

            if (west) {
                LoadVertices(BlockFace.W.GenerateVertices());
            }

            return new Mesh(vertices);
        }
    }
}