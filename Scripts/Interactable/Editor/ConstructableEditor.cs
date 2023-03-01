#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kap35.lego;
using UnityEditor;

[CustomEditor(typeof(Constructable))]
[CanEditMultipleObjects]
public class ConstructableEditor : Editor
{
    #region vars

        #region Events
            SerializedProperty onConstruct;
            SerializedProperty onDeconstruct;
        #endregion
        
        #region SerializedProperties

            private SerializedProperty isDestroyed;
            private SerializedProperty destructObject;
            private SerializedProperty constructObject;

        #endregion
        
        #region Folders
            bool showEvents = false;
            bool showSerializedProperties = false;
    #endregion

    #endregion
    
    #region Methods

        private void OnEnable() {
            onConstruct = serializedObject.FindProperty("_onConstruct");
            onDeconstruct = serializedObject.FindProperty("_onDeconstruct");
            isDestroyed = serializedObject.FindProperty("isDestroyed");
            destructObject = serializedObject.FindProperty("_destructObject");
            constructObject = serializedObject.FindProperty("_constructObject");
        }
        
        public override void OnInspectorGUI() {
            serializedObject.Update();
            DrawTitle("Constructable");
            Constructable constructable = (Constructable) target;
            if (constructable.GetConstructObject() == null || constructable.GetDestructObject() == null) {
                EditorGUILayout.HelpBox("You need to set the construct and destruct object", MessageType.Error);
            }
            DrawSerializedProperties();
            DrawEvents();
            serializedObject.ApplyModifiedProperties();
        }
        
        
        void DrawTitle(string title) {
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
        
        void DrawSerializedProperties() {
            showSerializedProperties = EditorGUILayout.Foldout(showSerializedProperties, "Settings");
            if (showSerializedProperties) {
                Constructable constructable = (Constructable) target;
                EditorGUILayout.PropertyField(isDestroyed);
                if (constructable.IsDestroyed()) {
                    EditorGUILayout.HelpBox("This object is destroyed when the game begin", MessageType.Info);
                } else {
                    EditorGUILayout.HelpBox("This object is constructed when the game begin", MessageType.Info);
                }
                EditorGUILayout.PropertyField(destructObject);
                EditorGUILayout.PropertyField(constructObject);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            
        }
        
        void DrawEvents() {
            showEvents = EditorGUILayout.Foldout(showEvents, "Events");
            if (showEvents) {
                EditorGUILayout.PropertyField(onConstruct);
                EditorGUILayout.PropertyField(onDeconstruct);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }


    #endregion
}

#endif