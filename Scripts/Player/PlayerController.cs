using System;
using System.Collections;
using UnityEngine;

namespace kap35 {
    namespace lego {
        [RequireComponent(typeof(CharacterController))]
        public class PlayerController : MonoBehaviour {
            [Header("Components")]
            private CharacterController characterController;
            [SerializeField] private Animator animator;

            [Header("Settings")]
            [SerializeField] private float speed = 6.0f;
            [SerializeField] private float runSpeed = 10.0f;
            [SerializeField] private float timeBetweenPunches = 0.5f;
            private bool running = false;
            private bool canPunch = true;
            
            [Header("Gravity")]
            [SerializeField] private float gravity = 9.81f;
            [SerializeField] private float jumpForce = 2.0f;
            private float verticalSpeed = 0.0f;
            
            [Header("Camera")]
            [SerializeField] private Transform cameraHolder;
            [SerializeField] private float mouseSensitivity = 1.0f;
            [SerializeField] private float upLimit = -50.0f;
            [SerializeField] private float downLimit = 50.0f;
            [SerializeField] private Transform hitPoint;
            [SerializeField] private float hitPointDistance = 1.0f;
            [SerializeField] private int damage = 10;

            private GameManger manager;
            private Rigidbody rbPlayer;
            private bool canMove = true;

            // Start is called before the first frame update
            void Start() {
                try {
                    characterController = GetComponent<CharacterController>();
//                    rbPlayer = GetComponent<Rigidbody>();
                    manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManger>();
                } catch (System.Exception e) {
                    Debug.Log(e);
                }
            }

            // Update is called once per frame
            void Update() {
                if (manager == null || manager.IsInPauseMenu())
                    return;
                Move();
                Rotate();
                Punch();
            }

            private void Move() {
                if (!canMove) {
                    animator.SetBool("run", false);
                    animator.SetBool("walk", false);
                    return;
                }
                float horizontalMove = Input.GetAxis("Horizontal");
                float verticalMove = Input.GetAxis("Vertical");
                
                bool moving = false;
                if (verticalMove != 0 || horizontalMove != 0) {
                    moving = true;
                    animator.SetBool("walk", true);
                } else  {
                    animator.SetBool("walk", false);
                }

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    running = true;
                    if (moving && animator != null) {
                        animator.SetBool("run", true);
                    }
                } else  {
                    running = false;
                    animator.SetBool("run", false);
                }

                if (characterController.isGrounded) {
                    verticalSpeed = 0.0f;
                    if (Input.GetKey(KeyCode.Space) && jumpForce > 0f) {
                        animator.SetTrigger("jump");
                        verticalSpeed = jumpForce;
                    }
                } else {
                    verticalSpeed -= gravity * Time.deltaTime;
                }

                float _speed = speed;
                if (running)
                {
                    _speed = runSpeed;
                }

                Vector3 gravityMove = new Vector3(0, verticalSpeed, 0);
                Vector3 move = transform.forward * verticalMove + transform.right * horizontalMove;
                characterController.Move(move * _speed * Time.deltaTime + gravityMove);
            }

            private void Rotate()
            {
                float horizontalRotation = Input.GetAxis("Mouse X");
                float verticalRotation = Input.GetAxis("Mouse Y");

                transform.Rotate(0, horizontalRotation * mouseSensitivity, 0);
                cameraHolder.Rotate(-verticalRotation * mouseSensitivity, 0, 0);
                hitPoint.Rotate(-verticalRotation * mouseSensitivity, 0, 0);

                Vector3 currentRotation = cameraHolder.localEulerAngles;
                if (currentRotation.x > 180)
                {
                    currentRotation.x -= 360;
                }

                currentRotation.x = Mathf.Clamp(currentRotation.x, upLimit, downLimit);
                cameraHolder.localRotation = Quaternion.Euler(currentRotation);
            }

            private void Punch() {
                if (!Input.GetKeyDown(KeyCode.Mouse0) || !canPunch) {
                    return;
                }
                canPunch = false;
                animator.SetTrigger("punch");
                StartCoroutine(waitingPunch());
                StartCoroutine(stopMovement(0.867f));
                RaycastHit hit;
                if (Physics.Raycast(hitPoint.position, hitPoint.forward, out hit, hitPointDistance)) {
                    Collider collider = hit.collider;
                    if (collider != null) {
                        Life life = collider.GetComponent<Life>();
                        if (life != null) {
                            life.TakeDamage(damage);
                        }
                    }
                }
            }

            IEnumerator waitingPunch()
            {
                yield return new WaitForSeconds(timeBetweenPunches);
                canPunch = true;
            }
            
            IEnumerator stopMovement(float time) {
                canMove = false;
                yield return new WaitForSeconds(time);
                canMove = true;
            }

            private void OnDrawGizmos()
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(hitPoint.position, hitPoint.forward * hitPointDistance);
            }

            public Transform GetHitPoint() {
                return hitPoint;
            }
        }
    }
}
