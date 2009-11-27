using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace MidiExamples
{
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
                Console.WriteLine("   {0}: {1}", i, OutputDevice.InstalledDevices[i]);
            }
            Console.Write("Choose the id of an output device...");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int deviceId;
                if (int.TryParse(keyInfo.Key.ToString(), out deviceId) &&
                    deviceId > 0 && deviceId < OutputDevice.InstalledDevices.Count)
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

        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(true);
        }
    }
}
