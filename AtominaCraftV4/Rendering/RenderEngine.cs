using AtominaCraftV4.Windowing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace AtominaCraftV4.Rendering {
    public static class RenderEngine {
        public static void BeginRenderWindow(ACWindow window) {

        }

        public static void EndRenderWindow(ACWindow window) {
            unsafe {
                GLFW.SwapBuffers(window.Handle);
            }
        }
    }
}