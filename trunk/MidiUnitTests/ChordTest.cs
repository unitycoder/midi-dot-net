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

namespace MidiUnitTests
{
    /// <summary>Unit tests for the ChordPattern class.</summary>
    [TestFixture]
    class ChordPatternTest
    {
        [Test]
        public void ConstructionErrors()
        {
            Assert.Throws(typeof(ArgumentNullException),
                () => new ChordPattern(null, "a", new int[] { 0, 1, 2, 3 },
                    new int[] { 0, 1, 2, 3 }));
            Assert.Throws(typeof(ArgumentNullException),
                () => new ChordPattern("a", null, new int[] { 0, 1, 2, 3 },
                        new int[] { 0, 1, 2, 3 }));
            Assert.Throws(typeof(ArgumentNullException),
                () => new ChordPattern("a", "b", null,
                    new int[] { 0, 1, 2, 3 }));
            Assert.Throws(typeof(ArgumentNullException),
                () => new ChordPattern("a", "b", new int[] { 0, 1, 2, 3 },
                    null));
            Assert.Throws(typeof(ArgumentException),
                () => new ChordPattern("a", "b", new int[] { 0, 1, 2, 3 },
                    new int[] { 0, 1, 2 }));
            Assert.Throws(typeof(ArgumentException),
                () => new ChordPattern("a", "b", new int[] { 1, 2, 3, 4 },
                    new int[] { 1, 2, 3, 4 }));
            Assert.Throws(typeof(ArgumentException),
                () => new ChordPattern("a", "b", new int[] { 0, 1, 2, 3, 3 },
                    new int[] { 0, 1, 2, 3, 3 }));
        }

        [Test]
        public void Equality()
        {
            Assert.AreEqual(
                new ChordPattern("a", "b", new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }),
                new ChordPattern("a", "b", new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }));
            Assert.AreNotEqual(
                new ChordPattern("a", "b", new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }),
                new ChordPattern("c", "b", new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }));
            Assert.AreNotEqual(
                new ChordPattern("a", "b", new int[] { 0, 1, 3 }, new int[] { 0, 1, 2 }),
                new ChordPattern("a", "b", new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }));
            Assert.AreNotEqual(
                new ChordPattern("a", "b", new int[] { 0, 1, 2 }, new int[] { 0, 1, 3 }),
                new ChordPattern("a", "b", new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }));
            Assert.AreNotEqual(
                new ChordPattern("a", "b", new int[] { 0, 1, 2, 3 }, new int[] { 0, 1, 2, 3 }),
                new ChordPattern("a", "b", new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }));
        }

        [Test]
        public void Properties()
        {
            ChordPattern cp = new ChordPattern("a", "b", new int[] { 0, 1, 2, 3 },
                new int[] { 0, 1, 2, 3 });
            Assert.AreEqual(cp.Name, "a");
            Assert.AreEqual(cp.Abbreviation, "b");
            Assert.AreEqual(cp.Ascent.Length, 4);
            Assert.AreEqual(cp.LetterOffsets.Length, 4);
        }
    }

    /// <summary>Unit tests for the Chord class.</summary>
    [TestFixture]
    class ChordTest
    {
        [Test]
        public void Construction()
        {
            Assert.Throws(typeof(ArgumentNullException),
                () => new Chord(new Note('F'), null, 0));
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => new Chord(new Note('F'), Chord.Major, -1));
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => new Chord(new Note('F'), Chord.Major, 7));
            Assert.AreEqual(new Chord("C"), new Chord(new Note('C'), Chord.Major, 0));
            Assert.AreEqual(new Chord("Cm"), new Chord(new Note('C'), Chord.Minor, 0));
            Assert.AreEqual(new Chord("C#m"), new Chord(new Note('C', Note.Sharp), Chord.Minor, 0));
            Assert.AreEqual(new Chord("Fbdim"), new Chord(new Note('F', Note.Flat),
                Chord.Diminished, 0));
            Assert.AreEqual(new Chord("C/E"), new Chord(new Note('C'), Chord.Major, 1));
            Assert.AreEqual(new Chord("Cm/Eb"), new Chord(new Note('C'), Chord.Minor, 1));
            Assert.AreEqual(new Chord("C/G"), new Chord(new Note('C'), Chord.Major, 2));
            Assert.AreEqual(new Chord("Cm/G"), new Chord(new Note('C'), Chord.Minor, 2));
            Assert.Throws(typeof(ArgumentException),
                () => new Chord((string)null));
            Assert.Throws(typeof(ArgumentException),
                () => new Chord(""));
            Assert.Throws(typeof(ArgumentException),
                () => new Chord("X"));
            Assert.Throws(typeof(ArgumentException),
                () => new Chord("Cx"));
            Assert.Throws(typeof(ArgumentException),
                () => new Chord("C#b"));
            Assert.Throws(typeof(ArgumentException),
                () => new Chord("C/X"));
            Assert.Throws(typeof(ArgumentException),
                () => new Chord("Fbdimx"));

        }

        [Test]
        public void Properties()
        {
            Assert.AreEqual(new Chord("Fm/Ab").Name, "Fm/Ab");
            Assert.AreEqual(new Chord("Fm/Ab").Root, new Note('F'));
            Assert.AreEqual(new Chord("Fm/Ab").Bass, new Note('A', Note.Flat));
        }

        [Test]
        public void RandomChords()
        {
            // Generate a bunch of random chords, and make sure that the Name property, when
            // parsed, generates an equivalent chord.  This ensures that our parsing and our
            // name generation are inverses of each other.  Also make sure that FindMatchingChords,
            // when called on the NoteSequence, contains the chord.
            Random random = new Random();
            for (int i = 0; i < 10000; ++i)
            {
                char letter = (char)(random.Next((int)'A', (int)'H'));
                int accidental = random.Next(-2, 3);
                ChordPattern pattern = Chord.Patterns[random.Next(0, Chord.Patterns.Length)];
                int inversion = random.Next(0, pattern.Ascent.Length);
                Chord c = new Chord(new Note(letter, accidental), pattern, inversion);
                Assert.AreEqual(c, new Chord(c.Name));
            }
        }

        /// <summary>
        /// Returns a comma-separated string with the note names of c's NoteSequence.
        /// </summary>
        private string SequenceString(Chord c)
        {
            string result = "";
            for (int i = 0; i < c.NoteSequence.Length; ++i)
            {
                if (i > 0)
                {
                    result += ", ";
                }
                result += c.NoteSequence[i].ToString();
            }
            return result;
        }

        [Test]
        public void NoteSequences()
        {
            Assert.AreEqual(SequenceString(new Chord("C")), "C, E, G");
            Assert.AreEqual(SequenceString(new Chord("C#")), "C#, E#, G#");
            Assert.AreEqual(SequenceString(new Chord("Ab")), "Ab, C, Eb");
            Assert.AreEqual(SequenceString(new Chord("Ab/Eb")), "Eb, Ab, C");
            Assert.AreEqual(SequenceString(new Chord("Cm")), "C, Eb, G");
            Assert.AreEqual(SequenceString(new Chord("Ebm")), "Eb, Gb, Bb");
            Assert.AreEqual(SequenceString(new Chord("G#m")), "G#, B, D#");
            Assert.AreEqual(SequenceString(new Chord("G#m/B")), "B, D#, G#");
            Assert.AreEqual(SequenceString(new Chord("Abm")), "Ab, Cb, Eb");
            Assert.AreEqual(SequenceString(new Chord("Cdim")), "C, Eb, Gb");
            Assert.AreEqual(SequenceString(new Chord("Cdim/Eb")), "Eb, Gb, C");
        }
    }
}
