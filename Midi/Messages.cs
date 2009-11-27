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

namespace Midi
{
    /// <summary>
    /// Base class for all MIDI messages.
    /// </summary>
    public abstract class Message
    {
        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="beatTime">The beat time for this message.</param>
        protected Message(float beatTime)
        {
            this.beatTime = beatTime;
        }

        /// <summary>
        /// Sends this message immediately, ignoring the beatTime.
        /// </summary>
        /// <returns>Additional messages which should be scheduled as a result of this message, or null.</returns>
        public abstract Message[] SendNow();

        /// <summary>
        /// Returns a copy of this message, shifted in time by the specified amount.
        /// </summary>
        public abstract Message MakeTimeShiftedCopy(float delta);

        /// <summary>
        /// Milliseconds since the music started.
        /// </summary>
        public float BeatTime { get { return beatTime; } }
        private float beatTime;
    }

    /// <summary>
    /// Base class for messages relevant to a specific device.
    /// </summary>
    public abstract class DeviceMessage : Message
    {
        /// <summary>
        /// Protected constructor.
        /// </summary>
        protected DeviceMessage(DeviceBase device, float beatTime)
            : base(beatTime)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }
            this.device = device;                    
        }

        /// <summary>
        /// The device from which this message originated, or for which it is destined.
        /// </summary>
        public DeviceBase Device
        {
            get
            {
                return device;
            }
        }
        private DeviceBase device;
    }

    /// <summary>
    /// Base class for messages relevant to a specific device channel.
    /// </summary>
    public abstract class ChannelMessage : DeviceMessage
    {
        /// <summary>
        /// Protected constructor.
        /// </summary>
        protected ChannelMessage(DeviceBase device, int channel, float beatTime)
            : base(device, beatTime)
        {
            if (channel < 0 || channel > 15)
            {
                throw new ArgumentOutOfRangeException("channel");
            }
            this.channel = channel;
        }

        /// <summary>
        /// Channel, 0..15, 10 reserved for percussion.
        /// </summary>
        public int Channel { get { return channel; } }
        private int channel;
    }

    /// <summary>
    /// Base class for messages relevant to a specific note.
    /// </summary>
    public abstract class NoteMessage : ChannelMessage
    {
        /// <summary>
        /// Protected constructor.
        /// </summary>
        protected NoteMessage(DeviceBase device, int channel, int note, int velocity, float beatTime)
            : base(device, channel, beatTime)
        {
            if (note < 0 || note > 127)
            {
                throw new ArgumentOutOfRangeException("note");
            }
            if (velocity < 0 || velocity > 127)
            {
                throw new ArgumentOutOfRangeException("velocity");
            }
            this.note = note;
            this.velocity = velocity;
        }

        /// <summary>
        /// Note, 0..127, middle C is 60.
        /// </summary>
        public int Note { get { return note; } }
        private int note;

        /// <summary>
        /// Velocity, 0..127.
        /// </summary>
        public int Velocity { get { return velocity; } }
        private int velocity;
    }

    /// <summary>
    /// Note On message.
    /// </summary>
    public class NoteOnMessage : NoteMessage
    {
        /// <summary>
        /// Constructs a Note On message.
        /// </summary>
        /// <param name="device">The device associated with this message.</param>
        /// <param name="channel">Channel, 0..15, 10 reserved for percussion.</param>
        /// <param name="note">Note, 0..127, middle C is 60.</param>
        /// <param name="velocity">Velocity, 0..127.</param>
        /// <param name="beatTime">Milliseconds since the music started.</param>
        public NoteOnMessage(DeviceBase device, int channel, int note, int velocity, float beatTime)
            : base(device, channel, note, velocity, beatTime) { }

        /// <summary>
        /// Sends this message immediately, ignoring the beatTime.
        /// </summary>
        /// <returns>Additional messages which should be scheduled as a result of this message, or null.</returns>
        public override Message[] SendNow()
        {
            ((OutputDevice)Device).sendNoteOnMessage(Channel, Note, Velocity);
            return null;
        }

        /// <summary>
        /// Returns a copy of this message, shifted in time by the specified amount.
        /// </summary>
        public override Message MakeTimeShiftedCopy(float delta)
        {
            return new NoteOnMessage(Device, Channel, Note, Velocity, BeatTime + delta);
        }
    }

    /// <summary>
    /// Note Off message.
    /// </summary>
    public class NoteOffMessage : NoteMessage
    {
        /// <summary>
        /// Constructs a Note Off message.
        /// </summary>
        /// <param name="device">The device associated with this message.</param>
        /// <param name="channel">Channel, 0..15, 10 reserved for percussion.</param>
        /// <param name="note">Note, 0..127, middle C is 60.</param>
        /// <param name="velocity">Velocity, 0..127.</param>
        /// <param name="beatTime">Milliseconds since the music started.</param>
        public NoteOffMessage(DeviceBase device, int channel, int note, int velocity, float beatTime)
            : base(device, channel, note, velocity, beatTime) { }

        /// <summary>
        /// Sends this message immediately, ignoring the beatTime.
        /// </summary>
        /// <returns>Additional messages which should be scheduled as a result of this message, or null.</returns>
        public override Message[] SendNow()
        {
            ((OutputDevice)Device).sendNoteOffMessage(Channel, Note, Velocity);
            return null;
        }

        /// <summary>
        /// Returns a copy of this message, shifted in time by the specified amount.
        /// </summary>
        public override Message MakeTimeShiftedCopy(float delta)
        {
            return new NoteOffMessage(Device, Channel, Note, Velocity, BeatTime + delta);
        }
    }

    /// <summary>
    /// A Note On message which schedules its own Note Off message when played.
    /// </summary>
    public class NoteOnOffMessage : NoteMessage
    {
        /// <summary>
        /// Constructs a Note On/Off message.
        /// </summary>
        /// <param name="device">The device associated with this message.</param>
        /// <param name="channel">Channel, 0..15, 10 reserved for percussion.</param>
        /// <param name="note">Note, 0..127, middle C is 60.</param>
        /// <param name="velocity">Velocity, 0..127.</param>
        /// <param name="beatTime">Milliseconds since the music started.</param>
        /// <param name="duration">Milliseconds of duration.</param>
        public NoteOnOffMessage(DeviceBase device, int channel, int note, int velocity, float beatTime, float duration)
            : base(device, channel, note, velocity, beatTime)
        {
            this.duration = duration;
        }

        /// <summary>
        /// Milliseconds of duration between the Note On and the Note Off.
        /// </summary>
        public float Duration { get { return duration; } }
        private float duration;

        /// <summary>
        /// Sends this message immediately, ignoring the beatTime.
        /// </summary>
        /// <returns>Additional messages which should be scheduled as a result of this message, or null.</returns>
        public override Message[] SendNow()
        {
            ((OutputDevice)Device).sendNoteOnMessage(Channel, Note, Velocity);
            return new Message[]{new NoteOffMessage(Device, Channel, Note, Velocity, BeatTime + Duration)};
        }

        /// <summary>
        /// Returns a copy of this message, shifted in time by the specified amount.
        /// </summary>
        public override Message MakeTimeShiftedCopy(float delta)
        {
            return new NoteOnOffMessage(Device, Channel, Note, Velocity, BeatTime + delta, Duration);
        }
    }

    /// <summary>
    /// Control change message.
    /// </summary>
    public class ControlChangeMessage : ChannelMessage
    {
        /// <summary>
        /// Construts a Control Change message.
        /// </summary>
        /// <param name="device">The device associated with this message.</param>
        /// <param name="channel">Channel, 0..15, 10 reserved for percussion.</param>
        /// <param name="control">Control, 0..119</param>
        /// <param name="value">Value, 0..127.</param>
        /// <param name="beatTime">Milliseconds since the music started.</param>
        public ControlChangeMessage(DeviceBase device, int channel, int control, int value, float beatTime)
            : base(device, channel, beatTime)
        {
            if (control < 0 || control > 119)
            {
                throw new ArgumentOutOfRangeException("control");
            }
            if (value < 0 || value > 127)
            {
                throw new ArgumentOutOfRangeException("control");
            }
            this.control = control;
            this.value = value;
        }

        /// <summary>
        /// Control, 0..119.
        /// </summary>
        public int Control { get { return control; } }
        private int control;

        /// <summary>
        /// Value, 0..127.
        /// </summary>
        public int Value { get { return value; } }
        private int value;

        /// <summary>
        /// Sends this message immediately, ignoring the beatTime.
        /// </summary>
        /// <returns>Additional messages which should be scheduled as a result of this message, or null.</returns>
        public override Message[] SendNow()
        {
            ((OutputDevice)Device).sendControlChangeMessage(Channel, Control, Value);
            return null;
        }

        /// <summary>
        /// Returns a copy of this message, shifted in time by the specified amount.
        /// </summary>
        public override Message MakeTimeShiftedCopy(float delta)
        {
            return new ControlChangeMessage(Device, Channel, Control, Value, BeatTime + delta);
        }
    }

    /// <summary>
    /// Pitch Bend message.
    /// </summary>
    public class PitchBendMessage : ChannelMessage
    {
        /// <summary>
        /// Constructs a Pitch Bend message.
        /// </summary>
        /// <param name="device">The device associated with this message.</param>
        /// <param name="channel">Channel, 0..15, 10 reserved for percussion.</param>
        /// <param name="value">Pitch bend value, 0..16383, 8192 is centered.</param>        
        /// <param name="beatTime">Milliseconds since the music started.</param>
        public PitchBendMessage(DeviceBase device, int channel, int value, float beatTime)
            : base(device, channel,beatTime)
        {
            if (value < 0 || value > 16383)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            this.value = value;
        }

        /// <summary>
        /// Pitch bend value, 0..16383, 8192 is centered.
        /// </summary>
        public int Value { get { return value; } }
        private int value;

        /// <summary>
        /// Sends this message immediately, ignoring the beatTime.
        /// </summary>
        /// <returns>Additional messages which should be scheduled as a result of this message, or null.</returns>
        public override Message[] SendNow()
        {
            ((OutputDevice)Device).sendPitchBendMessage(Channel, Value);
            return null;
        }

        /// <summary>
        /// Returns a copy of this message, shifted in time by the specified amount.
        /// </summary>
        public override Message MakeTimeShiftedCopy(float delta)
        {
            return new PitchBendMessage(Device, Channel, Value, BeatTime + delta);
        }
    }

    /// <summary>
    /// Program Change message.
    /// </summary>
    public class ProgramChangeMessage : ChannelMessage
    {
        /// <summary>
        /// Constructs a Program Change message.
        /// </summary>
        /// <param name="device">The device associated with this message.</param>
        /// <param name="channel">Channel, 0..15, 10 reserved for percussion.</param>
        /// <param name="preset">Preset, 0..127.</param>
        /// <param name="beatTime">Milliseconds since the music started.</param>
        public ProgramChangeMessage(DeviceBase device, int channel, int preset, float beatTime)
            : base(device, channel, beatTime)
        {
            if (preset < 0 || preset > 127)
            {
                throw new ArgumentOutOfRangeException("preset");
            }
            this.preset = preset;
        }

        /// <summary>
        /// Preset, 0..127.
        /// </summary>
        public int Preset { get { return preset; } }
        private int preset;

        /// <summary>
        /// Sends this message immediately, ignoring the beatTime.
        /// </summary>
        /// <returns>Additional messages which should be scheduled as a result of this message, or null.</returns>
        public override Message[] SendNow()
        {
            ((OutputDevice)Device).sendProgramChangeMessage(Channel, Preset);
            return null;
        }

        /// <summary>
        /// Returns a copy of this message, shifted in time by the specified amount.
        /// </summary>
        public override Message MakeTimeShiftedCopy(float delta)
        {
            return new ProgramChangeMessage(Device, Channel, Preset, BeatTime + delta);
        }
    }

    /// <summary>
    /// Pseudo-MIDI message used to arrange for a callback at a certain time.
    /// </summary>
    /// This message can be scheduled on an output device just like any other message
    /// (see MidiOutputDevice.Schedule()), though when its time comes and it gets "sent",
    /// it does not actually get sent to the device.  Instead, the provided callback is
    /// invoked.
    ///
    /// The idea is that the client can embed callback points in the music they've
    /// scheduled, so that (if the device gets to that point in the music) the client has
    /// an opportunity for some additional processing.
    ///
    /// The callback is invoked on the MidiOutputDevice's worker thread.
    public class CallbackMessage : Message
    {
        /// <summary>
        /// Delegate called when a CallbackMessage is sent.
        /// </summary>
        /// <param name="beatTime">The time at which this event was scheduled.</param>
        /// <returns>Additional messages which should be scheduled as a result of this callback, or null.</returns>
        public delegate Message[] CallbackType(float beatTime);
        
        /// <summary>
        /// Constructs a Callback message.
        /// </summary>
        /// <param name="callback">The callback to invoke when this message is "sent".</param>
        /// <param name="beatTime">Milliseconds since the music started.</param>
        public CallbackMessage(CallbackType callback, float beatTime)
            : base(beatTime)
        {
            this.callback = callback;
        }

        /// <summary>
        /// The callback to invoke when this message is "sent".
        /// </summary>
        public CallbackType Callback { get { return callback; } }
        private CallbackType callback;

        /// <summary>
        /// Sends this message immediately, ignoring the beatTime.
        /// </summary>
        /// <returns>Additional messages which should be scheduled as a result of this message, or null.</returns>
        public override Message[] SendNow()
        {
            return callback(BeatTime);
        }

        /// <summary>
        /// Returns a copy of this message, shifted in time by the specified amount.
        /// </summary>
        public override Message MakeTimeShiftedCopy(float delta)
        {
            return new CallbackMessage(callback, BeatTime + delta);
        }
    }
}
