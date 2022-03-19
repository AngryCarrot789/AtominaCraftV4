using AtominaCraftV4.REghZy.MathsF;

namespace AtominaCraftV4.Rendering.Meshes.Generator {
    public readonly struct TriangleFace {
        /*

                V6 ____________________ V5
                  /|                  /|
                 / |                 / |
                /  |                /  |
               /   |               /   |
           V1 /___________________/ V2 |
              |                   |    |
              |    |              |    |
              | V7 | _ _ _ _ _ _  | _  | V8
              |    /              |    /
              |   /               |   /
              |  /                |  /
              | /                 | /
              |/__________________|/
             V4                   V3

         */

        // vertex<number>
        public static readonly Vector3f V1   = new Vector3f(-1.0f,  1.0f,  1.0f);
        public static readonly Vector3f V2   = new Vector3f( 1.0f,  1.0f,  1.0f);
        public static readonly Vector3f V3   = new Vector3f( 1.0f, -1.0f,  1.0f);
        public static readonly Vector3f V4   = new Vector3f(-1.0f, -1.0f,  1.0f);
        public static readonly Vector3f V5   = new Vector3f( 1.0f,  1.0f, -1.0f);
        public static readonly Vector3f V6   = new Vector3f(-1.0f,  1.0f, -1.0f);
        public static readonly Vector3f V7   = new Vector3f(-1.0f, -1.0f, -1.0f);
        public static readonly Vector3f V8   = new Vector3f( 1.0f, -1.0f, -1.0f);

        // normal_<direction>
        public static readonly Vector3f N_N   = new Vector3f( 0.0f,  0.0f, -1.0f);
        public static readonly Vector3f N_S   = new Vector3f( 0.0f,  0.0f,  1.0f);
        public static readonly Vector3f N_E   = new Vector3f( 1.0f,  0.0f,  0.0f);
        public static readonly Vector3f N_W   = new Vector3f(-1.0f,  0.0f,  0.0f);
        public static readonly Vector3f N_U   = new Vector3f( 0.0f,  1.0f,  0.0f);
        public static readonly Vector3f N_D   = new Vector3f( 0.0f, -1.0f,  0.0f);

        public static readonly TriangleFace N_1 = new TriangleFace(V6, V5, V8, N_N);
        public static readonly TriangleFace N_2 = new TriangleFace(V6, V8, V7, N_N);
        public static readonly TriangleFace S_1 = new TriangleFace(V2, V1, V4, N_S);
        public static readonly TriangleFace S_2 = new TriangleFace(V2, V4, V3, N_S);
        public static readonly TriangleFace E_1 = new TriangleFace(V5, V2, V3, N_E);
        public static readonly TriangleFace E_2 = new TriangleFace(V5, V3, V8, N_E);
        public static readonly TriangleFace W_1 = new TriangleFace(V1, V6, V7, N_W);
        public static readonly TriangleFace W_2 = new TriangleFace(V1, V7, V4, N_W);
        public static readonly TriangleFace U_1 = new TriangleFace(V5, V6, V1, N_U);
        public static readonly TriangleFace U_2 = new TriangleFace(V5, V1, V2, N_U);
        public static readonly TriangleFace D_1 = new TriangleFace(V7, V8, V3, N_D);
        public static readonly TriangleFace D_2 = new TriangleFace(V7, V3, V4, N_D);

        public readonly Vector3f v1;
        public readonly Vector3f v2;
        public readonly Vector3f v3;
        public readonly Vector3f normal;

        public TriangleFace(Vector3f v1, Vector3f v2, Vector3f v3, Vector3f normal) {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.normal = normal;
        }
    }
}