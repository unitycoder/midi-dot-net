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
    /// Class representing a MIDI output device.
    /// </summary>
    /// The client cannot create instances of this class, but can retrieve a collection of installed devices through
    /// the static InstalledDevices property.
    /// 
    /// All methods on this class are threadsafe.  The open/close state for a specific instance is guarded by locking
    /// the MidiOutputDevice instance.  To perform multiple operations with the device open or closed, lock the instance.
    public class OutputDevice : DeviceBase
    {
        #region Public Methods and Properties

        /// <summary>
        /// Global list of devices installed on this system.
        /// </summary>
        public static ReadOnlyCollection<OutputDevice> InstalledDevices
        {
            get
            {
                lock (staticLock)
                {
                    if (installedDevices == null)
                    {
                        installedDevices = MakeDeviceList();
                    }
                    return new ReadOnlyCollection<OutputDevice>(installedDevices);
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
                lock (this)
                {
                    return isOpen;
                }
            }
        }

        /// <summary>
        /// Opens this output device.
        /// </summary>
        public void Open()
        {
            lock (this)
            {
                CheckNotOpen();
                CheckReturnCode(Win32API.midiOutOpen(out handle, deviceId, null, (UIntPtr)0));
                isOpen = true;
            }
        }

        /// <summary>
        /// Closes this output device.
        /// </summary>
        public void Close()
        {
            lock (this)
            {
                CheckOpen();
                CheckReturnCode(Win32API.midiOutClose(handle));
                isOpen = false;
            }
        }

        /// <summary>
        /// Silences all notes on this output device.
        /// </summary>
        public void SilenceAllNotes()
        {
            lock (this)
            {
                CheckOpen();
                CheckReturnCode(Win32API.midiOutReset(handle));
            }
        }

        /// <summary>
        /// Sends a Note On message to this MIDI output device.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="note">The note.</param>
        /// <param name="velocity">The velocity 0..127.</param>
        public void SendNoteOn(Channel channel, Note note, int velocity)
        {
            channel.Validate();
            note.Validate();
            lock (this)
            {
                CheckOpen();
                CheckReturnCode(Win32API.midiOutShortMsg(handle, ShortMsg.EncodeNoteOn(channel, note, velocity)));
            }
        }

        /// <summary>
        /// Sends a Note Off message to this MIDI output device.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="note">The note.</param>
        /// <param name="velocity">The velocity 0..127.</param>
        public void SendNoteOff(Channel channel, Note note, int velocity)
        {
            channel.Validate();
            note.Validate();
            lock (this)
            {
                CheckOpen();
                CheckReturnCode(Win32API.midiOutShortMsg(handle, ShortMsg.EncodeNoteOff(channel, note, velocity)));
            }
        }

        /// <summary>
        /// Sends a Note On message to Channel10 of this MIDI output device.
        /// </summary>
        /// <param name="percussion">The percussion.</param>
        /// <param name="velocity">The velocity 0..127.</param>
        /// This is simply shorthand ofr a Note On message on Channel10 with a percussion-specific note, so
        /// there is no corresponding message to receive from an input device.
        public void SendPercussion(Percussion percussion, int velocity)
        {
            percussion.Validate();
            lock (this)
            {
                CheckOpen();
                CheckReturnCode(Win32API.midiOutShortMsg(handle, ShortMsg.EncodeNoteOn(Channel.Channel10, (Note)percussion,
                    velocity)));
            }
        }

        /// <summary>
        /// Sends a Control Change message to this MIDI output device.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="control">The control.</param>
        /// <param name="value">The new value 0..127.</param>
        public void SendControlChange(Channel channel, Control control, int value)
        {
            lock (this)
            {
                CheckOpen();
                CheckReturnCode(Win32API.midiOutShortMsg(handle, ShortMsg.EncodeControlChange(channel, control, value)));
            }
        }

        /// <summary>
        /// Sends a Pitch Bend message to this MIDI output device.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="value">The pitch bend value, 0..16383, 8192 is centered.</param>
        public void SendPitchBend(Channel channel, int value)
        {
            lock (this)
            {
                CheckOpen();
                CheckReturnCode(Win32API.midiOutShortMsg(handle, ShortMsg.EncodePitchBend(channel, value)));
            }
        }

        /// <summary>
        /// Sends a Program Change message to this MIDI output device.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="instrument">The instrument.</param>
        public void SendProgramChange(Channel channel, Instrument instrument)
        {
            lock (this)
            {
                CheckOpen();
                CheckReturnCode(Win32API.midiOutShortMsg(handle, ShortMsg.EncodeProgramChange(channel, instrument)));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Makes sure rc is MidiWin32Wrapper.MMSYSERR_NOERROR.  If not, throws an exception with an
        /// appropriate error message.
        /// </summary>
        /// <param name="rc"></param>
        private static void CheckReturnCode(Win32API.MMRESULT rc)
        {
            if (rc != Win32API.MMRESULT.MMSYSERR_NOERROR)
            {
                StringBuilder errorMsg = new StringBuilder(128);
                rc = Win32API.midiOutGetErrorText(rc, errorMsg);
                if (rc != Win32API.MMRESULT.MMSYSERR_NOERROR)
                {
                    throw new DeviceException();
                }
                throw new DeviceException(errorMsg.ToString());
            }
        }

        /// <summary>
        /// Throws a MidiDeviceException if this device is not open.
        /// </summary>
        private void CheckOpen()
        {
            if (!isOpen)
            {
                throw new DeviceException("device not open");
            }
        }

        /// <summary>
        /// Throws a MidiDeviceException if this device is open.
        /// </summary>
        private void CheckNotOpen()
        {
            if (isOpen)
            {
                throw new DeviceException("device open");
            }
        }

        /// <summary>
        /// Private Constructor, only called by the getter for the InstalledDevices property.
        /// </summary>
        /// <param name="deviceId">Position of this device in the list of all devices.</param>
        /// <param name="caps">Win32 Struct with device metadata</param>
        private OutputDevice(UIntPtr deviceId, Win32API.MIDIOUTCAPS caps)
            : base(caps.szPname)
        {
            this.deviceId = deviceId;
            this.caps = caps;
            this.isOpen = false;
        }

        /// <summary>
        /// Private method for constructing the array of MidiOutputDevices by calling the Win32 api.
        /// </summary>
        /// <returns></returns>
        private static OutputDevice[] MakeDeviceList()
        {
            uint outDevs = Win32API.midiOutGetNumDevs();
            OutputDevice[] result = new OutputDevice[outDevs];
            for (uint deviceId = 0; deviceId < outDevs; deviceId++)
            {
                Win32API.MIDIOUTCAPS caps = new Win32API.MIDIOUTCAPS();
                Win32API.midiOutGetDevCaps((UIntPtr)deviceId, out caps);
                result[deviceId] = new OutputDevice((UIntPtr)deviceId, caps);
            }
            return result;
        }

        #endregion

        #region Private Fields

        // Access to the global state is guarded by lock(staticLock).
        private static Object staticLock = new Object();
        private static OutputDevice[] installedDevices = null;

        // The fields initialized in the constructor never change after construction,
        // so they don't need to be guarded by a lock.
        private UIntPtr deviceId;
        private Win32API.MIDIOUTCAPS caps;

        // Access to the Open/Close state is guarded by lock(this).
        private bool isOpen;
        private Win32API.HMIDIOUT handle;

        #endregion
    }
}
