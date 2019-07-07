using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movement_speed = 1.0f;
    public float rotation_speed = 5.0f;
    public bool moving = false;
    public float dead_zone = 0.01f;
    private float min_angle = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input_joystic = new Vector2(Input.GetAxis("LJoystickHorizontal"), Input.GetAxis("LJoystickVertical"));

        if (Mathf.Abs(input_joystic.x) > dead_zone || Mathf.Abs(input_joystic.y) > dead_zone)
        {
            moving = true;


            transform.position += new Vector3(input_joystic.x, 0, input_joystic.y) * movement_speed * Time.deltaTime;


            float target_degrees = Mathf.Atan2(input_joystic.x, input_joystic.y) * Mathf.Rad2Deg;
            float curret_degrees = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;

            float delta = -Mathf.DeltaAngle(target_degrees, curret_degrees);

            if (delta > min_angle)
                transform.rotation *= Quaternion.AngleAxis(delta * Time.deltaTime, Vector3.up);

           // transform.position += new Vector3((Input.GetAxis("LJoystickHorizontal") * movement_speed * Time.deltaTime), 0, (Input.GetAxis("LJoystickVertical") * movement_speed * Time.deltaTime));
           // transform.RotateAround(transform.position, new Vector3(0, 1, 0), Input.GetAxis("RJoystickHorizontal") * rotation_speed * Time.deltaTime);
        }
        else
            moving = false;


       
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 n = new Vector3(0, 0.6f, -1);

        n *= 10f/2;
        
        Gizmos.DrawLine(transform.position,-transform.forward);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.forward);
    }
}
