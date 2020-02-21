using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NAudio.Midi;

public class LaunchpadHandler : MonoBehaviour
{
    public static MidiIn midiinput;
    public static int midiinput_kind;
    public static bool midiinput_avail = false;
    public static MidiOut midioutput;
    public static int midioutput_kind;
    public static bool midioutput_avail = false;
    public static bool UP_PLAYER_CustomFirmware = false;


    public static void LoadDevices(GameObject Prefab) {
        Clear();

        for (int i = 0; i < MidiIn.NumberOfDevices; i++) { //MidiIn
            GameObject t = Instantiate(Prefab, new Vector3(439.55f, -45.7f, 0f), Quaternion.identity, GameObject.Find("InGridWithElements").transform);
            t.GetComponentInChildren<Text>().text = MidiIn.DeviceInfo(i).ProductName;
            t.tag = "MidiIn";
            t.GetComponent<ItemLocalHandler>().index = i;
        }

        for (int i = 0; i < MidiOut.NumberOfDevices; i++) { //MidiOut
            GameObject t = Instantiate(Prefab, new Vector3(439.55f, -45.7f, 0f), Quaternion.identity, GameObject.Find("OutGridWithElements").transform);
            t.GetComponentInChildren<Text>().text = MidiOut.DeviceInfo(i).ProductName;
            t.tag = "MidiOut";
            t.GetComponent<ItemLocalHandler>().index = i;
        }
    }

    public static void ConnectTheDevice(int input_index, int output_index, int input_kind, int output_kind) {
        Text statusText = GameObject.Find("StatusText").GetComponent<Text>();

        DisconnectMidi();

        if (GameObject.Find("ToggleIn").GetComponent<Toggle>().isOn == true) {
            midiinput = new MidiIn(input_index);
            midiinput.Start();
            midiinput_avail = true;
            midiinput_kind = input_kind;
            midiinput.MessageReceived += new EventHandler<MidiInMessageEventArgs>(MainProjectLoader.Launchpad_MessageReceived);
            
            statusText.text = "MIDI Status:\n   Input: " + MidiIn.DeviceInfo(input_index).ProductName;
        }
        else statusText.text = "MIDI Status:\n   Input: Not Connected";

        if (GameObject.Find("ToggleOut").GetComponent<Toggle>().isOn == true) {
            midioutput = new MidiOut(output_index);
            midioutput_avail = true;
            midioutput_kind = output_kind;

            statusText.text = statusText.text + "\n   Output: " + MidiOut.DeviceInfo(output_index).ProductName;
        }
        else statusText.text = statusText.text + "\n   Output: Not Connected";

        UP_PLAYER_CustomFirmware = GameObject.Find("ToggleCF").GetComponent<Toggle>().isOn;
    }

    public static void DisconnectMidi() {
        if (midiinput_avail == true) {
            midiinput.MessageReceived -= new EventHandler<MidiInMessageEventArgs>(MainProjectLoader.Launchpad_MessageReceived);
            
            midiinput.Stop();
            midiinput.Reset();
            midiinput.Dispose();

            midiinput_avail = false;
        }

        if (midioutput_avail == true) {
            midioutput.Dispose();
            midioutput_avail = false;

            MainProjectLoader.FlushLED(true);
        }

        GameObject.Find("StatusText").GetComponent<Text>().text = "MIDI Status:\n   Input: Not Connected\n   Output: Not Connected";
    }

    public static void Clear() {
        Transform h = GameObject.Find("InGridWithElements").transform;
        for (int i = 0; i < h.childCount; i++) { //MidiIn
            Destroy(h.GetChild(i).gameObject);
        }

        h = GameObject.Find("OutGridWithElements").transform;
        for (int i = 0; i < h.childCount; i++) { //MidiOut
            Destroy(h.GetChild(i).gameObject);
        }
    }

    public static void PadSendNote(int x, int y, int velocity, Color color, bool skipUI) { //instead velocity
        if (skipUI == false) {
            MainProjectLoader.ctrl["u" + x.ToString() + y.ToString()].colors = ColorLibrary.GetButtonColor(color, MainProjectLoader.ctrl["u" + x.ToString() + y.ToString()].colors);
        }

#region "Send To Launchpad"
        try
        {
            if (midioutput_avail == true) {
                if (midioutput_kind == 0)
                    midioutput.SendBuffer(new byte[] {144, Convert.ToByte((x - 1) * 16 + y - 1), Convert.ToByte(velocity)});
                else if (midioutput_kind == 1)
                    midioutput.SendBuffer(new byte[] {144, Convert.ToByte(10 * (9 - x) + y), Convert.ToByte(velocity)});
                else if (midioutput_kind == 2) {
                    if (UP_PLAYER_CustomFirmware == false)
                        midioutput.SendBuffer(new byte[] {144, Convert.ToByte(10 * (9 - x) + y), Convert.ToByte(velocity)});
                    else {
                        var a = new NAudio.Midi.NoteOnEvent(3000, 16, 10 * (9 - x) + y, Convert.ToByte(velocity), 10);
                        midioutput.Send(a.GetAsShortMessage());
                    }
                }
                else if (midioutput_kind == 3)
                {
                    var NoteOn = new NAudio.Midi.NoteOnEvent(0L, 3, MIDIFighter64_GetXY(x + y.ToString()), Convert.ToByte(velocity), 3000);
                    midioutput.Send(NoteOn.GetAsShortMessage());
                }
                else if (midioutput_kind == 4)
                {
                    midioutput.SendBuffer(new byte[] {144, Convert.ToByte(10 * (9 - x) + y), Convert.ToByte(velocity)});
                }
            }
        }
        catch
        {
            DisconnectMidi();
        }
        #endregion
    }

    public static void mcSendNote(int mc, int velocity, Color color, bool skipUI) {
        if (skipUI == false) {
            MainProjectLoader.ctrl["mc" + mc.ToString()].colors = ColorLibrary.GetButtonColor(color, MainProjectLoader.ctrl["mc" + mc.ToString()].colors);
        }

#region "Send To Launchpad"
        if (midioutput_avail == true)
        {
            try
            {
                if (midioutput_kind == 0)
                {
                    switch (mc)
                    {
                        case 1:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 104, Convert.ToByte(velocity)});
                                break;
                            }

                        case 2:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 105, Convert.ToByte(velocity)});
                                break;
                            }

                        case 3:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 106, Convert.ToByte(velocity)});
                                break;
                            }

                        case 4:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 107, Convert.ToByte(velocity)});
                                break;
                            }

                        case 5:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 108, Convert.ToByte(velocity)});
                                break;
                            }

                        case 6:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 109, Convert.ToByte(velocity)});
                                break;
                            }

                        case 7:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 110, Convert.ToByte(velocity)});
                                break;
                            }

                        case 8:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 111, Convert.ToByte(velocity)});
                                break;
                            }

                        case 9:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 8, Convert.ToByte(velocity)});
                                break;
                            }

                        case 10:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 24, Convert.ToByte(velocity)});
                                break;
                            }

                        case 11:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 40, Convert.ToByte(velocity)});
                                break;
                            }

                        case 12:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 56, Convert.ToByte(velocity)});
                                break;
                            }

                        case 13:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 72, Convert.ToByte(velocity)});
                                break;
                            }

                        case 14:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 88, Convert.ToByte(velocity)});
                                break;
                            }

                        case 15:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 104, Convert.ToByte(velocity)});
                                break;
                            }

                        case 16:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 120, Convert.ToByte(velocity)});
                                break;
                            }
                    }
                }
                else if (midioutput_kind == 1)
                {
                    switch (mc)
                    {
                        case 1:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 104, Convert.ToByte(velocity)});
                                break;
                            }

                        case 2:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 105, Convert.ToByte(velocity)});
                                break;
                            }

                        case 3:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 106, Convert.ToByte(velocity)});
                                break;
                            }

                        case 4:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 107, Convert.ToByte(velocity)});
                                break;
                            }

                        case 5:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 108, Convert.ToByte(velocity)});
                                break;
                            }

                        case 6:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 109, Convert.ToByte(velocity)});
                                break;
                            }

                        case 7:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 110, Convert.ToByte(velocity)});
                                break;
                            }

                        case 8:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 111, Convert.ToByte(velocity)});
                                break;
                            }

                        case 9:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 89, Convert.ToByte(velocity)});
                                break;
                            }

                        case 10:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 79, Convert.ToByte(velocity)});
                                break;
                            }

                        case 11:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 69, Convert.ToByte(velocity)});
                                break;
                            }

                        case 12:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 59, Convert.ToByte(velocity)});
                                break;
                            }

                        case 13:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 49, Convert.ToByte(velocity)});
                                break;
                            }

                        case 14:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 39, Convert.ToByte(velocity)});
                                break;
                            }

                        case 15:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 29, Convert.ToByte(velocity)});
                                break;
                            }

                        case 16:
                            {
                                midioutput.SendBuffer(new byte[] { 144, 19, Convert.ToByte(velocity)});
                                break;
                            }
                    }
                }
                else if (midioutput_kind == 2)
                {
                    switch (mc)
                    {
                        case 0:
                            {
                                midioutput.SendBuffer(new byte[] { 240, 0, 32, 41, 2, 16, 10, 99, Convert.ToByte(velocity), 247 });
                                break;
                            }

                        case 1:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 91, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 91, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 2:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 92, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 92, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 3:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 93, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 93, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 4:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 94, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 94, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 5:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 95, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 95, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 6:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 96, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 96, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 7:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 97, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 97, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 8:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 98, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 98, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        // 오류 발생시 여기를 176 (cc)로 바꿔야함.
                        case 9:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 144, 89, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 89, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 10:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 144, 79, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 79, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 11:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 144, 69, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 69, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 12:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 144, 59, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 59, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 13:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 144, 49, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 49, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 14:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 144, 39, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 39, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 15:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 144, 29, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 29, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 16:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 144, 19, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 19, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 17:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 8, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 8, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 18:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 7, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 7, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 19:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 6, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 6, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 20:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 5, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 5, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 21:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 4, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 4, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 22:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 3, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 3, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 23:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 2, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 2, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 24:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 1, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 1, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 25:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 10, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 10, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 26:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 20, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 20, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 27:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 30, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 30, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 28:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 40, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 40, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 29:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 50, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 50, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 30:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 60, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 60, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 31:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 70, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 70, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 32:
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 176, 80, Convert.ToByte(velocity)});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 80, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }

                        case 33: // 모드 라이트 전용
                            {
                                if (UP_PLAYER_CustomFirmware == false)
                                {
                                    midioutput.SendBuffer(new byte[] { 240, 0, 32, 41, 2, 16, 10, 99, Convert.ToByte(velocity), 247});
                                }
                                else
                                {
                                    var a = new NAudio.Midi.NoteOnEvent(3000, 16, 99, velocity, 10);
                                    midioutput.Send(a.GetAsShortMessage());
                                }

                                break;
                            }
                    }
                }
                else if (midioutput_kind == 4)
                {
                    switch (mc)
                    {
                        case 1:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 91, Convert.ToByte(velocity)});
                                break;
                            }

                        case 2:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 92, Convert.ToByte(velocity)});
                                break;
                            }

                        case 3:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 93, Convert.ToByte(velocity)});
                                break;
                            }

                        case 4:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 94, Convert.ToByte(velocity)});
                                break;
                            }

                        case 5:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 95, Convert.ToByte(velocity)});
                                break;
                            }

                        case 6:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 96, Convert.ToByte(velocity)});
                                break;
                            }

                        case 7:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 97, Convert.ToByte(velocity)});
                                break;
                            }

                        case 8:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 98, Convert.ToByte(velocity)});
                                break;
                            }

                        case 33: // 로고 라이트 전용
                            {
                                midioutput.SendBuffer(new byte[] { 176, 99, Convert.ToByte(velocity)});
                                break;
                            }

                        case 9:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 89, Convert.ToByte(velocity)});
                                break;
                            }

                        case 10:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 79, Convert.ToByte(velocity)});
                                break;
                            }

                        case 11:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 69, Convert.ToByte(velocity)});
                                break;
                            }

                        case 12:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 59, Convert.ToByte(velocity)});
                                break;
                            }

                        case 13:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 49, Convert.ToByte(velocity)});
                                break;
                            }

                        case 14:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 39, Convert.ToByte(velocity)});
                                break;
                            }

                        case 15:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 29, Convert.ToByte(velocity)});
                                break;
                            }

                        case 16:
                            {
                                midioutput.SendBuffer(new byte[] { 176, 19, Convert.ToByte(velocity)});
                                break;
                            }
                    }
                }
            }
            catch
            {
                midioutput_avail = false;
            }
        }
        #endregion
    }
#region "Launchpad Conversions"
    public static System.Drawing.Point MIDIFighter64_GetKey(int pitch)
    {
        switch (pitch)
        {
            case 64:
                {
                    return new System.Drawing.Point(1, 1);
                }

            case 65:
                {
                    return new System.Drawing.Point(1, 2);
                }

            case 66:
                {
                    return new System.Drawing.Point(1, 3);
                }

            case 67:
                {
                    return new System.Drawing.Point(1, 4);
                }

            case 96:
                {
                    return new System.Drawing.Point(1, 5);
                }

            case 97:
                {
                    return new System.Drawing.Point(1, 6);
                }

            case 98:
                {
                    return new System.Drawing.Point(1, 7);
                }

            case 99:
                {
                    return new System.Drawing.Point(1, 8);
                }

            case 60:
                {
                    return new System.Drawing.Point(2, 1);
                }

            case 61:
                {
                    return new System.Drawing.Point(2, 2);
                }

            case 62:
                {
                    return new System.Drawing.Point(2, 3);
                }

            case 63:
                {
                    return new System.Drawing.Point(2, 4);
                }

            case 92:
                {
                    return new System.Drawing.Point(2, 5);
                }

            case 93:
                {
                    return new System.Drawing.Point(2, 6);
                }

            case 94:
                {
                    return new System.Drawing.Point(2, 7);
                }

            case 95:
                {
                    return new System.Drawing.Point(2, 8);
                }

            case 56:
                {
                    return new System.Drawing.Point(3, 1);
                }

            case 57:
                {
                    return new System.Drawing.Point(3, 2);
                }

            case 58:
                {
                    return new System.Drawing.Point(3, 3);
                }

            case 59:
                {
                    return new System.Drawing.Point(3, 4);
                }

            case 88:
                {
                    return new System.Drawing.Point(3, 5);
                }

            case 89:
                {
                    return new System.Drawing.Point(3, 6);
                }

            case 90:
                {
                    return new System.Drawing.Point(3, 7);
                }

            case 91:
                {
                    return new System.Drawing.Point(3, 8);
                }

            case 52:
                {
                    return new System.Drawing.Point(4, 1);
                }

            case 53:
                {
                    return new System.Drawing.Point(4, 2);
                }

            case 54:
                {
                    return new System.Drawing.Point(4, 3);
                }

            case 55:
                {
                    return new System.Drawing.Point(4, 4);
                }

            case 84:
                {
                    return new System.Drawing.Point(4, 5);
                }

            case 85:
                {
                    return new System.Drawing.Point(4, 6);
                }

            case 86:
                {
                    return new System.Drawing.Point(4, 7);
                }

            case 87:
                {
                    return new System.Drawing.Point(4, 8);
                }

            case 48:
                {
                    return new System.Drawing.Point(5, 1);
                }

            case 49:
                {
                    return new System.Drawing.Point(5, 2);
                }

            case 50:
                {
                    return new System.Drawing.Point(5, 3);
                }

            case 51:
                {
                    return new System.Drawing.Point(5, 4);
                }

            case 80:
                {
                    return new System.Drawing.Point(5, 5);
                }

            case 81:
                {
                    return new System.Drawing.Point(5, 6);
                }

            case 82:
                {
                    return new System.Drawing.Point(5, 7);
                }

            case 83:
                {
                    return new System.Drawing.Point(5, 8);
                }

            case 44:
                {
                    return new System.Drawing.Point(6, 1);
                }

            case 45:
                {
                    return new System.Drawing.Point(6, 2);
                }

            case 46:
                {
                    return new System.Drawing.Point(6, 3);
                }

            case 47:
                {
                    return new System.Drawing.Point(6, 4);
                }

            case 76:
                {
                    return new System.Drawing.Point(6, 5);
                }

            case 77:
                {
                    return new System.Drawing.Point(6, 6);
                }

            case 78:
                {
                    return new System.Drawing.Point(6, 7);
                }

            case 79:
                {
                    return new System.Drawing.Point(6, 8);
                }

            case 40:
                {
                    return new System.Drawing.Point(7, 1);
                }

            case 41:
                {
                    return new System.Drawing.Point(7, 2);
                }

            case 42:
                {
                    return new System.Drawing.Point(7, 3);
                }

            case 43:
                {
                    return new System.Drawing.Point(7, 4);
                }

            case 72:
                {
                    return new System.Drawing.Point(7, 5);
                }

            case 73:
                {
                    return new System.Drawing.Point(7, 6);
                }

            case 74:
                {
                    return new System.Drawing.Point(7, 7);
                }

            case 75:
                {
                    return new System.Drawing.Point(7, 8);
                }

            case 36:
                {
                    return new System.Drawing.Point(8, 1);
                }

            case 37:
                {
                    return new System.Drawing.Point(8, 2);
                }

            case 38:
                {
                    return new System.Drawing.Point(8, 3);
                }

            case 39:
                {
                    return new System.Drawing.Point(8, 4);
                }

            case 68:
                {
                    return new System.Drawing.Point(8, 5);
                }

            case 69:
                {
                    return new System.Drawing.Point(8, 6);
                }

            case 70:
                {
                    return new System.Drawing.Point(8, 7);
                }

            case 71:
                {
                    return new System.Drawing.Point(8, 8);
                }
        }

        return new System.Drawing.Point();
    }

    public static int MIDIFighter64_GetXY(string pitch) //pitch: x;y
    {
        switch (pitch)
        {
            case "1;1":
                {
                    return 64;
                }

            case "1;2":
                {
                    return 65;
                }

            case "1;3":
                {
                    return 66;
                }

            case "1;4":
                {
                    return 67;
                }

            case "1;5":
                {
                    return 96;
                }

            case "1;6":
                {
                    return 97;
                }

            case "1;7":
                {
                    return 98;
                }

            case "1;8":
                {
                    return 99;
                }

            case "2;1":
                {
                    return 60;
                }

            case "2;2":
                {
                    return 61;
                }

            case "2;3":
                {
                    return 62;
                }

            case "2;4":
                {
                    return 63;
                }

            case "2;5":
                {
                    return 92;
                }

            case "2;6":
                {
                    return 93;
                }

            case "2;7":
                {
                    return 94;
                }

            case "2;8":
                {
                    return 95;
                }

            case "3;1":
                {
                    return 56;
                }

            case "3;2":
                {
                    return 57;
                }

            case "3;3":
                {
                    return 58;
                }

            case "3;4":
                {
                    return 59;
                }

            case "3;5":
                {
                    return 88;
                }

            case "3;6":
                {
                    return 89;
                }

            case "3;7":
                {
                    return 90;
                }

            case "3;8":
                {
                    return 91;
                }

            case "4;1":
                {
                    return 52;
                }

            case "4;2":
                {
                    return 53;
                }

            case "4;3":
                {
                    return 54;
                }

            case "4;4":
                {
                    return 55;
                }

            case "4;5":
                {
                    return 84;
                }

            case "4;6":
                {
                    return 85;
                }

            case "4;7":
                {
                    return 86;
                }

            case "4;8":
                {
                    return 87;
                }

            case "5;1":
                {
                    return 48;
                }

            case "5;2":
                {
                    return 49;
                }

            case "5;3":
                {
                    return 50;
                }

            case "5;4":
                {
                    return 51;
                }

            case "5;5":
                {
                    return 80;
                }

            case "5;6":
                {
                    return 81;
                }

            case "5;7":
                {
                    return 82;
                }

            case "5;8":
                {
                    return 83;
                }

            case "6;1":
                {
                    return 44;
                }

            case "6;2":
                {
                    return 45;
                }

            case "6;3":
                {
                    return 46;
                }

            case "6;4":
                {
                    return 47;
                }

            case "6;5":
                {
                    return 76;
                }

            case "6;6":
                {
                    return 77;
                }

            case "6;7":
                {
                    return 78;
                }

            case "6;8":
                {
                    return 79;
                }

            case "7;1":
                {
                    return 40;
                }

            case "7;2":
                {
                    return 41;
                }

            case "7;3":
                {
                    return 42;
                }

            case "7;4":
                {
                    return 43;
                }

            case "7;5":
                {
                    return 72;
                }

            case "7;6":
                {
                    return 73;
                }

            case "7;7":
                {
                    return 74;
                }

            case "7;8":
                {
                    return 75;
                }

            case "8;1":
                {
                    return 36;
                }

            case "8;2":
                {
                    return 37;
                }

            case "8;3":
                {
                    return 38;
                }

            case "8;4":
                {
                    return 39;
                }

            case "8;5":
                {
                    return 68;
                }

            case "8;6":
                {
                    return 69;
                }

            case "8;7":
                {
                    return 70;
                }

            case "8;8":
                {
                    return 71;
                }
        }

        return 11;
    }

    public static int LaunchPadS_MC_GetKey(int pitch)
    {
        switch (pitch)
        {
            case 104:
                {
                    return 1;
                }

            case 105:
                {
                    return 2;
                }

            case 106:
                {
                    return 3;
                }

            case 107:
                {
                    return 4;
                }

            case 108:
                {
                    return 5;
                }

            case 109:
                {
                    return 6;
                }

            case 110:
                {
                    return 7;
                }

            case 111:
                {
                    return 8;
                }

            case 8:
                {
                    return 9;
                }

            case 24:
                {
                    return 10;
                }

            case 40:
                {
                    return 11;
                }

            case 56:
                {
                    return 12;
                }

            case 72:
                {
                    return 13;
                }

            case 88:
                {
                    return 14;
                }

            /*
            case 104:
                {
                    return 15;
                }

            Q. MC 15는 어디 갔누?
            A. MC 1과 명령어가 겹쳐서 주석 처리 해놓았습니다. 자세한건 저도 몰라유.
            */

            case 120:
                {
                    return 16;
                }
        }

        return 1;
    }

    public static int LaunchPadMK2_MC_GetKey(int pitch)
    {
        switch (pitch)
        {
            case 104:
                {
                    return 1;
                }

            case 105:
                {
                    return 2;
                }

            case 106:
                {
                    return 3;
                }

            case 107:
                {
                    return 4;
                }

            case 108:
                {
                    return 5;
                }

            case 109:
                {
                    return 6;
                }

            case 110:
                {
                    return 7;
                }

            case 111:
                {
                    return 8;
                }

            case 89:
                {
                    return 9;
                }

            case 79:
                {
                    return 10;
                }

            case 69:
                {
                    return 11;
                }

            case 59:
                {
                    return 12;
                }

            case 49:
                {
                    return 13;
                }

            case 39:
                {
                    return 14;
                }

            case 29:
                {
                    return 15;
                }

            case 19:
                {
                    return 16;
                }
        }

        return 1;
    }

    public static int LaunchPadPro_MC_GetKey(int pitch)
    {
        switch (pitch)
        {
            case 91:
                {
                    return 1;
                }

            case 92:
                {
                    return 2;
                }

            case 93:
                {
                    return 3;
                }

            case 94:
                {
                    return 4;
                }

            case 95:
                {
                    return 5;
                }

            case 96:
                {
                    return 6;
                }

            case 97:
                {
                    return 7;
                }

            case 98:
                {
                    return 8;
                }

            // 체인 영역
            case 89:
                {
                    return 9;
                }

            case 79:
                {
                    return 10;
                }

            case 69:
                {
                    return 11;
                }

            case 59:
                {
                    return 12;
                }

            case 49:
                {
                    return 13;
                }

            case 39:
                {
                    return 14;
                }

            case 29:
                {
                    return 15;
                }

            case 19:
                {
                    return 16;
                }

            case 8:
                {
                    return 17;
                }

            case 7:
                {
                    return 18;
                }

            case 6:
                {
                    return 19;
                }

            case 5:
                {
                    return 20;
                }

            case 4:
                {
                    return 21;
                }

            case 3:
                {
                    return 22;
                }

            case 2:
                {
                    return 23;
                }

            case 1:
                {
                    return 24;
                }

            case 10:
                {
                    return 25;
                }

            case 20:
                {
                    return 26;
                }

            case 30:
                {
                    return 27;
                }

            case 40:
                {
                    return 28;
                }

            case 50:
                {
                    return 29;
                }

            case 60:
                {
                    return 30;
                }

            case 70:
                {
                    return 31;
                }

            case 80:
                {
                    return 32;
                }
        }

        return 1;
    }

    public static int LaunchPadX_MC_GetKey(int pitch)
    {
        switch (pitch)
        {
            case 91:
                {
                    return 1;
                }

            case 92:
                {
                    return 2;
                }

            case 93:
                {
                    return 3;
                }

            case 94:
                {
                    return 4;
                }

            case 95:
                {
                    return 5;
                }

            case 96:
                {
                    return 6;
                }

            case 97:
                {
                    return 7;
                }

            case 98:
                {
                    return 8;
                }

            case 99:
                {
                    return 33; // Novation 아이콘이 있는 CC 버튼
                }

            case 89:
                {
                    return 9;
                }

            case 79:
                {
                    return 10;
                }

            case 69:
                {
                    return 11;
                }

            case 59:
                {
                    return 12;
                }

            case 49:
                {
                    return 13;
                }

            case 39:
                {
                    return 14;
                }

            case 29:
                {
                    return 15;
                }

            case 19:
                {
                    return 16;
                }
        }

        return 1;
    }
#endregion
}