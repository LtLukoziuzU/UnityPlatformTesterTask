using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used to initialize the map squares at the start of game, 
//as well as holds possible map sprites for game controller to use (pre-set in Inspector)
public class MapController : MonoBehaviour
{

    public MapSquare[] mapSquares;
    public Sprite grassSprite, tilledSprite;
    public Sprite[] parsnipSprites, potatoSprites;

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

}
