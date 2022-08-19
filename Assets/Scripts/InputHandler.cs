using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KH
{
    public class InputHandler : MonoBehaviour
    {
        float horizontal;
        float vertical;
        float moveAmount;

        PlayerController inputActions;
        private Vector2 movementInput;
        private Rigidbody rigidbody;
        Vector3 moveDirection;
        Transform cameraObject;
        float movementSpeed = 5;
        Vector3 normalVector;
        Transform myTransform;
        float rotationSpeed = 10;
        Animator _animator;

        public bool isWindowOpened;


        /// <summary>
        /// This two OnEnable and OnDisable is necessary in order to use New Input System for Route and Un-route events
        /// </summary>
        private void OnEnable() => inputActions.Enable();
        private void OnDisable() => inputActions.Disable();

        private void Awake()
        {
            // singleton
            if (inputActions == null) {
                inputActions = new PlayerController();
                inputActions.Player.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            }
            rigidbody = GetComponent<Rigidbody>();
            cameraObject = Camera.main.transform;
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();

        }

        public void HandleMovement()
        {
            if (isWindowOpened)
                return;

            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = HandleMoveAmountCustomClamp(Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical)));

            moveDirection = cameraObject.forward * vertical;
            moveDirection += cameraObject.right * horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            moveDirection *= movementSpeed * moveAmount;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            _animator.SetFloat("Vertical", moveAmount);
        }

        public void HandleRotation()
        {
            Vector3 targetDir = Vector3.zero;

            targetDir = cameraObject.forward * vertical;
            targetDir += cameraObject.right * horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = transform.forward;

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * Time.deltaTime);

            transform.rotation = targetRotation;

        }

        float HandleMoveAmountCustomClamp(float moveAmount)
        {
            if (moveAmount > 0.55f)
                return moveAmount;
            else if (moveAmount > 0.1f && moveAmount < 0.55f)
                return 0.25f;
            else
                return 0f;
        }
    }
}
