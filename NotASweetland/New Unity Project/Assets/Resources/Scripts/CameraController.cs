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

    private float horizontal = 0f;
    private float vertical = 0f;

    public Transform pivot = null;

    public float min_rotation_camera_x = 45f;
    public float max_rotation_camera_x = 180f;

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

        if (Mathf.Abs(Input.GetAxis("RJoystickHorizontal")) >= dead_zone || Mathf.Abs(Input.GetAxis("RJoystickVertical")) >= dead_zone)
        {

            horizontal =  Input.GetAxis("RJoystickHorizontal") * rotate_speed;
            player.transform.Rotate(0, horizontal, 0);
            vertical =  Input.GetAxis("RJoystickVertical") * rotate_speed;
            pivot.transform.Rotate(-vertical, 0, 0);
        }

        if(pivot.rotation.eulerAngles.x > min_rotation_camera_x && pivot.rotation.eulerAngles.x < max_rotation_camera_x)
        {
            pivot.rotation = Quaternion.Euler(min_rotation_camera_x, 0, 0);
        }

        if(pivot.rotation.eulerAngles.x > max_rotation_camera_x && pivot.rotation.eulerAngles.x < (360f - min_rotation_camera_x))
        {
            pivot.rotation = Quaternion.Euler((360f-min_rotation_camera_x), 0, 0);
        }

        Quaternion q = Quaternion.Euler(pivot.transform.eulerAngles.x, player.transform.eulerAngles.y, 0);
        transform.position = player.transform.position - (q * offset);

        if (transform.position.y < player.transform.position.y)
            transform.position = new Vector3(transform.position.x, player.transform.position.y - 0.5f, transform.position.z);




        transform.LookAt(player.transform);
    }
}
