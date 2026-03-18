using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyBehaviour : MonoBehaviour
{
    // MOVING IN A CIRCLE

    private float radius = 0.3f;
    private float speed = 0.4f;
    // The point we are going around in circles
    private Vector2 basestartpoint;
    // Destination of our current move
    private Vector2 destination;
    // Start of our current move
    private Vector2 start;
    // Current move's progress
    private float progress = 0.0f;

    // ROTATING TO TARGET

    private float rotOffset = 90f;
    private float RotationSpeed = 3f;

    // Use this for initialization
    void Start()
    {
        start = transform.localPosition;
        basestartpoint = transform.localPosition;
        progress = 0.0f;

        PickNewRandomDestination();
    }

    // Update is called once per frame
    void Update()
    {
        bool reached = false;

        // Update our progress to our destination
        progress += speed * Time.deltaTime;

        // Check for the case when we overshoot or reach our destination
        if (progress >= 1.0f)
        {
            progress = 1.0f;
            reached = true;
        }

        // Update out position based on our start postion, destination and progress.
        transform.localPosition = (destination * progress) + start * (1 - progress);

        // If we have reached the destination, set it as the new start and pick a new random point. Reset the progress
        if (reached)
        {
            start = destination;
            PickNewRandomDestination();
            progress = 0.0f;
        }

        RotateGameObject(destination, RotationSpeed, rotOffset);
    }

    void PickNewRandomDestination()
    {
        // We add basestartpoint to the mix so that is doesn't go around a circle in the middle of the scene.
        destination = Random.insideUnitCircle * radius + basestartpoint;
    }

    private void RotateGameObject(Vector2 destination, float RotationSpeed, float offset)
    {
        Vector3 target = destination;
        //get the direction of the other object from current object
        Vector3 dir = target - transform.position;
        //get the angle from current direction facing to desired target
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //set the angle into a quaternion + sprite offset depending on initial sprite facing direction
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        //Roatate current game object to face the target using a slerp function which adds some smoothing to the move
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
    }

}