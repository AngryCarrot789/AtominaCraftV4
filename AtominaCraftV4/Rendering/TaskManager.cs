using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AtominaCraftV4.Rendering {
    public class TaskManager {
        private readonly Queue<InvocableAction> actions;

        public TaskManager() {
            this.actions = new Queue<InvocableAction>();
        }

        public void FireAndForget(Action action, Action<Exception> errorCallback = null) {
            if (action == null) {
                throw new ArgumentNullException(nameof(action), "Action cannot be null");
            }

            lock (this.actions) {
                this.actions.Enqueue(new InvocableAction(action, errorCallback));
            }
        }

        public Task InvokeAsync(Action action) {
            if (action == null) {
                throw new ArgumentNullException(nameof(action), "Action cannot be null");
            }

            InvocableAction invocableAction = new InvocableAction(action, null);
            lock (this.actions) {
                this.actions.Enqueue(invocableAction);
            }

            while (!invocableAction.completed) {
                Thread.Sleep(1);
            }

            if (invocableAction.error != null) {
                throw invocableAction.error;
            }

            return Task.CompletedTask;
        }

        public void ProcessQueue() {
            lock (this.actions) {
                Queue<InvocableAction> queue = this.actions;
                while (queue.TryDequeue(out InvocableAction action)) {
                    action.Invoke();
                }
            }
        }

        private class InvocableAction {
            public readonly Action action;
            public readonly Action<Exception> errorCallback;
            public volatile bool completed;
            public volatile Exception error;

            public InvocableAction(Action action, Action<Exception> errorCallback) {
                this.action = action;
                this.errorCallback = errorCallback;
            }

            public void Invoke() {
                try {
                    this.action();
                }
                catch (Exception e) {
                    this.error = e;
                    this.errorCallback?.Invoke(e);
                }

                this.completed = true;
            }
        }
    }
}