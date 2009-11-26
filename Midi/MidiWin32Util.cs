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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Midi
{
    /// <summary>
    /// Utility functions for the MidiWin32Wrapper API.
    /// </summary>
    class MidiWin32Util
    {
        /// <summary>
        /// Human-readable strings for the MOD_* constants.
        /// </summary>
        public static Dictionary<UInt32, string> technologyNames
            = new Dictionary<UInt32, string>()
        {
            {MidiWin32Wrapper.MOD_MIDIPORT,  "Hardware MIDI port"},
            {MidiWin32Wrapper.MOD_SYNTH,     "Synthesizer"},
            {MidiWin32Wrapper.MOD_SQSYNTH,   "Square wave synthesizer"},
            {MidiWin32Wrapper.MOD_FMSYNTH,   "FM synthesizer"},
            {MidiWin32Wrapper.MOD_MAPPER,    "Microsoft MIDI mapper"},
            {MidiWin32Wrapper.MOD_WAVETABLE, "Hardware wavetable synthesizer"},
            {MidiWin32Wrapper.MOD_SWSYNTH,   "Software synthesizer"}
        };

        /// <summary>
        ///  Converts a MOD_* constant to a human-readable string.
        /// </summary>
        /// <param name="technologyCode">The wTechnology field of a MIDIOUTCAPS struct.</param>
        /// <returns>The human-readable string for that field.</returns>
        public static string TechnologyString(UInt32 technologyCode)
        {
            if (technologyNames.ContainsKey(technologyCode))
            {
                return technologyNames[technologyCode];
            }
            else
            {
                return "Unknown";
            }
        }

        /// <summary>
        ///  Human-readable strings for the MIDICAPS_* constants.
        /// </summary>
        public static Dictionary<UInt32, string> extraFeatureNames
            = new Dictionary<UInt32, string>()
        {
            {MidiWin32Wrapper.MIDICAPS_VOLUME,   "Volume control"},
            {MidiWin32Wrapper.MIDICAPS_LRVOLUME, "Separate L/R volume control"},
            {MidiWin32Wrapper.MIDICAPS_CACHE,    "Patch caching"},
            {MidiWin32Wrapper.MIDICAPS_STREAM,   "Direct stream support"}
        };

        /// <summary>
        ///  Converts a flag with bitwise-'or' MIDICAPS_* constants to a human-readable string.
        /// </summary>
        /// <param name="featureMask">The dwSupport field of a MIDIOUTCAPS struct.</param>
        /// <returns>The human-readable string for that set of flags.</returns>
        public static string ExtraFeaturesString(UInt32 featureMask)
        {
            List<string> features = new List<string>();
            foreach (KeyValuePair<UInt32, string> kvp in extraFeatureNames)
            {
                if ((kvp.Key & featureMask) != 0)
                {
                    features.Add(kvp.Value);
                }
            }
            if (features.Count > 0)
            {
                return String.Join(", ", features.ToArray());
            }
            else
            {
                return "None";
            }
        }

        /// <summary>
        /// Sends a Note On message to a MIDI output device.
        /// </summary>
        /// <param name="hmo">The handle returned by midiOpenOpen.</param>
        /// <param name="channel">The channel 0..15.</param>
        /// <param name="note">The note 0..127 (middle C is 60).</param>
        /// <param name="velocity">The velocity 0..127.</param>
        /// <returns>Return code as in midiOutShortMsg.</returns>
        public static UInt32 sendNoteOnMessage(MidiWin32Wrapper.HMIDIOUT hmo,
                                               int channel, int note, int velocity)
        {
            #region Preconditions
            if (channel < 0 || channel > 15)
            {
                throw new ArgumentOutOfRangeException("Channel is out of range.");
            }
            if (note < 0 || note > 127) {
                throw new ArgumentOutOfRangeException("Note is out of range.");
            }
            if (velocity < 0 || velocity > 127)
            {
                throw new ArgumentOutOfRangeException("Velocity is out of range.");
            }
            #endregion
            return MidiWin32Wrapper.midiOutShortMsg(hmo, (UInt32)(0x90 | (channel) | (note << 8) | (velocity << 16)));
        }

        /// <summary>
        /// Sends a Note Off message to a MIDI output device.
        /// </summary>
        /// <param name="hmo">The handle returned by midiOpenOpen.</param>
        /// <param name="channel">The channel 0..15.</param>
        /// <param name="note">The note 0..127 (middle C is 60).</param>
        /// <param name="velocity">The velocity 0..127.</param>
        /// <returns>Return code as in midiOutShortMsg.</returns>
        public static UInt32 sendNoteOffMessage(MidiWin32Wrapper.HMIDIOUT hmo,
                                                int channel, int note, int velocity)
        {
            #region Preconditions
            if (channel < 0 || channel > 15)
            {
                throw new ArgumentOutOfRangeException("Channel is out of range.");
            }
            if (note < 0 || note > 127)
            {
                throw new ArgumentOutOfRangeException("Note is out of range.");
            }
            if (velocity < 0 || velocity > 127)
            {
                throw new ArgumentOutOfRangeException("Velocity is out of range.");
            }
            #endregion
            return MidiWin32Wrapper.midiOutShortMsg(hmo, (UInt32)(0x80 | (channel) | (note << 8) | (velocity << 16)));
        }

        /// <summary>
        /// Sends a Control Change message to a MIDI output device.
        /// </summary>
        /// <param name="hmo">The handle returned by midiOpenOpen.</param>
        /// <param name="channel">The channel 0..15.</param>
        /// <param name="control">The controller 0..119.</param>
        /// <param name="value">The new value 0..127.</param>
        /// <returns>Return code as in midiOutShortMsg.</returns>
        public static UInt32 sendControlChangeMessage(MidiWin32Wrapper.HMIDIOUT hmo,
                                                      int channel, int control, int value)
        {
            #region Preconditions
            if (channel < 0 || channel > 15)
            {
                throw new ArgumentOutOfRangeException("Channel is out of range.");
            }
            if (control < 0 || control > 119)
            {
                throw new ArgumentOutOfRangeException("Control is out of range.");
            }
            if (value < 0 || value > 127)
            {
                throw new ArgumentOutOfRangeException("Value is out of range.");
            }
            #endregion
            return MidiWin32Wrapper.midiOutShortMsg(hmo, (UInt32)(0xB0 | (channel) | (control << 8) | (value << 16)));
        }

        /// <summary>
        /// Sends a Program Change message to a MIDI output device.
        /// </summary>
        /// <param name="hmo">The handle returned by midiOpenOpen.</param>
        /// <param name="channel">The channel 0..15.</param>
        /// <param name="note">The preset to choose 0..127.</param>
        /// <returns>Return code as in midiOutShortMsg.</returns>
        public static UInt32 sendProgramChangeMessage(MidiWin32Wrapper.HMIDIOUT hmo,
                                                      int channel, int preset)
        {
            #region Preconditions
            if (channel < 0 || channel > 15)
            {
                throw new ArgumentOutOfRangeException("Channel is out of range.");
            }
            if (preset < 0 || preset > 127)
            {
                throw new ArgumentOutOfRangeException("Preset is out of range.");
            }
            #endregion
            return MidiWin32Wrapper.midiOutShortMsg(hmo, (UInt32)(0xC0 | (channel) | (preset << 8)));
        }

        /// <summary>
        /// Sends a Pitch Bend message to a MIDI output device.
        /// </summary>
        /// <param name="hmo">The handle returned by midiOpenOpen.</param>
        /// <param name="channel">The channel 0..15.</param>
        /// <param name="velocity">The pitch bend value, 0..16383, 8192 is centered.</param>
        /// <returns>Return code as in midiOutShortMsg.</returns>
        public static UInt32 sendPitchBendMessage(MidiWin32Wrapper.HMIDIOUT hmo,
                                                  int channel, int value)
        {
            #region Preconditions
            if (channel < 0 || channel > 15)
            {
                throw new ArgumentOutOfRangeException("Channel is out of range.");
            }
            if (value < 0 || value > 16383)
            {
                throw new ArgumentOutOfRangeException("Value is out of range.");
            }
            #endregion
            return MidiWin32Wrapper.midiOutShortMsg(hmo, (UInt32)(0xE0 | (channel) | ((value & 0x7f) << 8) | ((value & 0x3f80) << 9)));
        }

        /// <summary>
        /// Returns true if the given MidiInProc params describe a Note On message.
        /// </summary>
        /// <param name="dwParam1">The dwParam1 arg passed to MidiInProc.</param>
        /// <param name="dwParam2">The dwParam2 arg passed to MidiInProc.</param>
        public static bool IsNoteOnMessage(UIntPtr dwParam1, UIntPtr dwParam2)
        {
            return ((int)dwParam1 & 0xf0) == 0x90;
        }

        /// <summary>
        /// Returns true if the given MidiInProc params describe a Note Off message.
        /// </summary>
        /// <param name="dwParam1">The dwParam1 arg passed to MidiInProc.</param>
        /// <param name="dwParam2">The dwParam2 arg passed to MidiInProc.</param>
        public static bool IsNoteOffMessage(UIntPtr dwParam1, UIntPtr dwParam2)
        {
            return ((int)dwParam1 & 0xf0) == 0x80;
        }

        /// <summary>
        /// Decodes a Note On or Note Off message based on MidiInProc params.
        /// </summary>
        /// <param name="dwParam1">The dwParam1 arg passed to MidiInProc.</param>
        /// <param name="dwParam2">The dwParam2 arg passed to MidiInProc.</param>
        /// <param name="channel">Filled in with the channel, 0-15.</param>
        /// <param name="note">Filled in with the note, 0-127</param>
        /// <param name="velocity">Filled in with the velocity, 0.127</param>
        /// <param name="timestamp">Filled in with the timestamp in microseconds since midiInStart().</param>
        public static void DecodeNoteMessage(UIntPtr dwParam1, UIntPtr dwParam2,
                               out Byte channel, out Byte note, out Byte velocity, out UInt32 timestamp)
        {
            if (!IsNoteOnMessage(dwParam1, dwParam2) && !IsNoteOffMessage(dwParam1, dwParam2))
            {
                throw new ArgumentException("Not a note message.");
            }
            channel = (Byte)((int)dwParam1 & 0x0f);
            note = (Byte)(((int)dwParam1 & 0xff00) >> 8);
            velocity = (Byte)(((int)dwParam1 & 0xff0000) >> 16);
            timestamp = (UInt32)dwParam2;
        }

        /// <summary>
        /// Returns true if the given MidiInProc params describe a Control Change message.
        /// </summary>
        /// <param name="dwParam1">The dwParam1 arg passed to MidiInProc.</param>
        /// <param name="dwParam2">The dwParam2 arg passed to MidiInProc.</param>
        public static bool IsControlChangeMessage(UIntPtr dwParam1, UIntPtr dwParam2)
        {
            return ((int)dwParam1 & 0xf0) == 0xB0;
        }

        /// <summary>
        /// Decodes a Control Change message based on MidiInProc params.
        /// </summary>
        /// <param name="dwParam1">The dwParam1 arg passed to MidiInProc.</param>
        /// <param name="dwParam2">The dwParam2 arg passed to MidiInProc.</param>
        /// <param name="channel">Filled in with the channel, 0-15.</param>
        /// <param name="note">Filled in with the control, 0-119.</param>
        /// <param name="velocity">Filled in with the value, 0-127.</param>
        /// <param name="timestamp">Filled in with the timestamp in microseconds since midiInStart().</param>
        public static void DecodeControlChangeMessage(UIntPtr dwParam1, UIntPtr dwParam2,
                               out Byte channel, out Byte control, out Byte value, out UInt32 timestamp)
        {
            if (!IsControlChangeMessage(dwParam1, dwParam2))
            {
                throw new ArgumentException("Not a control message.");
            }
            channel = (Byte)((int)dwParam1 & 0x0f);
            control = (Byte)(((int)dwParam1 & 0xff00) >> 8);
            value = (Byte)(((int)dwParam1 & 0xff0000) >> 16);
            timestamp = (UInt32)dwParam2;
        }

        /// <summary>
        /// Returns true if the given MidiInProc params describe a Program Change message.
        /// </summary>
        /// <param name="dwParam1">The dwParam1 arg passed to MidiInProc.</param>
        /// <param name="dwParam2">The dwParam2 arg passed to MidiInProc.</param>
        public static bool IsProgramChangeMessage(UIntPtr dwParam1, UIntPtr dwParam2)
        {
            return ((int)dwParam1 & 0xf0) == 0xC0;
        }

        /// <summary>
        /// Decodes a Program Change message based on MidiInProc params.
        /// </summary>
        /// <param name="dwParam1">The dwParam1 arg passed to MidiInProc.</param>
        /// <param name="dwParam2">The dwParam2 arg passed to MidiInProc.</param>
        /// <param name="channel">Filled in with the channel, 0-15.</param>
        /// <param name="note">Filled in with the preset, 0-127</param>
        /// <param name="timestamp">Filled in with the timestamp in microseconds since midiInStart().</param>
        public static void DecodeProgramChangeMessage(UIntPtr dwParam1, UIntPtr dwParam2,
                               out Byte channel, out Byte preset, out UInt32 timestamp)
        {
            if (!IsProgramChangeMessage(dwParam1, dwParam2))
            {
                throw new ArgumentException("Not a program change message.");
            }
            channel = (Byte)((int)dwParam1 & 0x0f);
            preset = (Byte)(((int)dwParam1 & 0xff00) >> 8);
            timestamp = (UInt32)dwParam2;
        }

        /// <summary>
        /// Returns true if the given MidiInProc params describe a Pitch Bend message.
        /// </summary>
        /// <param name="dwParam1">The dwParam1 arg passed to MidiInProc.</param>
        /// <param name="dwParam2">The dwParam2 arg passed to MidiInProc.</param>
        public static bool IsPitchBendMessage(UIntPtr dwParam1, UIntPtr dwParam2)
        {
            return ((int)dwParam1 & 0xf0) == 0xE0;
        }

        /// <summary>
        /// Decodes a Pitch Bend message based on MidiInProc params.
        /// </summary>
        /// <param name="dwParam1">The dwParam1 arg passed to MidiInProc.</param>
        /// <param name="dwParam2">The dwParam2 arg passed to MidiInProc.</param>
        /// <param name="channel">Filled in with the channel, 0-15.</param>
        /// <param name="note">Filled in with the pitch bend value, 0..16383, 8192 is centered.</param>
        /// <param name="timestamp">Filled in with the timestamp in microseconds since midiInStart().</param>
        public static void DecodePitchBendMessage(UIntPtr dwParam1, UIntPtr dwParam2,
                               out Byte channel, out UInt16 value, out UInt32 timestamp)
        {
            if (!IsPitchBendMessage(dwParam1, dwParam2))
            {
                throw new ArgumentException("Not a pitch bend message.");
            }
            channel = (Byte)((int)dwParam1 & 0x0f);
            value = (UInt16)((((int)dwParam1 >> 9) & 0x3f80) | (((int)dwParam1 >> 8) & 0x7f));
            timestamp = (UInt32)dwParam2;
        }
    }
}
