using AtominaCraftV4.Entities;
using AtominaCraftV4.REghZy.MathsF;
using AtominaCraftV4.Rendering.Meshes;
using AtominaCraftV4.Rendering.Shaders;
using OpenTK.Graphics.OpenGL;

namespace AtominaCraftV4.Rendering.Renderers {
    public class RenderEntityCube : EntityRenderer<EntityCube> {
        private static readonly Shader CubeShader;
        private static readonly Mesh CubeMesh;

        static RenderEntityCube() {
            CubeMesh = CubeGen.GenerateCubeWithoutEBO(true, true, true, true, true, true);
            CubeShader = Shader.shaders["textureatlas"];
        }

        public const int ATLAS_ROWS = 32;

        public static Vector2f GetOffsetUV(EntityCube cube) {
            return new Vector2f(cube.textureId, 0);
        }

        public override void Render(EntityCube entity, Camera camera) {
            Matrix4 mv = GetEntityLocalView(entity).Transposed;
            Matrix4 mvp = camera.matrix * GetEntityWorldView(entity);
            CubeShader.Use();
            // CubeShader.SetUniformVec3("in_colour", entity.colour);
            CubeShader.SetUniformVec2("uv_offset", GetOffsetUV(entity));
            CubeShader.SetUniformMatrix4("mv", mv);
            CubeShader.SetUniformMatrix4("mvp", mvp);

            GL.BindVertexArray(CubeMesh.vao);
            if (entity.AllFacesVisible) {
                GL.DrawArrays(PrimitiveType.Triangles, 0, CubeMesh.vertexCount);
            }
            else {
                if (entity.TopVisible) {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                }

                if (entity.BottomVisible) {
                    GL.DrawArrays(PrimitiveType.Triangles, 6, 6);
                }

                if (entity.NorthVisible) {
                    GL.DrawArrays(PrimitiveType.Triangles, 12, 6);
                }

                if (entity.EastVisible) {
                    GL.DrawArrays(PrimitiveType.Triangles, 18, 6);
                }

                if (entity.SouthVisible) {
                    GL.DrawArrays(PrimitiveType.Triangles, 24, 6);
                }

                if (entity.WestVisible) {
                    GL.DrawArrays(PrimitiveType.Triangles, 30, 6);
                }
            }
        }
    }
}