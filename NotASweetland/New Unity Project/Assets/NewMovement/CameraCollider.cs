using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    public float target_distance = 1.0f;
    public LayerMask collision_mask = 0;
    public Transform collider_transform = null;
    private Camera main_camera = null;
    private float speed_damp_time = 0.0f;
    private float collision_radius = 1.0f;
    private Vector3 collider_velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        main_camera = Camera.main;
        speed_damp_time = main_camera.GetComponent<CameraRotation>().smooth_damp_time;   
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (main_camera == null)
            main_camera = Camera.main;


        collision_radius = CalculateCollisionRadiusCamera(main_camera);
        main_camera.transform.localPosition = -Vector3.forward * main_camera.nearClipPlane;

        UpdateLenght();

    }

    private void UpdateLenght()
    {
        Vector3 new_local_position = -Vector3.forward * GetTargetLenght();
        collider_transform.localPosition = Vector3.SmoothDamp(collider_transform.localEulerAngles, new_local_position, ref collider_velocity, speed_damp_time);
    }

    private float CalculateCollisionRadiusCamera(Camera cam)
    {
        float half_vertical_FOV = (cam.fieldOfView / 2.0f) * Mathf.Deg2Rad;
        float near_clip_half_height = Mathf.Tan(half_vertical_FOV) * cam.nearClipPlane;
        float near_clip_half_width = near_clip_half_height * cam.aspect;

        return new Vector2(near_clip_half_width,near_clip_half_height).magnitude;
    }

    private float GetTargetLenght()
    {
        Ray ray = new Ray(transform.position, -transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, Mathf.Max(0.001f, collision_radius), out hit, target_distance, collision_mask))
            return hit.distance;
        else
            return target_distance;
    }
}
