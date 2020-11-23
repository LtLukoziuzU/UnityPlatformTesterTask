using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public MapSquare[] mapSquares;

    // Start is called before the first frame update
    void Start()
    {
        mapSquares = GetComponentsInChildren<MapSquare>();
        for (int i = 0; i < 64; i++) 
        {
            mapSquares[i].squareState = "Grass";
            mapSquares[i].squareID = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
