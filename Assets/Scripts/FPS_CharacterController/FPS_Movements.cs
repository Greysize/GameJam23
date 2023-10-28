using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FPS_Controller
{
    public class FPS_Movements : MonoBehaviour
    {
#pragma warning disable 649
        [Header("Movement")]
        public bool CanMove = true;
        [SerializeField] CharacterController controller;
        [SerializeField] float speed = 11f;
        Vector2 MovementInput;
        [Header("Jump")]
        public bool CanJump = true;
        [SerializeField] float JumpHeight = 3.5f;
        bool isJumping;
        [Header("Gravity")]
        public bool isGravityActive = true;
        [SerializeField] float gravity = -30f;
        Vector3 MovementVerticalVelocity = Vector3.zero;
        [SerializeField] LayerMask groundMask;
        bool isGrounded;
        private float GravityMult;
        private FPS_InputManager FPS_Manager;

        private void Awake()
        {
            FPS_Manager = FindObjectOfType<FPS_InputManager>();
            controller = gameObject.GetComponent<CharacterController>();
            if (controller == null)
                Debug.Log(" ERR# : No Character controller Found on " + gameObject.name);
            if (FPS_Manager == null)
                Debug.Log(" ERR# : No Input Manager found");
        }

        private void FixedUpdate()
        {
            if (FPS_Manager.isInputActive)
            {
                // init
                if (isGravityActive)
                {
                    // Ground test
                    isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);
                    if (isGrounded)
                        MovementVerticalVelocity.y = 0f;
                    GravityMult = 1f;
                }
                else
                {
                    isGrounded = false;
                    MovementVerticalVelocity.y = 0f;
                    GravityMult = 0f;
                    isJumping = true;
                }
                //Horizontal Movmeents
                Vector3 MovementHorizontalVelocity = (transform.right * MovementInput.x + transform.forward * MovementInput.y) * speed;
                controller.Move(MovementHorizontalVelocity * Time.deltaTime);

                //Jump
                if (isJumping)
                {
                    if (isGrounded)
                        MovementVerticalVelocity.y = Mathf.Sqrt(-2f * JumpHeight * gravity);
                    isJumping = false;
                }

                //Gravity
                MovementVerticalVelocity.y += gravity * GravityMult * Time.deltaTime;
                controller.Move(MovementVerticalVelocity * Time.deltaTime);
            }
        }

        public void ReceiveInput(Vector2 _movInput)
        {
            if (FPS_Manager.isInputActive)
            {
                if (CanMove)
                    MovementInput = _movInput;
                else
                    MovementInput = Vector2.zero;
            }
            else
                MovementInput = Vector2.zero;
        }

        public void OnJumpPressed()
        {
            if (FPS_Manager.isInputActive)
            {
                if (CanJump)
                {
                    if (!FPS_Manager.isPosed)
                        isJumping = true;
                }
            }
        }
    }
}