using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing_controller : MonoBehaviour
{
    [SerializeField]
    public Pendulum pendulum;

    // Start is called before the first frame update
    void Start()
    {
        pendulum.Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = pendulum.MoveCharacter(transform.localPosition, Time.deltaTime);
    }
}
