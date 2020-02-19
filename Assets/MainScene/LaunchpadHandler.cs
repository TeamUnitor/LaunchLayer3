using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NAudio.Midi;

public class LaunchpadHandler
{
    public static MidiIn midiinput;
    public static int midiinput_kind;
    public static bool midiinput_avail;
    public static MidiOut midioutput;
    public static int midioutput_kind;
    public static bool midioutput_avail;

    public void LoadDevices() {

    }

    public void ConnectTheDevice(int index) {
        
        midiinput.MessageReceived += new EventHandler<MidiInMessageEventArgs>(MainProjectLoader.Launchpad_MessageReceived);
    }
}