using AtominaCraftV4.REghZy.MathsF;

namespace AtominaCraftV4.Rendering.Meshes {
    public struct Vertex {
        public Vector3f pos;    // vertex coordinate
        public Vector3f normal; // normal direction
        public Vector2f uv;     // texture coordinate

        public Vertex(Vector3f pos, Vector3f normal, Vector2f uv = default) {
            this.pos = pos;
            this.normal = normal;
            this.uv = uv;
        }

        public override string ToString() {
            return $"Vertex(Pos({this.pos.x}, {this.pos.y}, {this.pos.z}), Normal({this.normal.x}, {this.normal.y}, {this.normal.z}), UV({this.uv.x}, {this.uv.y}))";
        }
    }
}