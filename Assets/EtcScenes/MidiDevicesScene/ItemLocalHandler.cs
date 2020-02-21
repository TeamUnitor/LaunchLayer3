using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemLocalHandler : MonoBehaviour, IPointerClickHandler //Midi Devices Handler
{
    public int index = 0;

    public void OnPointerClick(PointerEventData eventData) {
        MidiLocalHandler h = GameObject.Find("MidiHandler").GetComponent<MidiLocalHandler>();
        if (gameObject.tag.Substring(0, 6) == "MidiIn")
            h.input_index = index;
        else 
            h.output_index = index;
    }
}
