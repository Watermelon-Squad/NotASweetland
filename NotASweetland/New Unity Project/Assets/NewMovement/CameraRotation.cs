using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform pivot = null;
    
    public float rotation_speed = 0.0f;
    [Tooltip("Function Smooth Damp: Gradually changes a vector towards a desired goal over time.\n The vector is smoothed by some spring - damper like function, which will never overshoot.The most common use is for smoothing a follow camera.")]
    public float smooth_damp_time = 0.025f;

    private Vector3 camera_velocity = Vector3.zero;


    public void SetSmoothPosition(Vector3 position)
    {
        transform.position = Vector3.SmoothDamp(transform.position, position, ref camera_velocity, smooth_damp_time);
    }

    public void SetRotation(Vector2 rot)
    {
        Quaternion camera_local_rotation = Quaternion.Euler(0.0f, rot.y, 0.0f);
        Quaternion pivot_local_rotation = Quaternion.Euler(rot.x, 0.0f, 0.0f);

        if(rotation_speed > 0.0f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, camera_local_rotation, rotation_speed * Time.deltaTime);
            pivot.localRotation = Quaternion.Slerp(pivot.localRotation, pivot_local_rotation, rotation_speed * Time.deltaTime);
        }
        else
        {
            transform.localRotation = camera_local_rotation;
            pivot.localRotation = pivot_local_rotation;
        }
    }
}
