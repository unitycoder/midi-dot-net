// Copyright (c) 2009, Tom Lokovic
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//     * Redistributions of source code must retain the above copyright notice,
//       this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Midi
{
    /// <summary>
    /// A clock for scheduling MIDI messages in a rate-adjustable, pausable timeline.
    /// </summary>
    public class Clock
    {
        /// <summary>
        /// Constructs a midi clock with a given beats-per-minute.
        /// </summary>
        /// <param name="beatsPerMinute">The initial beats-per-minute, which can be changed later.</param>
        public Clock(float beatsPerMinute)
        {
            if (beatsPerMinute <= 0)
            {
                throw new ArgumentOutOfRangeException("beatsPerMinute");
            }

            this.timingLock = new object();
            this.beatsPerMinute = beatsPerMinute;
            this.millisecondsPerBeat = 60000f / beatsPerMinute;
            this.millisecondFudge = 0;
            this.stopwatch = new Stopwatch();

            this.runLock = new object();
            this.isRunning = false;
            this.thread = null;

            this.threadLock = new object();
            this.threadLock = new Object();
            this.threadShouldExit = false;
            this.threadProcessingTime = 0;
            this.threadMessageQueue = new MessageQueue();
        }

        /// <summary>
        /// This clock's current time in beats.
        /// </summary>
        /// <remarks>
        /// <para>Normally, this method polls the clock's current time, and thus changes from
        /// moment to moment as long as the clock is running.  However, when called from the
        /// scheduler thread (that is, from a <see cref="Message.SendNow">Message.SendNow</see>
        /// method or a <see cref="CallbackMessage"/>), it returns the precise time at which the
        /// message was scheduled.</para>
        /// <para>For example, suppose a callback was scheduled for time T, and the scheduler
        /// managed to call that callback at time T+delta.  In the callback, BeatTime will
        /// return T for the duration of the callback.  In any other thread, BeatTime would
        /// return approximately T+delta.</para>
        /// </remarks>
        public float BeatTime
        {
            get
            {
                if (isSchedulerThread)
                {
                    return threadProcessingTime;
                }
                lock (timingLock)
                {
                    return (stopwatch.ElapsedMilliseconds + millisecondFudge) / millisecondsPerBeat;
                }
            }
        }

        /// <summary>
        /// Beats per minute property.
        /// </summary>
        /// Changing the value of BeatsPerMinute does not change the current beat time, but changes the rate at
        /// which BeatTime will progress hereafter.
        public float BeatsPerMinute
        {
            get
            {
                lock (timingLock)
                {
                    return beatsPerMinute;
                }
            }
            set
            {
                lock (timingLock)
                {
                    float newBeatsPerMinute = value;
                    float newMillisecondsPerBeat = 60000f / newBeatsPerMinute;
                    long currentMillis = stopwatch.ElapsedMilliseconds;
                    long currentFudgedMillis = currentMillis + millisecondFudge;
                    float beatTime = currentFudgedMillis / millisecondsPerBeat;
                    long newFudgedMillis = (long)(beatTime * newMillisecondsPerBeat);
                    long newMillisecondFudge = newFudgedMillis - currentMillis;
                    beatsPerMinute = newBeatsPerMinute;
                    millisecondsPerBeat = newMillisecondsPerBeat;
                    millisecondFudge = newMillisecondFudge;
                }
                // Pulse the threadlock in case the scheduler thread needs to reassess its timing based on
                // the new beatsPerMinute.
                lock (threadLock)
                {
                    Monitor.Pulse(threadLock);
                }
            }
        }

        /// <summary>
        /// Returns true if this clock is currently running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                if (isSchedulerThread)
                {
                    return true;
                }
                lock (runLock)
                {
                    return this.isRunning;
                }
            }
        }

        /// <summary>
        /// Starts or resumes the clock.
        /// </summary>
        public void Start()
        {
            if (isSchedulerThread)
            {
                throw new InvalidOperationException("Can't call Start() from the scheduler thread.");
            }
            lock (runLock)
            {
                if (isRunning)
                {
                    throw new InvalidOperationException("already started");
                }

                // Start the stopwatch.
                stopwatch.Start();

                // Start the scheduler thread.  This will cause it to start invoking messages in its thread.
                threadShouldExit = false;
                thread = new Thread(new ThreadStart(this.ThreadRun));
                thread.Start();
 
                // We now consider the MidiClock to actually be running.
                isRunning = true;
            }
        }

        /// <summary>
        /// Stops the clock (but does not reset its time or discard pending events).
        /// </summary>
        public void Stop()
        {
            if (isSchedulerThread)
            {
                throw new InvalidOperationException("Can't call Stop() from the scheduler thread.");
            }
            lock (runLock)
            {
                if (!isRunning)
                {
                    throw new InvalidOperationException("not started");
                }

                // Tell the thread to stop, wait for it to terminate, then discard it.  By the time this is done, we know
                // that the scheduler will not invoke any more messages.
                lock (threadLock)
                {
                    threadShouldExit = true;
                    Monitor.Pulse(threadLock);
                }
                thread.Join();
                thread = null;

                // Stop the stopwatch.
                stopwatch.Stop();

                // The MidiClock is no longer running.
                isRunning = false;
            }
        }

        /// <summary>
        /// Resets the clock to zero and discards pending events.
        /// </summary>
        public void Reset()
        {
            if (isSchedulerThread)
            {
                throw new InvalidOperationException("Can't call Reset() from the scheduler thread.");
            }
            lock (runLock)
            {
                if (isRunning)
                {
                    throw new InvalidOperationException("clock is running");
                }
                stopwatch.Reset();
                millisecondFudge = 0;
                lock (threadLock)
                {
                    threadMessageQueue.Clear();
                    Monitor.Pulse(threadLock);
                }
            }
        }

        /// <summary>
        /// Schedules a single message based on its beatTime.
        /// </summary>
        /// <param name="message">The message to schedule.</param>
        public void Schedule(Message message)
        {
            lock (threadLock)
            {
                threadMessageQueue.AddMessage(message);
                Monitor.Pulse(threadLock);                    
            }
        }

        /// <summary>
        /// Schedules a collection of messages, applying an optional time delta to the scheduled beatTime.
        /// </summary>
        /// <param name="messages">The message to send</param>
        /// <param name="beatTimeDelta">The delta to apply (or zero).</param>
        public void Schedule(List<Message> messages, float beatTimeDelta)
        {
            lock (threadLock)
            {
                if (beatTimeDelta == 0)
                {
                    foreach (Message message in messages)
                    {
                        threadMessageQueue.AddMessage(message);
                    }
                }
                else
                {
                    foreach (Message message in messages)
                    {
                        threadMessageQueue.AddMessage(message.MakeTimeShiftedCopy(beatTimeDelta));
                    }
                }
                Monitor.Pulse(threadLock);
            }
        }

        /// <summary>
        /// Returns the number of milliseconds from now until the specified beat time.
        /// </summary>
        /// <param name="beatTime">The beat time.</param>
        /// <returns>The positive number of milliseconds, or 0 if beatTime is in the past.</returns>
        private long MillisecondsUntil(float beatTime)
        {
            float now = (stopwatch.ElapsedMilliseconds + millisecondFudge) / millisecondsPerBeat;
            return Math.Max(0, (long)((beatTime - now) * millisecondsPerBeat));
        }

        /// <summary>
        /// Worker thread function.
        /// </summary>
        private void ThreadRun()
        {
            isSchedulerThread = true;
            lock (threadLock)
            {
                while (true)
                {
                    if (threadShouldExit)
                    {
                        return;
                    }
                    else if (threadMessageQueue.IsEmpty)
                    {
                        Monitor.Wait(threadLock);
                    }
                    else {
                        long millisToWait = MillisecondsUntil(threadMessageQueue.EarliestTimestamp);
                        if (millisToWait > 0)
                        {
                            Monitor.Wait(threadLock, (int)millisToWait);
                        }
                        else
                        {
                            threadProcessingTime = threadMessageQueue.EarliestTimestamp;
                            List<Message> timeslice = threadMessageQueue.PopEarliest();
                            foreach (Message message in timeslice)
                            {
                                Message[] moreMessages = message.SendNow();
                                if (moreMessages != null)
                                {
                                    foreach (Message message2 in moreMessages)
                                    {
                                        threadMessageQueue.AddMessage(message2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // The timing state is guarded by lock(timingLock).
        private object timingLock;
        private float beatsPerMinute;
        private float millisecondsPerBeat;
        private long millisecondFudge;
        private Stopwatch stopwatch;

        // Running state is guarded by lock(runLock).
        private object runLock;
        private bool isRunning;
        private Thread thread;

        // Thread state is guarded by lock(threadLock).
        private Object threadLock;
        private bool threadShouldExit;
        private float threadProcessingTime;
        private MessageQueue threadMessageQueue;

        /// <summary>
        /// Thread-local, set to true in the scheduler thread, false in all other threads.
        /// </summary>
        [ThreadStatic]
        static bool isSchedulerThread = false;
    }
}
