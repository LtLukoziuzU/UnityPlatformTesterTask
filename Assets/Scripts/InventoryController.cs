using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//used to initialize inventory and shop at the start of the game
//also keeps updating the item counter and slot highlighting depending on slot selected
//and has item sprites for game controller to use (the latter set in Inspector)
public class InventoryController : MonoBehaviour
{

    public Sprite highlight_sprite, nonHighlight_sprite, transparent_sprite;
    public int currentSlot;
    public Image[] images;
    public string[] items;
    public Text counter;
    public Sprite parsnipSprite, potatoSprite;

    //initializes inventory and shop
    //then hides the shop slots
    void Start() 
    {
        items = new string[16];
        currentSlot = 0;
        images = GetComponentsInChildren<Image>();
        images[0].sprite = highlight_sprite;        //each inventory slot is represented as two separate objects. 
        items[0] = "Hoe";                           //First one holds the sprite for slot itself (highlit or not), as well as info for the item
        items[1] = "1";                             //Second one holds the sprite for the item, as well as the count how many items it has
        items[2] = "Can";                           
        items[3] = "1";                             //First two slots are the tools
        for (int i = 4; i < 11; i += 2) 
        {
            items[i] = "Empty";                     //Then 4 empty slots
            items[i+1] = "0";
        }
        items[12] = "Seed_Parsnip";                 //And then the two shop slots, using the same controller/prefab
        items[13] = "30";
        items[14] = "Seed_Potato";
        items[15] = "15";
        images[12].gameObject.SetActive(false);
        images[14].gameObject.SetActive(false);
    }

    //Keeps the UI item counter updated depending on what slot is selected
    void Update() 
    {
        if (currentSlot <= 11) 
            counter.text = "Count: " + items[currentSlot+1];    
        else 
            counter.text = "Shop Item";
    }

    //Highlights the selected button and stores the slot ID, unhighlighting whatever slot was highlit before 
    public void HighlightSelectedButton(Image button) 
    {
        images[currentSlot].sprite = nonHighlight_sprite;
        currentSlot = int.Parse(button.gameObject.name.Substring(4));
        button.sprite = highlight_sprite;
    }

}
