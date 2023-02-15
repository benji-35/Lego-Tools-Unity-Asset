using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using kap35.lego;

[CanEditMultipleObjects]
[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor {
        #region vars

            SerializedProperty speed;
            SerializedProperty runSpeed;
            SerializedProperty gravity;
            SerializedProperty jumpForce;
            SerializedProperty mouseSensitivity;
            SerializedProperty upLimit;
            SerializedProperty downLimit;
            SerializedProperty bulletPrefab;
            SerializedProperty bulletSpawn;
            SerializedProperty bulletSpeed;
            SerializedProperty bulletLifeTime;
            SerializedProperty fireRate;

        #endregion

        #region folders

            bool gravitySettings = false;
            bool mouseSettings = false;
            bool bulletSettings = false;

        #endregion

        private void OnEnable()
        {
            speed = serializedObject.FindProperty("speed");
            runSpeed = serializedObject.FindProperty("runSpeed");
            gravity = serializedObject.FindProperty("gravity");
            jumpForce = serializedObject.FindProperty("jumpForce");
            mouseSensitivity = serializedObject.FindProperty("mouseSensitivity");
            upLimit = serializedObject.FindProperty("upLimit");
            downLimit = serializedObject.FindProperty("downLimit");
            bulletPrefab = serializedObject.FindProperty("bulletPrefab");
            bulletSpawn = serializedObject.FindProperty("bulletSpawn");
            bulletSpeed = serializedObject.FindProperty("bulletSpeed");
            bulletLifeTime = serializedObject.FindProperty("bulletLifeTime");
            fireRate = serializedObject.FindProperty("fireRate");
        }

        public override void OnInspectorGUI()
        {
            DrawBeginInspector();
        }
    
        private void DrawBeginInspector() {
            PlayerController controller = (PlayerController) target;
            var style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 20
            };
            EditorGUILayout.LabelField("Player Controller", style);
            EditorGUILayout.Space(20);
            EditorGUILayout.PropertyField(speed);
            EditorGUILayout.PropertyField(runSpeed);
            EditorGUILayout.Space(20);
            DrawGravityInspector();
            EditorGUILayout.Space(20);
            DrawMouseInspector();
        }
        
        private void DrawGravityInspector() {
            gravitySettings = EditorGUILayout.Foldout(gravitySettings, "Gravity Settings");
            if (gravitySettings) {
                EditorGUILayout.PropertyField(gravity);
                EditorGUILayout.PropertyField(jumpForce);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        
        private void DrawMouseInspector() {
            mouseSettings = EditorGUILayout.Foldout(mouseSettings, "Mouse Settings");
            if (mouseSettings) {
                EditorGUILayout.PropertyField(mouseSensitivity);
                EditorGUILayout.PropertyField(upLimit);
                EditorGUILayout.PropertyField(downLimit);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
}
