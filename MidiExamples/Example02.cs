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

namespace MidiExamples
{
    class Example02 : ExampleBase
    {
        public Example02()
            : base("Example02.cs", "Simple MIDI output example.")
        { }

        public override void Run()
        {
            Console.WriteLine("Making sure there is at least one output device available...");
            if (MidiOutputDevice.InstalledDevices.Count == 0)
            {
                Console.WriteLine("No output devices, so can't run this example.");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                return;
            }

            MidiOutputDevice outputDevice = MidiOutputDevice.InstalledDevices[0];
            Console.WriteLine("Opening the first output device ({0})...", outputDevice.Name);
            outputDevice.Open();

            Console.WriteLine("Playing arpeggiated C major chord (without releasing the notes)...");
            outputDevice.sendNoteOnMessage(0, 60, 80);
            Thread.Sleep(500);
            outputDevice.sendNoteOnMessage(0, 64, 80);
            Thread.Sleep(500);
            outputDevice.sendNoteOnMessage(0, 67, 80);
            Thread.Sleep(500);

            Console.WriteLine("Now applying the sustain pedal...");
            outputDevice.sendControlChangeMessage(0, 64, 127);

            Console.WriteLine("Now releasing the C chord notes, but they should keep ringing because of the sustain pedal.");
            outputDevice.sendNoteOffMessage(0, 60, 80);
            outputDevice.sendNoteOffMessage(0, 64, 80);
            outputDevice.sendNoteOffMessage(0, 67, 80);

            Console.WriteLine("Now bending the pitches down.");
            for (int i = 0; i < 17; ++i)
            {
                outputDevice.sendPitchBendMessage(0, 8192 - i * 450);
                Thread.Sleep(200);
            }

            Console.WriteLine("Now releasing the sustain pedal, which should silence the notes...");
            outputDevice.sendControlChangeMessage(0, 64, 0);
            Thread.Sleep(1000);

            Console.WriteLine("Closing the output device...");
            outputDevice.Close();

            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
