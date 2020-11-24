using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public MapSquare[] mapSquares;
    public Sprite grassSprite, tilledSprite;

    // Start is called before the first frame update
    void Start()
    {
        mapSquares = GetComponentsInChildren<MapSquare>();
        for (int i = 0; i < 64; i++) 
        {
            mapSquares[i].squareState = "Grass";
            mapSquares[i].squareID = i;
        }
        mapSquares[0].squareState = "Shop";
        mapSquares[63].squareState = "Bed";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
