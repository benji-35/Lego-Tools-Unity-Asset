using UnityEngine;

namespace kap35 {
    namespace lego {
        [RequireComponent(typeof(CharacterController), typeof(Rigidbody))]
        public class PlayerController : MonoBehaviour {
            [Header("Components")]
            private CharacterController characterController;

            [Header("Settings")] [SerializeField] private float speed = 6.0f;
            [SerializeField] private float runSpeed = 10.0f;
            private bool running = false;
            [Header("Gravity")] [SerializeField] private float gravity = 9.81f;

            [SerializeField] private float jumpForce = 2.0f;
            private float verticalSpeed = 0.0f;
            [Header("Camera")] [SerializeField] private Transform cameraHolder;
            [SerializeField] private float mouseSensitivity = 1.0f;
            [SerializeField] private float upLimit = -50.0f;
            [SerializeField] private float downLimit = 50.0f;

            private GameManger manager;

            // Start is called before the first frame update
            void Start() {
                try {
                    characterController = GetComponent<CharacterController>();
                    manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManger>();
                } catch (System.Exception e) {
                    Debug.Log(e);
                }
            }

            // Update is called once per frame
            void Update()
            {
                if (manager == null || manager.IsInPauseMenu())
                    return;
                Move();
                Rotate();
            }

            private void Move()
            {
                float horizontalMove = Input.GetAxis("Horizontal");
                float verticalMove = Input.GetAxis("Vertical");

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    running = true;
                }
                else
                {
                    running = false;
                }

                if (characterController.isGrounded)
                {
                    verticalSpeed = 0.0f;
                    if (Input.GetKeyDown(KeyCode.Space))
                        verticalSpeed = jumpForce;
                }
                else
                {
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

                Vector3 currentRotation = cameraHolder.localEulerAngles;
                if (currentRotation.x > 180)
                {
                    currentRotation.x -= 360;
                }

                currentRotation.x = Mathf.Clamp(currentRotation.x, upLimit, downLimit);
                cameraHolder.localRotation = Quaternion.Euler(currentRotation);
            }
        }
    }
}
