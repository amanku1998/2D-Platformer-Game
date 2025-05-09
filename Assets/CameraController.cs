using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float yOffset = 3f;  // Desired Y offset

    public float smoothSpeed = 0.125f;  // Adjust this value to control the smoothness
    public float minCameraY = 0f;        // Minimum Y position for the camera

    void Start()
    {
        // Set the initial camera position to the player's position with the offset
        float direction = Mathf.Sign(player.localScale.x);
        transform.position = new Vector3(player.position.x + 4f * direction, player.position.y + yOffset, offset.z);

        // Set the minimum Y position based on the starting Y position
        minCameraY = -3.5f;
    }

    void Update()
    {
        //transform.position = player.position + offset;
        //transform.LookAt(player);  // Optional: Makes camera look at player

        // Determine the direction of the player based on their localScale.x
        float direction = Mathf.Sign(player.localScale.x);  // 1 if facing right, -1 if facing left

        //// Set the camera position based on the player's position and the Y offset
        //transform.position = new Vector3(player.position.x + 5f * direction, player.position.y + yOffset, -10f);

        // Calculate the target position based on the player's position and offset
        Vector3 targetPosition = new Vector3(player.position.x + 4f * direction, player.position.y + yOffset, offset.z);

        // Clamp the camera's Y position to not go below the minimum
        targetPosition.y = Mathf.Max(targetPosition.y, minCameraY);

        // Smoothly interpolate the camera's position to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

    }
}
