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
    /// A chord.
    /// </summary>
    /// <remarks>
    /// <para>A chord is defined by its root note, the chord pattern, and the inversion.  The
    /// root note is described with a <see cref="NoteFamily"/> because we want to be able to talk
    /// about the chord independent of any one octave.  The pattern is given by the
    /// <see cref="Pattern"/> nested class.  The inversion is an integer indicating how many
    /// rotations the pattern has undergone.
    /// </para>
    /// <para>This class comes with a collection of predefined chord patterns, such as
    /// <see cref="Major"/> and <see cref="Chord.Minor"/>.</para>
    /// </remarks>
    public class Chord
    {
        /// <summary>
        /// Description of a chord's pattern starting at the root note.
        /// </summary>
        /// <remarks>
        /// This class describes the ascending sequence of notes included in a chord, starting with
        /// the root note.  It is described in terms of semitones relative to root; to apply it to
        /// particular tonic, pass one of these to the constructor of <see cref="Chord"/>.
        /// </remarks>
        public class Pattern
        {
            /// <summary>
            /// The name of the chord being described.
            /// </summary>
            public string Name
            {
                get
                {
                    return name;
                }
            }

            /// <summary>
            /// Shorthand string for this chord pattern.
            /// </summary>
            /// <remarks>
            /// This is the string used in the abbreviated name for a chord, placed immediately
            /// after the tonic and before the slashed inversion (if there is one).
            /// </remarks>
            public string Shorthand
            {
                get
                {
                    return shorthand;
                }
            }

            /// <summary>
            /// The ascending note sequence of the chord, in semitones-above-the-root.
            /// </summary>
            public int[] SemitoneSequence
            {
                get
                {
                    return semitoneSequence;
                }
            }

            /// <summary>
            /// Constructs a chord pattern.
            /// </summary>
            /// <param name="name">The name of the chord.</param>
            /// <param name="shorthand">The shorthand for the chord.  This is the string used in the
            /// abbreviated name for a chord, placed immediately after the tonic and before the
            /// slashed inversion (if there is one).</param>
            /// <param name="semitoneSequence">Array encoding the notes in the chord.  This
            /// must satisfy the requirements of <see cref="IsSequenceValid"/>.</param>
            /// <exception cref="ArgumentException">The pattern is invalid.</exception>
            public Pattern(string name, string shorthand, int[] semitoneSequence)
            {
                this.name = name;
                this.shorthand = shorthand;
                this.semitoneSequence = semitoneSequence;
                if (!IsSequenceValid(semitoneSequence))
                {
                    throw new ArgumentException("Invalid pattern.");
                }
            }

            /// <summary>
            /// Returns true if the given sequence is valid.
            /// </summary>
            /// <param name="semitoneSequence">The sequence to test.  The first element must be 0,
            /// to indicate beginning at the root note.  Then there must be a monotonically
            /// increasing sequence of notes, given in semitones-above-the-root.</param>
            /// <returns>True if the sequence is valid, false otherwise.</returns>
            public static bool IsSequenceValid(int[] semitoneSequence)
            {
                if (semitoneSequence == null || semitoneSequence.Length == 0 ||
                    semitoneSequence[0] != 0)
                {
                    return false;
                }
                for (int i = 1; i < semitoneSequence.Length; ++i)
                {
                    if (semitoneSequence[i] <= semitoneSequence[i - 1])
                    {
                        return false;
                    }
                }
                return true;
            }

            private string name;
            private string shorthand;
            private int[] semitoneSequence;
        }

        /// <summary>
        /// Constructs a chord from its root note, pattern, and inversion.
        /// </summary>
        /// <param name="rootNoteFamily">The root note of the chord.</param>
        /// <param name="pattern">The chord pattern.</param>
        /// <param name="inversion">The inversion, in [0..N-1] where N is the number of notes
        /// in pattern.</param>
        /// <exception cref="ArgumentOutOfRangeException">root is invalid or inversion is out of
        /// range.</exception>
        public Chord(NoteFamily rootNoteFamily, Pattern pattern, int inversion)
        {
            rootNoteFamily.Validate();
            if (inversion < 0 || inversion >= pattern.SemitoneSequence.Length)
            {
                throw new ArgumentOutOfRangeException("inversion out of range.");
            }
            this.rootNoteFamily = rootNoteFamily;
            this.pattern = pattern;
            this.inversion = inversion;
            this.invertedSequence = InvertSequence(pattern.SemitoneSequence, inversion);
        }

        /// <summary>
        /// Returns an inverted copy of semitoneSequence (or the original if inversion = 0).
        /// </summary>
        /// <param name="semitoneSequence"></param>
        /// <param name="inversion"></param>
        /// <returns></returns>
        private static int[] InvertSequence(int[] semitoneSequence, int inversion)
        {
            int[] result;
            if (inversion == 0)
            {
                result = semitoneSequence;
            }
            else
            {
                int[] orig = semitoneSequence;
                result = new int[orig.Length];
                for (int i = 0; i < orig.Length; ++i)
                {
                    result[i] = orig[(inversion + i) % orig.Length];
                }
                for (int i = 0; i < orig.Length - inversion; ++i)
                {
                    result[i] -= 12;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a list of chords which match the set of input notes.
        /// </summary>
        /// <param name="notes">Notes being analyzed.</param>
        /// <returns>A (possibly empty) list of chords.</returns>
        public static List<Chord> FindMatchingChords(List<Note> notes)
        {
            Note[] sorted = notes.ToArray();
            System.Array.Sort(sorted);
            int[] semitonesAboveBass = new int[sorted.Length];
            for (int i = 0; i < sorted.Length; ++i)
            {
                semitonesAboveBass[i] = sorted[i]-sorted[0];
            }

            List<Chord> result = new List<Chord>();
            foreach (Pattern pattern in Patterns)
            {
                int[] semitoneSequence = pattern.SemitoneSequence;
                if (semitoneSequence.Length != semitonesAboveBass.Length)
                {
                    continue;
                }
                for (int inversion = 0; inversion < semitoneSequence.Length; ++inversion)
                {
                    int[] invertedSequence = InvertSequence(semitoneSequence, inversion);
                    int[] iSemitonesAboveBass = new int[invertedSequence.Length];
                    for (int i = 0; i < invertedSequence.Length; ++i)
                    {
                        iSemitonesAboveBass[i] = invertedSequence[i] - invertedSequence[0];
                    }
                    bool equals = true;
                    for (int i = 0; i < iSemitonesAboveBass.Length; ++i)
                    {
                        if (iSemitonesAboveBass[i] != semitonesAboveBass[i])
                        {
                            equals = false;
                            break;
                        }
                    }
                    if (equals)
                    {
                        if (inversion == 0)
                        {
                            result.Add(new Chord(sorted[0].Family(), pattern, inversion));
                        }
                        else
                        {
                            result.Add(new Chord(sorted[sorted.Length - inversion].Family(),
                                pattern, inversion));
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// The name of this chord.
        /// </summary>
        public string Name
        {
            get
            {
                string result = rootNoteFamily.Name() + pattern.Shorthand;
                if (inversion != 0)
                {
                    result += "/" + BassNoteFamily.Name();
                }
                return result;
            }
        }

        /// <summary>
        /// The root note of the chord.
        /// </summary>
        public NoteFamily RootNoteFamily
        {
            get
            {
                return rootNoteFamily;
            }
        }

        /// <summary>
        /// The bass note of the chord.
        /// </summary>
        public NoteFamily BassNoteFamily
        {
            get
            {
                return (rootNoteFamily + invertedSequence[0]).Wrapped();
            }
        }

        /// <summary>
        /// The sequence of note families in this chord.
        /// </summary>
        /// <remarks>
        /// The result is in terms of note families (ie, not octave-specific).  The order in the
        /// result is based on the inversion, so the root note may not come first.
        /// </remarks>
        public NoteFamily[] NoteFamilies
        {
            get
            {
                NoteFamily[] result = new NoteFamily[invertedSequence.Length];
                for (int i = 0; i < result.Length; ++i)
                {
                    result[i] = (rootNoteFamily + invertedSequence[i]).Wrapped();
                }
                return result;
            }
        }

        /// <summary>
        /// Returns the notes in this chord when given a specific root note.
        /// </summary>
        /// <param name="root">The specific root note, whose family must be the same as this
        /// chord's RootNote.</param>
        /// <returns>The notes in the chord.</returns>
        /// <exception cref="ArgumentException">root is from the wrong family.</exception>
        public Note[] Notes(Note root)
        {
            if (root.Family() != RootNoteFamily)
            {
                throw new ArgumentException("Wrong note family.");
            }
            Note[] result = new Note[invertedSequence.Length];
            for (int i = 0; i < invertedSequence.Length; ++i)
            {
                result[i] = root + invertedSequence[i];
            }
            return result;
        }

        /// <summary>
        /// Pattern for Major chords.
        /// </summary>
        public static Pattern Major = new Pattern("Major", "", new int[] { 0, 4, 7 });

        /// <summary>
        /// Pattern for Minor chords.
        /// </summary>
        public static Pattern Minor = new Pattern("Minor", "min", new int[] { 0, 3, 7 });

        /// <summary>
        /// Pattern for Seventh chords.
        /// </summary>
        public static Pattern Seventh = new Pattern("Seventh", "7", new int[] { 0, 4, 7, 10 });

        /// <summary>
        /// Pattern for Augmented chords.
        /// </summary>
        public static Pattern Augmented = new Pattern("Augmented", "aug", new int[] { 0, 4, 8 });

        /// <summary>
        /// Pattern for Diminished chords.
        /// </summary>
        public static Pattern Diminished = new Pattern("Diminished", "dim", new int[] { 0, 3, 6 });

        /// <summary>
        /// Array of all the built-in chord patterns.
        /// </summary>
        public static Pattern[] Patterns = new Pattern[]
        {
            Major,
            Minor,
            Seventh,
            Augmented,
            Diminished
        };

        private NoteFamily rootNoteFamily;
        private Pattern pattern;
        int inversion;
        int[] invertedSequence;
    }
}
