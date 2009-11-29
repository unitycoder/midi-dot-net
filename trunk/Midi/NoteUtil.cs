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

namespace Midi
{
    /// <summary>
    /// Utility functions for dealing with notes.
    /// </summary>
    public class NoteUtil
    {
        /// <summary>
        ///  Returns true if note is within range for valid MIDI notes.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns>True if note is in 0..127.</returns>
        public static bool IsValidNote(int note)
        {
            return note >= 0 && note < 127;
        }

        /// <summary>
        /// Returns the 8 notes in the major scale starting at the given tonic and including the tonic above.
        /// </summary>
        /// <param name="tonic">The tonic note.</param>
        /// <returns>The ascending sequence of notes.</returns>
        public static Note[] MajorScaleStartingAt(Note tonic)
        {
            tonic.Validate();
            return new Note[] { tonic, tonic + 2, tonic + 4, tonic + 5, tonic + 7,
                tonic + 9, tonic + 11, tonic + 12 };
        }

        /// <summary>
        /// Returns true if note is in the major scale of tonic (regardless of octave).
        /// </summary>
        /// <param name="note">The note to test.</param>
        /// <param name="tonic">The tonic for the scale.</param>
        public static bool IsInMajorScale(Note note, Note tonic)
        {
            note.Validate();
            tonic.Validate();
            // Compute how many semitones note is above the nearest tonic below.
            int semitonesAbove = ((int)note % 12 - (int)tonic % 12);
            if (semitonesAbove < 0)
            {
                semitonesAbove += 12;
            }
            // Now apply make sure it isn't one of the notes skipped by the diatonic pattern.
            return semitonesAbove != 1 && semitonesAbove != 3 && semitonesAbove != 6 &&
                semitonesAbove != 8 && semitonesAbove != 10;
        }
    }
}
