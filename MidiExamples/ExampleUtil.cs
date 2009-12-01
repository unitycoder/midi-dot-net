using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace MidiExamples
{
    /// <summary>
    /// Utility functions for MIDI examples.
    /// </summary>
    public class ExampleUtil
    {
        /// <summary>
        /// Chooses an output device, possibly prompting the user at the console.
        /// </summary>
        /// <returns>The chosen output device, or null if none could be chosen.</returns>
        /// If there is exactly one output device, that one is chosen without prompting the user.
        public static OutputDevice ChooseOutputDeviceFromConsole()
        {
            if (OutputDevice.InstalledDevices.Count == 0)
            {
                return null;
            }
            if (OutputDevice.InstalledDevices.Count == 1) {
                return OutputDevice.InstalledDevices[0];
            }
            Console.WriteLine("Output Devices:");
            for (int i = 0; i < OutputDevice.InstalledDevices.Count; ++i)
            {
                Console.WriteLine("   {0}: {1}", i, OutputDevice.InstalledDevices[i].Name);
            }
            Console.Write("Choose the id of an output device...");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int deviceId = (int)keyInfo.Key - (int)ConsoleKey.D0;
                if (deviceId >= 0 && deviceId < OutputDevice.InstalledDevices.Count)
                {
                    return OutputDevice.InstalledDevices[deviceId];
                }
            }
        }

        /// <summary>
        /// Chooses an input device, possibly prompting the user at the console.
        /// </summary>
        /// <returns>The chosen input device, or null if none could be chosen.</returns>
        /// If there is exactly one input device, that one is chosen without prompting the user.
        public static InputDevice ChooseInputDeviceFromConsole()
        {
            if (InputDevice.InstalledDevices.Count == 0)
            {
                return null;
            }
            if (InputDevice.InstalledDevices.Count == 1)
            {
                return InputDevice.InstalledDevices[0];
            }
            Console.WriteLine("Input Devices:");
            for (int i = 0; i < InputDevice.InstalledDevices.Count; ++i)
            {
                Console.WriteLine("   {0}: {1}", i, InputDevice.InstalledDevices[i]);
            }
            Console.Write("Choose the id of an input device...");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int deviceId;
                if (int.TryParse(keyInfo.Key.ToString(), out deviceId) &&
                    deviceId > 0 && deviceId < InputDevice.InstalledDevices.Count)
                {
                    return InputDevice.InstalledDevices[deviceId];
                }
            }
        }

        /// <summary>
        /// Prints "Press any key to continue." with a newline, then waits for a key to be pressed.
        /// </summary>
        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Key mappings for mock MIDI keys on the QWERTY keyboard.
        /// </summary>
        private static Dictionary<ConsoleKey, int> mockKeys = new Dictionary<ConsoleKey,int>
        {
            {ConsoleKey.Q,        53},
            {ConsoleKey.D2,       54},
            {ConsoleKey.W,        55},
            {ConsoleKey.D3,       56},
            {ConsoleKey.E,        57},
            {ConsoleKey.D4,       58},
            {ConsoleKey.R,        59},
            {ConsoleKey.T,        60},
            {ConsoleKey.D6,       61},
            {ConsoleKey.Y,        62},
            {ConsoleKey.D7,       63},
            {ConsoleKey.U,        64},
            {ConsoleKey.I,        65},
            {ConsoleKey.D9,       66},
            {ConsoleKey.O,        67},
            {ConsoleKey.D0,       68},
            {ConsoleKey.P,        69},
            {ConsoleKey.OemMinus, 70},
            {ConsoleKey.Oem4,     71},
            {ConsoleKey.Oem6,     72}
        };

        /// <summary>
        /// If the specified key is one of the computer keys used for mock MIDI input, returns true
        /// and sets note to the value.
        /// </summary>
        /// <param name="key">The computer key pressed.</param>
        /// <param name="note">The note it mocks.</param>
        /// <returns></returns>
        public static bool IsMockNote(ConsoleKey key, out Note note)
        {
            if (mockKeys.ContainsKey(key))
            {
                note = (Note)mockKeys[key];
                return true;
            }
            note = 0;
            return false;
        }
    }
}
