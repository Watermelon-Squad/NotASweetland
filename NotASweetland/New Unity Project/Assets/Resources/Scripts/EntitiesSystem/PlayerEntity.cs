﻿using System.Collections;
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
            movement = Vector3.zero;

            if (Mathf.Abs(Input.GetAxis("LJoystickHorizontal")) >= dead_zone || Mathf.Abs(Input.GetAxis("LJoystickVertical")) >= dead_zone)
            {
                /* float horizontal = Input.GetAxis("LJoystickHorizontal") * Time.deltaTime;
                 float vertical = Input.GetAxis("LJoystickVertical") * Time.deltaTime;
                 float angle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;

                 transform.rotation = Quaternion.Euler(0.0f, Mathf.Clamp(angle, -10f, max_rot_velocity), 0.0f);*/

                Vector3 new_pos = new Vector3( transform.position.x + Input.GetAxis("LJoystickHorizontal")*movement_speed, transform.position.y, transform.position.z + Input.GetAxis("LJoystickVertical") * movement_speed);
                float target_angle = Mathf.Atan2(Input.GetAxis("LJoystickHorizontal"), Input.GetAxis("LJoystickVertical")) * Mathf.Rad2Deg;
                float current = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;

                float delta = Mathf.DeltaAngle(target_angle, current);
                //delta = Mathf.Clamp(-delta, -2000.0f, 2000.0f);

                transform.rotation *= Quaternion.AngleAxis(-delta * Time.deltaTime* 2, Vector3.up);


                movement = transform.forward * movement_speed;
            }

            movement.y = storey;

            if (Input.GetButtonDown("AButton"))
                movement.y = jump_force;

        }    

        movement.y = movement.y + (Physics.gravity.y * gravity_scale * Time.deltaTime);


        character_controller.Move(movement * Time.deltaTime);


    

    }
}
