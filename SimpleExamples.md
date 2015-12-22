## Introduction ##

This page gives some simple code snippets to give you a sense of how the [midi-dot-net](http://code.google.com/p/midi-dot-net/) API works.  For fuller, working examples see the [demo program](http://code.google.com/p/midi-dot-net/source/browse/trunk#trunk/MidiExamples) in the source distribution.


---


## Output ##

This snippet opens the first MIDI output device, sends a note On event, and then bends the pitch down.  See [Example02.cs](http://code.google.com/p/midi-dot-net/source/browse/trunk/MidiExamples/Example02.cs) for a fuller example.
```
OutputDevice outputDevice = OutputDevice.InstalledDevices[0];
outputDevice.Open();
outputDevice.SendNoteOn(Channel.Channel1, Pitch.C4, 80);  // Middle C, velocity 80
outputDevice.SendPitchBend(Channel.Channel1, 7000);  // 8192 is centered, so 7000 is bent down
...
```


---


## Input ##

This snippet opens the first MIDI input device, registers a NoteOn event listener, and then prints the names of notes played until a key on the computer keyboard is pressed.

```
public static void NoteOn(NoteOnMessage msg) {
   Console.WriteLine(msg.Pitch.NotePreferringSharps());
}
...
InputDevice inputDevice = InputDevice.InstalledDevices[0];
inputDevice.Open();
inputDevice.NoteOn += new InputDevice.NoteOnHandler(NoteOn);
inputDevice.StartReceiving(null);  // Note events will be received in another thread
Console.ReadKey();  // This thread waits for a keypress
...
```


---


## Input, Output, and Scheduling ##

This class implements a simple [arpeggiator](http://en.wikipedia.org/wiki/Arpeggiator).  It listens to an input device, and for each note played, it plays the [major third](http://en.wikipedia.org/wiki/Major_third) one beat later, and the [perfect fifth](http://en.wikipedia.org/wiki/Perfect_fifth) two beats later.  The effect is an arpeggiated major triad chord.  Time-delayed behavior like this is easy to implement with [Clock](http://code.google.com/p/midi-dot-net/source/browse/trunk/Midi/Clock.cs)'s scheduler.  (For a full example, see [Example04.cs](http://code.google.com/p/midi-dot-net/source/browse/trunk/MidiExamples/Example04.cs)).

```
class Arpeggiator {
    public Arpeggiator(InputDevice inputDevice, OutputDevice outputDevice, Clock clock) {
        this.inputDevice = inputDevice;
        this.outputDevice = outputDevice;
        this.clock = clock;

        if (inputDevice != null) {
            inputDevice.NoteOn += new InputDevice.NoteOnHandler(this.NoteOn);
            inputDevice.NoteOff += new InputDevice.NoteOffHandler(this.NoteOff);
        }
    }

    public void NoteOn(NoteOnMessage msg) {
        clock.Schedule(new NoteOnMessage(outputDevice, msg.Channel, msg.Pitch + 4,  msg.Velocity, msg.BeatTime + 1));
        clock.Schedule(new NoteOnMessage(outputDevice, msg.Channel, msg.Pitch + 7, msg.Velocity, msg.BeatTime + 2));
    }

    public void NoteOff(NoteOffMessage msg) {
        clock.Schedule(new NoteOffMessage(outputDevice, msg.Channel, msg.Pitch + 4, msg.Velocity, msg.BeatTime + 1));
        clock.Schedule(new NoteOffMessage(outputDevice, msg.Channel, msg.Pitch + 7, msg.Velocity, msg.BeatTime + 2));
    }

    private InputDevice inputDevice;
    private OutputDevice outputDevice;
    private Clock clock;
}
```


---
