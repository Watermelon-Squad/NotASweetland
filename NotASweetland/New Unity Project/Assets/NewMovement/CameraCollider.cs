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
    private float collision_radius = 0.25f;
    private Vector3 collider_velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        main_camera = Camera.main;
        //speed_damp_time = main_camera.GetComponent<CameraRotation>().smooth_damp_time;   
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (main_camera == null)
            main_camera = Camera.main;

        if (main_camera != null)
        {
            collision_radius = CalculateCollisionRadiusCamera(main_camera);
            main_camera.transform.localPosition = -Vector3.forward * main_camera.nearClipPlane;
        }
        UpdateLenght();

    }

    private void UpdateLenght()
    {
        Vector3 new_local_position = -Vector3.forward * GetTargetLenght();
        collider_transform.localPosition = Vector3.SmoothDamp(collider_transform.position, new_local_position, ref collider_velocity, speed_damp_time);
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
        {
            return hit.distance;
        }
        else
            return target_distance;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, collider_transform.position);
        DrawGizmoSphere(collider_transform.position,collision_radius );

    }

    private void DrawGizmoSphere(Vector3 pos, float radius)
    {
        Quaternion rot = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

        int alphaSteps = 8;
        int betaSteps = 16;

        float deltaAlpha = Mathf.PI / alphaSteps;
        float deltaBeta = 2.0f * Mathf.PI / betaSteps;

        for (int a = 0; a < alphaSteps; a++)
        {
            for (int b = 0; b < betaSteps; b++)
            {
                float alpha = a * deltaAlpha;
                float beta = b * deltaBeta;

                Vector3 p1 = pos + rot * GetSphericalPoint(alpha, beta, radius);
                Vector3 p2 = pos + rot * GetSphericalPoint(alpha + deltaAlpha, beta, radius);
                Vector3 p3 = pos + rot * GetSphericalPoint(alpha + deltaAlpha, beta - deltaBeta, radius);

                Gizmos.DrawLine(p1, p2);
                Gizmos.DrawLine(p2, p3);
            }
        }
    }

    private Vector3 GetSphericalPoint(float alpha, float beta, float radius)
    {
        Vector3 point;
        point.x = radius * Mathf.Sin(alpha) * Mathf.Cos(beta);
        point.y = radius * Mathf.Sin(alpha) * Mathf.Sin(beta);
        point.z = radius * Mathf.Cos(alpha);

        return point;
    }

}
