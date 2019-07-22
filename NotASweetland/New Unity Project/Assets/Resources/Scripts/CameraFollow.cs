﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float dead_zone = 0.1f;

    public float rotate_speed = 1f;

    [HideInInspector]
    public bool fixed_camera = false;

    private GameObject player = null;
    private Vector3 offset = Vector3.zero;

    private float horizontal = 0f;
    private float vertical = 0f;

    public Transform pivot = null;

    public float min_rotation_camera_x = 45f;
    public float max_rotation_camera_x = 180f;

    [HideInInspector]
    public bool preparing_arm = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = player.transform.position - transform.position;

        pivot.transform.position = player.transform.position;
        pivot.transform.parent = player.transform;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!preparing_arm)
        {

            if (Mathf.Abs(Input.GetAxis("RJoystickHorizontal")) >= dead_zone || Mathf.Abs(Input.GetAxis("RJoystickVertical")) >= dead_zone)
            {
                horizontal = Input.GetAxis("RJoystickHorizontal") * rotate_speed;
                pivot.transform.Rotate(0, -horizontal, 0);
                vertical = Input.GetAxis("RJoystickVertical") * rotate_speed;
                pivot.transform.Rotate(-vertical, 0, 0);
            }

            if (pivot.rotation.eulerAngles.x > min_rotation_camera_x && pivot.rotation.eulerAngles.x < max_rotation_camera_x)
            {
                pivot.rotation = Quaternion.Euler(min_rotation_camera_x, pivot.eulerAngles.y, pivot.eulerAngles.z);
            }

            if (pivot.rotation.eulerAngles.x > max_rotation_camera_x && pivot.rotation.eulerAngles.x < (360f - min_rotation_camera_x))
            {
                pivot.rotation = Quaternion.Euler((360f - min_rotation_camera_x), pivot.eulerAngles.y, pivot.eulerAngles.z);
            }

            Quaternion q = Quaternion.Euler(pivot.transform.eulerAngles.x, pivot.transform.eulerAngles.y, 0);
            transform.position = pivot.transform.position - (q * offset);

            if (transform.position.y < pivot.transform.position.y)
                transform.position = new Vector3(transform.position.x, pivot.transform.position.y - 0.5f, transform.position.z);



            
                transform.LookAt(player.transform);
        }
        else
        {
            if (Mathf.Abs(Input.GetAxis("RJoystickHorizontal")) >= dead_zone || Mathf.Abs(Input.GetAxis("RJoystickVertical")) >= dead_zone)
            {
                horizontal = Input.GetAxis("RJoystickHorizontal") * rotate_speed;
                transform.Rotate(0, horizontal, 0);
                vertical = Input.GetAxis("RJoystickVertical") * rotate_speed;
                transform.Rotate(vertical, 0, 0);
            }
        }
    }
}