using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    private Rigidbody2D rb2d;
    private float moveHor, moveVert;
    private Touch touch;
    private bool isTouched = false;
    private Vector2 touchDelta = new Vector2(0f, 0f);
    public InventoryController inventory;
    public MapController map;
    private GameObject inventoryObj, mapObj;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        inventoryObj = GameObject.FindWithTag("InventoryController");
        mapObj = GameObject.FindWithTag("MapController");
        inventory = inventoryObj.GetComponent<InventoryController>();
        map = mapObj.GetComponent<MapController>();
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
        Vector2 movement;
        if (isTouched) 
        {
            movement = touchDelta;
        } else {
            movement = new Vector2 (moveHor, moveVert);
        }
        rb2d.AddForce (movement * speed);
    }

    public void TryDigging() 
    {

    }

}
