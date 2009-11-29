using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Midi
{
    /// <summary>
    /// Interval measuring the relationship between pitches.
    /// </summary>
    public enum Interval
    {
        /// <summary>Unison interval, 0 semitones</summary>
        Unison        = 0,
        /// <summary>Semitone interval, 1 semitone</summary>
        Semitone = 1,
        /// <summary>Whole Tone interval, 2 semitones</summary>
        WholeTone = 2,
        /// <summary>Minor Third interval, 3 semitones</summary>
        MinorThird = 3,
        /// <summary>Major Third interval, 4 semitones</summary>
        MajorThird = 4,
        /// <summary>Perfect Fourth interval, 5 semitones</summary>
        PerfectFourth = 5,
        /// <summary>Tritone interval, 6 semitones</summary>
        Tritone = 6,
        /// <summary>Perfect Fifth interval, 7 semitones</summary>
        PerfectFifth = 7,
        /// <summary>Minor Sixth interval, 8 semitones</summary>
        MinorSixth = 8,
        /// <summary>Major Sixth interval, 9 semitones</summary>
        MajorSixth = 9,
        /// <summary>Minor seventh interval, 10 semitones</summary>
        MinorSeventh = 10,
        /// <summary>Major Seventh interval, 11 semitones</summary>
        MajorSeventh = 11,
        /// <summary>Octave interval, 12 semitones</summary>
        Octave = 12
    }

    /// <summary>
    /// Extension methods for the Interval enum.
    /// </summary>
    public static class IntervalExtensionMethods
    {
        /// <summary>
        /// Table of interval names.
        /// </summary>
        private static string[] IntervalNames = new string[]
        {
            "Unison",
            "Semitone",
            "Whole tone",
            "Minor third",
            "Major third",
            "Perfect fourth",
            "Tritone",
            "Perfect fifth",
            "Minor sixth",
            "Major sixth",
            "Minor seventh",
            "Major seventh",
            "Octave"
        };

        /// <summary>
        /// Returns the human-readable name of an interval.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <returns>The human-readable name.</returns>
        public static string Name(this Interval interval)
        {
            int value = Math.Abs((int)interval);
            if (value >= 0 && value <= 12)
            {
                return IntervalNames[value];
            }
            else
            {
                return String.Format("{0} semitones", value);
            }
        }
    }
}
