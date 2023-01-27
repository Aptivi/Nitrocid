
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Threading;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Misc.Threading
{
    /// <summary>
    /// The kernel thread to simplify the access to making new threads, starting them, and stopping them
    /// </summary>
    public class KernelThread
    {

        internal bool isCritical;
        private Thread BaseThread;
        private bool isReady;
        private readonly ThreadStart ThreadDelegate;
        private readonly ThreadStart InitialThreadDelegate;
        private readonly ParameterizedThreadStart ThreadDelegateParameterized;
        private readonly ParameterizedThreadStart InitialThreadDelegateParameterized;
        private readonly bool IsParameterized;

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
        /// Makes a new kernel thread
        /// </summary>
        /// <param name="ThreadName">The thread name</param>
        /// <param name="Background">Indicates if the kernel thread is background</param>
        /// <param name="Executor">The thread delegate</param>
        public KernelThread(string ThreadName, bool Background, ThreadStart Executor)
        {
            InitialThreadDelegate = Executor;
            Executor = () => StartInternalNormal(InitialThreadDelegate);
            BaseThread = new Thread(Executor) { Name = ThreadName, IsBackground = Background };
            IsParameterized = false;
            ThreadDelegate = Executor;
            Name = ThreadName;
            IsBackground = Background;
            isReady = true;
            DebugWriter.WriteDebug(DebugLevel.I, "Made a new kernel thread {0} with ID {1}", ThreadName, BaseThread.ManagedThreadId);
            ThreadManager.kernelThreads.Add(this);
        }

        /// <summary>
        /// Makes a new kernel thread
        /// </summary>
        /// <param name="ThreadName">The thread name</param>
        /// <param name="Background">Indicates if the kernel thread is background</param>
        /// <param name="Executor">The thread delegate</param>
        public KernelThread(string ThreadName, bool Background, ParameterizedThreadStart Executor)
        {
            InitialThreadDelegateParameterized = Executor;
            Executor = (Parameter) => StartInternalParameterized(InitialThreadDelegateParameterized, Parameter);
            BaseThread = new Thread(Executor) { Name = ThreadName, IsBackground = Background };
            IsParameterized = true;
            ThreadDelegateParameterized = Executor;
            Name = ThreadName;
            IsBackground = Background;
            isReady = true;
            DebugWriter.WriteDebug(DebugLevel.I, "Made a new kernel thread {0} with ID {1}", ThreadName, BaseThread.ManagedThreadId);
            ThreadManager.kernelThreads.Add(this);
        }

        /// <summary>
        /// Starts the kernel thread
        /// </summary>
        public void Start()
        {
            if (!IsReady)
                throw new KernelException(KernelExceptionType.ThreadNotReadyYet);

            DebugWriter.WriteDebug(DebugLevel.I, "Starting kernel thread {0} with ID {1}", BaseThread.Name, BaseThread.ManagedThreadId);
            BaseThread.Start();
        }

        /// <summary>
        /// Starts the kernel thread
        /// </summary>
        /// <param name="Parameter">The parameter class instance containing multiple parameters, or a usual single parameter</param>
        public void Start(object Parameter)
        {
            if (!IsReady)
                throw new KernelException(KernelExceptionType.ThreadNotReadyYet);

            DebugWriter.WriteDebug(DebugLevel.I, "Starting kernel thread {0} with ID {1} with parameters", BaseThread.Name, BaseThread.ManagedThreadId);
            BaseThread.Start(Parameter);
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
            DebugWriter.WriteDebug(DebugLevel.I, "Stopping kernel thread {0} with ID {1}", Name, BaseThread.ManagedThreadId);
            BaseThread.Interrupt();
            isReady = false;
            if (regen)
                Regen();
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
                DebugWriter.WriteDebug(DebugLevel.I, "Waiting for kernel thread {0} with ID {1}", BaseThread.Name, BaseThread.ManagedThreadId);
                BaseThread.Join();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException) && ex.GetType().Name != nameof(ThreadStateException))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Can't wait for kernel thread: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        /// <summary>
        /// Waits for the kernel thread to finish unless the waiting timed out
        /// </summary>
        public bool Wait(int timeoutMs)
        {
            if (!BaseThread.IsAlive)
                return false;

            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Waiting for kernel thread {0} with ID {1} for {2} milliseconds", BaseThread.Name, BaseThread.ManagedThreadId, timeoutMs);
                return BaseThread.Join(timeoutMs);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException) && ex.GetType().Name != nameof(ThreadStateException))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Can't wait for kernel thread: {0}", ex.Message);
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
            if (IsReady)
                throw new KernelException(KernelExceptionType.ThreadOperation, Translate.DoTranslation("Can't regenerate the kernel thread while the same thread is already running."));

            // Remake the thread to avoid illegal state exceptions
            if (IsParameterized)
                BaseThread = new Thread(ThreadDelegateParameterized) { Name = Name, IsBackground = IsBackground };
            else
                BaseThread = new Thread(ThreadDelegate) { Name = Name, IsBackground = IsBackground };
            DebugWriter.WriteDebug(DebugLevel.I, "Made a new kernel thread {0} with ID {1}", Name, BaseThread.ManagedThreadId);
            isReady = true;
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
                DebugWriter.WriteDebug(DebugLevel.E, "Thread {0} [{1}] failed: {2}", Name, BaseThread.ManagedThreadId, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                KernelPanic.KernelError(Kernel.KernelErrorLevel.C, false, 0, Translate.DoTranslation("Kernel thread {0} failed.") + " {1}", ex, Name, ex.Message);
            }
        }

        private void StartInternalParameterized(ParameterizedThreadStart action, object arg)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Starting action...");
                action(arg);
                DebugWriter.WriteDebug(DebugLevel.I, "Thread exited peacefully.");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Thread {0} [{1}] failed: {2}", Name, BaseThread.ManagedThreadId, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                KernelPanic.KernelError(Kernel.KernelErrorLevel.C, false, 0, Translate.DoTranslation("Kernel thread {0} failed.") + " {1}", ex, Name, ex.Message);
            }
        }

    }
}
