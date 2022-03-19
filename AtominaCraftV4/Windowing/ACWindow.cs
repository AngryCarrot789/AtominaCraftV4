using System;
using System.Reflection;
using System.Threading;
using AtominaCraftV4.Entities;
using AtominaCraftV4.Rendering;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ErrorCode = OpenTK.Windowing.GraphicsLibraryFramework.ErrorCode;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;

namespace AtominaCraftV4.Windowing {
    public class ACWindow {
        private static ACWindow main;

        public static ACWindow MainWindow => main;

        private string title;
        private int width;
        private int height;
        private unsafe Window* handle;
        private readonly Keyboard keyboard;
        private readonly Mouse mouse;

        public string Title {
            get => this.title;
            set {
                unsafe {
                    GLFW.SetWindowTitle(this.handle, this.title = value);
                }
            }
        }

        public int Width {
            get => this.width;
            set {
                unsafe {
                    GLFW.SetWindowSize(this.handle, this.width = value, this.height);
                }
            }
        }

        public int Height {
            get => this.height;
            set {
                unsafe {
                    GLFW.SetWindowSize(this.handle, this.width, this.height = value);
                }
            }
        }

        public bool ShouldClose {
            get {
                unsafe {
                    return GLFW.WindowShouldClose(this.handle);
                }
            }
            set {
                unsafe {
                    GLFW.SetWindowShouldClose(this.handle, value);
                }
            }
        }

        public unsafe Window* Handle => this.handle;

        public Mouse Mouse => this.mouse;

        public Keyboard Keyboard => this.keyboard;

        private bool isCursorLatched;
        private bool isDisposed;
        private bool isOpen;
        private readonly ContextAPI API;
        private readonly Version APIVersion;
        private readonly bool _isFocused;
        private readonly GLFWCallbacks.WindowSizeCallback wndSizeCallback;

        public ContextProfile Profile { get; set; }

        public unsafe ACWindow(NativeWindowSettings settings, string title, int width = 1280, int height = 720) {
            main = this;
            this.API = settings.API;
            switch (settings.API) {
                case ContextAPI.NoAPI:
                    GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.NoApi);
                    break;
                case ContextAPI.OpenGLES:
                    GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlEsApi);
                    break;
                case ContextAPI.OpenGL:
                    GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlApi);
                    break;
            }

            GLFW.WindowHint(WindowHintInt.ContextVersionMajor, settings.APIVersion.Major);
            GLFW.WindowHint(WindowHintInt.ContextVersionMinor, settings.APIVersion.Minor);
            this.APIVersion = settings.APIVersion;
            this.Flags = settings.Flags;
            if ((settings.Flags & ContextFlags.ForwardCompatible) != 0)
                GLFW.WindowHint(WindowHintBool.OpenGLForwardCompat, true);
            if ((settings.Flags & ContextFlags.Debug) != 0)
                GLFW.WindowHint(WindowHintBool.OpenGLDebugContext, true);

            this.Profile = settings.Profile;
            switch (settings.Profile) {
                case ContextProfile.Any:
                    GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Any);
                    break;
                case ContextProfile.Compatability:
                    GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Compat);
                    break;
                case ContextProfile.Core:
                    GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            GLFW.WindowHint(WindowHintBool.Focused, settings.StartFocused);
            GLFW.WindowHint(WindowHintBool.Visible, true);
            GLFW.WindowHint(WindowHintInt.Samples, settings.NumberOfSamples);
            VideoMode* videoMode = GLFW.GetVideoMode(settings.CurrentMonitor.ToUnsafePtr<Monitor>());
            int? bits = settings.RedBits;
            GLFW.WindowHint(WindowHintInt.RedBits, bits ?? videoMode->RedBits);
            bits = settings.GreenBits;
            GLFW.WindowHint(WindowHintInt.GreenBits, bits ?? videoMode->GreenBits);
            bits = settings.BlueBits;
            GLFW.WindowHint(WindowHintInt.BlueBits, bits ?? videoMode->BlueBits);
            bits = settings.AlphaBits;
            if (bits.HasValue) {
                GLFW.WindowHint(WindowHintInt.AlphaBits, bits.Value);
            }

            GLFW.WindowHint(WindowHintInt.RefreshRate, videoMode->RefreshRate);
            IGLFWGraphicsContext sharedContext = settings.SharedContext;
            void* voidPtr = (void*) (sharedContext != null ? sharedContext.WindowPtr : IntPtr.Zero);
            this.handle = GLFW.CreateWindow(width, height, title, (Monitor*) IntPtr.Zero, (Window*) voidPtr);

            bits = settings.DepthBits;
            if (bits.HasValue) {
                GLFW.WindowHint(WindowHintInt.DepthBits, bits.Value);
            }

            bits = settings.StencilBits;
            if (bits.HasValue) {
                GLFW.WindowHint(WindowHintInt.StencilBits, bits.Value);
            }

            if (settings.API != ContextAPI.NoAPI)
                this.Context = new GLFWGraphicsContext(this.handle);
            this.Context?.MakeCurrent();
            GLFWBindingsContext provider;
            try {
                Assembly assembly = Assembly.Load("OpenTK.Graphics");

                void LoadBindings(string typeNamespace) {
                    Type type = assembly.GetType("OpenTK.Graphics." + typeNamespace + ".GL");
                    if (type != null) {
                        type.GetMethod("LoadBindings")?.Invoke(null, new object[1] {provider});
                    }
                }

                provider = new GLFWBindingsContext();
                LoadBindings("ES11");
                LoadBindings("ES20");
                LoadBindings("ES30");
                LoadBindings("OpenGL");
                LoadBindings("OpenGL4");
            }
            catch {
                return;
            }

            GLFW.SetInputMode(this.handle, LockKeyModAttribute.LockKeyMods, true);
            this.title = title;
            this.width = width;
            this.height = height;

            if (this.handle == null) {
                ErrorCode code = GLFW.GetError(out string err);
                throw new Exception($"Failed to create window (err code {code}): {err}");
            }

            GLFW.SetWindowSizeCallback(this.handle, this.wndSizeCallback = (window, w, h) => {
                if (window == this.handle) {
                    this.width = w;
                    this.height = h;
                    UseViewport();
                    if (EntityPlayer.MainCamera != null) {
                        EntityPlayer.MainCamera.UpdateProjection();
                    }
                }
            });

            this.keyboard = new Keyboard(this);
            this.mouse = new Mouse(this);
        }

        public IGLFWGraphicsContext Context { get; set; }

        private void Focus() {
            unsafe {
                GLFW.FocusWindow(this.handle);
            }
        }

        public ContextFlags Flags { get; set; }

        public void setCursorCaptured() {
            unsafe {
                GLFW.SetInputMode(this.handle, CursorStateAttribute.Cursor, CursorModeValue.CursorDisabled);
            }

            this.isCursorLatched = true;
        }

        public void setCursorUnCaptured() {
            unsafe {
                GLFW.SetInputMode(this.handle, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
            }

            this.isCursorLatched = false;
        }

        public void TickUpdate() {
            this.keyboard.UpdateFrame();
            this.mouse.UpdateFrame();
        }

        public void MakeCurrent() {
            unsafe {
                GLFW.MakeContextCurrent(this.handle);
            }
        }

        public void Show() {
            CheckNotDisposed();
            if (this.isOpen) {
                throw new Exception("Window is already open");
            }

            this.isOpen = true;
            unsafe {
                GLFW.ShowWindow(this.handle);
            }
        }

        private void MarkViewPortModified() {

        }

        public void Hide() {
            CheckNotDisposed();
            if (!this.isOpen) {
                throw new Exception("Window is not open");
            }

            this.isOpen = false;
            unsafe {
                GLFW.HideWindow(this.handle);
            }
        }

        public void Close() {
            setCursorUnCaptured();
            CheckNotDisposed();
            this.isDisposed = true;
            this.isOpen = false;
            unsafe {
                GLFW.SetWindowShouldClose(this.handle, true);
            }

            Destroy();
        }

        private void Destroy() {
            unsafe {
                this.mouse.Dispose();
                this.keyboard.Dispose();
                GLFW.DestroyWindow(this.handle);
            }
        }

        private void CheckNotDisposed() {
            if (this.isDisposed) {
                throw new Exception("Window is disposed");
            }
        }

        public void UseViewport() {
            GL.Viewport(0, 0, this.width, this.height);
        }

        public void OnMouseUp(MouseButton button, InputAction action, KeyModifiers mods) {

        }

        public void OnMouseDown(MouseButton button, InputAction action, KeyModifiers mods) {

        }
    }
}