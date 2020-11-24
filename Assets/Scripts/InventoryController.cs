using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{

    public Sprite highlight_sprite, nonHighlight_sprite, transparent_sprite;
    public int currentSlot;
    public Image[] images;
    public string[] items;
    public Text counter;

    void Start() 
    {
        items = new string[16];
        currentSlot = 0;
        images = GetComponentsInChildren<Image>();
        images[0].sprite = highlight_sprite;
        items[0] = "Hoe";
        items[1] = "1";
        items[2] = "Can";
        items[3] = "1";
        for (int i = 4; i < 11; i += 2) 
        {
            items[i] = "Empty";
            items[i+1] = "0";
        }
        items[12] = "Seed_Parsnip";
        items[13] = "30";
        items[14] = "Seed_Potato";
        items[15] = "15";
        images[12].gameObject.SetActive(false);
        images[14].gameObject.SetActive(false);
    }

    void Update() 
    {
        if (currentSlot <= 11) 
            counter.text = "Count: " + items[currentSlot+1];    
        else 
            counter.text = "Shop Item";
    }

    public void HighlightSelectedButton(Image button) 
    {
        images[currentSlot].sprite = nonHighlight_sprite;
        currentSlot = int.Parse(button.gameObject.name.Substring(4));
        button.sprite = highlight_sprite;
    }

}
