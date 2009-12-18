using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace MidiUnitTests
{
    /// <summary>Unit tests for the Percussion enum.</summary>
    [TestFixture]
    class PercussionTest
    {
        [Test]
        public void Validity()
        {
            Assert.True(Percussion.BassDrum2.IsValid());
            Percussion.BassDrum2.Validate();
            Assert.True(Percussion.OpenTriangle.IsValid());
            Percussion.OpenTriangle.Validate();
            Assert.False(((Percussion)(34)).IsValid());
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => ((Percussion)(34)).Validate());
            Assert.False(((Percussion)(82)).IsValid());
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => ((Percussion)(82)).Validate());
        }

        [Test]
        public void Naming()
        {
            Assert.AreEqual(Percussion.VibraSlap.Name(), "Vibra Slap");
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => ((Percussion)(82)).Name());
        }
    }
}
