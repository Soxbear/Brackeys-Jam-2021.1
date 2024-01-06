using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallHider : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Player")
            GetComponent<TilemapRenderer>().enabled = false;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.name == "Player")
            GetComponent<TilemapRenderer>().enabled = true;
    }

}
