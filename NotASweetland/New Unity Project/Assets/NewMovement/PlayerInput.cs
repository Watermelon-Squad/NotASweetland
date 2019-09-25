using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 move_input { get; private set; }
    public Vector2 last_move_input { get; private set; }
    public Vector2 camera_input { get; private set; }

    public bool jump_input { get; private set; }

    public void UpdateInput()
    {
        move_input = new Vector2(Input.GetAxis("RJoystickHorizontal"), Input.GetAxis("RJoystickVertical") * 2);

        // Comprobation dead zone? or not needed
        //Save last vector input?

        camera_input = new Vector2(Input.GetAxis("LJoystickHorizontal"),Input.GetAxis("LJoystickVertical") * 1.2f);
        jump_input = Input.GetButton("AButton");

    }
}
