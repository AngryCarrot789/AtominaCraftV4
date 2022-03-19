using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using AtominaCraftV4.Entities;
using AtominaCraftV4.REghZy.MathsF;

namespace AtominaCraftV4.Rendering.Renderers {
    public abstract class EntityRenderer<T> : EntityRenderer where T : Entity {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override void Render(Entity entity, Camera camera) {
            Render((T) entity, camera);
        }

        public abstract void Render(T entity, Camera camera);
    }

    public abstract class EntityRenderer {
        private static readonly Dictionary<Type, EntityRenderer> Renderers;

        static EntityRenderer() {
            Renderers = new Dictionary<Type, EntityRenderer>();
        }

        public abstract void Render(Entity entity, Camera camera);

        public static void RegisterEntity(Type entity, EntityRenderer renderer) {
            Renderers[entity] = renderer;
        }

        public static void RegisterEntity<T>(EntityRenderer renderer) where T : Entity {
            Renderers[typeof(T)] = renderer;
        }

        public static Matrix4 GetEntityWorldView(Entity entity) {
            return Matrix4.LocalToWorld(entity.pos, entity.euler, entity.scale);
        }

        public static Matrix4 GetEntityLocalView(EntityCube entity) {
            return Matrix4.WorldToLocal(entity.pos, entity.euler, entity.scale);
        }

        public static void RenderEntity(Entity entity, Camera camera) {
            if (Renderers.TryGetValue(entity.GetType(), out EntityRenderer renderer)) {
                try {
                    renderer.Render(entity, camera);
                }
                catch (Exception e) {
                    throw new Exception($"Failed to render entity at {entity.pos}: {entity}", e);
                }
            }
        }
    }
}