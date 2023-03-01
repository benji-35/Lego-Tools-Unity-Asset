using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace kap35
{
    namespace lego
    {
        public class Coin : MonoBehaviour
        {
            [Header("Coin Settings")]
            [SerializeField] private int coinValue = 1;
            [SerializeField] private float timeAlive = 60f;

            [SerializeField] private float jumpForce = 5f;
            [SerializeField] private AudioSource audioSource;
            
            [Header("Detection Settings")]
            [SerializeField] private float detectDistance = 1f;
            
            private PlayerController pl;
            private bool canGrab = true;

            // Start is called before the first frame update
            void Start()
            {
                pl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                Destroy(gameObject, timeAlive);
                if (coinValue <= 100)
                {
                    GetComponent<Renderer>().material.color = Color.yellow;
                }
                else if (coinValue <= 10000)
                {
                    GetComponent<Renderer>().material.color = Color.gray;
                }
                else if (coinValue > 10000)
                {
                    GetComponent<Renderer>().material.color = new Color(255, 0, 255);
                }
            }

            private void Update()
            {
                if (pl == null)
                    Destroy(gameObject);
                if (Vector3.Distance(transform.position, pl.transform.position) < detectDistance) {
                    GrabCoin();
                }
            }

            private void GrabCoin()
            {
                if (!canGrab)
                    return;
                if (audioSource != null)
                    audioSource.Play();
                GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
                if (obj != null)
                    obj.GetComponent<GameManger>().AddCoin(coinValue);
                GetComponent<MeshRenderer>().enabled = false;
                canGrab = false;
                StartCoroutine(waitingForDestroy());
            }

            private void OnCollisionEnter(Collision collision)
            {
                if (collision.gameObject.tag != "ground")
                {
                    return;
                }

                GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }


            public void SetCoinValue(int value)
            {
                coinValue = value;
            }

            IEnumerator waitingForDestroy()
            {
                yield return new WaitForSeconds(1);
                Destroy(gameObject);
            }

            private void OnDrawGizmos()
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, detectDistance);
            }
        }
    }
}