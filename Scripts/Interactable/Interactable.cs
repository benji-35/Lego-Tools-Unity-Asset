using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace kap35
{
    namespace lego
    {
        public class Interactable : MonoBehaviour
        {
            [SerializeField] private UnityEvent onInteract;
            [SerializeField] private UnityEvent onInteractEnd;
            [SerializeField] private bool isInteracting = false;
            [SerializeField] private float interactionTime = 0f;
            [SerializeField] private Image interactImage;
            [SerializeField] private GameObject interactCanvas;
            private bool intercated = false;
            private bool isInteracting_ = false;

            private void Start()
            {
                if (interactImage != null) {
                    if (interactionTime > 0) {
                        interactImage.fillAmount = 0f;
                    } else {
                        interactImage.fillAmount = 1f;
                    }
                }
            }
            
            private void Update() {
                if (interactImage != null) {
                    interactCanvas.SetActive(isInteracting);
                }
                if (isInteracting && Input.GetKeyDown(KeyCode.E) && interactionTime <= 0f) {
                    intercated = true;
                    GameManger manager = GetGameManager();
                    if (manager != null)
                        manager.HideInteract();
                    onInteract.Invoke();
                    onInteractEnd.Invoke();
                    isInteracting = false;
                    OnInteract();
                } else if (isInteracting && interactionTime > 0f) {
                    if (Input.GetKeyDown(KeyCode.E)) {
                        onInteract.Invoke();
                        OnInteract();
                        isInteracting_ = true;
                        StartCoroutine(InteractTiming(interactionTime));
                    }
                    if (Input.GetKeyUp(KeyCode.E)) {
                        isInteracting_ = false;
                        StopCoroutine(InteractTiming(interactionTime));
                    }
                }
                if (isInteracting_ && !isInteracting) {
                    isInteracting_ = false;
                    StopCoroutine(InteractTiming(interactionTime));
                }
            }

            protected virtual void OnInteract() { }
            protected virtual void OnInteractEnd() { }

            private void OnTriggerEnter(Collider other) {
                if (other.tag != "Player")
                    return;
                intercated = false;
                GameManger manager = GetGameManager();
                if (manager != null)
                    manager.ShowInteract();
                isInteracting = true;
            }

            private void OnTriggerExit(Collider other) {
                if (other.tag != "Player")
                    return;
                GameManger manager = GetGameManager();
                if (!intercated && manager != null)
                    manager.HideInteract();
                isInteracting = false;
            }

            protected GameManger GetGameManager() {
                GameObject manager = GameObject.FindGameObjectWithTag("GameManager");
                if (manager == null)
                    return null;
                return manager.GetComponent<GameManger>();
            }

            public void AddOnInteractEvent(UnityAction action) {
                onInteract.AddListener(action);
            }
            
            public void AddOnInteractEndEvent(UnityAction action) {
                onInteractEnd.AddListener(action);
            }

            IEnumerator InteractTiming(float seconds) {
                while (seconds > 0f) {
                    if (!isInteracting_) {
                        if (interactImage != null)
                            interactImage.fillAmount = 0f;
                        break;
                    }

                    seconds -= Time.deltaTime;
                    if (interactImage != null)
                        interactImage.fillAmount = 1f - seconds / interactionTime;
                    yield return null;
                }

                if (isInteracting_) {
                    onInteractEnd.Invoke();
                    OnInteractEnd();
                }
            }

            private void OnEnable()
            {
                if (interactCanvas != null)
                    interactCanvas.SetActive(false);
            }

            private void OnDisable()
            {
                if (interactCanvas != null)
                    interactCanvas.SetActive(false);
            }
        }
    }
}
