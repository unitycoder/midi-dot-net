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
using System.Collections.ObjectModel;
using System.Text;

namespace Midi
{
    /// <summary>
    /// Class representing a MIDI input device.
    /// </summary>
    /// The client cannot create instances of this class, but can retrieve a collection of installed devices through
    /// the static InstalledDevices property.
    public class MidiInputDevice : MidiDevice
    {
        #region Public Methods and Properties

        /// <summary>
        /// Delegate called to compute timestamp for a message.
        /// </summary>
        public delegate float TimeDelegate();

        /// <summary>
        /// Delegate called with a Note On message.
        /// </summary>
        public delegate void NoteOnHandler(NoteOnMessage msg);

        /// <summary>
        /// Delegate called with a Note Off message.
        /// </summary>
        public delegate void NoteOffHandler(NoteOffMessage msg);

        /// <summary>
        /// Delegate called with a Control Change message.
        /// </summary>
        public delegate void ControlChangeHandler(ControlChangeMessage msg);

        /// <summary>
        /// Delegate called with a Program Change message.
        /// </summary>
        public delegate void ProgramChangeHandler(ProgramChangeMessage msg);

        /// <summary>
        /// Delegate called with a Pitch Bend message.
        /// </summary>
        public delegate void PitchBendHandler(PitchBendMessage msg);

        /// <summary>
        /// Event called with a Note On message.
        /// </summary>
        public event NoteOnHandler NoteOn;

        /// <summary>
        /// Event called with a Note Off message.
        /// </summary>
        public event NoteOffHandler NoteOff;

        /// <summary>
        /// Event called with a Control Change message.
        /// </summary>
        public event ControlChangeHandler ControlChange;

        /// <summary>
        /// Event called with a Program Change message.
        /// </summary>
        public event ProgramChangeHandler ProgramChange;

        /// <summary>
        /// Event called with a Pitch Bend message.
        /// </summary>
        public event PitchBendHandler PitchBend;

        /// <summary>
        /// Global list of input devices installed on this system.
        /// </summary>
        public static ReadOnlyCollection<MidiInputDevice> InstalledDevices
        {
            get
            {
                lock (staticLock)
                {
                    if (installedDevices == null)
                    {
                        installedDevices = MakeDeviceList();
                    }
                    return new ReadOnlyCollection<MidiInputDevice>(installedDevices);
                }
            }
        }

        /// <summary>
        /// True if this device has been successfully opened.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                if (isInsideInputHandler)
                {
                    return true;
                }
                lock (this)
                {
                    return isOpen;
                }
            }
        }

        /// <summary>
        /// Opens this input device.
        /// </summary>
        /// <param name="timeDelegate">Function to call to compute timestamp for each incoming event.  If null, then the Win32
        /// millisecond time will be used.</param>
        /// Opening a device connects to the hardware, but messages cannot be received until a timeframe is
        /// established by calling Start().
        public void Open(TimeDelegate timeDelegate)
        {
            if (isInsideInputHandler)
            {
                throw new InvalidOperationException("Can't call Open() from inside an input handler.");
            }
            lock (this)
            {
                CheckNotOpen();
                CheckReturnCode(MidiWin32Wrapper.midiInOpen(out handle, deviceId, new MidiWin32Wrapper.MidiInProc(InputCallback),
                    (UIntPtr)0));
                this.timeDelegate = timeDelegate;
                isOpen = true;
            }
        }

        /// <summary>
        /// Closes this input device.
        /// </summary>
         public void Close()
        {
            if (isInsideInputHandler)
            {
                throw new InvalidOperationException("Can't call Close() from inside an input handler.");
            }
            lock (this)
            {
                CheckOpen();
                CheckReturnCode(MidiWin32Wrapper.midiInClose(handle));
                isOpen = false;
            }
        }

         /// <summary>
         /// True if this device is receiving messages.
         /// </summary>
         public bool IsReceiving
         {
             get
             {
                 if (isInsideInputHandler)
                 {
                     return true;
                 }
                 lock (this)
                 {
                     return isReceiving;
                 }
             }
         }

         /// <summary>
         /// Starts this input device receiving messages.
         /// </summary>
         public void StartReceiving()
         {
             if (isInsideInputHandler)
             {
                 throw new InvalidOperationException("Can't call StartReceiving() from inside an input handler.");
             }
             lock (this)
             {
                 CheckOpen();
                 CheckNotReceiving();
                 CheckReturnCode(MidiWin32Wrapper.midiInStart(handle));
                 isReceiving = true;
             }
         }

         /// <summary>
         /// Stops this input device from receiving messages.
         /// </summary>
         public void StopReceiving()
         {
             if (isInsideInputHandler)
             {
                 throw new InvalidOperationException("Can't call StopReceiving() from inside an input handler.");
             }
             lock (this)
             {
                 CheckReceiving();
                 CheckReturnCode(MidiWin32Wrapper.midiInStop(handle));
                 isReceiving = false;
             }
         }

        #endregion

        #region Private Methods

        /// <summary>
        /// Makes sure rc is MidiWin32Wrapper.MMSYSERR_NOERROR.  If not, throws an exception
        /// with an appropriate error message.
        /// </summary>
        /// <param name="rc"></param>
        private static void CheckReturnCode(UInt32 rc)
        {
            if (rc != MidiWin32Wrapper.MMSYSERR_NOERROR)
            {
                StringBuilder errorMsg = new StringBuilder(128);
                rc = MidiWin32Wrapper.midiInGetErrorText(rc, errorMsg);
                if (rc != MidiWin32Wrapper.MMSYSERR_NOERROR)
                {
                    throw new MidiDeviceException();
                }
                throw new MidiDeviceException(errorMsg.ToString());
            }
        }

        /// <summary>
        /// Throws a MidiDeviceException if this device is not open.
        /// </summary>
        private void CheckOpen()
        {
            if (!isOpen)
            {
                throw new MidiDeviceException("device not open");
            }
        }

        /// <summary>
        /// Throws a MidiDeviceException if this device is open.
        /// </summary>
        private void CheckNotOpen()
        {
            if (isOpen)
            {
                throw new MidiDeviceException("device open");
            }
        }

        /// <summary>
        /// Throws a MidiDeviceException if this device is not receiving.
        /// </summary>
        private void CheckReceiving()
        {
            if (!isReceiving)
            {
                throw new MidiDeviceException("device not receiving");
            }
        }

        /// <summary>
        /// Throws a MidiDeviceException if this device is receiving.
        /// </summary>
        private void CheckNotReceiving()
        {
            if (isReceiving)
            {
                throw new MidiDeviceException("device receiving");
            }
        }

        /// <summary>
        /// Private Constructor, only called by the getter for the InstalledDevices property.
        /// </summary>
        /// <param name="deviceId">Position of this device in the list of all devices.</param>
        /// <param name="caps">Win32 Struct with device metadata</param>
        private MidiInputDevice(UIntPtr deviceId, MidiWin32Wrapper.MIDIINCAPS caps)
            : base(caps.szPname)
        {
            this.deviceId = deviceId;
            this.caps = caps;
            this.isOpen = false;
            this.timeDelegate = null;
        }

        /// <summary>
        /// Private method for constructing the array of MidiInputDevices by calling the Win32 api.
        /// </summary>
        /// <returns></returns>
        private static MidiInputDevice[] MakeDeviceList()
        {
            uint inDevs = MidiWin32Wrapper.midiInGetNumDevs();
            MidiInputDevice[] result = new MidiInputDevice[inDevs];
            for (uint deviceId = 0; deviceId < inDevs; deviceId++)
            {
                MidiWin32Wrapper.MIDIINCAPS caps = new MidiWin32Wrapper.MIDIINCAPS();
                MidiWin32Wrapper.midiInGetDevCaps((UIntPtr)deviceId, out caps);
                result[deviceId] = new MidiInputDevice((UIntPtr)deviceId, caps);
            }
            return result;
        }

        /// <summary>
        /// The input callback for midiOutOpen.
        /// </summary>
        private void InputCallback(MidiWin32Wrapper.HMIDIIN hMidiIn, UInt32 wMsg, UIntPtr dwInstance, UIntPtr dwParam1, UIntPtr dwParam2)
        {
            isInsideInputHandler = true;
            try
            {
                if (wMsg == MidiWin32Wrapper.MIM_DATA)
                {
                    Byte channel;
                    Byte note;
                    Byte velocity;
                    Byte control;
                    Byte controlValue;
                    Byte preset;
                    UInt16 bendValue;
                    UInt32 win32Timestamp;
                    if (MidiWin32Util.IsNoteOnMessage(dwParam1, dwParam2))
                    {
                        if (NoteOn != null)
                        {
                            MidiWin32Util.DecodeNoteMessage(dwParam1, dwParam2, out channel, out note,
                                out velocity, out win32Timestamp);
                            NoteOn(new NoteOnMessage(this, channel, note, velocity,
                                timeDelegate == null ? win32Timestamp : timeDelegate()));
                        }
                    }
                    else if (MidiWin32Util.IsNoteOffMessage(dwParam1, dwParam2))
                    {
                        if (NoteOff != null)
                        {
                            MidiWin32Util.DecodeNoteMessage(dwParam1, dwParam2, out channel, out note,
                                out velocity, out win32Timestamp);
                            NoteOff(new NoteOffMessage(this, channel, note, velocity,
                                timeDelegate == null ? win32Timestamp : timeDelegate()));
                        }
                    }
                    else if (MidiWin32Util.IsControlChangeMessage(dwParam1, dwParam2))
                    {
                        if (ControlChange != null)
                        {
                            MidiWin32Util.DecodeControlChangeMessage(dwParam1, dwParam2, out channel,
                                out control, out controlValue, out win32Timestamp);
                            ControlChange(new ControlChangeMessage(this, channel, control, controlValue,
                                timeDelegate == null ? win32Timestamp : timeDelegate()));
                        }
                    }
                    else if (MidiWin32Util.IsProgramChangeMessage(dwParam1, dwParam2))
                    {
                        if (ProgramChange != null)
                        {
                            MidiWin32Util.DecodeProgramChangeMessage(dwParam1, dwParam2, out channel,
                                out preset, out win32Timestamp);
                            ProgramChange(new ProgramChangeMessage(this, channel, preset,
                                timeDelegate == null ? win32Timestamp : timeDelegate()));
                        }
                    }
                    else if (MidiWin32Util.IsPitchBendMessage(dwParam1, dwParam2))
                    {
                        if (PitchBend != null)
                        {
                            MidiWin32Util.DecodePitchBendMessage(dwParam1, dwParam2, out channel,
                                out bendValue, out win32Timestamp);
                            PitchBend(new PitchBendMessage(this, channel, bendValue,
                                timeDelegate == null ? win32Timestamp : timeDelegate()));
                        }
                    }
                    else
                    {
                        // Unsupported messages are ignored.
                    }
                }
            }
            finally
            {
                isInsideInputHandler = false;
            }
        }

        #endregion

        #region Private Fields

        // Access to the global state is guarded by lock(staticLock).
        private static Object staticLock = new Object();
        private static MidiInputDevice[] installedDevices = null;

        // The fields initialized in the constructor never change after construction,
        // so they don't need to be guarded by a lock.
        private UIntPtr deviceId;
        private MidiWin32Wrapper.MIDIINCAPS caps;

        // Access to the Open/Close state is guarded by lock(this).
        private bool isOpen;
        private bool isReceiving;
        private TimeDelegate timeDelegate;
        private MidiWin32Wrapper.HMIDIIN handle;

        /// <summary>
        /// Thread-local, set to true when called by an input handler, false in all other threads.
        /// </summary>
        [ThreadStatic]
        static bool isInsideInputHandler = false;

        #endregion
    }
}