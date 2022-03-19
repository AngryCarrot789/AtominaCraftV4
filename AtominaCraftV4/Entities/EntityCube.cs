using AtominaCraftV4.REghZy.MathsF;
using AtominaCraftV4.Rendering.Meshes;
using AtominaCraftV4.Rendering.Renderers;
using AtominaCraftV4.Rendering.Shaders;

namespace AtominaCraftV4.Entities {
    public class EntityCube : Entity {
        public Vector3f colour;
        public int textureId;

        public int visibility;

        private void SetFlag(int mask, bool state) {
            if (state) {
                this.visibility |= mask;
            }
            else {
                this.visibility &= ~mask;
            }
        }

        public bool TopVisible {
            get => (this.visibility & 0b000001) != 0;
            set => SetFlag(0b000001, value);
        }

        public bool BottomVisible {
            get => (this.visibility & 0b000010) != 0;
            set => SetFlag(0b000010, value);
        }

        public bool NorthVisible {
            get => (this.visibility & 0b000100) != 0;
            set => SetFlag(0b000100, value);
        }

        public bool SouthVisible {
            get => (this.visibility & 0b001000) != 0;
            set => SetFlag(0b001000, value);
        }

        public bool EastVisible {
            get => (this.visibility & 0b010000) != 0;
            set => SetFlag(0b010000, value);
        }

        public bool WestVisible {
            get => (this.visibility & 0b100000) != 0;
            set => SetFlag(0b100000, value);
        }

        public bool AllFacesVisible {
            get => this.visibility == 0b111111;
            set => this.visibility = value ? 0b111111 : 0b000000;
        }

        public EntityCube() {
            this.colour = new Vector3f(0.8f, 0.2f, 1.0f);
            this.textureId = 0;
        }

        static EntityCube() {
            EntityRenderer.RegisterEntity<EntityCube>(new RenderEntityCube());
        }
    }
}