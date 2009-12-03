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

namespace Midi
{
    /// <summary>
    /// A scale based on a particular tonic.
    /// </summary>
    /// <remarks>
    /// <para>For our purposes, a scale is defined by a tonic and then the pattern that it uses to
    /// ascend up to the next tonic and then descend back to the original tonic.  The tonic
    /// is described with a <see cref="NoteFamily"/> because it is not specific to any one octave.
    /// The ascending/descending pattern is provided by the <see cref="Pattern"/> nested class.
    /// </para>
    /// <para>This class comes with a collection of predefined patterns, such as
    /// <see cref="Major">Scale.Major</see> and <see cref="Scale.HarmonicMinor">
    /// Scale.HarmonicMinor</see>.</para>
    /// </remarks>
    public class Scale
    {
        /// <summary>
        /// Description of a scale's ascending/descending pattern through an octave.
        /// </summary>
        /// <remarks>
        /// This class describes the general behavior of a scale as it ascends from a tonic up to the
        /// next tonic and back down again.  It is described in terms of semitones relative to the
        /// tonic; to apply it to particular tonic, pass one of these to the constructor of
        /// <see cref="Scale"/>.
        /// </remarks>
        public class Pattern
        {
            /// <summary>
            /// The name of the scale being described.
            /// </summary>
            public string Name
            {
                get
                {
                    return name;
                }
            }

            /// <summary>
            /// The ascending/descending sequence of the scale, given in the constructor. 
            /// </summary>
            public int[] AscendingDescendingPattern
            {
                get
                {
                    return ascendingDescendingPattern;
                }
            }

            /// <summary>
            /// Returns true if the given note is included in the ascending part of the pattern.
            /// </summary>
            /// <param name="note">The note, given in semitones above the tonic.</param>
            /// <returns>True if that note is in the ascending pattern.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Note is not in [0..12].</exception>
            public bool ContainedInAscending(int note)
            {
                if (note < 0 || note > 12)
                {
                    throw new ArgumentOutOfRangeException("Note out of range.");
                }
                return ascendingMask[note];
            }

            /// <summary>
            /// Returns true if the given note is included in the descending part of the pattern.
            /// </summary>
            /// <param name="note">The note, given in semitones above the tonic.</param>
            /// <returns>True if that note is in the descending pattern.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Note is not in [0..12].</exception>
            public bool ContainedInDescending(int note)
            {
                if (note < 0 || note > 12)
                {
                    throw new ArgumentOutOfRangeException("Note out of range.");
                }
                return descendingMask[note];
            }

            /// <summary>
            /// Cnnstructs a scale description.
            /// </summary>
            /// <param name="name">The name of the scale.</param>
            /// <param name="ascendingDescendingPattern">Array encoding the behavior of the scale as it
            /// ascends from the tonic up to the next tonic and back.  The first element must be 0, to
            /// indicate beginning at the tonic.  Then there must be a monotonically increasing sequence
            /// of notes, given in semitones-above-the-tonic.  Then there must be a 12, to indicate
            /// arrival at the tonic above.  Then there must be a monotonically decreasing sequence of
            /// notes, given in semitones-above-the-tonic.  The last element must be 0, to indicate
            /// arrival back at the original tonic.</param>
            /// <exception cref="ArgumentException">The pattern is invalid.</exception>
            public Pattern(string name, int[] ascendingDescendingPattern)
            {
                this.name = name;
                this.ascendingDescendingPattern = ascendingDescendingPattern;
                this.ascendingMask =
                    new bool[13] { true, false, false, false, false, false, false, false, false, false,
                    false, false, true };
                this.descendingMask =
                    new bool[13] { true, false, false, false, false, false, false, false, false, false,
                    false, false, true };
                if (!ComputeMasks())
                {
                    throw new ArgumentException("Invalid pattern.");
                }
            }

            /// <summary>
            /// Fills ascendingMask and descendingMask based on ascendingDescendingPattern.
            /// </summary>
            /// <returns>True if the operation succeeded, false if the pattern was invalid.</returns>
            private bool ComputeMasks()
            {
                // First make sure it's non-empty and starts at zero.
                if (ascendingDescendingPattern == null || ascendingDescendingPattern.Length == 0 ||
                    ascendingDescendingPattern[0] != 0)
                {
                    return false;
                }
                // Now run through the rest of the pattern and make sure it ascends and then descends,
                // and populate the masks as we go.
                bool ascending = true;
                for (int i = 1; i < ascendingDescendingPattern.Length; ++i)
                {
                    if (ascending)
                    {
                        // Make sure we've just gone up.
                        if (ascendingDescendingPattern[i] <= ascendingDescendingPattern[i - 1])
                        {
                            return false;
                        }
                        // Make sure we haven't gone up too far.
                        if (ascendingDescendingPattern[i] > 12)
                        {
                            return false;
                        }
                        // If we've reached 12, start descending, otherwise add this to the ascending
                        // mask.
                        if (ascendingDescendingPattern[i] == 12)
                        {
                            ascending = false;
                        }
                        else
                        {
                            // Add this to the ascending mask.
                            ascendingMask[AscendingDescendingPattern[i]] = true;
                        }
                    }
                    else
                    {
                        // Make sure we've just gone down.
                        if (ascendingDescendingPattern[i] >= ascendingDescendingPattern[i - 1])
                        {
                            return false;
                        }
                        // Make sure we haven't gone down too far.
                        if (ascendingDescendingPattern[i] < 0)
                        {
                            return false;
                        }
                        // If we've reached 0, make sure we're the last element, otherwise add this
                        // to the mask.
                        if (ascendingDescendingPattern[i] == 0)
                        {
                            if (i != ascendingDescendingPattern.Length - 1)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            // Add this to the descending mask.
                            descendingMask[ascendingDescendingPattern[i]] = true;
                        }
                    }
                }
                return true;
            }

            private string name;
            private int[] ascendingDescendingPattern;
            private readonly bool[] ascendingMask;
            private readonly bool[] descendingMask;
        }

        /// <summary>
        /// Constructs a scale from its pattern description and its tonic.
        /// </summary>
        /// <param name="tonic"></param>
        /// <param name="pattern"></param>
        public Scale(NoteFamily tonic, Pattern pattern)
        {
            this.tonic = tonic;
            this.pattern = pattern;
        }
 
        /// <summary>
        /// Returns the sequence of notes generated by this scale when moving from start to finish.
        /// </summary>
        /// <param name="start">The first note in the traversal.</param>
        /// <param name="finish">The last note in the traversal.</param>
        /// <returns>The sequence of notes.  The result always includes start and finish, and then
        /// includes whichever intervening notes are implied by scale when moving in that direction.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">start or finish is out-of-range.
        /// </exception>
        public List<Note> Traverse(Note start, Note finish)
        {
            start.Validate();
            finish.Validate();
            List<Note> result = new List<Note>();
            if (finish > start)
            {
                for (Note n = start; n <= finish; ++n)
                {
                    if (n == start || n == finish || ContainsWhenAscending(n))
                        result.Add(n);
                }
            }
            else if (finish < start)
            {
                for (Note n = start; n >= finish; --n)
                {
                    if (n == start || n == finish || ContainsWhenDescending(n))
                        result.Add(n);
                }
            }
            else
            {
                result.Add(start);
            }
            return result;
        }

        /// <summary>
        /// Returns true if note would be in this scale when ascending through the note.
        /// </summary>
        /// <param name="note">The note</param>
        /// <returns>True if note is included when ascending.</returns>
        /// <exception cref="ArgumentOutOfRangeException">note is out-of-range.</exception>
        public bool ContainsWhenAscending(Note note)
        {
            return pattern.ContainedInAscending(note.SemitonesAbove(Tonic));
        }

        /// <summary>
        /// Returns true if note would be in this scale when decending through the note.
        /// </summary>
        /// <param name="note">The note</param>
        /// <returns>True if note is included when decending.</returns>
        /// <exception cref="ArgumentOutOfRangeException">note is out-of-range.</exception>
        public bool ContainsWhenDescending(Note note)
        {
            return pattern.ContainedInDescending(note.SemitonesAbove(Tonic));
        }

        /// <summary>
        /// The name of this scale.
        /// </summary>
        public string Name
        {
            get
            {
                return tonic.Name() + " " + pattern.Name;
            }
        }

        /// <summary>
        /// The tonic of the scale.
        /// </summary>
        public NoteFamily Tonic
        {
            get
            {
                return tonic;
            }
        }

        /// <summary>
        /// Pattern for Major scales.
        /// </summary>
        public static Pattern Major =
            new Pattern("Major",
                new int[] { 0, 2, 4, 5, 7, 9, 11, 12, 11, 9, 7, 5, 4, 2, 0 });

        /// <summary>
        /// Pattern for Natural Minor scales.
        /// </summary>
        public static Pattern NaturalMinor =
            new Pattern("Natural Minor",
                new int[] { 0, 2, 3, 5, 7, 8, 10, 12, 10, 8, 7, 5, 3, 2, 0 });

        /// <summary>
        /// Pattern for Harmonic Minor scales.
        /// </summary>
        public static Pattern HarmonicMinor =
            new Pattern("Harmonic Minor",
                 new int[] { 0, 2, 3, 5, 7, 8, 11, 12, 11, 8, 7, 5, 3, 2, 0 });

        /// <summary>
        /// Pattern for Melodic Minor scales.
        /// </summary>
        public static Pattern MelodicMinor =
            new Pattern("Melodic Minor",
                  new int[] { 0, 2, 3, 5, 7, 9, 11, 12, 10, 8, 7, 5, 3, 2, 0 });

        /// <summary>
        /// Pattern for Chromatic scales.
        /// </summary>
        public static Pattern Chromatic =
            new Pattern("Chromatic",
                  new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
                      11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 });

        private NoteFamily tonic;
        private Pattern pattern;
    }
}
