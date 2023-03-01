using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace kap35
{
    namespace lego
    {
        public class DiscussManager : Interactable
        {
            //first discuss
            [SerializeField] private Discuss firstDiscuss;

            //event when the discuss is finish
            [SerializeField] private UnityEvent onDiscussionFinish;

            //can restart the discuss
            [SerializeField] private bool canRestartDiscuss = false;
            [SerializeField] private Animator animator;
            
            [Header("Animation Settings")]
            [SerializeField] private bool wavingHands = false;
            [SerializeField] private float wavingHandsArea = 1.2f;

            //number of time the player discuss with the object
            private int discusses = 0;

            //current discuss
            private Discuss currentDiscuss = null;

            //is the player discussing with the object
            private bool isDiscussing = false;
            private GameObject player;

            // Start is called before the first frame update
            void Start() {
                player = GameObject.FindGameObjectWithTag("Player");
                if (firstDiscuss == null)
                {
                    Debug.LogWarning("No discuss found");
                }
                onDiscussionFinish.AddListener(StopAnimations);
                if (!canRestartDiscuss) {
                    onDiscussionFinish.AddListener(DisplayDiscuss);
                }
            }

            private void DisableDiscuss() {
                this.enabled = false;
            }

            private void StopAnimations()
            {
                if (animator == null)
                    return;
                animator.SetBool("wavingHand", false);
                animator.SetBool("talking", false);
            }

            protected override void OnInteract() {
                StartDiscuss();
            }

            private void StartDiscuss() {
                if (firstDiscuss == null)
                    return;
                if (discusses == 0 || canRestartDiscuss) {
                    if (animator != null) {
                        animator.SetBool("talking", true);
                        if (wavingHands) {
                            animator.SetBool("wavingHand", false);
                        }
                    }
                    isDiscussing = true;
                    currentDiscuss = firstDiscuss;
                    OpenDiscuss();
                }
            }

            public void AddEventOnFinish(UnityAction action, Quest whichQuest)
            {
                onDiscussionFinish.AddListener(action);
            }

            public void CallFinishDiscuss()
            {
                onDiscussionFinish.Invoke();
            }

            private void OpenDiscuss()
            {
                GameManger gameManager = GetGameManager();
                if (gameManager == null)
                {
                    return;
                }

                gameManager.OpenDiscuss(firstDiscuss, this);
                DisplayDiscuss();
            }

            private void CloseDiscuss() {
                isDiscussing = false;
                currentDiscuss = null;
                onDiscussionFinish.Invoke();
                discusses++;
                GameManger gameManager = GetGameManager();
                if (gameManager == null) {
                    return;
                }

                gameManager.CloseDiscuss();
            }

            public void NextDiscuss()
            {
                if (currentDiscuss == null)
                {
                    if (isDiscussing)
                        CloseDiscuss();
                    return;
                }

                currentDiscuss = currentDiscuss.nextDiscussion;
                if (currentDiscuss == null)
                {
                    CloseDiscuss();
                    return;
                }

                DisplayDiscuss();
            }

            private void DisplayDiscuss()
            {
                if (!isDiscussing || currentDiscuss == null)
                    return;
                GameManger gameManager = GetGameManager();
                if (gameManager == null)
                {
                    return;
                }

                gameManager.SetInteractText(currentDiscuss.talker, currentDiscuss.text);
            }

            private void OnDrawGizmos() {
                Gizmos.DrawWireSphere(transform.position, wavingHandsArea);
            }

            protected override void OnUpdate() {
                if (player == null || wavingHands == false || animator == null)
                    return;
                if (Vector3.Distance(player.transform.position, transform.position) <= wavingHandsArea) {
                    animator.SetBool("wavingHand", true);
                } else {
                    animator.SetBool("wavingHand", false);                    
                }
            }
        }
    }
}