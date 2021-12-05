using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnBorderScript : MonoBehaviour
{
    public Transform topBorder;
    public Transform bottomBorder;
    [Header("This is not required to work.")]
    public Transform respawn;
    private Transform player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if(player.position.y < bottomBorder.position.y)
        {
            if (respawn != null)
            {
                player.position = new Vector3(respawn.position.x, respawn.position.y, player.position.z);
            }
            else
            {
                player.position = new Vector3(player.position.x, topBorder.position.y, player.position.z);
            }
        }
    }
}
