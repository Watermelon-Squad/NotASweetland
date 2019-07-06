using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{

    public float movement_speed = 1.0f;
    public float rotation_speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3((Input.GetAxis("LJoystickHorizontal") * movement_speed * Time.deltaTime), 0,(Input.GetAxis("LJoystickVertical") * movement_speed * Time.deltaTime)));
        transform.RotateAround(transform.position, new Vector3(0, 1, 0), Input.GetAxis("RJoystickHorizontal") * rotation_speed * Time.deltaTime);
    }
}
