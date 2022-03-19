using System;
using System.Collections.Generic;
using System.Numerics;
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
            EntityCube cube = new EntityCube();
            cube.pos = new Vector3f(0, 0, -10);
            cube.textureId = 2;
            cube.AllFacesVisible = false;
            cube.TopVisible = true;
            cube.WestVisible = true;

            EntityCube floor = new EntityCube();
            floor.scale = new Vector3f(100.0f, 0.1f, 100.0f);
            floor.pos = new Vector3f(0.0f, -10.0f, 0.0f);
            floor.colour = new Vector3f(0.3f, 0.3f, 0.7f);
            floor.textureId = 0;
            floor.AllFacesVisible = true;

            this.world.entities.Add(this.player);
            this.world.entities.Add(cube);
            this.world.entities.Add(floor);

            int y = 0;
            for (int z = -1; z <= 1; z++) {
                for (int x = -1; x <= 1; x++) {
                    GenerateChunk(this.world.GetChunkAt(x, z), y);
                }

                ++y;
            }
        }

        private void GenerateChunk(Chunk chunk, int step) {
            int y = step;
            for (int z = 0; z < 16; z++) {
                for (int x = 0; x < 16; x++) {
                    chunk.SetBlockIdMeta(x, y, z, Block.GRASS.id, 0);
                }

                ++y;
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
            // settings.Profile = ContextProfile.Compatability;
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

            // GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            // GL.ShadeModel(ShadingModel.Smooth);
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

            this.window.TickUpdate();

            SetupGame();
            SetupRender();

            this.isRunning = true;
            this.logger.Info($"GuiGL successfully initialised in {Time.GetSystemMillis() - this.initTime} milliseconds");

            const double target_tickrate = 200;
            const double target_framerate = 90;
            const double delta_game_tick = 1000.0f / target_tickrate;
            const double delta_game_fps = 1000.0f / target_framerate;

            double time = 0.0d;
            double lastRender = Time.GetSystemMillisD();

            double appTickStart = 0.0d;
            double appTickEnd = 0.0d;
            double appInterval = 0.0d;
            double appDelay = 0.0d;
            double appNextTick = 0.0d;

            double renderTickStart = 0.0d;
            double renderTickEnd = lastRender;
            double renderInterval = 0.0d;
            double renderDelay = 0.0d;
            double renderNextTick = 0.0d;

            TextureAtlas.Use();

            // TODO: maybe implement a tick catchup system, in case the app misses a tick for some reason. Should be fine though
            try {
                while (true) {
                    time = Time.GetSystemMillisD();

                    // appTickStart = Time.GetSystemMillisD();
                    DoGlobalTick();
                    // appTickEnd = Time.GetSystemMillisD();
                    // appInterval = appTickEnd - appTickStart;
                    // appDelay = delta_game_tick - appInterval;
                    // appNextTick = Time.GetSystemMillisD() + appDelay;

                    if (ACWindow.MainWindow.ShouldClose) {
                        BeginSafeShutdown();
                    }

                    if (this.isShuttingDown) {
                        DoShutdown();
                        return;
                    }

                    // renderTickStart = Time.GetSystemMillisD();
                    lastRender = renderTickEnd;
                    DoGlobalRender();
                    renderTickEnd = Time.GetSystemMillisD();
                    // renderInterval = renderTickEnd - renderTickStart;
                    // renderDelay = delta_game_fps - renderInterval;
                    // renderNextTick = Time.GetSystemMillisD() + renderDelay;
                    double delta = (renderTickEnd - lastRender) / 1000.0d;
                    Delta.time_d = delta;
                    Delta.time = (float) delta;
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
            ACWindow.MainWindow.TickUpdate();
            GLFW.PollEvents();
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

        public void DoGameUpdate() {
            unsafe {
                GLFW.SetWindowTitle(this.window.Handle, $"AtominaCraft - {Math.Round(Delta.time, 5)}");
            }

            this.world.Update();
        }

        public void DoGameRender() {
            // don't clear ColorBufferBit if using sky
            GL.Clear(ClearBufferMask.DepthBufferBit);
            this.sky.Draw(this.player.camera);

            // GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            // GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // foreach (Entity entity in this.world.entities) {
            //     GL.PushMatrix();
            //     EntityRenderer.RenderEntity(entity, this.player.camera);
            //     GL.PopMatrix();
            // }

            foreach (LKDEntry<Chunk> entry in this.world.chunks) {
                Tessellator.DrawChunk(entry.value);
            }


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
    }
}
