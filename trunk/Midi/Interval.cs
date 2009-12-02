using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Midi
{
    /// <summary>
    /// Interval measuring the relationship between pitches.
    /// </summary>
    /// <remarks>
    /// <para>This enum is simply for making interval operations more explicit.  When adding to or
    /// subtracting from the <see cref="Note"/> enum, one can either use ints...</para>
    /// <code>Note n = Note.C4 + 5;</code>
    /// <para>...or use the Interval enum, cast to int...</para>
    /// <code>Note n = Note.C4 + (int)Interval.PerfectFourth;</code>
    /// <para>These two examples are equivalent.  The benefit of the latter is simply that it makes
    /// the intention more explicit.</para>
    /// <para>This enum has extension methods, such as <see cref="IntervalExtensionMethods.Name"/>,
    /// defined in <see cref="IntervalExtensionMethods"/>.</para>
    /// </remarks>
    /// <seealso cref="Note"/>
    public enum Interval
    {
        /// <summary>Unison interval, 0 semitones</summary>
        Unison = 0,
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
        /// <returns>The human-readable name.  If the interval is less than an octave, it gives
        /// the standard term (eg, "Major third").  If the interval is more than an octave, it
        /// gives the number of semitones in the interval.</returns>
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
