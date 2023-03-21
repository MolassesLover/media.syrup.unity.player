// Copyright © 2022, Maeve "Molasses" Garside
// Licensed under the MIT license, making this script copyleft.
// Check the LICENSE.md file for further information.

using UnityEngine;

namespace SyrupPlayer
{
public class RigidbodyController : MonoBehaviour
{
    public GameObject player;

    [Header("Camera")]
    [Tooltip("The main camera; automatically selected on awake.")]
    public Transform mainCamera;
    private GameObject[] cameras;

    [Header("Locomotion")]
    public WallRunning wallRunComponent; 
    private Vector3 wallForward;
    [Tooltip("Determines the player's movement speed.")]
    [Range(1f, 10f)]
    [Min(1f)]
    public float movementSpeed = 5;
    public float turnSmoothing = 0.1f;
    [Range(0.1f, 0.5f)]
    [Min(0.1f)]
    private float turnVelocity;
    public Rigidbody playerRigidbody;

    // Awake() is called when the script instance is being loaded.
    private void Awake() {
        cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        mainCamera = cameras[0].GetComponent<Transform>();
    }

    // Update() is called once per frame.
    private void Update() {
        playerRigidbody.useGravity = !wallRunComponent.gluedToWall;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f) {
            if ((wallRunComponent.wallLeft || wallRunComponent.wallRight)) {
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.y);

                wallForward = Vector3.Cross(wallRunComponent.wallNormal, transform.up);

                wallRunComponent.gluedToWall = true;
            } else {
                wallRunComponent.gluedToWall = false;
                player.GetComponent<CharacterController>().enabled = true;
            }

            if (wallRunComponent.gluedToWall) {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnSmoothing);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                transform.position = transform.position + moveDirection.normalized * movementSpeed * Time.deltaTime;
            }
        }
    }
}
}