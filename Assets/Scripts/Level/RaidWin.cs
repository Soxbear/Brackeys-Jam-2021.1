using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidWin : MonoBehaviour
{

    public GameManager GameManager;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("END");
        if (collision.gameObject.tag == "BasicEnemy")
            GameManager.EndGame();
    }
}
