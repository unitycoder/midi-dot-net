using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Midi
{
    /// <summary>
    /// MIDI Control, used in Control Change messages.
    /// </summary>
    /// <remarks>
    /// This is an incomplete list of controls, and the details of how each control is encoded and
    /// used is complicated.  See the MIDI spec for details.
    ///
    /// The most commonly used control is SustainPedal, which is considered off when &lt; 64, on when &gt; 64.
    /// </remarks>
    public enum Control
    {
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        ModulationWheel = 1,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        DataEntryMSB = 6,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        Volume = 7,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        Pan = 10,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        Expression = 11,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        DataEntryLSB = 38,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        SustainPedal = 64,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        ReverbLevel = 91,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        TremoloLevel = 92,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        ChorusLevel = 93,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        CelesteLevel = 94,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        PhaserLevel = 95,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        NonRegisteredParameterLSB = 98,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        NonRegisteredParameterMSB = 99,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        RegisteredParameterNumberLSB = 100,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        RegisteredParameterNumberMSB = 101,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        AllControllersOff = 121,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        AllNotesOff = 123
    }

    /// <summary>
    /// Extension methods for the Control enum.
    /// </summary>
    /// Be sure to "using midi" if you want to use these as extension methods.
    public static class ControlExtensionMethods
    {
        /// <summary>
        /// Returns true if the specified control is valid.
        /// </summary>
        /// <param name="control">The Control to test.</param>
        public static bool IsValid(this Control control)
        {
            return (int)control >= 0 && (int)control < 128;
        }

        /// <summary>
        /// Throws an exception if control is not valid.
        /// </summary>
        /// <param name="control">The control to validate.</param>
        public static void Validate(this Control control)
        {
            if (!control.IsValid())
            {
                throw new Exception("invalid Control");
            }
        }

        /// <summary>
        /// Table of control names.
        /// </summary>
        public static Dictionary<int, string> ControlNames = new Dictionary<int, string>
        {
            {1, "Modulation wheel"},
            {6, "Data Entry MSB"},
            {7, "Volume"},
            {10, "Pan"},
            {11, "Expression"},
            {38, "Data Entry LSB"},
            {64, "Sustain pedal"},
            {91, "Reverb level"},
            {92, "Tremolo level"},
            {93, "Chorus level"},
            {94, "Celeste level"},
            {95, "Phaser level"},
            {98, "Non-registered Parameter LSB"},
            {99, "Non-registered Parameter MSB"},
            {100, "Registered Parameter Number LSB"},
            {101, "Registered Parameter Number MSB"},
            {121, "All controllers off"},
            {123, "All notes off"}
        };

        /// <summary>
        /// Returns the human-readable name of a MIDI control.
        /// </summary>
        /// <param name="control">The control.</param>
        public static string Name(this Control control)
        {
            control.Validate();
            if (ControlNames.ContainsKey((int)control))
            {
                return ControlNames[(int)control];
            }
            else
            {
                return "Other Control (see MIDI spec for details).";
            }
        }
    }
}
