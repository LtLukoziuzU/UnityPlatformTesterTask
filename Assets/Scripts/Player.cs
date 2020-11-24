using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private int money, day;
    public GameObject slot12, slot14, sellB, buyB, leaveB, useB;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        inventoryObj = GameObject.FindWithTag("InventoryController");
        mapObj = GameObject.FindWithTag("MapController");
        inventory = inventoryObj.GetComponent<InventoryController>();
        map = mapObj.GetComponent<MapController>();
        money = 500;
        day = 1;
    }

    // Update is called once per frame
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

    public void UseButton() 
    {
        mask = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.1f, mask);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            MapSquare hitSquare = hit.collider.gameObject.GetComponent<MapSquare>();
            if (hitSquare.squareState == "Bed")
                DoSleep();
            else if (hitSquare.squareState == "Shop")
                StartShop();
            else switch (inventory.items[inventory.currentSlot]) 
            {
                case "Hoe":
                    HoeTool(hitSquare);
                    break;
                case "Can":
                    CanTool(hitSquare);
                    break;
                default:
                    break;
            }
        }
    }

    void HoeTool(MapSquare square)
    {
        SpriteRenderer squareSprite = square.gameObject.GetComponent<SpriteRenderer>();
        switch (square.squareState) 
        {
            case "Grass":
                squareSprite.sprite = map.tilledSprite;
                squareSprite.color = new Color(0.785f, 0.785f, 0.785f, 1f);
                square.squareState = "Tilled";
                break;
            case "Plant":

                break;
            default:
                break;
        }

    }

    void CanTool(MapSquare square) 
    {
        SpriteRenderer squareSprite = square.gameObject.GetComponent<SpriteRenderer>();
        switch (square.squareState) 
        {
            case "Tilled":
                squareSprite.sprite = map.tilledSprite;
                squareSprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                square.squareState = "Watered";
                break;
            case "Plant":

                break;
            default:
                break;
        }
    }

    void DoSleep()
    {
        day++;
        dayCounter.text = "Day: " + day.ToString();
        foreach (MapSquare mapSquare in map.mapSquares)
        {
            SpriteRenderer squareSprite = mapSquare.gameObject.GetComponent<SpriteRenderer>();
            switch (mapSquare.squareState) 
            {
                case "Tilled":
                    squareSprite.sprite = map.grassSprite;
                    squareSprite.color = new Color(0.785f, 0.785f, 0.785f, 1f);
                    mapSquare.squareState = "Grass";
                    break;
                case "Watered":
                    squareSprite.color = new Color(0.785f, 0.785f, 0.785f, 1f);
                    mapSquare.squareState = "Tilled";
                    break;
                case "WateredPlant":

                    break;
                default:
                    break;
            }
        }

    }

    void StartShop()
    {
        slot12.SetActive(true);
        slot14.SetActive(true);
        buyB.SetActive(true);
        sellB.SetActive(true);
        leaveB.SetActive(true);
        useB.SetActive(false);
        isShopping = true;
    }

    public void LeaveShop()
    {
        if (inventory.currentSlot > 11)
            inventory.HighlightSelectedButton(inventory.images[0]); 
        slot12.SetActive(false);
        slot14.SetActive(false);
        buyB.SetActive(false);
        sellB.SetActive(false);
        leaveB.SetActive(false);
        useB.SetActive(true);
        isShopping = false;
    }

    public void TryBuying()
    {
        int slot = 0;
        if ((inventory.currentSlot > 11) && (money >= int.Parse(inventory.items[inventory.currentSlot+1])))
        {
            for (int i = 4; i < 11; i += 2)
                if (inventory.items[i] == inventory.items[inventory.currentSlot])
                    slot = i;
            if (slot == 0)
                for (int i = 4; i < 11; i += 2)
                    if (inventory.items[i] == "Empty")
                    {
                        
                        slot = i;
                        i += 10;
                    }
            inventory.images[slot+1].sprite = inventory.images[inventory.currentSlot+1].sprite;
            inventory.items[slot] = inventory.items[inventory.currentSlot];
            int count = int.Parse(inventory.items[slot+1]) + 1;
            inventory.items[slot+1] = count.ToString();
            money -= int.Parse(inventory.items[inventory.currentSlot+1]);
            moneyCounter.text = "Money: " + money.ToString();
        }
    }

    public void TrySelling()
    {
        if ((inventory.currentSlot > 3) && (inventory.currentSlot < 11) && (inventory.items[inventory.currentSlot+1] != "0")) 
        {
            switch (inventory.items[inventory.currentSlot])
            {
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
            moneyCounter.text = "Money: " + money.ToString();
            int count = int.Parse(inventory.items[inventory.currentSlot+1]) - 1; 
            inventory.items[inventory.currentSlot+1] = count.ToString();
            if (inventory.items[inventory.currentSlot+1] == "0")
            {
                inventory.items[inventory.currentSlot] = "Empty";
                inventory.images[inventory.currentSlot+1].sprite = inventory.transparent_sprite;
            }
        }
    }

}
