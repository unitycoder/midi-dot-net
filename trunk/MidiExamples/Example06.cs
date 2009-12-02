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
using System.Text;
using Midi;

namespace MidiExamples
{
    public class Example06 : ExampleBase
    {
        public Example06()
            : base("Example06.cs", "Plays a scale from each MIDI key pressed.")
        { }
        
        class Drummer
        {
            public Drummer(Clock clock, OutputDevice outputDevice, int beatsPerMeasure)
            {
                this.clock = clock;
                this.outputDevice = outputDevice;
                this.beatsPerMeasure = beatsPerMeasure;
                this.messagesForOneMeasure = new List<Message>();
                for (int i = 0; i < beatsPerMeasure; ++i) {
                    Percussion percussion = i == 0 ? Percussion.PedalHiHat : Percussion.MidTom1;
                    int velocity = i == 0 ? 100 : 40;
                    messagesForOneMeasure.Add(new PercussionMessage(outputDevice, percussion,
                        velocity, i));
                }
                messagesForOneMeasure.Add(new CallbackMessage(
                    new CallbackMessage.CallbackType(CallbackHandler), 0));
                clock.Schedule(messagesForOneMeasure, 0);
            }
            private Message[] CallbackHandler(float beatTime)
            {
                // Round up to the next measure boundary.
                float timeOfNextMeasure = beatTime + beatsPerMeasure;
                clock.Schedule(messagesForOneMeasure, timeOfNextMeasure);
                return null;
            }
            private Clock clock;
            private OutputDevice outputDevice;
            private int beatsPerMeasure;
            private List<Message> messagesForOneMeasure;
        }

        class Scaler
        {
            public Scaler(Clock clock, InputDevice inputDevice, OutputDevice outputDevice)
            {
                this.clock = clock;
                this.inputDevice = inputDevice;
                this.outputDevice = outputDevice;
                if (inputDevice != null)
                {
                    inputDevice.NoteOn += new InputDevice.NoteOnHandler(this.NoteOn);
                }
            }

            public void NoteOn(NoteOnMessage msg)
            {
                Note[] scale = NoteUtil.MajorScaleStartingAt(msg.Note);
                for (int i = 1; i < scale.Count(); ++i)
                {
                    clock.Schedule(new NoteOnOffMessage(outputDevice, msg.Channel, scale[i],
                    msg.Velocity, msg.BeatTime + i, 0.99f));
                }
            }

            private Clock clock;
            private InputDevice inputDevice;
            private OutputDevice outputDevice;
        }

        public override void Run()
        {            
            if (OutputDevice.InstalledDevices.Count == 0)
            {
                Console.WriteLine("Can't do anything with no output device.");
                return;
            }

            float beatsPerMinute = 180;
            Clock clock = new Clock(beatsPerMinute);

            OutputDevice outputDevice = OutputDevice.InstalledDevices[0];
            outputDevice.Open();

            Drummer drummer = new Drummer(clock, outputDevice, 4);

            InputDevice inputDevice = null;
            if (InputDevice.InstalledDevices.Count > 0)
            {
                // Just pick the first input device.  This will throw an exception if there isn't
                // one.
                inputDevice = InputDevice.InstalledDevices[0];
                inputDevice.Open();
            }
            Scaler scaler = new Scaler(clock, inputDevice, outputDevice);

            clock.Start();
            if (inputDevice != null)
            {
                inputDevice.StartReceiving(clock);
            }

            bool done = false;

            while (!done)
            {
                Console.Clear();
                Console.WriteLine("BPM = {0}, Playing = {1}", clock.BeatsPerMinute,
                    clock.IsRunning);
                Console.WriteLine("Escape = Quit, '[' = slower, ']' = faster, 'P' = Toggle Play");
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Escape)
                {
                    done = true;
                }
                else if (key == ConsoleKey.Oem4)
                {
                    clock.BeatsPerMinute -= 2;
                }
                else if (key == ConsoleKey.Oem6)
                {
                    clock.BeatsPerMinute += 2;
                }
                else if (key == ConsoleKey.P)
                {
                    if (clock.IsRunning)
                    {
                        clock.Stop();
                        if (inputDevice != null)
                        {
                            inputDevice.StopReceiving();
                        }
                        outputDevice.SilenceAllNotes();
                    }
                    else
                    {
                        clock.Start();
                        if (inputDevice != null)
                        {
                            inputDevice.StartReceiving(clock);
                        }
                    }
                }
                else if (key == ConsoleKey.D1)
                {
                    NoteOnMessage msg = new NoteOnMessage(outputDevice, Channel.Channel1, Note.C4,
                        80, clock.BeatTime);
                    NoteOffMessage msg2 = new NoteOffMessage(outputDevice, Channel.Channel1,
                        Note.C4, 80, clock.BeatTime+0.99f);
                    clock.Schedule(msg);
                    clock.Schedule(msg2);
                    scaler.NoteOn(msg);
                }
            }

            if (clock.IsRunning)
            {
                clock.Stop();
                if (inputDevice != null)
                {
                    inputDevice.StopReceiving();
                }
                outputDevice.SilenceAllNotes();
            }

            outputDevice.Close();
            if (inputDevice != null)
            {
                inputDevice.Close();
            }
        }
    }
}
