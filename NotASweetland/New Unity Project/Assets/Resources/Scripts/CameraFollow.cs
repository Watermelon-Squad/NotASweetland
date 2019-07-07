using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform player_transform = null;
    private float player_speed = 1.0f;


    public float radius = 10.0f;
    private float count = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        player_transform = GameObject.FindGameObjectWithTag("Player").transform;
        player_speed = player_transform.gameObject.GetComponent<PlayerMovement>().movement_speed;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = player_transform.position - transform.position;
        direction.y = 0;

        if (direction.magnitude > radius )
        {
            direction = direction.normalized * player_speed * Time.deltaTime;
            transform.position += direction;
        }

        transform.LookAt(player_transform);

        if (!player_transform.gameObject.GetComponent<PlayerMovement>().moving)
        {
            Vector3 n = (player_transform.position - transform.position);
            n.y = 0;
            float angle = Vector3.Angle(n.normalized, Vector3.forward);

            if (angle > 1)
            {
                transform.RotateAround(player_transform.position, Vector3.up, angle * Time.deltaTime);
            }
        }

       

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

    }

}
