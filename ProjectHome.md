## Overview ##

This [.NET](http://www.microsoft.com/NET) library provides convenient, threadsafe access to [MIDI](http://www.midi.org) devices.  It does this by binding to the [Win32 API](http://msdn.microsoft.com/en-us/library/ms712733(VS.85).aspx) with [P/Invoke](http://msdn.microsoft.com/en-us/library/aa288468(VS.71).aspx) and then wrapping that in an object-oriented API which feels right at home in C# / .NET.

For a taste of what the API offers, check out the [example snippets](http://code.google.com/p/midi-dot-net/wiki/SimpleExamples).

## Features ##

  * **MIDI**
    * Easy access to MIDI [input](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.InputDevice.html) and [output](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.OutputDevice.html) devices.
    * Register input handlers using C# events / delegates.
    * Full support for sending and receiving [Note On](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.NoteOnMessage.html), [Note Off](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.NoteOffMessage.html), [Control Change](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.ControlChangeMessage.html), [Program Change](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.ProgramChangeMessage.html), and [Pitch Bend](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.PitchBendMessage.html) messages.
    * Convenient enums for General MIDI [Channels](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Channel.html), [Pitches](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Pitch.html), [Instruments](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Instrument.html), [Controls](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Control.html), and [Percussions](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Percussion.html).
  * **Scheduling**
    * A powerful [scheduler](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Clock.html) that lets you schedule MIDI messages, pause, unpause, and adjust beats-per-minute _while it runs_.
    * Support for [callback messages](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.CallbackMessage.html), so you can schedule call-outs to your own code at specific instants.  Callback messages are scheduled just like MIDI messages, so they are subject to pause, unpause, and beats-per-minute as well.
    * Callback messages can schedule additional messages when triggered (_self-propagating messages_), which allows for a flexible continuation-passing style of control.
    * Built-in [NoteOnOffMessage](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.NoteOnOffMessage.html) schedules the Off message when the On message triggers.
  * **Music Theory**
    * Octave-independent [Note](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Note.html) class which differentiates among enharmonic variants (eg, D♯; and E♭).  Notes can be resolved to specific pitches as needed.
    * [Scale](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Scale.html) class built from tonic note and ascent pattern.  Includes built-in patterns ([Major](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Scale.Major.html), [Melodic Minor Ascending](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Scale.MelodicMinorAscending.html), [Chromatic](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Scale.Chromatic.html), etc).
    * [Chord](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Chord.html) class built from tonic note, ascent pattern, and inversion.  Includes built-in chord patterns ([Major](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Chord.Major.html), [Seventh](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Chord.Seventh.html), [Diminished](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/Midi~Midi.Chord.Diminished.html), etc).
  * **Documentation**
    * Interactive demo programs, both [console](http://code.google.com/p/midi-dot-net/source/browse/trunk#trunk/ConsoleDemo) and [GUI](http://code.google.com/p/midi-dot-net/source/browse/trunk#trunk/GUIDemo).
    * Comprehensive [API documentation](http://midi-dot-net.googlecode.com/svn/trunk/Midi/docs/~Midi.html).
    * [Unit tests](http://code.google.com/p/midi-dot-net/source/browse/trunk#trunk/MidiUnitTests).

<table><tr>
<td valign='top'>
<h2>Not-Yet-Features</h2>

<ul><li>No support for extended MIDI messages such as System Exclusive ("sysex") messages.<br>
</li><li>No MIDI file I/O.</li></ul>

<h2>Requirements</h2>

<ul><li>.NET Framework 3.5.<br>
</li><li>Visual Studio C# 2008 (Express or Professional) or later.<br>
</li><li>winmm.dll, the Win32 multimedia API which is standard on all modern Windows installs.</li></ul>

<h2>Downloads</h2>

Source and binary distributions are available in the <a href='http://code.google.com/p/midi-dot-net/downloads/list'>Downloads</a> section.<br>
</td><td valign='top'><img width='350' height='268' src='https://lh6.googleusercontent.com/_tYS7EAsNKug/TWfOxyL3zLI/AAAAAAAAAo0/meGfGbG_464/s800/MonkeyMusic350.png' /></td></tr></table>
Copyright © 2009 Tom Lokovic

