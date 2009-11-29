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
        /// <summary> MIDI Channel 1. </summary>
        Channel1  = 0,
        /// <summary> MIDI Channel 2. </summary>
        Channel2 = 1,
        /// <summary> MIDI Channel 3. </summary>
        Channel3 = 2,
        /// <summary> MIDI Channel 4. </summary>
        Channel4 = 3,
        /// <summary> MIDI Channel 5. </summary>
        Channel5 = 4,
        /// <summary> MIDI Channel 6. </summary>
        Channel6 = 5,
        /// <summary> MIDI Channel 7. </summary>
        Channel7 = 6,
        /// <summary> MIDI Channel 8. </summary>
        Channel8 = 7,
        /// <summary> MIDI Channel 9. </summary>
        Channel9 = 8,
        /// <summary> MIDI Channel 10, the dedicated percussion channel. </summary>
        Channel10 = 9,
        /// <summary> MIDI Channel 11. </summary>
        Channel11 = 10,
        /// <summary> MIDI Channel 12. </summary>
        Channel12 = 11,
        /// <summary> MIDI Channel 13. </summary>
        Channel13 = 12,
        /// <summary> MIDI Channel 14. </summary>
        Channel14 = 13,
        /// <summary> MIDI Channel 15. </summary>
        Channel15 = 14,
        /// <summary> MIDI Channel 16. </summary>
        Channel16 = 15
    };

    /// <summary>
    /// Extension methods for the Channel enum.
    /// </summary>
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

        /// <summary>General MIDI instrument 0 ("Acoustic Grand Piano").</summary>
        AcousticGrandPiano = 0,
        /// <summary>General MIDI instrument 1 ("Bright Acoustic Piano").</summary>
        BrightAcousticPiano = 1,
        /// <summary>General MIDI instrument 2 ("Electric Grand Piano").</summary>
        ElectricGrandPiano = 2,
        /// <summary>General MIDI instrument 3 ("Honky Tonk Piano").</summary>
        HonkyTonkPiano = 3,
        /// <summary>General MIDI instrument 4 ("Electric Piano 1").</summary>
        ElectricPiano1 = 4,
        /// <summary>General MIDI instrument 5 ("Electric Piano 2").</summary>
        ElectricPiano2 = 5,
        /// <summary>General MIDI instrument 6 ("Harpsichord").</summary>
        Harpsichord = 6,
        /// <summary>General MIDI instrument 7 ("Clavinet").</summary>
        Clavinet = 7,

        // Chromatic Percussion Family:

        /// <summary>General MIDI instrument 8 ("Celesta").</summary>
        Celesta = 8,
        /// <summary>General MIDI instrument 9 ("Glockenspiel").</summary>
        Glockenspiel = 9,
        /// <summary>General MIDI instrument 10 ("Music Box").</summary>
        MusicBox = 10,
        /// <summary>General MIDI instrument 11 ("Vibraphone").</summary>
        Vibraphone = 11,
        /// <summary>General MIDI instrument 12 ("Marimba").</summary>
        Marimba = 12,
        /// <summary>General MIDI instrument 13 ("Xylophone").</summary>
        Xylophone = 13,
        /// <summary>General MIDI instrument 14 ("Tubular Bells").</summary>
        TubularBells = 14,
        /// <summary>General MIDI instrument 15 ("Dulcimer").</summary>
        Dulcimer = 15,

        // Organ Family:

        /// <summary>General MIDI instrument 16 ("Drawbar Organ").</summary>
        DrawbarOrgan = 16,
        /// <summary>General MIDI instrument 17 ("Percussive Organ").</summary>
        PercussiveOrgan = 17,
        /// <summary>General MIDI instrument 18 ("Rock Organ").</summary>
        RockOrgan = 18,
        /// <summary>General MIDI instrument 19 ("Church Organ").</summary>
        ChurchOrgan = 19,
        /// <summary>General MIDI instrument 20 ("Reed Organ").</summary>
        ReedOrgan = 20,
        /// <summary>General MIDI instrument 21 ("Accordion").</summary>
        Accordion = 21,
        /// <summary>General MIDI instrument 22 ("Harmonica").</summary>
        Harmonica = 22,
        /// <summary>General MIDI instrument 23 ("Tango Accordion").</summary>
        TangoAccordion = 23,

        // Guitar Family:

        /// <summary>General MIDI instrument 24 ("Acoustic Guitar (nylon)").</summary>
        AcousticGuitarNylon = 24,
        /// <summary>General MIDI instrument 25 ("Acoustic Guitar (steel)").</summary>
        AcousticGuitarSteel = 25,
        /// <summary>General MIDI instrument 26 ("Electric Guitar (jazz)").</summary>
        ElectricGuitarJazz = 26,
        /// <summary>General MIDI instrument 27 ("Electric Guitar (clean)").</summary>
        ElectricGuitarClean = 27,
        /// <summary>General MIDI instrument 28 ("Electric Guitar (muted)").</summary>
        ElectricGuitarMuted = 28,
        /// <summary>General MIDI instrument 29 ("Overdriven Guitar").</summary>
        OverdrivenGuitar = 29,
        /// <summary>General MIDI instrument 30 ("Distortion Guitar").</summary>
        DistortionGuitar = 30,
        /// <summary>General MIDI instrument 31 ("Guitar Harmonics").</summary>
        GuitarHarmonics = 31,

        // Bass Family:

        /// <summary>General MIDI instrument 32 ("Acoustic Bass").</summary>
        AcousticBass = 32,
        /// <summary>General MIDI instrument 33 ("Electric Bass (finger)").</summary>
        ElectricBassFinger = 33,
        /// <summary>General MIDI instrument 34 ("Electric Bass (pick)").</summary>
        ElectricBassPick = 34,
        /// <summary>General MIDI instrument 35 ("Fretless Bass").</summary>
        FretlessBass = 35,
        /// <summary>General MIDI instrument 36 ("Slap Bass 1").</summary>
        SlapBass1 = 36,
        /// <summary>General MIDI instrument 37 ("Slap Bass 2").</summary>
        SlapBass2 = 37,
        /// <summary>General MIDI instrument 38 ("Synth Bass 1").</summary>
        SynthBass1 = 38,
        /// <summary>General MIDI instrument 39("Synth Bass 2").</summary>
        SynthBass2 = 39,

        // Strings Family:

        /// <summary>General MIDI instrument 40 ("Violin").</summary>
        Violin = 40,
        /// <summary>General MIDI instrument 41 ("Viola").</summary>
        Viola = 41,
        /// <summary>General MIDI instrument 42 ("Cello").</summary>
        Cello = 42,
        /// <summary>General MIDI instrument 43 ("Contrabass").</summary>
        Contrabass = 43,
        /// <summary>General MIDI instrument 44 ("Tremolo Strings").</summary>
        TremoloStrings = 44,
        /// <summary>General MIDI instrument 45 ("Pizzicato Strings").</summary>
        PizzicatoStrings = 45,
        /// <summary>General MIDI instrument 46 ("Orchestral Harp").</summary>
        OrchestralHarp = 46,
        /// <summary>General MIDI instrument 47 ("Timpani").</summary>
        Timpani = 47,

        // Ensemble Family:

        /// <summary>General MIDI instrument 48 ("String Ensemble 1").</summary>
        StringEnsemble1 = 48,
        /// <summary>General MIDI instrument 49 ("String Ensemble 2").</summary>
        StringEnsemble2 = 49,
        /// <summary>General MIDI instrument 50 ("Synth Strings 1").</summary>
        SynthStrings1 = 50,
        /// <summary>General MIDI instrument 51 ("Synth Strings 2").</summary>
        SynthStrings2 = 51,
        /// <summary>General MIDI instrument 52 ("Choir Aahs").</summary>
        ChoirAahs = 52,
        /// <summary>General MIDI instrument 53 ("Voice oohs").</summary>
        VoiceOohs = 53,
        /// <summary>General MIDI instrument 54 ("Synth Voice").</summary>
        SynthVoice = 54,
        /// <summary>General MIDI instrument 55 ("Orchestra Hit").</summary>
        OrchestraHit = 55,
        
        // Brass Family:

        /// <summary>General MIDI instrument 56 ("Trumpet").</summary>
        Trumpet = 56,
        /// <summary>General MIDI instrument 57 ("Trombone").</summary>
        Trombone = 57,
        /// <summary>General MIDI instrument 58 ("Tuba").</summary>
        Tuba = 58,
        /// <summary>General MIDI instrument 59 ("Muted Trumpet").</summary>
        MutedTrumpet = 59,
        /// <summary>General MIDI instrument 60 ("French Horn").</summary>
        FrenchHorn = 60,
        /// <summary>General MIDI instrument 61 ("Brass Section").</summary>
        BrassSection = 61,
        /// <summary>General MIDI instrument 62 ("Synth Brass 1").</summary>
        SynthBrass1 = 62,
        /// <summary>General MIDI instrument 63 ("Synth Brass 2").</summary>
        SynthBrass2 = 63,

        // Reed Family:

        /// <summary>General MIDI instrument 64 ("Soprano Sax").</summary>
        SopranoSax = 64,
        /// <summary>General MIDI instrument 65 ("Alto Sax").</summary>
        AltoSax = 65,
        /// <summary>General MIDI instrument 66 ("Tenor Sax").</summary>
        TenorSax = 66,
        /// <summary>General MIDI instrument 67 ("Baritone Sax").</summary>
        BaritoneSax = 67,
        /// <summary>General MIDI instrument 68 ("Oboe").</summary>
        Oboe = 68,
        /// <summary>General MIDI instrument 69 ("English Horn").</summary>
        EnglishHorn = 69,
        /// <summary>General MIDI instrument 70 ("Bassoon").</summary>
        Bassoon = 70,
        /// <summary>General MIDI instrument 71 ("Clarinet").</summary>
        Clarinet = 71,

        // Pipe Family:

        /// <summary>General MIDI instrument 72 ("Piccolo").</summary>
        Piccolo = 72,
        /// <summary>General MIDI instrument 73 ("Flute").</summary>
        Flute = 73,
        /// <summary>General MIDI instrument 74 ("Recorder").</summary>
        Recorder = 74,
        /// <summary>General MIDI instrument 75 ("PanFlute").</summary>
        PanFlute = 75,
        /// <summary>General MIDI instrument 76 ("Blown Bottle").</summary>
        BlownBottle = 76,
        /// <summary>General MIDI instrument 77 ("Shakuhachi").</summary>
        Shakuhachi = 77,
        /// <summary>General MIDI instrument 78 ("Whistle").</summary>
        Whistle = 78,
        /// <summary>General MIDI instrument 79 ("Ocarina").</summary>
        Ocarina = 79,

        // Synth Lead Family:

        /// <summary>General MIDI instrument 80 ("Lead 1 (square)").</summary>
        Lead1Square = 80,
        /// <summary>General MIDI instrument 81 ("Lead 2 (sawtooth)").</summary>
        Lead2Sawtooth = 81,
        /// <summary>General MIDI instrument 82 ("Lead 3 (calliope)").</summary>
        Lead3Calliope = 82,
        /// <summary>General MIDI instrument 83 ("Lead 4 (chiff)").</summary>
        Lead4Chiff = 83,
        /// <summary>General MIDI instrument 84 ("Lead 5 (charang)").</summary>
        Lead5Charang = 84,
        /// <summary>General MIDI instrument 85 ("Lead 6 (voice)").</summary>
        Lead6Voice = 85,
        /// <summary>General MIDI instrument 86 ("Lead 7 (fifths)").</summary>
        Lead7Fifths = 86,
        /// <summary>General MIDI instrument 87 ("Lead 8 (bass + lead)").</summary>
        Lead8BassPlusLead = 87,

        // Synth Pad Family:

        /// <summary>General MIDI instrument 88 ("Pad 1 (new age)").</summary>
        Pad1NewAge = 88,
        /// <summary>General MIDI instrument 89 ("Pad 2 (warm)").</summary>
        Pad2Warm = 89,
        /// <summary>General MIDI instrument 90 ("Pad 3 (polysynth)").</summary>
        Pad3Polysynth = 90,
        /// <summary>General MIDI instrument 91 ("Pad 4 (choir)").</summary>
        Pad4Choir = 91,
        /// <summary>General MIDI instrument 92 ("Pad 5 (bowed)").</summary>
        Pad5Bowed = 92,
        /// <summary>General MIDI instrument 93 ("Pad 6 (metallic)").</summary>
        Pad6Metallic = 93,
        /// <summary>General MIDI instrument 94 ("Pad 7 (halo)").</summary>
        Pad7Halo = 94,
        /// <summary>General MIDI instrument 95 ("Pad 8 (sweep)").</summary>
        Pad8Sweep = 95,

        // Synth Effects Family:

        /// <summary>General MIDI instrument 96 ("FX 1 (rain)").</summary>
        FX1Rain = 96,
        /// <summary>General MIDI instrument 97 ("FX 2 (soundtrack)").</summary>
        FX2Soundtrack = 97,
        /// <summary>General MIDI instrument 98 ("FX 3 (crystal)").</summary>
        FX3Crystal = 98,
        /// <summary>General MIDI instrument 99 ("FX 4 (atmosphere)").</summary>
        FX4Atmosphere = 99,
        /// <summary>General MIDI instrument 100 ("FX 5 (brightness)").</summary>
        FX5Brightness = 100,
        /// <summary>General MIDI instrument 101 ("FX 6 (goblins)").</summary>
        FX6Goblins = 101,
        /// <summary>General MIDI instrument 102 ("FX 7 (echoes)").</summary>
        FX7Echoes = 102,
        /// <summary>General MIDI instrument 103 ("FX 8 (sci-fi)").</summary>
        FX8SciFi = 103,
        
        // Ethnic Family:

        /// <summary>General MIDI instrument 104 ("Sitar").</summary>
        Sitar = 104,
        /// <summary>General MIDI instrument 105 ("Banjo").</summary>
        Banjo = 105,
        /// <summary>General MIDI instrument 106 ("Shamisen").</summary>
        Shamisen = 106,
        /// <summary>General MIDI instrument 107 ("Koto").</summary>
        Koto = 107,
        /// <summary>General MIDI instrument 108 ("Kalimba").</summary>
        Kalimba = 108,
        /// <summary>General MIDI instrument 109 ("Bagpipe").</summary>
        Bagpipe = 109,
        /// <summary>General MIDI instrument 110 ("Fiddle").</summary>
        Fiddle = 110,
        /// <summary>General MIDI instrument 111 ("Shanai").</summary>
        Shanai = 111,

        // Percussive Family:

        /// <summary>General MIDI instrument 112 ("Tinkle Bell").</summary>
        TinkleBell = 112,
        /// <summary>General MIDI instrument 113 (Agogo"").</summary>
        Agogo = 113,
        /// <summary>General MIDI instrument 114 ("Steel Drums").</summary>
        SteelDrums = 114,
        /// <summary>General MIDI instrument 115 ("Woodblock").</summary>
        Woodblock = 115,
        /// <summary>General MIDI instrument 116 ("Taiko Drum").</summary>
        TaikoDrum = 116,
        /// <summary>General MIDI instrument 117 ("Melodic Tom").</summary>
        MelodicTom = 117,
        /// <summary>General MIDI instrument 118 ("Synth Drum").</summary>
        SynthDrum = 118,
        /// <summary>General MIDI instrument 119 ("Reverse Cymbal").</summary>
        ReverseCymbal = 119,

        // Sound Effects Family:

        /// <summary>General MIDI instrument 120 ("Guitar Fret Noise").</summary>
        GuitarFretNoise = 120,
        /// <summary>General MIDI instrument 121 ("Breath Noise").</summary>
        BreathNoise = 121,
        /// <summary>General MIDI instrument 122 ("Seashore").</summary>
        Seashore = 122,
        /// <summary>General MIDI instrument 123 ("Bird Tweet").</summary>
        BirdTweet = 123,
        /// <summary>General MIDI instrument 124 ("Telephone Ring").</summary>
        TelephoneRing = 124,
        /// <summary>General MIDI instrument 125 ("Helicopter").</summary>
        Helicopter = 125,
        /// <summary>General MIDI instrument 126 ("Applause").</summary>
        Applause = 126,
        /// <summary>General MIDI instrument 127 ("Gunshot").</summary>
        Gunshot = 127
    };

    /// <summary>
    /// Extension methods for the Instrument enum.
    /// </summary>
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
        /// <summary>General MIDI percussion 35 ("Bass Drum 2").</summary>
        BassDrum2 = 35,
        /// <summary>General MIDI percussion 36 ("Bass Drum 1").</summary>
        BassDrum1 = 36,
        /// <summary>General MIDI percussion 37 ("Side Stick").</summary>
        SideStick = 37,
        /// <summary>General MIDI percussion 38 ("Snare Drum 1").</summary>
        SnareDrum1 = 38,
        /// <summary>General MIDI percussion 39 ("Hand Clap").</summary>
        HandClap = 39,
        /// <summary>General MIDI percussion 40 ("Snare Drum 2").</summary>
        SnareDrum2 = 40,
        /// <summary>General MIDI percussion 41 ("Low Tom 2").</summary>
        LowTom2 = 41,
        /// <summary>General MIDI percussion 42 ("Closed Hi-hat").</summary>
        ClosedHiHat = 42,
        /// <summary>General MIDI percussion 43 ("Low Tom 1").</summary>
        LowTom1 = 43,
        /// <summary>General MIDI percussion 44 ("Pedal Hi-hat").</summary>
        PedalHiHat = 44,
        /// <summary>General MIDI percussion 45 ("Mid Tom 2").</summary>
        MidTom2 = 45,
        /// <summary>General MIDI percussion 46 ("Open Hi-hat").</summary>
        OpenHiHat = 46,
        /// <summary>General MIDI percussion 47 ("Mid Tom 1").</summary>
        MidTom1 = 47,
        /// <summary>General MIDI percussion 48 ("High Tom 2").</summary>
        HighTom2 = 48,
        /// <summary>General MIDI percussion 49 ("Crash Cymbal 1").</summary>
        CrashCymbal1 = 49,
        /// <summary>General MIDI percussion 50 ("High Tom 1").</summary>
        HighTom1 = 50,
        /// <summary>General MIDI percussion 51 ("Ride Cymbal 1").</summary>
        RideCymbal1 = 51,
        /// <summary>General MIDI percussion 52 ("Chinese Cymbal").</summary>
        ChineseCymbal = 52,
        /// <summary>General MIDI percussion 53 ("Ride Bell").</summary>
        RideBell = 53,
        /// <summary>General MIDI percussion 54 ("Tambourine").</summary>
        Tambourine = 54,
        /// <summary>General MIDI percussion 55 ("Splash Cymbal").</summary>
        SplashCymbal = 55,
        /// <summary>General MIDI percussion 56 ("Cowbell").</summary>
        Cowbell = 56,
        /// <summary>General MIDI percussion 57 ("Crash Cymbal 2").</summary>
        CrashCymbal2 = 57,
        /// <summary>General MIDI percussion 58 ("Vibra Slap").</summary>
        VibraSlap = 58,
        /// <summary>General MIDI percussion 59 ("Ride Cymbal 2").</summary>
        RideCymbal2 = 59,
        /// <summary>General MIDI percussion 60 ("High Bongo").</summary>
        HighBongo = 60,
        /// <summary>General MIDI percussion 61 ("Low Bongo").</summary>
        LowBongo = 61,
        /// <summary>General MIDI percussion 62 ("Mute High Conga").</summary>
        MuteHighConga = 62,
        /// <summary>General MIDI percussion 63 ("Open High Conga").</summary>
        OpenHighConga = 63,
        /// <summary>General MIDI percussion 64 ("Low Conga").</summary>
        LowConga = 64,
        /// <summary>General MIDI percussion 65 ("High Timbale").</summary>
        HighTimbale = 65,
        /// <summary>General MIDI percussion 66 ("Low Timbale").</summary>
        LowTimbale = 66,
        /// <summary>General MIDI percussion 67 ("High Agogo").</summary>
        HighAgogo = 67,
        /// <summary>General MIDI percussion 68 ("Low Agogo").</summary>
        LowAgogo = 68,
        /// <summary>General MIDI percussion 69 ("Cabasa").</summary>
        Cabasa = 69,
        /// <summary>General MIDI percussion 70 ("Maracas").</summary>
        Maracas = 70,
        /// <summary>General MIDI percussion 71 ("Short Whistle").</summary>
        ShortWhistle = 71,
        /// <summary>General MIDI percussion 72 ("Long Whistle").</summary>
        LongWhistle = 72,
        /// <summary>General MIDI percussion 73 ("Short Guiro").</summary>
        ShortGuiro = 74,
        /// <summary>General MIDI percussion 74 ("Long Guiro").</summary>
        LongGuiro = 74,
        /// <summary>General MIDI percussion 75 ("Claves").</summary>
        Claves = 75,
        /// <summary>General MIDI percussion 76 ("High Wood Block").</summary>
        HighWoodBlock = 76,
        /// <summary>General MIDI percussion 77 ("Low Wood Block").</summary>
        LowWoodBlock = 77,
        /// <summary>General MIDI percussion 78 ("Mute Cuica").</summary>
        MuteCuica = 78,
        /// <summary>General MIDI percussion 79 ("Open Cuica").</summary>
        OpenCuica = 79,
        /// <summary>General MIDI percussion 80 ("Mute Triangle").</summary>
        MuteTriangle = 80,
        /// <summary>General MIDI percussion 81 ("Open Triangle").</summary>
        OpenTriangle = 81
    };

    /// <summary>
    /// Extension methods for the Percussion enum.
    /// </summary>
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
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        ModulationWheel = 1,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        DataEntryMSB = 6,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        Volume = 7,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        Pan = 10,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        Expression = 11,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        DataEntryLSB = 38,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        SustainPedal = 64,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        ReverbLevel = 91,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        TremoloLevel = 92,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        ChorusLevel = 93,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        CelesteLevel = 94,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        PhaserLevel = 95,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        NonRegisteredParameterLSB = 98,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        NonRegisteredParameterMSB = 99,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        RegisteredParameterNumberLSB = 100,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        RegisteredParameterNumberMSB = 101,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        AllControllersOff = 121,
        /// <summary>General MIDI Control--See MIDI spec for details.</summary>
        AllNotesOff = 123
    }

    /// <summary>
    /// Extension methods for the Control enum.
    /// </summary>
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
