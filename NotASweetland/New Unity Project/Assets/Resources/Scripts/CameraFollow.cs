using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform player_transform = null;
    private float player_speed = 1.0f;

    public float radius = 10.0f;

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

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

    }

}
