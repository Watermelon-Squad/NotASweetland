using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pendulum
{

    public Transform character_tr;
    public Tether tether;
    public Arm arm;
    public Balancing_character balancing_character;

    private Vector3 previous_pos;

    public void Initialise()
    {
        character_tr.transform.parent = tether.tether_transform;
        arm.length = Vector3.Distance(character_tr.transform.localPosition, tether.position);
    }

    public Vector3 MoveCharacter(Vector3 pos, Vector3 prev_pos, float dt)
    {
        balancing_character.velocity += GetConstraintVelocity(pos, prev_pos, dt);
        balancing_character.ApplyGravity();
        balancing_character.ApplyDamping();
        balancing_character.CapBalancingSpeed();

        pos += balancing_character.velocity * dt;

        if(Vector3.Distance(pos, tether.position) < arm.length)
        {
            pos = Vector3.Normalize(pos - tether.position) * arm.length;
            arm.length = (Vector3.Distance(pos, tether.position));
            return pos;
        }

        previous_pos = pos;

        return pos;
    }

    public Vector3 MoveCharacter(Vector3 pos, float dt)
    {
        balancing_character.velocity += GetConstraintVelocity(pos, previous_pos, dt);
        balancing_character.ApplyGravity();
        balancing_character.ApplyDamping();
        balancing_character.CapBalancingSpeed();

        pos += balancing_character.velocity * dt;

        if (Vector3.Distance(pos, tether.position) < arm.length)
        {
            pos = Vector3.Normalize(pos - tether.position) * arm.length;
            arm.length = (Vector3.Distance(pos, tether.position));
            return pos;
        }

        previous_pos = pos;

        return pos;
    }

    public Vector3 GetConstraintVelocity(Vector3 current_pos, Vector3 previous_pos, float dt)
    {
        float distance_to_tether;
        Vector3 constraint_pos;
        Vector3 predicted_pos;

        distance_to_tether = Vector3.Distance(current_pos, tether.position);
        if (distance_to_tether > arm.length)
        {
            constraint_pos = Vector3.Normalize(current_pos - tether.position) * arm.length;
            predicted_pos = (constraint_pos - previous_pos) / dt;
            return predicted_pos;
        }
            return Vector3.zero; 
    }

    public void SwitchTether(Vector3 new_pos)
    {
        character_tr.transform.parent = null;
        tether.tether_transform.position = new_pos;
        character_tr.transform.parent = tether.tether_transform;
        tether.position = tether.tether_transform.InverseTransformPoint(new_pos);
        arm.length = Vector3.Distance(character_tr.transform.localPosition, tether.position);
    }

    public Vector3 Fall(Vector3 pos, float dt)
    {
        balancing_character.ApplyGravity();
        balancing_character.ApplyDamping();
        balancing_character.CapBalancingSpeed();

        pos += balancing_character.velocity * dt;
        return pos;
    }

}
