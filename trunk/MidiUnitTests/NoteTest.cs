using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace MidiUnitTests
{
    /// <summary>Unit tests for the Note class.</summary>
    [TestFixture]
    class NoteTest
    {
        [Test]
        public void ConstructionErrors()
        {
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => new Note('H'));
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => new Note('c'));
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => new Note('c', Note.Natural));
        }
        
        [Test]
        public void Equality()
        {
            Assert.AreEqual(new Note('C'), new Note('C', Note.Natural));
            Assert.AreEqual(new Note('C', Note.Natural), new Note('C', Note.Natural));
            Assert.AreEqual(new Note('C', Note.Sharp), new Note('C', Note.Sharp));
            Assert.AreNotEqual(new Note('C', Note.Natural), new Note('C', Note.Sharp));
            Assert.AreNotEqual(new Note('B', Note.Natural), new Note('C', Note.Natural));
            Assert.AreNotEqual(new Note('B', Note.Sharp), new Note('C', Note.Natural));
            Assert.AreNotEqual(new Note('C', Note.Sharp), new Note('D', Note.Flat));
        }

        [Test]
        public void IsEnharmonicWithTest()
        {
            Assert.True(new Note('C').IsEharmonicWith(new Note('C')));
            Assert.True(new Note('B', Note.Sharp).IsEharmonicWith(new Note('C', Note.Natural)));
            Assert.True(new Note('C', Note.Natural).IsEharmonicWith(new Note('B', Note.Sharp)));
            Assert.True(new Note('B', Note.DoubleSharp).IsEharmonicWith(new Note('C', Note.Sharp)));
            Assert.False(new Note('B', Note.DoubleSharp).IsEharmonicWith(
                new Note('D', Note.Natural)));
            Assert.True(new Note('B', 3).IsEharmonicWith(new Note('D', Note.Natural)));
            Assert.True(new Note('D', -3).IsEharmonicWith(new Note('B', Note.Natural)));
            Assert.True(new Note('C', 11).IsEharmonicWith(new Note('B', Note.Natural)));
        }

        [Test]
        public void PropertiesTest()
        {
            Assert.AreEqual(new Note('C').Letter, 'C');
            Assert.AreEqual(new Note('C', Note.Sharp).Letter, 'C');
            Assert.AreEqual(new Note('D', Note.Sharp).Letter, 'D');
            Assert.AreEqual(new Note('D', Note.Flat).Letter, 'D');
            Assert.AreEqual(new Note('B').Letter, 'B');

            Assert.AreEqual(new Note('C').Accidental, Note.Natural);
            Assert.AreEqual(new Note('C', Note.Sharp).Accidental, Note.Sharp);
            Assert.AreEqual(new Note('D', Note.Sharp).Accidental, Note.Sharp);
            Assert.AreEqual(new Note('D', Note.Flat).Accidental, Note.Flat);
            Assert.AreEqual(new Note('B').Accidental, Note.Natural);

            Assert.AreEqual(new Note('C').PositionInOctave, 0);
            Assert.AreEqual(new Note('C', Note.Sharp).PositionInOctave, 1);
            Assert.AreEqual(new Note('D', Note.Sharp).PositionInOctave, 3);
            Assert.AreEqual(new Note('D', Note.Flat).PositionInOctave, 1);
            Assert.AreEqual(new Note('B').PositionInOctave, 11);
            Assert.AreEqual(new Note('B', Note.Sharp).PositionInOctave, 0);
        }

        [Test]
        public void ToStringTest()
        {
            Assert.AreEqual(new Note('C').ToString(), "C");
            Assert.AreEqual(new Note('F', 5).ToString(), "F#####");
            Assert.AreEqual(new Note('G', -3).ToString(), "Gbbb");
        }

        [Test]
        public void NoteToPitchTest()
        {
            Assert.AreEqual(new Note('C').PitchInOctave(4), Pitch.C4);
            Assert.AreEqual(new Note('C', Note.Sharp).PitchInOctave(2), Pitch.CSharp2);
            Assert.AreEqual(new Note('D', Note.Flat).PitchInOctave(2), Pitch.CSharp2);
            Assert.AreEqual(new Note('B').PitchInOctave(-2), (Pitch)(-1));

            Assert.AreEqual(new Note('C').PitchAtOrAbove(Pitch.C4), Pitch.C4);
            Assert.AreEqual(new Note('C', Note.Sharp).PitchAtOrAbove(Pitch.C4), Pitch.CSharp4);
            Assert.AreEqual(new Note('F', Note.Flat).PitchAtOrAbove(Pitch.C4), Pitch.E4);
            Assert.AreEqual(new Note('B', Note.Sharp).PitchAtOrAbove(Pitch.C4), Pitch.C4);
            Assert.AreEqual(new Note('C').PitchAtOrAbove(Pitch.B3), Pitch.C4);
            Assert.AreEqual(new Note('F').PitchAtOrAbove(Pitch.G3), Pitch.F4);

            Assert.AreEqual(new Note('C').PitchAtOrBelow(Pitch.C4), Pitch.C4);
            Assert.AreEqual(new Note('C', Note.Sharp).PitchAtOrBelow(Pitch.C4), Pitch.CSharp3);
            Assert.AreEqual(new Note('F', Note.Flat).PitchAtOrBelow(Pitch.C4), Pitch.E3);
            Assert.AreEqual(new Note('B', Note.Sharp).PitchAtOrBelow(Pitch.C4), Pitch.C4);
            Assert.AreEqual(new Note('C').PitchAtOrBelow(Pitch.B3), Pitch.C3);
            Assert.AreEqual(new Note('F').PitchAtOrBelow(Pitch.G3), Pitch.F3);
        }

        [Test]
        public void SemitonesTest()
        {
            Assert.AreEqual(new Note('C').SemitonesUpTo(new Note('C')), 0);
            Assert.AreEqual(new Note('C').SemitonesUpTo(new Note('C', Note.Sharp)), 1);
            Assert.AreEqual(new Note('C', Note.Sharp).SemitonesUpTo(new Note('C')), 11);
            Assert.AreEqual(new Note('B').SemitonesUpTo(new Note('C')), 1);
            Assert.AreEqual(new Note('B').SemitonesUpTo(new Note('C', Note.Flat)), 0);
            Assert.AreEqual(new Note('G').SemitonesUpTo(new Note('F')), 10);

            Assert.AreEqual(new Note('C').SemitonesDownTo(new Note('C')), 0);
            Assert.AreEqual(new Note('C').SemitonesDownTo(new Note('C', Note.Sharp)), 11);
            Assert.AreEqual(new Note('C', Note.Sharp).SemitonesDownTo(new Note('C')), 1);
            Assert.AreEqual(new Note('B').SemitonesDownTo(new Note('C')), 11);
            Assert.AreEqual(new Note('B').SemitonesDownTo(new Note('C', Note.Flat)), 0);
            Assert.AreEqual(new Note('G').SemitonesDownTo(new Note('F')), 2);
        }
    }
}
