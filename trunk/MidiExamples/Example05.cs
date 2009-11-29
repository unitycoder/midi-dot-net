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
using Midi;
using System.Threading;
using System.Collections.Generic;

namespace MidiExamples
{
    class Arpeggiator
    {
        public Arpeggiator(InputDevice inputDevice, OutputDevice outputDevice, Clock clock)
        {
            this.inputDevice = inputDevice;
            this.outputDevice = outputDevice;
            this.clock = clock;

            if (inputDevice != null)
            {
                inputDevice.NoteOn += new InputDevice.NoteOnHandler(this.NoteOn);
                inputDevice.NoteOff += new InputDevice.NoteOffHandler(this.NoteOff);
            }
        }

        public void NoteOn(NoteOnMessage msg)
        {
            clock.Schedule(new NoteOnMessage(outputDevice, msg.Channel, msg.Note + 4, msg.Velocity, msg.BeatTime + 1));
            clock.Schedule(new NoteOnMessage(outputDevice, msg.Channel, msg.Note + 7, msg.Velocity, msg.BeatTime + 2));
        }

        public void NoteOff(NoteOffMessage msg)
        {
            clock.Schedule(new NoteOffMessage(outputDevice, msg.Channel, msg.Note + 4, msg.Velocity, msg.BeatTime + 1));
            clock.Schedule(new NoteOffMessage(outputDevice, msg.Channel, msg.Note + 7, msg.Velocity, msg.BeatTime + 2));
        }

        private InputDevice inputDevice;
        private OutputDevice outputDevice;
        private Clock clock;
    }
    
    public class Example04 : ExampleBase
    {
        public Example04()
            : base("Example04.cs", "Arpeggiator.")
        { }

        public override void Run()
        {
            // Create a clock running at the specified beats per minute.
            int beatsPerMinute = 180;
            Clock clock = new Clock(beatsPerMinute);
            
            // Utility function prompts user to choose an output device (or if there is only one, returns that one).
            OutputDevice outputDevice = ExampleUtil.ChooseOutputDeviceFromConsole();
            if (outputDevice == null)
            {
                Console.WriteLine("No output devices, so can't run this example.");
                ExampleUtil.PressAnyKeyToContinue();
                return;
            }
            outputDevice.Open();

            // Utility function prompts user to choose an input device (or if there is only one, returns that one).
            InputDevice inputDevice = ExampleUtil.ChooseInputDeviceFromConsole();
            if (inputDevice != null)
            {
                inputDevice.Open(() => clock.BeatTime);
            }

            Arpeggiator arpeggiator = new Arpeggiator(inputDevice, outputDevice, clock);

            Console.WriteLine("Press Escape when finished.");

            clock.Start();
            if (inputDevice != null)
            {
                inputDevice.StartReceiving();
            }

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
                Note note;
                if (ExampleUtil.IsMockNote(keyInfo.Key, out note))
                {
                    NoteOnMessage noteOn = new NoteOnMessage(outputDevice, 0, note, 100, clock.BeatTime);
                    NoteOffMessage noteOff = new NoteOffMessage(outputDevice, 0, note, 100, clock.BeatTime+1);
                    clock.Schedule(noteOn);
                    clock.Schedule(noteOff);
                    arpeggiator.NoteOn(noteOn);
                    arpeggiator.NoteOff(noteOff);
                }
            }

            clock.Stop();

            // Close the devices.
            outputDevice.Close();

            if (inputDevice != null)
            {
                inputDevice.StopReceiving();
                inputDevice.Close();
            }

            // All done.
        }
    }
}
