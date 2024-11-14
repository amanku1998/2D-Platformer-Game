using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float yOffset = 5f;  // Desired Y offset

    void Update()
    {
        //transform.position = player.position + offset;
        //transform.LookAt(player);  // Optional: Makes camera look at player

        // Set the camera position based on the player's position and the Y offset
        transform.position = new Vector3(player.position.x, player.position.y + yOffset, -10f);
    }
}
