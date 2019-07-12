using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dead_zone = 0.1f;

    public float rotate_speed = 1f;

    [HideInInspector]
    public bool fixed_camera = false;

    private GameObject player = null;
    private Vector3 offset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Mathf.Abs(Input.GetAxis("RJoystickHorizontal")) >= dead_zone || Mathf.Abs(Input.GetAxis("RJoystickVertical")) >= dead_zone)
        {
            
            float horizontal = player.transform.eulerAngles.y - Input.GetAxis("RJoystickHorizontal") * rotate_speed;
            float vertical = player.transform.eulerAngles.x - Input.GetAxis("RJoystickVertical") * rotate_speed;

            Quaternion q = Quaternion.Euler(vertical, horizontal, 0);
            transform.position = player.transform.position - (q * offset);
        }
        else
          if (!fixed_camera)
            transform.position = player.transform.position - offset;

        // float desiredY = player.transform.eulerAngles.y;
        //float desiredX = player.transform.eulerAngles.x;

        // Quaternion q = Quaternion.Euler(Vector3.up * desiredY);
        //  transform.position = player.transform.position - (q * offset);





        transform.LookAt(player.transform);
    }
}
