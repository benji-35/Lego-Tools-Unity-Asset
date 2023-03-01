#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kap35.lego;
using UnityEditor;

[CustomEditor(typeof(ButtonInteract))]
[CanEditMultipleObjects]
public class ButtonInteractEditor : InteractableEditor {
    
    #region vars
        
            #region Events
                SerializedProperty onButtonActive;
                SerializedProperty onButtonInactive;
            #endregion
            
            #region SerializedProperties
                SerializedProperty displayableState;
                SerializedProperty active;
                SerializedProperty animator;
            #endregion
            
            #region Folders
                bool showButtonEvents = false;
                bool showButtonSerializedProperties = false;
            #endregion
    #endregion
    
    #region methods
    
        #region unityMethods

        protected override void OnEnabling() {
            onButtonActive = serializedObject.FindProperty("onButtonActive");
            onButtonInactive = serializedObject.FindProperty("onButtonInactive");
            displayableState = serializedObject.FindProperty("displayableState");
            active = serializedObject.FindProperty("active");
            animator = serializedObject.FindProperty("animator");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            DrawTitle("Button Interactable - Script");
            DrawSerializedProperties();
            DrawEvents();
            DrawTitle("Button Settings");
            DrawButtonSerializedProperties();
            DrawButtonEvents();
            serializedObject.ApplyModifiedProperties();
        }
        
        #endregion
    
        #region customMethods
        
            void DrawButtonSerializedProperties() {
                showButtonSerializedProperties = EditorGUILayout.Foldout(showButtonSerializedProperties, "Button Settings");
                if (showButtonSerializedProperties) {
                    EditorGUILayout.HelpBox("Displayable State is the object that will change color when the button is active or inactive", MessageType.Info);
                    EditorGUILayout.PropertyField(displayableState);
                    EditorGUILayout.HelpBox("Active is the state of the button. If true, the button is active", MessageType.Info);
                    EditorGUILayout.PropertyField(active);
                    EditorGUILayout.PropertyField(animator);
                }
            }
            
            void DrawButtonEvents() {
                showButtonEvents = EditorGUILayout.Foldout(showButtonEvents, "Button Events");
                if (showButtonEvents) {
                    EditorGUILayout.PropertyField(onButtonActive);
                    EditorGUILayout.PropertyField(onButtonInactive);
                }
            }

            #endregion
    #endregion
    
}

#endif