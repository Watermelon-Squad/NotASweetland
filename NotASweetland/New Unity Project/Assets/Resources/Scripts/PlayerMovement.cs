using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movement_speed = 1.0f;
    public float jump_force = 300.0f;
    public float rot_speed = 10f;
    public bool moving = false;
    private bool jumping = false; 

    private Animator anim = null;
    Vector3 new_dir = Vector3.zero;

    public GameObject cross = null;
    public GameObject dot = null;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input_joystic = new Vector2(Input.GetAxis("LJoystickHorizontal"), Input.GetAxis("LJoystickVertical"));

        if (Input.GetButtonDown("R1"))
        {
            anim.SetBool("Preparing_arm", true);
            anim.Play("Preparing_arm");

            Camera.main.GetComponent<CameraFollow>().preparing_arm = true;
            dot.SetActive(true);

            Time.timeScale = 0.25f;
        }
        else if(Input.GetButton("R1"))
        {
            //cross.transform.GetChild(0).GetChild(0).transform.position;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight / 2, 0));
            RaycastHit hit;

            //Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject.name);
                cross.SetActive(true);
                // cross.transform.position = hit.transform.gameObject.transform.position;
                cross.transform.position =  hit.point;
            }
        }
        else if (Input.GetButtonUp("R1"))
        {
            anim.SetBool("Preparing_arm", false);
            Camera.main.GetComponent<CameraFollow>().preparing_arm = false;
            dot.SetActive(false);
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

            if (Mathf.Abs(input_joystic.x) > 0 || Mathf.Abs(input_joystic.y) > 0) 
            {
                moving = true;
                if (!jumping)
                {
                    anim.SetBool("walk", true);
                    anim.Play("Run");
                }

                transform.Rotate(new Vector3(0, 1, 0), input_joystic.x * Time.deltaTime * rot_speed * 10);

                if(input_joystic.y > 0)
                transform.position += (new Vector3(0, 0, input_joystic.y) * movement_speed * Time.deltaTime).magnitude * transform.forward;
                else
                    transform.position += (new Vector3(0, 0, input_joystic.y) * movement_speed/3 * Time.deltaTime).magnitude * (-transform.forward);
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


