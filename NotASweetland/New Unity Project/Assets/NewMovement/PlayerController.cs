using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerController", menuName = "Not a Sweetland/Controllers/PlayerController")]
public class PlayerController : EntityController
{
    private PlayerInput player_input = null;
    private CameraRotation camera_rotation = null;

    public float rotation_sensitivity = 2.0f;

    public override void EntityInit()
    {
        player_input = FindObjectOfType<PlayerInput>();
        camera_rotation = FindObjectOfType<CameraRotation>();
    }

    public override void EntityUpdate()
    {
        player_input.UpdateInput();

        UpdateRotation();
        character.SetMovement(GetMovement());
        character.SetJump(player_input.jump_input);
    }

    public override void EntityFixedUpdate()
    {
        camera_rotation.SetSmoothPosition(character.transform.position);
        camera_rotation.SetRotation(character.GetCharacterRotation());
    }

    private void UpdateRotation()
    {
        Vector2 character_rotation = character.GetCharacterRotation();
        Vector2 camera_input = player_input.camera_input;

        float pitch = character_rotation.x - (camera_input.y * rotation_sensitivity);
        float yaw = character_rotation.y + (camera_input.x * rotation_sensitivity);

        character.SetCharacterRotation(new Vector2(pitch, yaw));
    }

    private Vector3 GetMovement()
    {
        Quaternion yaw_rotation = Quaternion.Euler(0.0f, character.GetCharacterRotation().y, 0.0f);
        Vector3 forward = yaw_rotation * Vector3.forward;
        Vector3 right = yaw_rotation * Vector3.right;

        Vector3 movement = forward * player_input.move_input.y + right * player_input.move_input.x;

        if (movement.sqrMagnitude > 1.0f)
            movement.Normalize();

        return movement;

    }

}
