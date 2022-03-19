using AtominaCraftV4.REghZy.MathsF;

namespace AtominaCraftV4.Entities {
    public class Entity {
        public Vector3f pos;
        public Vector3f scale;
        public Vector3f euler;
        public Vector3f velocity;

        public Entity() {
            this.pos = Vector3f.Zero;
            this.scale = Vector3f.One;
            this.euler = Vector3f.Zero;
        }

        public virtual void Update() {
            this.pos += this.velocity;
            this.velocity = Vector3f.Zero;
        }
    }
}