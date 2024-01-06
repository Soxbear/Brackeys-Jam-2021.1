using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    Vector3 offset = new Vector3(0, 0, -100);
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
