using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace MidiUnitTests
{
    /// <summary>Unit tests for the Pitch enum.</summary>
    [TestFixture]
    class PitchTest
    {
        [Test]
        public void IsInMidiRangeTest()
        {
            Assert.False(((Pitch)(-1)).IsInMidiRange());
            Assert.True(((Pitch)(0)).IsInMidiRange());
            Assert.True(((Pitch)(127)).IsInMidiRange());
            Assert.False(((Pitch)(128)).IsInMidiRange());
        }

        [Test]
        public void OctaveTest()
        {
            Assert.AreEqual(Pitch.C4.Octave(), 4);
            Assert.AreEqual(Pitch.B3.Octave(), 3);
            Assert.AreEqual(((Pitch)(0)).Octave(), -1);
            Assert.AreEqual(((Pitch)(-1)).Octave(), -2);
        }

        [Test]
        public void PositionInOctaveTest()
        {
            Assert.AreEqual(Pitch.C4.PositionInOctave(), 0);
            Assert.AreEqual(Pitch.B3.PositionInOctave(), 11);
            Assert.AreEqual(((Pitch)(0)).PositionInOctave(), 0);
            Assert.AreEqual(((Pitch)(-1)).PositionInOctave(), 11);
        }

        [Test]
        public void CommonNoteTest()
        {
            Assert.AreEqual(Pitch.C4.CommonNote(), new Note('C', Note.Natural));
            Assert.AreEqual(Pitch.CSharp4.CommonNote(), new Note('C', Note.Sharp));
            Assert.AreEqual(Pitch.B3.CommonNote(), new Note('B', Note.Natural));
            Assert.AreEqual(((Pitch)(0)).CommonNote(), new Note('C', Note.Natural));
            Assert.AreEqual(((Pitch)(-1)).CommonNote(), new Note('B', Note.Natural));
        }

        [Test]
        public void NoteWithLetterTest()
        {
            Assert.AreEqual(Pitch.C4.NoteWithLetter('C'), new Note('C', Note.Natural));
            Assert.AreEqual(Pitch.C4.NoteWithLetter('B'), new Note('B', Note.Sharp));
            Assert.AreEqual(Pitch.C4.NoteWithLetter('D'), new Note('D', Note.DoubleFlat));
            Assert.AreEqual(Pitch.CSharp4.NoteWithLetter('C'), new Note('C', Note.Sharp));
            Assert.AreEqual(Pitch.CSharp4.NoteWithLetter('B'), new Note('B', Note.DoubleSharp));
            Assert.AreEqual(Pitch.CSharp4.NoteWithLetter('D'), new Note('D', Note.Flat));
            Assert.AreEqual(Pitch.B3.NoteWithLetter('B'), new Note('B', Note.Natural));
            Assert.AreEqual(Pitch.B3.NoteWithLetter('C'), new Note('C', Note.Flat));
            Assert.AreEqual(Pitch.B3.NoteWithLetter('D'), new Note('D', -3));
            Assert.AreEqual(Pitch.B3.NoteWithLetter('E'), new Note('E', -5));
        }

        [Test]
        public void AddSubtractTest()
        {
            Assert.AreEqual(Pitch.C4 + 1, Pitch.CSharp4);
            Assert.AreEqual(Pitch.F4 + 12, Pitch.F5);
            Assert.AreEqual(Pitch.C4 - 1, Pitch.B3);
            Assert.AreEqual(Pitch.D3 - 5, Pitch.A2);
        }
    }
}
