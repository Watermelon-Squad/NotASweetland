using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Define all the classes that the entities are going to have.

[System.Serializable]
public class CharacterMovement
{
    public float acceleration = 10.0f;
    public float decceleration = 10.0f;
    public float max_speed = 10.0f;
    public float jump_speed = 10.0f;
    public float cancel_jump_speed = 10.0f;
}
[System.Serializable]
public class CharacterRotation
{
    public float min_pitch_angle = -45.0f;
    public float max_pitch_angle = 75.0f;
    public float min_rotation_speed = 60.0f;
    public float max_rotation_speed = 100.0f;

    [SerializeField] private bool rotation_from_movement = true;
    [SerializeField] private bool rotation_from_input = false;

    public bool is_rotation_movement { get { return rotation_from_movement; } set { SetRotationFromMovement(value); } }
    public bool is_rotation_input { get { return rotation_from_input; } set { SetRotationFromInput(value); } }

    private void SetRotationFromMovement(bool is_movement)
    {
        rotation_from_movement = is_movement;
        rotation_from_input = !rotation_from_input;
    }

    private void SetRotationFromInput(bool is_input)
    {
        rotation_from_input = is_input;
        rotation_from_movement = !rotation_from_movement;
    }

}
[System.Serializable]
public class CharacterGravity
{
    public float gravity = 10.0f;
    public float grounded_gravity = 4.0f;
    public float max_fall_speed = 20.0f;
}

public class Character : MonoBehaviour
{
    //Classes for entities.
    public CharacterMovement character_movement = null;
    public CharacterRotation character_rotation = null;
    public CharacterGravity character_gravity = null;

    public EntityController controller = null;

    private CharacterController character_controller = null;

    private float character_horizontal_speed = 0.0f;
    private float target_horizontal_speed = 0.0f;

    private Vector3 movement_input = Vector3.zero;
    private Vector3 last_movement_input = Vector3.zero;

    private bool has_movement_input = false;

    private float vertical_speed = 0.0f;
    private bool jump_input = false;

    private Vector2 rotation_angles;

    public bool is_grounded { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {
        controller.EntityInit();
        controller.character = this;

        character_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        controller.EntityUpdate();
    }

    private void FixedUpdate()
    {
        UpdateCharacterState();
        controller.EntityFixedUpdate();
    }

    private void UpdateCharacterState()
    {
        UpdateCharacterSpeed();

        Vector3 new_movement = (character_horizontal_speed * GetDirection() + vertical_speed * Vector3.up) * Time.deltaTime;
        character_controller.Move(new_movement);

        new_movement.y = 0.0f;

        UpdateCharacterRotation(new_movement);

        is_grounded = character_controller.isGrounded;

    }

    private void UpdateCharacterSpeed()
    {
        //Vertical

        if(is_grounded)
        {
            vertical_speed = -character_gravity.grounded_gravity;
            
            if(jump_input)
            {
                vertical_speed = character_movement.jump_speed;
                is_grounded = false;
            }
        }
        else
        {
            if (!jump_input && vertical_speed > 0.0f)
                vertical_speed = Mathf.MoveTowards(vertical_speed, -character_gravity.max_fall_speed, character_movement.cancel_jump_speed * Time.deltaTime);

            vertical_speed = Mathf.MoveTowards(vertical_speed, -character_gravity.max_fall_speed, character_gravity.gravity * Time.deltaTime);
        }

        //Horizontal
        Vector3 movement_input_ = movement_input;

        if (movement_input_.sqrMagnitude > 1.0f)
            movement_input_.Normalize();

        target_horizontal_speed = movement_input_.magnitude * character_movement.max_speed;
        float acceleration = 0.0f;

        if (has_movement_input)
            acceleration = character_movement.acceleration;
        else
            acceleration = character_movement.decceleration;

        character_horizontal_speed = Mathf.MoveTowards(character_horizontal_speed, target_horizontal_speed, acceleration * Time.deltaTime);

    }

    private Vector3 GetDirection()
    {
        Vector3 direction = Vector3.zero;

        if (has_movement_input)
            direction = movement_input;
        else
            direction = last_movement_input;

        if (direction.sqrMagnitude > 1.0f)
            direction.Normalize();

        return direction;
    }

    private void UpdateCharacterRotation(Vector3 movement)
    {
        if(character_rotation.is_rotation_movement && movement.sqrMagnitude > 0.001f)
        {
            float rotation_speed = Mathf.Lerp(character_rotation.max_rotation_speed, character_rotation.min_rotation_speed, character_horizontal_speed / target_horizontal_speed);

            Quaternion target_rotation = Quaternion.LookRotation(movement, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, rotation_speed * Time.deltaTime);
        }
        else if(character_rotation.is_rotation_input)
        {
            Quaternion target_rotation = Quaternion.Euler(0.0f, rotation_angles.y, 0.0f);
            transform.rotation = target_rotation;
        }
    }

    public void SetCharacterRotation(Vector2 rotation)
    {
        float pitch = rotation.x % 360.0f;
        pitch = Mathf.Clamp(pitch, character_rotation.min_pitch_angle, character_rotation.max_pitch_angle);

        float yaw = rotation.y % 360.0f;

        rotation_angles = new Vector2(pitch, yaw);
    }

    public Vector2 GetCharacterRotation()
    {
        return rotation_angles;
    }

    public void SetJump(bool jump)
    {
        jump_input = jump;
    }

    public void SetMovement(Vector3 movement_input)
    {
        bool has_input = movement_input.sqrMagnitude > 0.0f;

        if(has_movement_input && ! has_input)
        {
            last_movement_input = this.movement_input;
        }

        this.movement_input = movement_input;
        has_movement_input = has_input;

    }

}
