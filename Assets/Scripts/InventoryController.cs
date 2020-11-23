using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{

    public Sprite highlight_sprite, nonHighlight_sprite;
    public int currentSlot;
    private Image[] images;
    public string[] items;

    void Start() 
    {
        items = new string[12];
        currentSlot = 0;
        images = GetComponentsInChildren<Image>();
        images[0].sprite = highlight_sprite;
        items[0] = "Hoe";
        items[1] = "1";
        items[2] = "Can";
        items[3] = "1";
        for (int i = 4; i < 11; i = i+2) 
        {
            items[i] = "Empty";
            items[i+1] = "0";
        }
    }

    public void HighlightSelectedButton(Image button) 
    {
        images[currentSlot].sprite = nonHighlight_sprite;
        currentSlot = int.Parse(button.gameObject.name.Substring(4));
        button.sprite = highlight_sprite;
    }

}
