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
            /// <param name="semitoneSequence">Array encoding the notes in the chord.
            /// The first element must be 0, to indicate beginning at the root note.  Then there
            /// must be a monotonically increasing sequence of notes, given in
            /// semitones-above-the-root.</param>
            /// <exception cref="ArgumentException">The pattern is invalid.</exception>
            public Pattern(string name, int[] semitoneSequence)
            {
                this.name = name;
                this.semitoneSequence = semitoneSequence;
                if (!IsSequenceValid(semitoneSequence))
                {
                    throw new ArgumentException("Invalid pattern.");
                }
            }

            /// <summary>
            /// Returns true if the given sequence is valid.
            /// </summary>
            /// <param name="semitoneSequence">The semitone sequence.</param>
            /// <returns>True if it starts at zero and is monotonically increasing.</returns>
            private static bool IsSequenceValid(int[] semitoneSequence)
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
            private int[] semitoneSequence;
        }

        /// <summary>
        /// Constructs a chord from its root note, pattern, and inversion.
        /// </summary>
        /// <param name="root">The root note of the chord.</param>
        /// <param name="pattern">The chord pattern.</param>
        /// <param name="inversion">The inversion, in [0..N-1] where N is the number of notes
        /// in pattern.</param>
        /// <exception cref="ArgumentOutOfRangeException">root is invalid or inversion is out of
        /// range.</exception>
        public Chord(NoteFamily root, Pattern pattern, int inversion)
        {
            root.Validate();
            if (inversion < 0 || inversion >= pattern.SemitoneSequence.Length)
            {
                throw new ArgumentOutOfRangeException("inversion out of range.");
            }
            this.root = root;
            this.pattern = pattern;
            this.inversion = inversion;
            if (inversion == 0)
            {
                this.invertedSequence = pattern.SemitoneSequence;
            }
            else
            {
                int[] orig = pattern.SemitoneSequence;
                this.invertedSequence = new int[orig.Length];
                for (int i = 0; i < orig.Length; ++i)
                {
                    this.invertedSequence[i] = orig[(inversion + i) % orig.Length];
                }
                for (int i = 0; i < orig.Length - inversion; ++i)
                {
                    this.invertedSequence[i] -= 12;
                }
            }
        }

        /// <summary>
        /// The name of this chord.
        /// </summary>
        public string Name
        {
            get
            {
                if (inversion != 0)
                {
                    return root.Name() + " " + pattern.Name + " over " + Bass.Name();
                }
                return root.Name() + " " + pattern.Name;
            }
        }

        /// <summary>
        /// The root note of the chord.
        /// </summary>
        public NoteFamily Root
        {
            get
            {
                return root;
            }
        }

        /// <summary>
        /// The bass note of the chord.
        /// </summary>
        public NoteFamily Bass
        {
            get
            {
                return (root + invertedSequence[0]).Wrapped();
            }
        }

        /// <summary>
        /// The sequence of notes in this chord.
        /// </summary>
        /// <remarks>
        /// The result is in terms of note families (ie, not octave-specific).  The order in the
        /// result is based on the inversion, so the root note may not come first.
        /// </remarks>
        public NoteFamily[] Notes
        {
            get
            {
                NoteFamily[] result = new NoteFamily[invertedSequence.Length];
                for (int i = 0; i < result.Length; ++i)
                {
                    result[i] = (root + invertedSequence[i]).Wrapped();
                }
                return result;
            }
        }

        /// <summary>
        /// Pattern for Major chord.
        /// </summary>
        public static Pattern Major = new Pattern("Major", new int[] { 0, 4, 7 });

        /// <summary>
        /// Pattern for Minor chord.
        /// </summary>
        public static Pattern Minor = new Pattern("Minor", new int[] { 0, 3, 7 });

        /// <summary>
        /// Array of all the built-in chord patterns.
        /// </summary>
        public static Pattern[] Patterns = new Pattern[]
        {
            Major,
            Minor
        };

        private NoteFamily root;
        private Pattern pattern;
        int inversion;
        int[] invertedSequence;
    }
}
