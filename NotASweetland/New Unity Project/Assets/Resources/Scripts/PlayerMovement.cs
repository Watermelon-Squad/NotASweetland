using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movement_speed = 1.0f;
    public float rotation_speed = 5.0f;
    public float jump_force = 300.0f;
    public bool moving = false;
    private bool jumping = false;
    public float dead_zone = 0.01f;
    private float min_angle = 0.1f;
    

    private Animator anim = null;
    //private Transform camera = null;
    Vector3 new_dir = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
       // camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input_joystic = new Vector2(Input.GetAxis("LJoystickHorizontal"), Input.GetAxis("LJoystickVertical"));

        if (Input.GetButtonDown("R1"))
        {
            anim.SetBool("Preparing_arm", true);
            anim.Play("Preparing_arm");
            Time.timeScale = 0.25f;
        }

        else if (Input.GetButtonUp("R1"))
        {
            anim.SetBool("Preparing_arm", false);
            Time.timeScale = 1.0f;
        }

        if (Input.GetAxis("R2") > 0.5f)
        {
            anim.SetBool("Preparing_arm", false);
            anim.SetBool("Throwing_arm", true);
            anim.Play("Throwing_arm");
        }

        else
        {
            anim.SetBool("Throwing_arm", false);
        }



        if (Input.GetButtonDown("AButton") && !jumping)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3 (0.0f, jump_force, 0.0f));
            anim.SetBool("jump", true);
            anim.Play("Jumping");
            jumping = true;
        }

        if (!anim.GetBool("Throwing_arm"))
        {

            if (Mathf.Abs(input_joystic.x) > dead_zone || Mathf.Abs(input_joystic.y) > dead_zone) //la dead zone se define en  edit->project settings->input
            {
                moving = true;
                if (!jumping)
                {
                    anim.SetBool("walk", true);
                    anim.Play("Run");
                }

                transform.position += new Vector3(input_joystic.x, 0, input_joystic.y) * movement_speed * Time.deltaTime;


                float target_degrees = Mathf.Atan2(input_joystic.x, input_joystic.y) * Mathf.Rad2Deg;
                float curret_degrees = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;

                float delta = -Mathf.DeltaAngle(target_degrees, curret_degrees);

                if (Mathf.Abs(delta) > min_angle)
                    transform.rotation *= Quaternion.AngleAxis(delta * Time.deltaTime, Vector3.up);

            }

            else
            {
                moving = false;
            }
        }

        else
        {

        }

        if (!jumping && !anim.GetBool("Preparing_arm") && !anim.GetBool("Throwing_arm") && !moving)
        {
            anim.SetBool("walk", false);
            anim.Play("Idle");
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        jumping = false;
    }

}


