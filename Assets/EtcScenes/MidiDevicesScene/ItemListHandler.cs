using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemListHandler : MonoBehaviour, IPointerClickHandler //Launchpad List Handler
{
    public int index = 0;

    void Start() {
        index = int.Parse(transform.name.Substring(4, 1)) - 1;
    }

    public void OnPointerClick(PointerEventData eventData) {
        MidiLocalHandler h = GameObject.Find("MidiHandler").GetComponent<MidiLocalHandler>();
        if (gameObject.tag.Substring(0, 6) == "MidiIn")
            h.input_kind = index;
        else 
            h.output_kind = index;
    }
}
