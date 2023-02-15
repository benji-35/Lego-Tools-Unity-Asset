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
            
            //camera part
            SerializedProperty camera;
            SerializedProperty animator;
            
            //hit
            SerializedProperty hit;
            SerializedProperty hitDistance;
            SerializedProperty damage;
            

        #endregion

        #region folders

            bool gravitySettings = false;
            bool mouseSettings = false;
            bool bulletSettings = false;
            bool hitSettings = false;

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
            camera = serializedObject.FindProperty("cameraHolder");
            animator = serializedObject.FindProperty("animator");
            hit = serializedObject.FindProperty("hitPoint");
            hitDistance = serializedObject.FindProperty("hitPointDistance");
            damage = serializedObject.FindProperty("damage");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawBeginInspector();
            serializedObject.ApplyModifiedProperties();
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
            EditorGUILayout.PropertyField(camera);
            EditorGUILayout.PropertyField(animator);
            EditorGUILayout.Space(20);
            DrawGravityInspector();
            EditorGUILayout.Space(20);
            DrawMouseInspector();
            EditorGUILayout.Space(20);
            DrawHitInspector();
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
        
        private void DrawHitInspector() {
            hitSettings = EditorGUILayout.Foldout(hitSettings, "Hit Settings");
            if (hitSettings) {
                EditorGUILayout.PropertyField(hit);
                EditorGUILayout.PropertyField(hitDistance);
                EditorGUILayout.PropertyField(damage);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
}
