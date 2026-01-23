//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;

namespace Nitrocid.Kernel.Threading
{
    /// <summary>
    /// The kernel thread to simplify the access to making new threads, starting them, and stopping them
    /// </summary>
    public class KernelThread
    {

        internal bool isCritical;
        internal Thread BaseThread;
        internal readonly List<KernelThread> ChildThreads = [];
        private bool isReady;
        private bool isStopping;
        private readonly ThreadStart? ThreadDelegate;
        private readonly ThreadStart? InitialThreadDelegate;
        private readonly ParameterizedThreadStart? ThreadDelegateParameterized;
        private readonly ParameterizedThreadStart? InitialThreadDelegateParameterized;
        private readonly bool IsParameterized;
        private readonly KernelThread? parentThread;

        /// <summary>
        /// The name of the thread
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Is the thread a background thread?
        /// </summary>
        public bool IsBackground { get; private set; }

        /// <summary>
        /// Is the kernel thread alive?
        /// </summary>
        public bool IsAlive => BaseThread.IsAlive;

        /// <summary>
        /// Is the kernel thread ready to start?
        /// </summary>
        public bool IsReady => isReady;

        /// <summary>
        /// Is the kernel thread critical (essential for the kernel)? Unkillable by the kernel task manager
        /// </summary>
        public bool IsCritical => isCritical;

        /// <summary>
        /// Is the kernel thread stopping? Use this flag to make your thread stop all operations when <see cref="Stop()"/> is called.
        /// </summary>
        public bool IsStopping => isStopping;

        /// <summary>
        /// Parent thread. If this thread is a parent thread, returns null. On child threads, returns a parent thread that spawned this thread.
        /// </summary>
        public KernelThread? ParentThread => parentThread;

        /// <summary>
        /// Thread ID for this kernel thread
        /// </summary>
        public int ThreadId => BaseThread.ManagedThreadId;

        /// <summary>
        /// Gets child threads count
        /// </summary>
        public int ChildThreadCount => ChildThreads.Count;

        /// <summary>
        /// Makes a new kernel thread
        /// </summary>
        /// <param name="ThreadName">The thread name</param>
        /// <param name="Background">Indicates if the kernel thread is background</param>
        /// <param name="Executor">The thread delegate</param>
        public KernelThread(string ThreadName, bool Background, ThreadStart Executor) :
            this(ThreadName, Background, Executor, false, null)
        { }

        /// <summary>
        /// Makes a new kernel thread
        /// </summary>
        /// <param name="ThreadName">The thread name</param>
        /// <param name="Background">Indicates if the kernel thread is background</param>
        /// <param name="Executor">The thread delegate</param>
        /// <param name="Child">Specifies whether the thread is going to be a child thread</param>
        /// <param name="ParentThread">If <paramref name="Child"/> is on, this should be specified to specify a thread that spawned the parent thread</param>
        private KernelThread(string ThreadName, bool Background, ThreadStart Executor, bool Child, KernelThread? ParentThread)
        {
            InitialThreadDelegate = Executor;
            Executor = () => StartInternalNormal(InitialThreadDelegate);
            BaseThread = new Thread(Executor) { Name = ThreadName, IsBackground = Background };
            IsParameterized = false;
            ThreadDelegate = Executor;
            Name = ThreadName;
            IsBackground = Background;
            isReady = true;
            DebugWriter.WriteDebug(DebugLevel.I, "Made a new kernel thread {0} with ID {1}, Child: {2}", vars: [ThreadName, ThreadId, Child]);
            if (Child && ParentThread is null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "This parent thread was specified as a child with ParentThread as null. Unsetting Child...");
                Child = false;
            }
            if (!Child)
                ThreadManager.kernelThreads.Add(this);
            else
                parentThread = ParentThread;
        }

        /// <summary>
        /// Makes a new kernel thread
        /// </summary>
        /// <param name="ThreadName">The thread name</param>
        /// <param name="Background">Indicates if the kernel thread is background</param>
        /// <param name="Executor">The thread delegate</param>
        public KernelThread(string ThreadName, bool Background, ParameterizedThreadStart Executor) :
            this(ThreadName, Background, Executor, false, null)
        { }

        /// <summary>
        /// Makes a new kernel thread
        /// </summary>
        /// <param name="ThreadName">The thread name</param>
        /// <param name="Background">Indicates if the kernel thread is background</param>
        /// <param name="Executor">The thread delegate</param>
        /// <param name="Child">Specifies whether the thread is going to be a child thread</param>
        /// <param name="ParentThread">If <paramref name="Child"/> is on, this should be specified to specify a thread that spawned the parent thread</param>
        private KernelThread(string ThreadName, bool Background, ParameterizedThreadStart Executor, bool Child, KernelThread? ParentThread)
        {
            InitialThreadDelegateParameterized = Executor;
            Executor = (Parameter) => StartInternalParameterized(InitialThreadDelegateParameterized, Parameter);
            BaseThread = new Thread(Executor) { Name = ThreadName, IsBackground = Background };
            IsParameterized = true;
            ThreadDelegateParameterized = Executor;
            Name = ThreadName;
            IsBackground = Background;
            isReady = true;
            DebugWriter.WriteDebug(DebugLevel.I, "Made a new kernel thread {0} with ID {1}, Child: {2}", vars: [ThreadName, ThreadId, Child]);
            if (Child && ParentThread is null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "This parent thread was specified as a child with ParentThread as null. Unsetting Child...");
                Child = false;
            }
            if (!Child)
                ThreadManager.kernelThreads.Add(this);
            else
                parentThread = ParentThread;
        }

        /// <summary>
        /// Starts the kernel thread
        /// </summary>
        public void Start()
        {
            if (!IsReady)
                throw new KernelException(KernelExceptionType.ThreadNotReadyYet);
            if (BaseThread.ThreadState.HasFlag(ThreadState.Stopped) || IsAlive)
                return;

            DebugWriter.WriteDebug(DebugLevel.I, "Starting kernel thread {0} with ID {1}", vars: [BaseThread.Name, ThreadId]);
            BaseThread.Start();
            DebugWriter.WriteDebug(DebugLevel.I, "Starting child threads kernel thread {0} with ID {1}", vars: [BaseThread.Name, ThreadId]);
            StartChildThreads(null);
            DebugWriter.WriteDebug(DebugLevel.I, "Complete.");
        }

        /// <summary>
        /// Starts the kernel thread
        /// </summary>
        /// <param name="Parameter">The parameter class instance containing multiple parameters, or a usual single parameter</param>
        public void Start(object? Parameter)
        {
            if (!IsReady)
                throw new KernelException(KernelExceptionType.ThreadNotReadyYet);
            if (BaseThread.ThreadState.HasFlag(ThreadState.Stopped) || IsAlive)
                return;

            // Start the parent thread
            DebugWriter.WriteDebug(DebugLevel.I, "Starting kernel thread {0} with ID {1} with parameters", vars: [BaseThread.Name, ThreadId]);
            BaseThread.Start(Parameter);
            DebugWriter.WriteDebug(DebugLevel.I, "Starting child threads kernel thread {0} with ID {1} with parameters", vars: [BaseThread.Name, ThreadId]);
            StartChildThreads(Parameter);
            DebugWriter.WriteDebug(DebugLevel.I, "Complete.");
        }

        /// <summary>
        /// Stops the kernel thread
        /// </summary>
        public void Stop() =>
            Stop(true);

        /// <summary>
        /// Stops the kernel thread
        /// </summary>
        /// <param name="regen">Whether to regenerate the kernel thread</param>
        public void Stop(bool regen)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Stopping kernel thread {0} with ID {1}", vars: [Name, ThreadId]);
                isStopping = true;
                BaseThread.Interrupt();
                DebugWriter.WriteDebug(DebugLevel.I, "Stopping child threads for kernel thread {0} with ID {1}", vars: [Name, ThreadId]);
                StopChildThreads();
                if (!Wait(60000))
                    DebugWriter.WriteDebug(DebugLevel.W, "Either the parent thread or the child thread timed out for 60000 ms waiting for it to stop");
                isReady = false;
                DebugWriter.WriteDebug(DebugLevel.I, "Finished with regen {0}", vars: [regen]);
                if (regen)
                    Regen();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException) && ex.GetType().Name != nameof(ThreadStateException))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Can't stop the kernel thread: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            isStopping = false;
        }

        /// <summary>
        /// Waits for the kernel thread to finish
        /// </summary>
        public void Wait()
        {
            if (!BaseThread.IsAlive)
                return;

            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Waiting for kernel thread {0} with ID {1}", vars: [BaseThread.Name, ThreadId]);
                BaseThread.Join();
                DebugWriter.WriteDebug(DebugLevel.I, "Waiting for child threads for kernel thread {0} with ID {1}", vars: [BaseThread.Name, ThreadId]);
                WaitForChildThreads();
                DebugWriter.WriteDebug(DebugLevel.I, "Waited.");
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException) && ex.GetType().Name != nameof(ThreadStateException))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Can't wait for kernel thread: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        /// <summary>
        /// Waits for the kernel thread to finish unless the waiting timed out
        /// </summary>
        public bool Wait(int timeoutMs)
        {
            if (!BaseThread.IsAlive)
                return true;

            try
            {
                bool SuccessfullyWaited = true;
                DebugWriter.WriteDebug(DebugLevel.I, "Waiting for kernel thread {0} with ID {1} for {2} milliseconds", vars: [BaseThread.Name, ThreadId, timeoutMs]);
                if (!BaseThread.Join(timeoutMs))
                    SuccessfullyWaited = false;
                DebugWriter.WriteDebug(DebugLevel.I, "Waiting for child kernel threads for {0} milliseconds", vars: [timeoutMs]);
                if (!WaitForChildThreads(timeoutMs))
                    SuccessfullyWaited = false;
                DebugWriter.WriteDebug(DebugLevel.I, "Waited.");
                return SuccessfullyWaited;
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException) && ex.GetType().Name != nameof(ThreadStateException))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Can't wait for kernel thread: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Regenerates the kernel thread
        /// </summary>
        public void Regen()
        {
            // We can't regen the kernel thread unless Stop() is called first.
            DebugWriter.WriteDebug(DebugLevel.I, "Ready to regenerate? {0}", vars: [!IsReady || BaseThread.ThreadState != ThreadState.Running]);
            if (IsReady && BaseThread.ThreadState == ThreadState.Running)
                throw new KernelException(KernelExceptionType.ThreadOperation, Translate.DoTranslation("Can't regenerate the kernel thread while the same thread is already running."));

            // Remake the thread to avoid illegal state exceptions
            if (IsParameterized && ThreadDelegateParameterized is not null)
                BaseThread = new Thread(ThreadDelegateParameterized) { Name = Name, IsBackground = IsBackground };
            else if (ThreadDelegate is not null)
                BaseThread = new Thread(ThreadDelegate) { Name = Name, IsBackground = IsBackground };
            else
                throw new KernelException(KernelExceptionType.ThreadOperation, Translate.DoTranslation("Thread can't be regenerated") + $". {Name}");
            DebugWriter.WriteDebug(DebugLevel.I, "Regenerated a new kernel thread {0} with ID {1} successfully.", vars: [Name, ThreadId]);
            isReady = true;
        }

        /// <summary>
        /// Adds the child thread to this parent thread
        /// </summary>
        /// <param name="ThreadName">The thread name</param>
        /// <param name="Background">Indicates if the kernel thread is background</param>
        /// <param name="Executor">The thread delegate</param>
        /// <exception cref="KernelException"></exception>
        public void AddChild(string ThreadName, bool Background, ThreadStart Executor)
        {
            if (Executor is null)
                throw new KernelException(KernelExceptionType.ThreadOperation, Translate.DoTranslation("Child thread start action can't be null."));

            KernelThread target = new(ThreadName, Background, Executor, true, this);
            ChildThreads.Add(target);
            DebugWriter.WriteDebug(DebugLevel.I, "Added a new child kernel thread {0} with ID {1}", vars: [ThreadName, target.ThreadId]);
            if (IsAlive && !IsStopping)
                target.Start();
        }

        /// <summary>
        /// Adds the child thread to this parent thread
        /// </summary>
        /// <param name="ThreadName">The thread name</param>
        /// <param name="Background">Indicates if the kernel thread is background</param>
        /// <param name="Executor">The thread delegate</param>
        /// <exception cref="KernelException"></exception>
        public void AddChild(string ThreadName, bool Background, ParameterizedThreadStart Executor)
        {
            if (Executor is null)
                throw new KernelException(KernelExceptionType.ThreadOperation, Translate.DoTranslation("Child thread start action can't be null."));

            KernelThread target = new(ThreadName, Background, Executor, true, this);
            ChildThreads.Add(target);
            DebugWriter.WriteDebug(DebugLevel.I, "Added a new child kernel thread {0} with ID {1}", vars: [ThreadName, target.ThreadId]);
            if (IsAlive && !IsStopping)
                target.Start();
        }

        /// <summary>
        /// Gets an individual child thread
        /// </summary>
        /// <param name="threadIdx">Thread index</param>
        /// <returns>A <see cref="KernelThread"/> instance of a child thread</returns>
        /// <exception cref="KernelException"></exception>
        public KernelThread GetChild(int threadIdx)
        {
            if (threadIdx < 0 || threadIdx >= ChildThreads.Count)
                throw new KernelException(KernelExceptionType.ThreadOperation, Translate.DoTranslation("Child thread index number is invalid."));

            return ChildThreads[threadIdx];
        }

        private void StartInternalNormal(ThreadStart action)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Starting action...");
                action();
                DebugWriter.WriteDebug(DebugLevel.I, "Thread exited peacefully.");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Thread {0} [{1}] failed: {2}", vars: [Name, ThreadId, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                KernelPanic.KernelErrorContinuable(Translate.DoTranslation("Kernel thread {0} failed.") + " {1}", ex, Name, ex.Message);
            }
        }

        private void StartInternalParameterized(ParameterizedThreadStart action, object? arg)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Starting action...");
                action(arg);
                DebugWriter.WriteDebug(DebugLevel.I, "Thread exited peacefully.");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Thread {0} [{1}] failed: {2}", vars: [Name, ThreadId, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                KernelPanic.KernelErrorContinuable(Translate.DoTranslation("Kernel thread {0} failed.") + " {1}", ex, Name, ex.Message);
            }
        }

        private void StartChildThreads(object? Parameter)
        {
            // Start the child threads
            DebugWriter.WriteDebug(DebugLevel.I, "Starting {0} child threads...", vars: [ChildThreads.Count]);
            foreach (var ChildThread in ChildThreads)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "For parent kernel thread {0} with ID {1}, starting the child thread {2} with ID {3} [child parameterized: {4}].", vars: [BaseThread.Name, ThreadId, ChildThread.Name, ChildThread.ThreadId, ChildThread.IsParameterized]);
                if (ChildThread.IsParameterized)
                    ChildThread.Start(Parameter);
                else
                    ChildThread.Start();
            }
        }

        private void StopChildThreads()
        {
            // Stop the child threads
            var ActiveChildThreads = ChildThreads.Where((thread) => thread.IsAlive).ToArray();
            DebugWriter.WriteDebug(DebugLevel.I, "Stopping {0} active child threads...", vars: [ActiveChildThreads.Length]);
            foreach (var ChildThread in ActiveChildThreads)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "For parent kernel thread {0} with ID {1}, stopping the child thread {2} with ID {3}.", vars: [BaseThread.Name, ThreadId, ChildThread.Name, ChildThread.ThreadId]);
                ChildThread.Stop();
            }
        }

        private void WaitForChildThreads()
        {
            // Stop the child threads
            var ActiveChildThreads = ChildThreads.Where((thread) => thread.IsAlive).ToArray();
            DebugWriter.WriteDebug(DebugLevel.I, "Waiting for {0} active child threads...", vars: [ActiveChildThreads.Length]);
            foreach (var ChildThread in ActiveChildThreads)
            {
                // Just to make sure
                if (!ChildThread.IsAlive)
                    continue;

                DebugWriter.WriteDebug(DebugLevel.I, "For parent kernel thread {0} with ID {1}, stopping the child thread {2} with ID {3}.", vars: [BaseThread.Name, ThreadId, ChildThread.Name, ChildThread.ThreadId]);
                ChildThread.Wait();
            }
        }

        private bool WaitForChildThreads(int timeoutMs)
        {
            // Stop the child threads
            var ActiveChildThreads = ChildThreads.Where((thread) => thread.IsAlive).ToArray();
            bool SuccessfullyWaited = true;
            DebugWriter.WriteDebug(DebugLevel.I, "Waiting for {0} active child threads...", vars: [ActiveChildThreads.Length]);
            foreach (var ChildThread in ActiveChildThreads)
            {
                // Just to make sure
                if (!ChildThread.IsAlive)
                    continue;

                DebugWriter.WriteDebug(DebugLevel.I, "For parent kernel thread {0} with ID {1}, stopping the child thread {2} with ID {3}.", vars: [BaseThread.Name, ThreadId, ChildThread.Name, ChildThread.ThreadId]);
                if (!ChildThread.Wait(timeoutMs))
                    SuccessfullyWaited = false;
            }
            return SuccessfullyWaited;
        }
    }
}
