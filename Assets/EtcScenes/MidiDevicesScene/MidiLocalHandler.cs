using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiLocalHandler : MonoBehaviour
{
    public int input_index = 0;
    public int output_index = 0;
    public int input_kind = 2; //0: S / Mini, 1: MK2, 2: Pro, 3: MF54, 4: X / Mini MK3
    public int output_kind = 2; //0: S / Mini, 1: MK2, 2: Pro, 3: MF54, 4: X / Mini MK3
    public GameObject prefab;

    void Start() {
        LaunchpadHandler.LoadDevices(prefab); //same as Load Function
    }

    public void MidiDevices_Load() {
        LaunchpadHandler.LoadDevices(prefab);
    }

    public void MidiDevices_Connect() {
        LaunchpadHandler.ConnectTheDevice(input_index, output_index, input_kind, output_kind);
    }
}
