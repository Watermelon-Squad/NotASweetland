using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movement_speed = 1.0f;
    public float rotation_speed = 5.0f;
    public bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Mathf.Abs(Input.GetAxis("LJoystickHorizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("LJoystickVertical")) > 0.1f)
        {
            moving = true;
        }
        else
            moving = false;

        transform.Translate(new Vector3((Input.GetAxis("LJoystickHorizontal") * movement_speed * Time.deltaTime), 0, (Input.GetAxis("LJoystickVertical") * movement_speed * Time.deltaTime)));
        transform.RotateAround(transform.position, new Vector3(0, 1, 0), Input.GetAxis("RJoystickHorizontal") * rotation_speed * Time.deltaTime);
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
