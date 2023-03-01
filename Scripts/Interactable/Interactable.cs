using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace kap35
{
    namespace lego
    {
        public class Interactable : MonoBehaviour
        {
            [SerializeField] private UnityEvent onInteract;
            [SerializeField] private UnityEvent onInteractEnd;
            [SerializeField] private float interactDistance = 1f;
            [SerializeField] private bool isInteracting = false;
            [SerializeField] private float interactionTime = 0f;
            [SerializeField] private Image interactImage;
            [SerializeField] private GameObject interactCanvas;
            [SerializeField] private Transform discussRefPoint;
            private bool intercated = false;
            private bool isInteracting_ = false;
            private Transform refPoint;
            private PlayerController pl;

            private void Start()
            {
                pl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                if (discussRefPoint == null) {
                    refPoint = transform;
                } else {
                    refPoint = discussRefPoint;
                }
                if (interactImage != null) {
                    if (interactionTime > 0) {
                        interactImage.fillAmount = 0f;
                    } else {
                        interactImage.fillAmount = 1f;
                    }
                }
            }
            
            private void Update() {
                OnUpdate();
                if (Vector3.Distance(refPoint.position, pl.transform.position) <= interactDistance && !isInteracting) {
                    DetectInteracting();
                } else if (isInteracting) {
                    StopInteracting();                    
                }
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
            
            private void OnDrawGizmos() {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, interactDistance);
            }

            private void DetectInteracting()
            {
                intercated = false;
                GameManger manager = GetGameManager();
                if (manager != null)
                    manager.ShowInteract();
                isInteracting = true;
            }

            private void StopInteracting() {
                GameManger manager = GetGameManager();
                if (!intercated && manager != null)
                    manager.HideInteract();
                isInteracting = false;
            }
            
            protected virtual void OnUpdate() {}

            protected virtual void OnInteract() { }
            protected virtual void OnInteractEnd() { }

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

            private void OnEnable() {
                if (interactCanvas != null)
                    interactCanvas.SetActive(false);
            }

            private void OnDisable() {
                if (interactCanvas != null)
                    interactCanvas.SetActive(false);
            }
        }
    }
}
