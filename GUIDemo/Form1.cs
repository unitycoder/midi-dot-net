using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Midi;

namespace GUIDemo
{
    public partial class Form1 : Form
    {
        // Constructor for the window.
        public Form1()
        {
            InitializeComponent();
        }

        // Called when the window loads.
        private void Form1_Load(object sender, EventArgs e)
        {
            // Set up a clock for scheduling output events.
            clock = new Clock(140);

            // Create persistent delegates which we can add and remove to events.
            noteOnHandler = new InputDevice.NoteOnHandler(this.NoteOn);
            noteOffHandler = new InputDevice.NoteOffHandler(this.NoteOff);

            // Populate the list box with input devices, preferring the one from the previous
            // session if any.
            InputDevice preferredInputDevice =
                    InputDevice.DeviceWithSpec(Properties.Settings.Default.MidiInputDeviceSpec);
            foreach (InputDevice device in InputDevice.InstalledDevices)
            {
                int idx = inputListBox.Items.Add(device.Spec);
                if (device == preferredInputDevice)
                {
                    UseInputDevice(device);
                    inputListBox.SelectedIndex = idx;
                }
            }

            // Populate the list box with output devices, preferring the one from the previous
            // session if any.
            OutputDevice preferredOutputDevice =
                OutputDevice.DeviceWithSpec(Properties.Settings.Default.MidiOutputDeviceSpec);
            foreach (OutputDevice device in OutputDevice.InstalledDevices)
            {
                int idx = outputListBox.Items.Add(device.Spec);
                if (device == preferredOutputDevice)
                {
                    UseOutputDevice(device);
                    outputListBox.SelectedIndex = idx;
                }
            }
        }

        // Called when the window is closed.
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Stop the clock if necessary, which joins its thread.
            if (clock.IsRunning) {
                clock.Stop();
            }
            // Close the output device if we have one.
            if (outputDevice != null && outputDevice.IsOpen)
            {
                Properties.Settings.Default.MidiOutputDeviceSpec = outputDevice.Spec;
                outputDevice.Close();
            }
            // Stop and close the input device if we have one.
            if (inputDevice != null && inputDevice.IsOpen)
            {
                Properties.Settings.Default.MidiInputDeviceSpec = inputDevice.Spec;
                if (inputDevice.IsReceiving)
                {
                    inputDevice.StopReceiving();
                }
                inputDevice.Close();
            }
            // Save properties so we get the same devices next time.
            Properties.Settings.Default.Save();
        }

        // Called when the input list box changes selection.
        private void inputListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ignore out-of-range indices.
            if (inputListBox.SelectedIndex < 0 ||
                inputListBox.SelectedIndex >= InputDevice.InstalledDevices.Count)
            {
                return;
            }
            UseInputDevice(InputDevice.InstalledDevices[inputListBox.SelectedIndex]);
        }

        // Called when the output list box changes selection.
        private void outputListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ignore out-of-range indices.
            if (outputListBox.SelectedIndex < 0 ||
                outputListBox.SelectedIndex >= OutputDevice.InstalledDevices.Count)
            {
                return;
            }
            UseOutputDevice(OutputDevice.InstalledDevices[outputListBox.SelectedIndex]);
        }

        // Called when the play button is clicked.  Schedules a series of note events on the
        // output device if there is one.
        private void playButton_Click(object sender, EventArgs e)
        {
            if (outputDevice != null && outputDevice.IsOpen)
            {
                clock.Schedule(new NoteOnOffMessage(outputDevice, Channel.Channel1, Pitch.C4, 80, clock.Time, clock, 1));
                clock.Schedule(new NoteOnOffMessage(outputDevice, Channel.Channel1, Pitch.E4, 80, clock.Time + 1, clock, 1));
                clock.Schedule(new NoteOnOffMessage(outputDevice, Channel.Channel1, Pitch.G4, 80, clock.Time + 2, clock, 1));
            }
        }

        // Switches input devices, turning off the old one (if any) and turning on the new one.
        private void UseInputDevice(InputDevice newInputDevice)
        {
            if (newInputDevice == inputDevice) {
                return;
            }
            if (inputDevice != null)
            {
                if (inputDevice.IsOpen)
                {
                    if (inputDevice.IsReceiving)
                    {
                        inputDevice.StopReceiving();
                    }
                    inputDevice.Close();
                }
                inputDevice.NoteOn -= noteOnHandler;
                inputDevice.NoteOff -= noteOffHandler;
            }
            inputDevice = newInputDevice;
            inputDevice.NoteOn += noteOnHandler;
            inputDevice.NoteOff += noteOffHandler;
            inputDevice.Open();
            inputDevice.StartReceiving(null);
        }

        // Switches output devices, turning off the old one (if any) and turning on the new one.
        private void UseOutputDevice(OutputDevice newOutputDevice)
        {
            if (newOutputDevice == outputDevice)
            {
                return;
            }
            if (clock.IsRunning)
            {
                clock.Stop();
                clock.Reset();
            }
            if (outputDevice != null)
            {
                if (outputDevice.IsOpen)
                {
                    outputDevice.Close();
                }
            }
            outputDevice = newOutputDevice;
            outputDevice.Open();
            clock.Start();
        }

        // Method called when the input device receives a NoteOn message.  Updates
        // the input status label.  Respects GUI thread affinity by invoking to the
        // GUI thread if necessary.
        public void NoteOn(NoteOnMessage msg)
        {
            if (InvokeRequired) {
                BeginInvoke(noteOnHandler, msg);
                return;
            }
            inputStatusLabel.Text = String.Format("Note On {0}", msg.Pitch);
        }

        // Method called when the input device receives a NoteOff message.  Updates
        // the input status label.  Respects GUI thread affinity by invoking to the
        // GUI thread if necessary.
        public void NoteOff(NoteOffMessage msg)
        {
            if (InvokeRequired)
            {
                BeginInvoke(noteOffHandler, msg);
                return;
            }
            inputStatusLabel.Text = String.Format("Note Off {0}", msg.Pitch);
        }

        // Persistent delegate objects for the note handlers.
        private InputDevice.NoteOnHandler noteOnHandler;        
        private InputDevice.NoteOffHandler noteOffHandler;

        // Clock, input device, output device.
        private Clock clock = null;
        private InputDevice inputDevice = null;
        private OutputDevice outputDevice = null;
    }
}
