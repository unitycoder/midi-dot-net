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
    class Example03 : ExampleBase
    {
        public Example03()
            : base("Example03.cs", "Alphabetic keys play MIDI percussion sounds.")
        { }

        // Struct representing a Percussion note number and name.
        private struct PercussionNote
        {
            public int noteNum;
            public string noteName;
            public PercussionNote(int noteNum, string noteName)
            {
                this.noteNum = noteNum;
                this.noteName = noteName;
            }
        }

        // Key mappings for the first 26 MIDI percussion notes, used when Shift isn't pressed.
        private static Dictionary<ConsoleKey, PercussionNote> unshiftedNotes =
            new Dictionary<ConsoleKey, PercussionNote>
        {
            {ConsoleKey.Q, new PercussionNote(35, "Bass Drum 2")},
            {ConsoleKey.W, new PercussionNote(36, "Bass Drum 1")},
            {ConsoleKey.E, new PercussionNote(37, "Side Stick")},
            {ConsoleKey.R, new PercussionNote(38, "Snare Drum 1")},
            {ConsoleKey.T, new PercussionNote(39, "Hand Clap")},
            {ConsoleKey.Y, new PercussionNote(40, "Snare Drum 2")},
            {ConsoleKey.U, new PercussionNote(41, "Low Tom 2")},
            {ConsoleKey.I, new PercussionNote(42, "Closed Hi-hat")},
            {ConsoleKey.O, new PercussionNote(43, "Low Tom 1")},
            {ConsoleKey.P, new PercussionNote(44, "Pedal Hi-hat")},
            {ConsoleKey.A, new PercussionNote(45, "Mid Tom 2")},
            {ConsoleKey.S, new PercussionNote(46, "Open Hi-hat")},
            {ConsoleKey.D, new PercussionNote(47, "Mid Tom 1")},
            {ConsoleKey.F, new PercussionNote(48, "High Tom 2")},
            {ConsoleKey.G, new PercussionNote(49, "Crash Cymbal 1")},
            {ConsoleKey.H, new PercussionNote(50, "High Tom 1")},
            {ConsoleKey.J, new PercussionNote(51, "Ride Cymbal 1")},
            {ConsoleKey.K, new PercussionNote(52, "Chinese Cymbal")},
            {ConsoleKey.L, new PercussionNote(53, "Ride Bell")},
            {ConsoleKey.Z, new PercussionNote(54, "Tambourine")},
            {ConsoleKey.X, new PercussionNote(55, "Splash Cymbal")},
            {ConsoleKey.C, new PercussionNote(56, "Cowbell")},
            {ConsoleKey.V, new PercussionNote(57, "Crash Cymbal 2")},
            {ConsoleKey.B, new PercussionNote(58, "Vibra Slap")},
            {ConsoleKey.N, new PercussionNote(59, "Ride Cymbal 2")},
            {ConsoleKey.M, new PercussionNote(60, "High Bongo")}
        };

        // Key mappings for the rest of the MIDI percussion notes, used when Shift is pressed.
        private static Dictionary<ConsoleKey, PercussionNote> shiftedNotes =
            new Dictionary<ConsoleKey, PercussionNote>
        {
            {ConsoleKey.Q, new PercussionNote(61, "Low Bongo")},
            {ConsoleKey.W, new PercussionNote(62, "Mute High Conga")},
            {ConsoleKey.E, new PercussionNote(63, "Open High Conga")},
            {ConsoleKey.R, new PercussionNote(64, "Low Conga")},
            {ConsoleKey.T, new PercussionNote(65, "High Timbale")},
            {ConsoleKey.Y, new PercussionNote(66, "Low Timbale")},
            {ConsoleKey.U, new PercussionNote(67, "High Agogo")},
            {ConsoleKey.I, new PercussionNote(68, "Low Agogo")},
            {ConsoleKey.O, new PercussionNote(69, "Cabasa")},
            {ConsoleKey.P, new PercussionNote(70, "Maracas")},
            {ConsoleKey.A, new PercussionNote(71, "Short Whistle")},
            {ConsoleKey.S, new PercussionNote(72, "Long Whistle")},
            {ConsoleKey.D, new PercussionNote(73, "Short Guiro")},
            {ConsoleKey.F, new PercussionNote(74, "Long Guiro")},
            {ConsoleKey.G, new PercussionNote(75, "Claves")},
            {ConsoleKey.H, new PercussionNote(76, "High Wood Block")},
            {ConsoleKey.J, new PercussionNote(77, "Low Wood Block")},
            {ConsoleKey.K, new PercussionNote(78, "Mute Cuica")},
            {ConsoleKey.L, new PercussionNote(79, "Open Cuica")},
            {ConsoleKey.Z, new PercussionNote(80, "Mute Triangle")},
            {ConsoleKey.X, new PercussionNote(81, "Open Triangle")}
        };

        public override void Run()
        {
            // Utility function prompts user to choose an output device (or if there is only one, returns that one).
            OutputDevice outputDevice = ExampleUtil.ChooseOutputDeviceFromConsole();
            if (outputDevice == null)
            {
                Console.WriteLine("No output devices, so can't run this example.");
                ExampleUtil.PressAnyKeyToContinue();
                return;
            }
            outputDevice.Open();

            Console.WriteLine("Press alphabetic keys (with and without SHIFT) to play MIDI percussion sounds.");
            Console.WriteLine("Press Escape when finished.");
            Console.WriteLine();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else if ((keyInfo.Modifiers & ConsoleModifiers.Shift) != 0)
                {
                    if (shiftedNotes.ContainsKey(keyInfo.Key))
                    {
                        PercussionNote note = shiftedNotes[keyInfo.Key];
                        Console.Write("\rNote {0} ({1})         ", note.noteNum, note.noteName);
                        outputDevice.sendNoteOnMessage(9, note.noteNum, 90);
                    }
                }
                else
                {
                    if (unshiftedNotes.ContainsKey(keyInfo.Key))
                    {
                        PercussionNote note = unshiftedNotes[keyInfo.Key];
                        Console.Write("\rNote {0} ({1})         ", note.noteNum, note.noteName);
                        outputDevice.sendNoteOnMessage(9, note.noteNum, 90);
                    }
                }
            }

            // Close the output device.
            outputDevice.Close();

            // All done.
        }
    }
}
