using System;
using System.Collections;
using System.Runtime.ExceptionServices;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using REghZy.Utils;

namespace AtominaCraftV4.Windowing {
    public class Keyboard {
        public static Keyboard Instance { get; private set; }

        private readonly BitArray keysDown = new BitArray(349);
        private readonly BitArray keysDownPrev = new BitArray(349);

        private readonly ACWindow window;
        private readonly GLFWCallbacks.KeyCallback keyPressCallback;

        public bool IsAnyKeyDown {
            get {
                for (int index = 0; index < this.keysDown.Length; ++index) {
                    if (this.keysDown[index])
                        return true;
                }

                return false;
            }
        }

        public bool this[Keys key] {
            get => IsKeyDown(key);
            private set => this.keysDown[(int) key] = value;
        }

        public Keyboard(ACWindow window) {
            Instance = this;
            this.window = window;

            unsafe {
                GLFW.SetKeyCallback(window.Handle, this.keyPressCallback = OnKeyPressed);
            }
        }

        private unsafe void OnKeyPressed(Window* window, Keys key, int scancode, InputAction action, KeyModifiers mods) {
            if (action == InputAction.Release) {
                if (key != Keys.Unknown) {
                    this[key] = false;
                }

                OnKeyUp(key, scancode, action, mods);
            }
            else {
                if (key != Keys.Unknown) {
                    this[key] = true;
                }

                OnKeyDown(key, scancode, action, mods);
            }
        }

        private void OnKeyUp(Keys key, int scancode, InputAction action, KeyModifiers modifiers) {

        }

        private void OnKeyDown(Keys key, int scancode, InputAction action, KeyModifiers modifiers) {

        }

        public void EndFrame() {
            this.keysDownPrev.SetAll(false);
            this.keysDownPrev.Or(this.keysDown);
        }

        public bool IsKeyDown(Keys key) => this.keysDown[(int) key];

        public bool WasKeyDown(Keys key) => this.keysDownPrev[(int) key];

        public bool IsKeyPressed(Keys key) => this.keysDown[(int) key] && !this.keysDownPrev[(int) key];

        public bool IsKeyReleased(Keys key) => !this.keysDown[(int) key] && this.keysDownPrev[(int) key];

        public void Dispose() {

        }
    }
}