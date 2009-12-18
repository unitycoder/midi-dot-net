using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace MidiUnitTests
{
     /// <summary>Unit tests for the Control enum.</summary>
    [TestFixture]
    class ControlTest
    {
        [Test]
        public void Validity()
        {
            Assert.True(Control.Volume.IsValid());
            Control.Volume.Validate();
            Assert.True(Control.AllNotesOff.IsValid());
            Control.AllNotesOff.Validate();
            Assert.False(((Control)(-1)).IsValid());
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => ((Control)(-1)).Validate());
            Assert.False(((Control)(128)).IsValid());
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => ((Control)(128)).Validate());
        }

        [Test]
        public void Naming()
        {
            Assert.AreEqual(Control.SustainPedal.Name(), "Sustain pedal");
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => ((Control)(128)).Name());
        }
    }
}
