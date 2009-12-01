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

        // Key mappings for the first 26 MIDI percussion notes, used when Shift isn't pressed.
        private static Dictionary<ConsoleKey, Percussion> unshiftedNotes =
            new Dictionary<ConsoleKey, Percussion>
        {
            {ConsoleKey.Q, Percussion.BassDrum1},
            {ConsoleKey.W, Percussion.BassDrum2},
            {ConsoleKey.E, Percussion.SideStick},
            {ConsoleKey.R, Percussion.SnareDrum1},
            {ConsoleKey.T, Percussion.HandClap},
            {ConsoleKey.Y, Percussion.SnareDrum2},
            {ConsoleKey.U, Percussion.LowTom2},
            {ConsoleKey.I, Percussion.ClosedHiHat},
            {ConsoleKey.O, Percussion.LowTom1},
            {ConsoleKey.P, Percussion.PedalHiHat},
            {ConsoleKey.A, Percussion.MidTom2},
            {ConsoleKey.S, Percussion.OpenHiHat},
            {ConsoleKey.D, Percussion.MidTom1},
            {ConsoleKey.F, Percussion.HighTom2},
            {ConsoleKey.G, Percussion.CrashCymbal1},
            {ConsoleKey.H, Percussion.HighTom1},
            {ConsoleKey.J, Percussion.RideCymbal1},
            {ConsoleKey.K, Percussion.ChineseCymbal},
            {ConsoleKey.L, Percussion.RideBell},
            {ConsoleKey.Z, Percussion.Tambourine},
            {ConsoleKey.X, Percussion.SplashCymbal},
            {ConsoleKey.C, Percussion.Cowbell},
            {ConsoleKey.V, Percussion.CrashCymbal2},
            {ConsoleKey.B, Percussion.VibraSlap},
            {ConsoleKey.N, Percussion.RideCymbal2},
            {ConsoleKey.M, Percussion.HighBongo}
        };

        // Key mappings for the rest of the MIDI percussion notes, used when Shift is pressed.
        private static Dictionary<ConsoleKey, Percussion> shiftedNotes =
            new Dictionary<ConsoleKey, Percussion>
        {
            {ConsoleKey.Q, Percussion.LowBongo},
            {ConsoleKey.W, Percussion.MuteHighConga},
            {ConsoleKey.E, Percussion.OpenHighConga},
            {ConsoleKey.R, Percussion.LowConga},
            {ConsoleKey.T, Percussion.HighTimbale},
            {ConsoleKey.Y, Percussion.LowTimbale},
            {ConsoleKey.U, Percussion.HighAgogo},
            {ConsoleKey.I, Percussion.LowAgogo},
            {ConsoleKey.O, Percussion.Cabasa},
            {ConsoleKey.P, Percussion.Maracas},
            {ConsoleKey.A, Percussion.ShortWhistle},
            {ConsoleKey.S, Percussion.LongWhistle},
            {ConsoleKey.D, Percussion.ShortGuiro},
            {ConsoleKey.F, Percussion.LongGuiro},
            {ConsoleKey.G, Percussion.Claves},
            {ConsoleKey.H, Percussion.HighWoodBlock},
            {ConsoleKey.J, Percussion.LowWoodBlock},
            {ConsoleKey.K, Percussion.MuteCuica},
            {ConsoleKey.L, Percussion.OpenCuica},
            {ConsoleKey.Z, Percussion.MuteTriangle},
            {ConsoleKey.X, Percussion.OpenTriangle}
             };

        public override void Run()
        {
            // Utility function prompts user to choose an output device (or if there is only one,
            // returns that one).
            OutputDevice outputDevice = ExampleUtil.ChooseOutputDeviceFromConsole();
            if (outputDevice == null)
            {
                Console.WriteLine("No output devices, so can't run this example.");
                ExampleUtil.PressAnyKeyToContinue();
                return;
            }
            outputDevice.Open();

            Console.WriteLine("Press alphabetic keys (with and without SHIFT) to play MIDI "+
                "percussion sounds.");
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
                        Percussion note = shiftedNotes[keyInfo.Key];
                        Console.Write("\rNote {0} ({1})         ", (int)note, note.Name());
                        outputDevice.SendPercussion(note, 90);
                    }
                }
                else
                {
                    if (unshiftedNotes.ContainsKey(keyInfo.Key))
                    {
                        Percussion note = unshiftedNotes[keyInfo.Key];
                        Console.Write("\rNote {0} ({1})         ", (int)note, note.Name());
                        outputDevice.SendPercussion(note, 90);
                    }
                }
            }

            // Close the output device.
            outputDevice.Close();

            // All done.
        }
    }
}
