using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace MidiUnitTests
{
    /// <summary>Unit tests for the Channel enum.</summary>
    [TestFixture]
    class ChannelTest
    {
        [Test]
        public void Validity()
        {
            Assert.True(Channel.Channel1.IsValid());
            Channel.Channel1.Validate();
            Assert.True(Channel.Channel6.IsValid());
            Channel.Channel16.Validate();
            Assert.False(((Channel)(-1)).IsValid());
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => ((Channel)(-1)).Validate());
            Assert.False(((Channel)(16)).IsValid());
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => ((Channel)(16)).Validate());
        }

        [Test]
        public void Naming()
        {
            Assert.AreEqual(Channel.Channel3.Name(), "Channel 3");
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => ((Channel)(16)).Name());
        }
    }
}
