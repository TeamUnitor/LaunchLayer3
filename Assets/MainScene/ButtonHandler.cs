using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (transform.name.Substring(0, 2) != "mc") {
        int x = int.Parse(transform.name.Substring(1, 1));
        int y = int.Parse(transform.name.Substring(2, 1));

        MainProjectLoader.Pad_VTouch(MainProjectLoader.UniPack_SelectedChain, x, y, 1);
        }
        else { //mc
            int y = int.Parse(transform.name.Substring(2, transform.name.Length - 2));
            
            MainProjectLoader.Pad_VTouch(MainProjectLoader.UniPack_SelectedChain, -1, y, 1);
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (transform.name.Substring(0, 2) != "mc") {
        int x = int.Parse(transform.name.Substring(1, 1));
        int y = int.Parse(transform.name.Substring(2, 1));

        MainProjectLoader.Pad_VTouch(MainProjectLoader.UniPack_SelectedChain, x, y, 0);
        }
        else { //mc
            int y = int.Parse(transform.name.Substring(2, transform.name.Length - 2));
            
            MainProjectLoader.Pad_VTouch(MainProjectLoader.UniPack_SelectedChain, -1, y, 0);
        }
    }
}
