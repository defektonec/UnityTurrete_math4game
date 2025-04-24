using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //speed and running
    private float currentSpeed;
    [SerializeField] float walkSpeed = 5.0f;
    [SerializeField] float runSpeed = 10.0f;
    [SerializeField] float jumpForce = 3.0f;

    //jump and gravity
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float radius = 0.2f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float gravity;

    [SerializeField] private Transform cameraTransform;
    private CharacterController characterController;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, radius, groundMask);
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Лёгкое "прилипание" к земле
        }

        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        Vector3 moveDirection = Vector3.zero;

        if (inputDirection.magnitude >= 0.1f)
        {
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;
            moveDirection.Normalize();
        }

        // horizontal movement
        Vector3 horizontalMovement = moveDirection * currentSpeed;

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // gravitation
        velocity.y += gravity * Time.deltaTime;

        // final movement
        Vector3 finalMovement = horizontalMovement;
        finalMovement.y = velocity.y;
        characterController.Move(finalMovement * Time.deltaTime);

    }


    private void OnDrawGizmos()
    {
        if (groundChecker != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundChecker.position, radius);
        }
    }
}
