using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace kap35
{
    namespace lego
    {
        public class PlayerController : MonoBehaviour
        {
            [Header("Components")] [SerializeField]
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
            [Header("Shooter")] [SerializeField] private GameObject bulletPrefab;
            [SerializeField] private Transform bulletSpawn;
            [SerializeField] private float bulletSpeed = 10.0f;
            [SerializeField] private float bulletLifeTime = 2.0f;
            [SerializeField] private float fireRate = 0.5f;
            private float nextFire = 0.0f;

            private GameManger manager;

            // Start is called before the first frame update
            void Start()
            {

                manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManger>();
            }

            private void Fire()
            {
                if (Input.GetButton("Fire1") && Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    Vector3 rotation = cameraHolder.rotation.eulerAngles;
                    bulletSpawn.rotation = Quaternion.Euler(rotation);
                    GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                    bullet.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    Rigidbody rb = bullet.GetComponent<Rigidbody>();
                    rb.velocity = bulletSpawn.forward * bulletSpeed;
                    Destroy(bullet, bulletLifeTime);
                }
            }

            // Update is called once per frame
            void Update()
            {
                if (manager == null || manager.IsInPauseMenu())
                    return;
                Move();
                Rotate();
                Fire();
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