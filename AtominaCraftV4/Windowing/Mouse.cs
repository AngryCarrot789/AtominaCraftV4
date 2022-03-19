using System;
using System.Collections;
using System.Numerics;
using AtominaCraftV4.REghZy.MathsF;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace AtominaCraftV4.Windowing {
    public class Mouse {
        public static Mouse Instance { get; private set; }

        private readonly ACWindow window;

        private readonly BitArray buttons = new BitArray(16);
        private readonly BitArray buttonsPrevious = new BitArray(16);
        private float mouseX;
        private float mouseY;
        private float prevMouseX;
        private float prevMouseY;

        public Vector2f scroll;
        public Vector2f previousScroll;

        private readonly GLFWCallbacks.MouseButtonCallback mbttnCallback;
        private readonly GLFWCallbacks.CursorPosCallback mposCallback;

        public bool IsAnyButtonDown {
            get {
                for (int index = 0; index < 16; ++index) {
                    if (this.buttons[index])
                        return true;
                }

                return false;
            }
        }

        public bool IsButtonDown(MouseButton button) => this.buttons[(int) button];

        public bool this[MouseButton button] {
            get => this.buttons[(int) button];
            private set => this.buttons[(int) button] = value;
        }

        public Vector2f Position {
            get => new Vector2f((float) this.mouseX, (float) this.mouseY);
            set {
                this.mouseX = value.x;
                this.mouseY = value.y;
            }
        }

        public Vector2f PreviousPosition {
            get => new Vector2f((float) this.prevMouseX, (float) this.prevMouseY);
            set {
                this.prevMouseX = value.x;
                this.prevMouseY = value.y;
            }
        }

        public Vector2f Scroll {
            get => this.scroll;
            private set => this.scroll = value;
        }

        public Vector2f PreviousScroll {
            get => this.previousScroll;
            private set => this.previousScroll = value;
        }

        public float ChangeX => this.mouseX - this.prevMouseX;
        public float ChangeY => this.mouseY - this.prevMouseY;

        public Vector2f MouseDelta => this.Position - this.PreviousPosition;

        public Vector2f ScrollDelta => this.scroll - this.previousScroll;

        public float X => (float) this.mouseX;
        public float Y => (float) this.mouseY;
        public float PreviousX => (float) this.prevMouseX;
        public float PreviousY => (float) this.prevMouseY;

        public Mouse(ACWindow window) {
            Instance = this;

            unsafe {
                this.window = window;
                GLFW.SetMouseButtonCallback(this.window.Handle, this.mbttnCallback = OnMouseButton);
                GLFW.SetCursorPosCallback(this.window.Handle, this.mposCallback = OnMousePosition);
            }
        }

        private unsafe void OnMouseButton(Window* hwnd, MouseButton button, InputAction action, KeyModifiers mods) {
            if (action == InputAction.Release) {
                this[button] = false;
                OnMouseUp(button, action, mods);
            }
            else {
                this[button] = true;
                OnMouseDown(button, action, mods);
            }
        }

        private unsafe void OnMousePosition(Window* window, double x, double y) {
            this.prevMouseX = this.mouseX;
            this.prevMouseY = this.mouseY;
            this.mouseX = (float) x;
            this.mouseY = (float) y;

            // Console.WriteLine($"{this.prevMouseX} {this.prevMouseY} -> {x} {y} ({this.ChangeX} {this.ChangeY})");
        }

        public void UpdateFrame() {
            this.buttonsPrevious.SetAll(false);
            this.buttonsPrevious.Or(this.buttons);
            this.prevMouseX = this.mouseX;
            this.prevMouseY = this.mouseY;
        }

        private void OnMouseUp(MouseButton button, InputAction action, KeyModifiers mods) {
            this.window.OnMouseUp(button, action, mods);
        }

        private void OnMouseDown(MouseButton button, InputAction action, KeyModifiers mods) {
            this.window.OnMouseDown(button, action, mods);
        }

        public bool WasButtonDown(MouseButton button) => this.buttonsPrevious[(int) button];

        public void Dispose() {

        }
    }
}