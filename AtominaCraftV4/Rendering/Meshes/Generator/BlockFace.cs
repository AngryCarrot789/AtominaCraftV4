using System.Collections.Generic;
using AtominaCraftV4.REghZy.MathsF;

namespace AtominaCraftV4.Rendering.Meshes.Generator {
    public readonly struct BlockFace {
        public static readonly Vector2f T1  = new Vector2f(0.0f, 0.0f);
        public static readonly Vector2f T2  = new Vector2f(1.0f, 0.0f);
        public static readonly Vector2f T3  = new Vector2f(1.0f, 1.0f);
        public static readonly Vector2f T4  = new Vector2f(0.0f, 1.0f);

        public static readonly BlockFace N = new BlockFace(TriangleFace.N_1, TriangleFace.N_2);
        public static readonly BlockFace S = new BlockFace(TriangleFace.S_1, TriangleFace.S_2);
        public static readonly BlockFace E = new BlockFace(TriangleFace.E_1, TriangleFace.E_2);
        public static readonly BlockFace W = new BlockFace(TriangleFace.W_1, TriangleFace.W_2);
        public static readonly BlockFace U = new BlockFace(TriangleFace.U_1, TriangleFace.U_2);
        public static readonly BlockFace D = new BlockFace(TriangleFace.D_1, TriangleFace.D_2);

        public readonly TriangleFace f1;
        public readonly TriangleFace f2;
        public readonly Vector3f normal;

        public BlockFace(TriangleFace f1, TriangleFace f2) {
            this.f1 = f1;
            this.f2 = f2;
            this.normal = f1.normal;
        }

        public IEnumerable<Vertex> GenerateVertices() {
            yield return new Vertex(this.f1.v1, this.normal, T2);
            yield return new Vertex(this.f1.v2, this.normal, T1);
            yield return new Vertex(this.f1.v3, this.normal, T4);
            yield return new Vertex(this.f2.v1, this.normal, T2);
            yield return new Vertex(this.f2.v2, this.normal, T4);
            yield return new Vertex(this.f2.v3, this.normal, T3);
        }
    }
}