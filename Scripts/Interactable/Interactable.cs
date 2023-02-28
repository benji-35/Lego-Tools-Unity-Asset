using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            private bool intercated = false;
            private bool isInteracting_ = false;

            private void Update() {
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
                yield return new WaitForSeconds(seconds);
                onInteractEnd.Invoke();
                OnInteractEnd();
            }
        }
    }
}
