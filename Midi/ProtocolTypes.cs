using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Midi
{
    #region MIDI Channels.

    /// <summary>
    /// MIDI Channels.
    /// </summary>
    /// MIDI channels are named starting at 1, but encoded programmatically starting at 0.
    ///
    /// Channel10 is the dedicated percussion channel.
    public enum Channel
    {
        Channel1  = 0,
        Channel2  = 1,
        Channel3  = 2,
        Channel4  = 3,
        Channel5  = 4,
        Channel6  = 5,
        Channel7  = 6,
        Channel8  = 7,
        Channel9  = 8,
        Channel10 = 9,
        Channel11 = 10,
        Channel12 = 11,
        Channel13 = 12,
        Channel14 = 13,
        Channel15 = 14,
        Channel16 = 15
    };

    public static class ChannelExtensionMethods
    {
        /// <summary>
        /// Returns true if the specified channel is valid.
        /// </summary>
        /// <param name="channel">The channel to test.</param>
        public static bool IsValid(this Channel channel)
        {
            return (int)channel >= 0 && (int)channel < 16;
        }

        /// <summary>
        /// Table of channel names.
        /// </summary>
        private static string[] ChannelNames = new string[]
        {
            "Channel 1",
            "Channel 2",
            "Channel 3",
            "Channel 4",
            "Channel 5",
            "Channel 6",
            "Channel 7",
            "Channel 8",
            "Channel 9",
            "Channel 10"
        };

        /// <summary>
        /// Returns the human-readable name of a MIDI channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public static string Name(this Channel channel)
        {
            if (!channel.IsValid())
            {
                throw new InvalidOperationException("invalid Channel");
            }
            return ChannelNames[(int)channel];
        }
    }

    #endregion

    #region MIDI Instruments.

    /// <summary>
    /// General MIDI instruments.
    /// </summary>
    /// Officially, MIDI instruments are one-indexed, but we have them zero-indexed here since that's how
    /// they're used programmatically.
    public enum Instrument
    {
        // Piano Family:
        AcousticGrandPiano = 0,
        BrightAcousticPiano = 1,
        ElectricGrandPiano = 2,
        HonkyTonkPiano = 3,
        ElectricPiano1 = 4,
        ElectricPiano2 = 5,
        Harpsichord = 6,
        Clavinet = 7,

        // Chromatic Percussion Family:
        Celesta = 8,
        Glockenspiel = 9,
        MusicBox = 10,
        Vibraphone = 11,
        Marimba = 12,
        Xylophone = 13,
        TubularBells = 14,
        Dulcimer = 15,

        // Organ Family:
        DrawbarOrgan = 16,
        PercussiveOrgan = 17,
        RockOrgan = 18,
        ChurchOrgan = 19,
        ReedOrgan = 20,
        Accordion = 21,
        Harmonica = 22,
        TangoAccordion = 23,

        // Guitar Family:
        AcousticGuitarNylon = 24,
        AcousticGuitarSteel = 25,
        ElectricGuitarJass = 26,
        ElectricGuitarClean = 27,
        ElectricGuitarMuted = 28,
        OverdrivenGuitar = 29,
        DistortionGuitar = 30,
        GuitarHarmonics = 31,

        // Bass Family:
        AcousticBass = 32,
        ElectricBassFinger = 33,
        ElectricBassPick = 34,
        FretlessBass = 35,
        SlapBass1 = 36,
        SlapBass2 = 37,
        SynthBass1 = 38,
        SynthBass2 = 39,

        // Strings Family:
        Violin = 40,
        Viola = 41,
        Cello = 42,
        Contrabass = 43,
        TremoloStrings = 44,
        PizzicatoStrings = 45,
        OrchestralHarp = 46,
        Timpani = 47,

        // Ensemble Family:
        StringEnsemble1 = 48,
        StringEnsemble2 = 49,
        SynthStrings1 = 50,
        SynthStrings2 = 51,
        ChoirAahs = 52,
        VoiceOohs = 53,
        SynthVoice = 54,
        OrchestraHit = 55,
        
        // Brass Family:
        Trumpet = 56,
        Trombone = 57,
        Tuba = 58,
        MutedTrumpet = 59,
        FrenchHorn = 60,
        BrassSection = 61,
        SynthBrass1 = 62,
        SynthBrass2 = 63,

        // Reed Family:
        SopranoSax = 64,
        AltoSax = 65,
        TenorSax = 66,
        BaritoneSax = 67,
        Oboe = 68,
        EnglishHorn = 69,
        Bassoon = 70,
        Clarinet = 71,

        // Pipe Family:
        Piccolo = 72,
        Flute = 73,
        Recorder = 74,
        PanFlute = 75,
        BlownBottle = 76,
        Shakuhachi = 77,
        Whistle = 78,
        Ocarina = 79,

        // Synth Lead Family:
        Lead1Square = 80,
        Lead2Sawtooth = 81,
        Lead3Calliope = 82,
        Lead4Chiff = 83,
        Lead5Charang = 84,
        Lead6Voice = 85,
        Lead7Fifths = 86,
        Lead8BassPlusLead = 87,

        // Synth Pad Family:
        Pad1NewAge = 88,
        Pad2Warm = 89,
        Pad3Polysynth = 90,
        Pad4Choir = 91,
        Pad5Bowed = 92,
        Pad6Metallic = 93,
        Pad7Halo = 94,
        Pad8Sweep = 95,

        // Synth Effects Family:
        FX1Rain = 96,
        FX2Soundtrack = 97,
        FX3Crystal = 98,
        FX4Atmosphere = 99,
        FX5Brightness = 100,
        FX6Goblins = 101,
        FX7Echoes = 102,
        FX8SciFi = 103,
        
        // Ethnic Family:
        Sitar = 104,
        Banjo = 105,
        Shamisen = 106,
        Koto = 107,
        Kalimba = 108,
        Bagpipe = 109,
        Fiddle = 110,
        Shanai = 111,

        // Percussive Family:
        TinkleBell = 112,
        Agogo = 113,
        SteelDrums = 114,
        Woodblock = 115,
        TaikoDrum = 116,
        MelodicTom = 117,
        SynthDrum = 118,
        ReverseCymbal = 119,

        // Sound Effects Family:
        GuitarFretNoise = 120,
        BreathNoise = 121,
        Seashore = 122,
        BirdTweet = 123,
        TelephoneRing = 124,
        Helicopter = 125,
        Applause = 126,
        Gunshot = 127
    };

    public static class InstrumentExtensionMethods
    {
        /// <summary>
        /// Returns true if the specified instrument is valid.
        /// </summary>
        /// <param name="instrument">The instrument to test.</param>
        public static bool IsValid(this Instrument instrument)
        {
            return (int)instrument >= 0 && (int)instrument < 128;
        }
        
        /// <summary>
        /// General Midi instrument names, used by GetInstrumentName().
        /// </summary>
        private static string[] InstrumentNames = new string[]
        {
            // Piano Family:
            "Acoustic Grand Piano",
            "Bright Acoustic Piano",
            "Electric Grand Piano",
            "Honky-tonk Piano",
            "Electric Piano 1",
            "Electric Piano 2",
            "Harpsichord",
            "Clavinet",

            // Chromatic Percussion Family:
            "Celesta",
            "Glockenspiel",
            "Music Box",
            "Vibraphone",
            "Marimba",
            "Xylophone",
            "Tubular Bells",
            "Dulcimer",

            // Organ Family:
            "Drawbar Organ",
            "Percussive Organ",
            "Rock Organ",
            "Church Organ",
            "Reed Organ",
            "Accordion",
            "Harmonica",
            "Tango Accordion",

            // Guitar Family:
            "Acoustic Guitar (nylon)",
            "Acoustic Guitar (steel)",
            "Electric Guitar (jazz)",
            "Electric Guitar (clean)",
            "Electric Guitar (muted)",
            "Overdriven Guitar",
            "Distortion Guitar",
            "Guitar harmonics",

            // Bass Family:
            "Acoustic Bass",
            "Electric Bass (finger)",
            "Electric Bass (pick)",
            "Fretless Bass",
            "Slap Bass 1",
            "Slap Bass 2",
            "Synth Bass 1",
            "Synth Bass 2",

            // Strings Family:
            "Violin",
            "Viola",
            "Cello",
            "Contrabass",
            "Tremolo Strings",
            "Pizzicato Strings",
            "Orchestral Harp",
            "Timpani",

            // Ensemble Family:
            "String Ensemble 1",
            "String Ensemble 2",
            "Synth Strings 1",
            "Synth Strings 2",
            "Choir Aahs",
            "Voice Oohs",
            "Synth Voice",
            "Orchestra Hit",

            // Brass Family:
            "Trumpet",
            "Trombone",
            "Tuba",
            "Muted Trumpet",
            "French Horn",
            "Brass Section",
            "Synth Brass 1",
            "Synth Brass 2",
            	
            // Reed Family:
            "Soprano Sax",
            "Alto Sax",
            "Tenor Sax",
            "Baritone Sax",
            "Oboe",
            "English Horn",
            "Bassoon",
            "Clarinet",

            // Pipe Family:
            "Piccolo",
            "Flute",
            "Recorder",
            "Pan Flute",
            "Blown Bottle",
            "Shakuhachi",
            "Whistle",
            "Ocarina",

            // Synth Lead Family:
            "Lead 1 (square)",
            "Lead 2 (sawtooth)",
            "Lead 3 (calliope)",
            "Lead 4 (chiff)",
            "Lead 5 (charang)",
            "Lead 6 (voice)",
            "Lead 7 (fifths)",
            "Lead 8 (bass + lead)",

            // Synth Pad Family:
            "Pad 1 (new age)",
            "Pad 2 (warm)",
            "Pad 3 (polysynth)",
            "Pad 4 (choir)",
            "Pad 5 (bowed)",
            "Pad 6 (metallic)",
            "Pad 7 (halo)",
            "Pad 8 (sweep)",

            // Synth Effects Family:
            "FX 1 (rain)",
            "FX 2 (soundtrack)",
            "FX 3 (crystal)",
            "FX 4 (atmosphere)",
            "FX 5 (brightness)",
            "FX 6 (goblins)",
            "FX 7 (echoes)",
            "FX 8 (sci-fi)",

            // Ethnic Family:
            "Sitar",
            "Banjo",
            "Shamisen",
            "Koto",
            "Kalimba",
            "Bag pipe",
            "Fiddle",
            "Shanai",

            // Percussive Family:
            "Tinkle Bell",
            "Agogo",
            "Steel Drums",
            "Woodblock",
            "Taiko Drum",
            "Melodic Tom",
            "Synth Drum",
            "Reverse Cymbal",

            // Sound Effects Family:
            "Guitar Fret Noise",
            "Breath Noise",
            "Seashore",
            "Bird Tweet",
            "Telephone Ring",
            "Helicopter",
            "Applause",
            "Gunshot"
        };

        /// <summary>
        /// Returns the human-readable name of a MIDI instrument.
        /// </summary>
        /// <param name="instrument">The instrument.</param>
        public static string Name(this Instrument instrument)
        {
            if (!instrument.IsValid())
            {
                throw new InvalidOperationException("invalid Instrument");
            }
            return InstrumentNames[(int)instrument];
        }
    }

    #endregion

    #region MIDI Percussions

    /// <summary>
    /// General MIDI percussion notes.
    /// </summary>
    /// In General MIDI, notes played on channel 10 (channel 9 in code) make the following percussion sounds,
    /// regardless of any ProgramChange messages on that channel.
    public enum Percussion
    {
        BassDrum2 = 35,
        BassDrum1 = 36,
        SideStick = 37,
        SnareDrum1 = 38,
        HandClap = 39,
        SnareDrum2 = 40,
        LowTom2 = 41,
        ClosedHiHat = 42,
        LowTom1 = 43,
        PedalHiHat = 44,
        MidTom2 = 45,
        OpenHiHat = 46,
        MidTom1 = 47,
        HighTom2 = 48,
        CrashCymbal1 = 49,
        HighTom1 = 50,
        RideCymbal1 = 51,
        ChineseCymbal = 52,
        RideBell = 53,
        Tambourine = 54,
        SplashCymbal = 55,
        Cowbell = 56,
        CrashCymbal2 = 57,
        VibraSlap = 58,
        RideCymbal2 = 59,
        HighBongo = 60,
        LowBongo = 61,
        MuteHighConga = 62,
        OpenHighConga = 63,
        LowConga = 64,
        HighTimbale = 65,
        LowTimbale = 66,
        HighAgogo = 67,
        LowAgogo = 68,
        Cabasa = 69,
        Maracas = 70,
        ShortWhistle = 71,
        LongWhistle = 72,
        ShortGuiro = 74,
        LongGuiro = 74,
        Claves = 75,
        HighWoodBlock = 76,
        LowWoodBlock = 77,
        MuteCuica = 78,
        OpenCuica = 79,
        MuteTriangle = 80,
        OpenTriangle = 81
    };

    public static class PercussionExtensionMethods
    {
        /// <summary>
        /// Returns true if the specified percussion is valid.
        /// </summary>
        /// <param name="percussion">The percussion to test.</param>
        public static bool IsValid(this Percussion percussion)
        {
            return (int)percussion >= 35 && (int)percussion < 81;
        }
        
        private static string[] PercussionNames = new string[]
        {
            "Bass Drum 2",
            "Bass Drum 1",
            "Side Stick",
            "Snare Drum 1",
            "Hand Clap",
            "Snare Drum 2",
            "Low Tom 2",
            "Closed Hi-hat",
            "Low Tom 1",
            "Pedal Hi-hat",
            "Mid Tom 2",
            "Open Hi-hat",
            "Mid Tom 1",
            "High Tom 2",
            "Crash Cymbal 1",
            "High Tom 1",
            "Ride Cymbal 1",
            "Chinese Cymbal",
            "Ride Bell",
            "Tambourine",
            "Splash Cymbal",
            "Cowbell",
            "Crash Cymbal 2",
            "Vibra Slap",
            "Ride Cymbal 2",
            "High Bongo",
            "Low Bongo",
            "Mute High Conga",
            "Open High Conga",
            "Low Conga",
            "High Timbale",
            "Low Timbale",
            "High Agogo",
            "Low Agogo",
            "Cabasa",
            "Maracas",
            "Short Whistle",
            "Long Whistle",
            "Short Guiro",
            "Long Guiro",
            "Claves",
            "High Wood Block",
            "Low Wood Block",
            "Mute Cuica",
            "Open Cuica",
            "Mute Triangle",
            "Open Triangle"
        };

        /// <summary>
        /// Returns the human-readable name of a MIDI percussion.
        /// </summary>
        /// <param name="percussion">The percussion.</param>
        public static string Name(this Percussion percussion)
        {
            if (!percussion.IsValid())
            {
                throw new InvalidOperationException("invalid Percussion");
            }
            return PercussionNames[(int)percussion];
        }
    }

    #endregion

    #region Controls

    /// <summary>
    /// MIDI Controls as used in Control Change messages.
    /// </summary>
    /// This is an incomplete list of controls, and the details of how each control is encoded and
    /// used is complicated.  See the MIDI spec for details.
    ///
    /// The most commonly used control is SustainPedal, which is considered off when &lt; 64, on when &gt; 64.
    public enum Control
    {
        ModulationWheel = 1,
        DataEntryMSB = 6,
        Volume = 7,
        Pan = 10,
        Expression = 11,
        DataEntryLSB = 38,
        SustainPedal = 64,
        ReverbLevel = 91,
        TremoloLevel = 92,
        ChorusLevel = 93,
        CelesteLevel = 94,
        PhaserLevel = 95,
        NonRegisteredParameterLSB = 98,
        NonRegisteredParameterMSB = 99,
        RegisteredParameterNumberLSB = 100,
        RegisteredParameterNumberMSB = 101,
        AllControllersOff = 121,
        AllNotesOff = 123
    }

    public static class ControlExtensionMethods
    {
        /// <summary>
        /// Returns true if the specified control is valid.
        /// </summary>
        /// <param name="control">The Control to test.</param>
        public static bool IsValid(this Control control)
        {
            return (int)control >= 0 && (int)control < 128;
        }

        /// <summary>
        /// Table of control names.
        /// </summary>
        public static Dictionary<int, string> ControlNames = new Dictionary<int, string>
        {
            {1, "Modulation wheel"},
            {6, "Data Entry MSB"},
            {7, "Volume"},
            {10, "Pan"},
            {11, "Expression"},
            {38, "Data Entry LSB"},
            {64, "Sustain pedal"},
            {91, "Reverb level"},
            {92, "Tremolo level"},
            {93, "Chorus level"},
            {94, "Celeste level"},
            {95, "Phaser level"},
            {98, "Non-registered Parameter LSB"},
            {99, "Non-registered Parameter MSB"},
            {100, "Registered Parameter Number LSB"},
            {101, "Registered Parameter Number MSB"},
            {121, "All controllers off"},
            {123, "All notes off"}
        };

        /// <summary>
        /// Returns the human-readable name of a MIDI control.
        /// </summary>
        /// <param name="control">The control.</param>
        public static string Name(this Control control)
        {
            if (!control.IsValid())
            {
                throw new InvalidOperationException("invalid Control");
            }
            if (ControlNames.ContainsKey((int)control))
            {
                return ControlNames[(int)control];
            }
            else
            {
                return "Other Control (see MIDI spec for details).";
            }
        }
    }

    #endregion
}
