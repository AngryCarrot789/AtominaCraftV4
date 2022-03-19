using System;
using System.Security;
using System.Threading;
using AtominaCraftV4.Blocks;
using AtominaCraftV4.Entities;
using AtominaCraftV4.Logs;
using AtominaCraftV4.REghZy.MathsF;
using AtominaCraftV4.Rendering;
using AtominaCraftV4.Rendering.Meshes;
using AtominaCraftV4.Rendering.Renderers;
using AtominaCraftV4.Rendering.Shaders;
using AtominaCraftV4.Windowing;
using AtominaCraftV4.Worlding;
using AtominaCraftV4.Worlding.Chunking;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using REghZy.Collections.Dictionaries;
using REghZy.Utils;
using ErrorCode = OpenTK.Windowing.GraphicsLibraryFramework.ErrorCode;
using Quaternion = AtominaCraftV4.REghZy.MathsF.Quaternion;

namespace AtominaCraftV4 {
    public class AtominaCraft {
        private long initTime;
        private volatile bool isRunning;
        private volatile bool isShuttingDown;

        public double deltaTime;
        private long totalAppTicks;
        private long totalRenderTicks;

        public const bool UseWireframe = false;

        private static TaskManager tasks;
        public static TaskManager Tasks => tasks;
        public bool IsRunning => this.isRunning;
        public readonly ILogger logger;
        private ACWindow window;

        private World world;
        private EntityPlayer player;
        private Sky sky;

        public AtominaCraft() {
            this.logger = ConsoleLogManager.GetLogger();
            tasks = new TaskManager();
        }

        public void SetupGame() {
            this.world = new World("world");
            this.sky = new Sky();
            this.player = new EntityPlayer();
            this.world.entities.Add(this.player);

            EntityCube cube = new EntityCube();
            cube.pos = new Vector3f(32f, 20f, 32f);
            cube.scale = new Vector3f(3f);
            cube.AllFacesVisible = true;
            this.world.entities.Add(cube);

            this.player.pos = new Vector3f(40f, 20f, 40f);
            this.player.camera.yaw = Rotation.DegreesToRadians(45);
            this.player.camera.pitch = Rotation.DegreesToRadians(-25);

            for (int z = -1; z <= 1; z++) {
                for (int x = -1; x <= 1; x++) {
                    for (int y = 0; y < 5; y++) {
                        GenerateChunk(this.world.GetChunkAt(x, z), y);
                    }
                }
            }
        }

        private void GenerateChunk(Chunk chunk, int y) {
            for (int x = 0; x < 16; x++) {
                for (int z = 0; z < 16; z++) {
                    chunk.SetBlockIdMeta(x, y, z, Block.GRASS.id, 0);
                }
            }
        }

        public void SetupRender() {
            ACWindow.MainWindow.setCursorCaptured();
        }

        public void InitGL() {
            try {
                GL.LoadBindings(new GLFWBindingsContext());
            }
            catch (Exception e) {
                throw new Exception("Failed to load GL11 bindings", e);
            }

            if (!GLFW.Init()) {
                throw new Exception("Failed to initialise GLFW");
            }

            GLFW.SetErrorCallback(ErrorCallback);

            NativeWindowSettings settings = new NativeWindowSettings();
            settings.Profile = ContextProfile.Compatability;
            this.window = new ACWindow(settings, "AtominaCraft!!! :)");
            this.window.MakeCurrent();
            this.window.Show();
            this.window.UseViewport();

            GL.ClearColor(0.2f, 0.4f, 0.8f, 1.0f);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.DepthMask(true);

            if (UseWireframe) {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                GL.ShadeModel(ShadingModel.Smooth);
            }
        }

        private void ErrorCallback(ErrorCode error, string description) {
            Console.WriteLine("FATAL ERROR!");
            Console.WriteLine("Error code: " + error);
            Console.WriteLine(description);
            DoShutdown();
        }

        public void Run() {
            this.initTime = Time.GetSystemMillis();

            InitGL();

            unsafe {
                GLFW.GetCursorPos(this.window.Handle, out double x, out double y);
                Mouse.Instance.Position = new Vector2f((float) x, (float) y);
                Mouse.Instance.PreviousPosition = new Vector2f((float) x, (float) y);
            }

            this.window.EndFrame();

            SetupGame();
            SetupRender();

            this.isRunning = true;
            this.logger.Info($"GuiGL successfully initialised in {Time.GetSystemMillis() - this.initTime} milliseconds");

            double tickStart = Time.GetSystemMillisD();
            double tickEnd = tickStart + 1.0d;

            try {
                TextureAtlas.Load();
                TextureAtlas.Use();
            }
            catch (Exception e) {
                throw new Exception("Failed to load texture atlas", e);
            }

            GL.BindVertexArray(Tessellator.vao);

            // TODO: maybe implement a tick catchup system, in case the app misses a tick for some reason. Should be fine though
            try {
                while (true) {
                    tickStart = Time.GetSystemMillisD();
                    Delta.time_d = (tickStart - tickEnd) / 1000.0d;
                    Delta.time = (float) Delta.time_d;

                    DoGlobalTick();

                    if (ACWindow.MainWindow.ShouldClose) {
                        BeginSafeShutdown();
                    }

                    if (this.isShuttingDown) {
                        DoShutdown();
                        return;
                    }

                    DoGlobalRender();
                    tickEnd = tickStart;
                }
            }
            catch (ThreadInterruptedException e) {
                throw new Exception("Engine thread was interrupted while sleeping", e);
            }
            catch (Exception e) {
                throw new Exception("Unexpected exception during tick", e);
            }
        }

        public void DoGlobalTick() {
            this.totalAppTicks++;
            ACWindow.MainWindow.EndFrame();
            GLFW.PollEvents();

            ACWindow.MainWindow.BeginFrame();
            tasks.ProcessQueue();
            if (ACWindow.MainWindow.Keyboard.IsKeyDown(Keys.Escape)) {
                BeginSafeShutdown();
                return;
            }

            try {
                DoGameUpdate();
            }
            catch (Exception e) {
                throw new Exception("Failed to update game", e);
            }
        }

        public void DoGlobalRender() {
            this.totalRenderTicks++;
            try {
                RenderEngine.BeginRenderWindow(ACWindow.MainWindow);
                DoGameRender();
                RenderEngine.EndRenderWindow(ACWindow.MainWindow);
            }
            catch (Exception e) {
                throw new Exception("Failed to render game", e);
            }
        }

        public static string EnsureStringLength(string value, int len = 10, char fill = ' ') {
            if (value.Length >= len) {
                return value.Substring(0, len);
            }
            else {
                return value + StringUtils.Repeat(fill, len - value.Length);
            }
        }

        public void DoGameUpdate() {
            this.world.Update();

            unsafe {
                Vector3f pos = (this.player.pos).Round(1);
                string a = EnsureStringLength(pos.x.ToString(), 6);
                string b = EnsureStringLength(pos.y.ToString(), 6);
                string c = EnsureStringLength(pos.z.ToString(), 6);
                GLFW.SetWindowTitle(this.window.Handle, $"AtominaCraft - {Math.Round(Delta.time, 5)} - {a} | {b} | {c}");
            }

        }

        public void DoGameRender() {
            // don't clear ColorBufferBit if using sky
            if (UseWireframe) {
                GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            }
            else {
                GL.Clear(ClearBufferMask.DepthBufferBit);
                this.sky.Draw(this.player.camera);
            }

            foreach (Entity entity in this.world.entities) {
                EntityRenderer.RenderEntity(entity, this.player.camera);
            }

            Tessellator.TextureShader.Use();
            foreach (LKDEntry<Chunk> entry in this.world.chunks) {
                Tessellator.DrawChunk(entry.value);
                // DrawChunkCenterOutline(this.player.camera, entry.value);
            }

            // DrawXYZ(EntityPlayer.MainCamera.proj, this.player.camera.yaw, this.player.camera.pitch);

            // Tessellator.DrawCube(new Vector3f(0, 0, 0), 2, 0b111111);
        }

        public void BeginSafeShutdown() {
            if (this.isRunning) {
                this.isShuttingDown = true;
            }
        }

        private void DoShutdown() {
            this.isShuttingDown = true;
            this.isRunning = false;
            this.logger.Info("Shutting down...");
            GLFW.Terminate();
        }

        private static void Vertex4(in Vector4f v) {
            GL.Vertex4(v.x, v.y, v.z, v.w);
        }

        public static void DrawChunkCenterOutline(Camera camera, Chunk chunk, float r = 0.2f, float g = 0.8f, float b = 0.3f) {
            Matrix4 mvp = camera.matrix * Matrix4.LocalToWorld(new Vector3f((chunk.x << 4) + 8.0f, 128.0f, (chunk.z << 4) + 8.0f), Vector3f.Zero, new Vector3f(8.0f, 128.0f, 8.0f));
            Vector4f v1 = mvp * new Vector4f(1.0f, 1.0f, -1.0f, 1.0f);
            Vector4f v2 = mvp * new Vector4f(1.0f, -1.0f, -1.0f, 1.0f);
            Vector4f v3 = mvp * new Vector4f(-1.0f, -1.0f, -1.0f, 1.0f);
            Vector4f v4 = mvp * new Vector4f(-1.0f, 1.0f, -1.0f, 1.0f);
            Vector4f v5 = mvp * new Vector4f(-1.0f, 1.0f, 1.0f, 1.0f);
            Vector4f v6 = mvp * new Vector4f(-1.0f, -1.0f, 1.0f, 1.0f);
            Vector4f v7 = mvp * new Vector4f(1.0f, -1.0f, 1.0f, 1.0f);
            Vector4f v8 = mvp * new Vector4f(1.0f, 1.0f, 1.0f, 1.0f);

            GL.DepthFunc(DepthFunction.Lequal);
            GL.UseProgram(0);

            GL.LineWidth(2);

            GL.Color3(r, g, b);
            GL.Begin(PrimitiveType.LineLoop);
            Vertex4(v1);
            Vertex4(v2);
            Vertex4(v3);
            Vertex4(v4);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            Vertex4(v4);
            Vertex4(v5);
            Vertex4(v6);
            Vertex4(v3);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            Vertex4(v6);
            Vertex4(v5);
            Vertex4(v8);
            Vertex4(v7);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            Vertex4(v8);
            Vertex4(v7);
            Vertex4(v2);
            Vertex4(v1);
            GL.End();

            GL.DepthFunc(DepthFunction.Less);
            GL.LineWidth(1);
        }

        public static void DrawXYZ(in Matrix4 proj, float rotX, float rotY) {
            Vector3f position = new Vector3f(0.0f, 0.0f, -1.0f);
            Vector3f scale = new Vector3f(0.1f);

            Quaternion y = Quaternion.AngleAxis(-rotY, Vector3f.Up);
            Quaternion x = Quaternion.AngleAxis(-rotX, Vector3f.Backward);
            Vector3f euler = (y * x).ToEuler();

            Matrix4 worldView = Matrix4.LocalToWorld(position, euler, scale);
            Matrix4 worldProjected = proj * worldView;
            Vector4f c = worldProjected * new Vector4f(0, 0, 0, 1);
            Vector4f xP = worldProjected * new Vector4f(1.5f, 0.0f, 0.0f, 1);
            Vector4f yP = worldProjected * new Vector4f(0.0f, 1.5f, 0.0f, 1);
            Vector4f zP = worldProjected * new Vector4f(0.0f, 0.0f, 1.5f, 1);
            Vector4f xN = worldProjected * new Vector4f(-0.5f, 0.0f, 0.0f, 1);
            Vector4f yN = worldProjected * new Vector4f(0.0f, -0.5f, 0.0f, 1);
            Vector4f zN = worldProjected * new Vector4f(0.0f, 0.0f, -0.5f, 1);

            GL.LineWidth(2);
            GL.DepthFunc(DepthFunction.Always);
            GL.UseProgram(0);

            // positives

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(1.0f, 0.0f, 0.0f);
            Vertex4(c);
            Vertex4(xP);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(0.0f, 1.0f, 0.0f);
            Vertex4(c);
            Vertex4(yP);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(0.0f, 0.0f, 1.0f);
            Vertex4(c);
            Vertex4(zP);
            GL.End();

            // negatives

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(0.5f, 0.0f, 0.0f);
            Vertex4(c);
            Vertex4(xN);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(0.0f, 0.5f, 0.0f);
            Vertex4(c);
            Vertex4(yN);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(0.0f, 0.0f, 0.5f);
            Vertex4(c);
            Vertex4(zN);
            GL.End();

            Vector4f v1 = worldProjected * new Vector4f(1.0f, 1.0f, -1.0f, 1.0f);
            Vector4f v2 = worldProjected * new Vector4f(1.0f, -1.0f, -1.0f, 1.0f);
            Vector4f v3 = worldProjected * new Vector4f(-1.0f, -1.0f, -1.0f, 1.0f);
            Vector4f v4 = worldProjected * new Vector4f(-1.0f, 1.0f, -1.0f, 1.0f);
            Vector4f v5 = worldProjected * new Vector4f(-1.0f, 1.0f, 1.0f, 1.0f);
            Vector4f v6 = worldProjected * new Vector4f(-1.0f, -1.0f, 1.0f, 1.0f);
            Vector4f v7 = worldProjected * new Vector4f(1.0f, -1.0f, 1.0f, 1.0f);
            Vector4f v8 = worldProjected * new Vector4f(1.0f, 1.0f, 1.0f, 1.0f);

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(0.8f, 0.3f, 0.7f);
            Vertex4(v1);
            Vertex4(v2);
            Vertex4(v3);
            Vertex4(v4);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            Vertex4(v4);
            Vertex4(v5);
            Vertex4(v6);
            Vertex4(v3);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            Vertex4(v6);
            Vertex4(v5);
            Vertex4(v8);
            Vertex4(v7);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            Vertex4(v8);
            Vertex4(v7);
            Vertex4(v2);
            Vertex4(v1);
            GL.End();

            GL.DepthFunc(DepthFunction.Lequal);
            GL.LineWidth(1);
        }
    }
}
