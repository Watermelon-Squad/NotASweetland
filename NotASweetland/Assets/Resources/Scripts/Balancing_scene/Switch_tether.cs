using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_tether : MonoBehaviour
{

    public Transform newTether;
    public Swing_controller swing_Controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            swing_Controller.pendulum.SwitchTether(newTether.transform.position);
        }

    }
}
