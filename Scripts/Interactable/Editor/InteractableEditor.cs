#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kap35.lego;
using UnityEditor;

[CustomEditor(typeof(Interactable))]
[CanEditMultipleObjects]
public class InteractableEditor : Editor {
    #region vars
    
        #region Events
            SerializedProperty onInteract;
            SerializedProperty onInteractEnd;
        #endregion
        
        #region SerializedProperties
            SerializedProperty interactDistance;
            SerializedProperty isInteracting;
            SerializedProperty interactionTime;
            SerializedProperty interactImage;
            SerializedProperty interactCanvas;
            SerializedProperty discussRefPoint;
            SerializedProperty debugDistance;
        #endregion
        
        #region Folders
            bool showEvents = false;
            bool showSerializedProperties = false;
    #endregion
    
    #endregion
    
    #region methods
    
        #region unityMethods

        private void OnEnable()
        {
            onInteract = serializedObject.FindProperty("onInteract");
            onInteractEnd = serializedObject.FindProperty("onInteractEnd");
            interactDistance = serializedObject.FindProperty("interactDistance");
            isInteracting = serializedObject.FindProperty("isInteracting");
            interactionTime = serializedObject.FindProperty("interactionTime");
            interactImage = serializedObject.FindProperty("interactImage");
            interactCanvas = serializedObject.FindProperty("interactCanvas");
            discussRefPoint = serializedObject.FindProperty("discussRefPoint");
            debugDistance = serializedObject.FindProperty("debugDistance");
            OnEnabling();
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            DrawTitle("Interactable");
            DrawSerializedProperties();
            DrawEvents();
            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    
        #region customMethods

            protected virtual void OnEnabling() {}

            protected void DrawTitle(string title) {
                var style = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold,
                    fontSize = 20
                };
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(20);
                EditorGUILayout.LabelField(title, style);
                EditorGUILayout.Space(20);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(20);
            }
        
            protected void DrawSerializedProperties() {
                showSerializedProperties = EditorGUILayout.Foldout(showSerializedProperties, "Interactable Settings");
                if (showSerializedProperties) {
                    EditorGUILayout.PropertyField(interactDistance);
                    EditorGUILayout.PropertyField(isInteracting);
                    EditorGUILayout.PropertyField(interactionTime);
                    EditorGUILayout.PropertyField(interactImage);
                    EditorGUILayout.PropertyField(interactCanvas);
                    EditorGUILayout.PropertyField(discussRefPoint);
                    EditorGUILayout.PropertyField(debugDistance);
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        
            protected void DrawEvents() {
                showEvents = EditorGUILayout.Foldout(showEvents, "Interactable Events");
                if (showEvents) {
                    EditorGUILayout.PropertyField(onInteract);
                    EditorGUILayout.PropertyField(onInteractEnd);
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        
        #endregion
    #endregion
}

#endif
