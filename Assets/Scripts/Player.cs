using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//initially was going to be used just for Player controls, ended up being main GameController too
public class Player : MonoBehaviour
{

    public float speed;
    private Rigidbody2D rb2d;
    private float moveHor, moveVert;
    private Touch touch;
    private bool isTouched = false;
    private bool isShopping = false;
    private Vector2 touchDelta = new Vector2(0f, 0f);
    public InventoryController inventory;
    public MapController map;
    private GameObject inventoryObj, mapObj;
    private LayerMask mask;
    public Text moneyCounter, dayCounter;
    private int money, day, parsnipStages, potatoStages;
    public GameObject slot12, slot14, sellB, buyB, leaveB, useB;


    //Instantiate some default values, find components for movement (Rigidbody) and controlling Inventory and Map (their controllers)
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        inventoryObj = GameObject.FindWithTag("InventoryController");
        mapObj = GameObject.FindWithTag("MapController");
        inventory = inventoryObj.GetComponent<InventoryController>();
        map = mapObj.GetComponent<MapController>();
        money = 500;
        day = 1;
        parsnipStages = 4;
        potatoStages = 2;
    }

    //Get Movement inputs
    void Update()
    {
        if (Input.touchCount > 0) 
        {
            isTouched = true;
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
                touchDelta = new Vector2(Mathf.Clamp(touch.deltaPosition.x, -1f, 1f), Mathf.Clamp(touch.deltaPosition.y, -1f, 1f));
            else touchDelta = new Vector2(0f, 0f);
        } else {
            isTouched = false;
            moveHor = Input.GetAxis ("Horizontal");
            moveVert = Input.GetAxis ("Vertical");
        }
    }

    //Actually move, assuming player is not in shop at the movement
    void FixedUpdate() 
    {
        if (!isShopping)
        {
            Vector2 movement;
            if (isTouched) 
            {
                movement = touchDelta;
            } else {
                movement = new Vector2 (moveHor, moveVert);
            }
            rb2d.AddForce (movement * speed);
        }
    }

    //The multi-tool of this game, use button controls
    public void UseButton() 
    {
        mask = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.1f, mask);                  //Raycast the square you're at
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            MapSquare hitSquare = hit.collider.gameObject.GetComponent<MapSquare>();
            if (hitSquare.squareState == "Bed")                                                             //Decide what to do depending on square/tool
                DoSleep();                                                                                  //Priority for squares (shop, bed) over tools/seeds, 
            else if (hitSquare.squareState == "Shop")                                                       //as you cannot use items on those squares
                StartShop();
            else switch (inventory.items[inventory.currentSlot]) 
            {
                case "Hoe":
                    HoeTool(hitSquare);
                    break;
                case "Can":
                    CanTool(hitSquare);
                    break;
                case "Seed_Parsnip":
                    if ((hitSquare.squareState == "Tilled") || (hitSquare.squareState == "Watered"))
                        PlantSeed("Seed_Parsnip", hitSquare);
                    break;
                case "Seed_Potato":
                    if ((hitSquare.squareState == "Tilled") || (hitSquare.squareState == "Watered"))
                        PlantSeed("Seed_Potato", hitSquare);
                    break;
                default:
                    break;
            }
        }
    }

    //Hoe actions to either till the ground or harvest the plants
    void HoeTool(MapSquare square)
    {
        SpriteRenderer squareSprite = square.gameObject.GetComponent<SpriteRenderer>();
        switch (square.squareState) 
        {
            case "Grass":                                                                   //if it's grass, till it
                squareSprite.sprite = map.tilledSprite;
                square.squareState = "Tilled";
                break;
            case "Plant":                                                                   //if there's a (watered) plant on square, check if it is in final stage
                if (square.plantStage == square.totalStages)                                //if yes, harvest it
                    HarvestPlant(square);
                break;
            case "WateredPlant":
                if (square.plantStage == square.totalStages)
                    HarvestPlant(square);
                break;
            default:
                break;
        }

    }

    //Watering can actions to water the tilled ground/plants
    void CanTool(MapSquare square) 
    {
        SpriteRenderer squareSprite = square.gameObject.GetComponent<SpriteRenderer>();
        switch (square.squareState) 
        {
            case "Tilled":
                squareSprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);                       //watering always uses same sprite, just darkens it
                square.squareState = "Watered";
                break;
            case "Plant":
                squareSprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                square.squareState = "WateredPlant";
                break;
            default:
                break;
        }
    }

    //Processes which oversee changes that happen to map squares with the day change
    void DoSleep()
    {
        day++;                                                                                          //advance the day
        dayCounter.text = "Day: " + day.ToString();
        foreach (MapSquare mapSquare in map.mapSquares)
        {
            SpriteRenderer squareSprite = mapSquare.gameObject.GetComponent<SpriteRenderer>();          
            switch (mapSquare.squareState) 
            {
                case "Tilled":                                                                          //if it's just empty tilled ground, revert to grass
                    squareSprite.sprite = map.grassSprite;
                    mapSquare.squareState = "Grass";
                    break;
                case "Watered":                                                                         //if it's watered tilled ground, make it dry but keep it tilled 
                    squareSprite.color = new Color(0.785f, 0.785f, 0.785f, 1f);                         
                    mapSquare.squareState = "Tilled";
                    break;
                case "WateredPlant":                                                                    //and if it's a watered plant, do two things:          
                    if (mapSquare.plantStage < mapSquare.totalStages)                                   //1) Grow it (if not in harvestable)
                        GrowStage(mapSquare, squareSprite);                                             //2) Dry it
                    squareSprite.color = new Color(0.785f, 0.785f, 0.785f, 1f);
                    mapSquare.squareState = "Plant";
                    break;
                default:
                    break;
            }
        }

    }

    //Enables shop interface
    void StartShop()
    {
        slot12.SetActive(true);
        slot14.SetActive(true);
        buyB.SetActive(true);
        sellB.SetActive(true);
        leaveB.SetActive(true);
        useB.SetActive(false);
        isShopping = true;          //stops player from moving
    }

    //Disables shop interface
    public void LeaveShop()
    {
        if (inventory.currentSlot > 11)                                     //if player currently has a shop item selected
            inventory.HighlightSelectedButton(inventory.images[0]);         //reset selection to first slot
        slot12.SetActive(false);
        slot14.SetActive(false);
        buyB.SetActive(false);
        sellB.SetActive(false);
        leaveB.SetActive(false);
        useB.SetActive(true);
        isShopping = false;         //allow player to move
    }

    //Try buying an item if there's enough money (and technically also checks if there's a fitting slot, but in current prototype that's always true) 
    public void TryBuying()
    {
        int slot = 0;
        if ((inventory.currentSlot > 11) && (money >= int.Parse(inventory.items[inventory.currentSlot+1])))         //first, make sure we're on a shop item and actually have money for that item
        {                                                                                                           
            for (int i = 4; i < 11; i += 2)                                                                         //then look in inventory if the item player is buying exists in inventory
                if (inventory.items[i] == inventory.items[inventory.currentSlot])                                   //if yes, set the slot to it
                    slot = i;
            if (slot == 0)  
            {
                for (int i = 4; i < 11; i += 2)                                                                     //if not, look for earliest empty slot in inventory
                    if (inventory.items[i] == "Empty")
                    {
                        slot = i;
                        i += 10;
                    }
                inventory.images[slot+1].sprite = inventory.images[inventory.currentSlot+1].sprite;                 //if empty slot is used to buy the item, add the sprite to slot
                inventory.items[slot] = inventory.items[inventory.currentSlot];                                     //and what item you're adding
            }
            
            int count = int.Parse(inventory.items[slot+1]) + 1;                                                     //and finally, actually increase the count and remove the cash
            inventory.items[slot+1] = count.ToString();                                                             //item counter is updated continuously
            money -= int.Parse(inventory.items[inventory.currentSlot+1]);                                           //meanwhile, money counter is only updated when actual change happens
            moneyCounter.text = "Money: " + money.ToString();
        }
    }

    //Try selling an item, assuming player has selected a valid inventory slot (first two slots are reserved for hoe/can) and it's not empty
    public void TrySelling()
    {
        if ((inventory.currentSlot > 3) && (inventory.currentSlot < 11) && (inventory.items[inventory.currentSlot+1] != "0")) 
        {
            switch (inventory.items[inventory.currentSlot])            
            {                                                       //add the money depending on what you sell
                case "Seed_Parsnip":
                    money += 30;
                    break;
                case "Parsnip":
                    money += 60;
                    break;
                case "Seed_Potato":
                    money += 15;
                    break;
                case "Potato":
                    money += 30;
                    break;
                default:
                    break;
            }
            moneyCounter.text = "Money: " + money.ToString();       //money counter is only updated when actual change happens
            ReduceItemCount();                                      //reduce the amount of item (and possibly make slot empty if last piece was sold)
        }
    }

    //Called by DoSleep, actually grows the plant (by increasing stage and changing the sprite to next stage)
    public void GrowStage(MapSquare mapSquare, SpriteRenderer squareSprite) 
    {
        mapSquare.plantStage++;
        switch (mapSquare.plantType)
        {
            case "Parsnip":
                squareSprite.sprite = map.parsnipSprites[mapSquare.plantStage];
                break;
            case "Potato":
                squareSprite.sprite = map.potatoSprites[mapSquare.plantStage];
                break;
            default:
                break;
        }
    }

    //Called by HoeTool, does the actual planting (by setting up the relevant MapSquare with info of the plant and its first stage sprite)
    public void PlantSeed(string seedType, MapSquare square) 
    {
        SpriteRenderer squareSprite = square.gameObject.GetComponent<SpriteRenderer>();
        switch (seedType) 
        {
            case "Seed_Parsnip":
                square.totalStages = parsnipStages;
                squareSprite.sprite = map.parsnipSprites[0];
                square.plantType = "Parsnip";
                break;
            case "Seed_Potato":
                square.totalStages = potatoStages;
                squareSprite.sprite = map.potatoSprites[0];
                square.plantType = "Potato";
                break;
        }
        if (square.squareState == "Tilled")                         //retain the correct watering data when changing the state to act like a plant
            square.squareState = "Plant";
        else
            square.squareState = "WateredPlant";
        ReduceItemCount();                                          //remove the seed from inventory (possibly making the slot empty)
    }

    //As removing an item from inventory doesn't matter on what it actually holds, it was written off as its own function
    //Called by both PlantSeed and TrySelling
    public void ReduceItemCount()
    {
        int count = int.Parse(inventory.items[inventory.currentSlot+1]) - 1; 
        inventory.items[inventory.currentSlot+1] = count.ToString();
        if (inventory.items[inventory.currentSlot+1] == "0")
        {                                                                               //if this was the last piece of item, reset slot to Empty
            inventory.items[inventory.currentSlot] = "Empty";
            inventory.images[inventory.currentSlot+1].sprite = inventory.transparent_sprite;
        }
    }

    //Called by HoeTool, harvests the plant and puts it into inventory, resetting the MapSquare to tilled ground 
    public void HarvestPlant(MapSquare square) 
    {
        int slot = 0;
        for (int i = 4; i < 11; i += 2)                     //same logic as in TryBuying, look for either same item in inventory, or (if not found) an empty slot
            if (inventory.items[i] == square.plantType)
                slot = i;
        if (slot == 0)
        {
            for (int i = 4; i < 11; i += 2)
                if (inventory.items[i] == "Empty")
                {
                    slot = i;
                    i += 10;
                }
            switch (square.plantType)
            {                                               //if it's an empty slot, add the correct sprite
                case "Parsnip":
                    inventory.images[slot+1].sprite = inventory.parsnipSprite;
                    break;
                case "Potato":
                    inventory.images[slot+1].sprite = inventory.potatoSprite;
                    break;
                default:
                    break;
            }
            inventory.items[slot] = square.plantType;       //and also set the slot
        }
        int count = int.Parse(inventory.items[slot+1]) + 1; //increase the count of the harvested item in inventory
        inventory.items[slot+1] = count.ToString();
        square.plantType = "";                              //here starts MapSquare resetting
        square.plantStage = 0;
        if (square.squareState == "Plant")                  //keep the watering data correct
            square.squareState = "Tilled";
        else
            square.squareState = "Watered";
        SpriteRenderer squareSprite = square.gameObject.GetComponent<SpriteRenderer>();
        squareSprite.sprite = map.tilledSprite;             //finally, reset the sprite
    }

}
