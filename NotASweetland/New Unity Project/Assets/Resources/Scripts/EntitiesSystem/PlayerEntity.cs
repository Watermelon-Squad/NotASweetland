using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity
{
    public float dead_zone = 0.1f;

    public float jump_force = 10.0f;
    public float gravity_scale = 0.7f;

    private CharacterController character_controller = null;
    private Vector3 movement = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        character_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public override void Movement()
    {
        float storey = movement.y;
        if (character_controller.isGrounded )
        {
            if (Mathf.Abs(Input.GetAxis("LJoystickHorizontal")) >= dead_zone || Mathf.Abs(Input.GetAxis("LJoystickVertical")) >= dead_zone)
                movement = (transform.forward * Input.GetAxis("LJoystickVertical"))+ (transform.right * Input.GetAxis("LJoystickHorizontal"));
            else
                movement = Vector3.zero;

            movement = movement.normalized * movement_speed;
            movement.y = storey;

            if (Input.GetButtonDown("AButton"))
                movement.y = jump_force;
        }
        
         movement.y = movement.y + (Physics.gravity.y * gravity_scale * Time.deltaTime);


        character_controller.Move(movement * Time.deltaTime);

    }
}
