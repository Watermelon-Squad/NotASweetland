using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Balancing_character
{

    public Vector3 velocity;
    public float gravity = 20.0f;
    public Vector3 gravitydirection = new Vector3(0, 1, 0);
    public float max_speed;

    Vector3 dampingDirection;
    public float drag;

    public void ApplyGravity()
    {
        velocity -= gravitydirection * gravity * Time.deltaTime;
    }

    public void ApplyDamping() //dampling == resistance or lose of force
    {
        dampingDirection = -velocity;
        dampingDirection *= drag;
        velocity += dampingDirection;
    }

    public void CapBalancingSpeed()
    {
        velocity = Vector3.ClampMagnitude(velocity, max_speed);
    }
}
