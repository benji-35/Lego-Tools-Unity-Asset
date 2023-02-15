#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using kap35.lego;

[CustomEditor(typeof(Quest))]
[CanEditMultipleObjects]
public class QuestEditor : Editor {
    #region vars
    
        #region serializedProperties
            
            private SerializedProperty questName;
            private SerializedProperty questDescription;
            private SerializedProperty questType;
            private SerializedProperty onFinish;
            private SerializedProperty onStart;

            #region moveVars

                private SerializedProperty waypoints;
                private SerializedProperty colorGizmos;
                private SerializedProperty drawGizmos;

            #endregion

            #region talkVars

                private SerializedProperty talkTo;          

            #endregion

            #region EnemyVars

                private SerializedProperty enemy;

            #endregion
            
            #region DisplayingLogo

                private float minSize;
                private float maxSize;
                
                private float maxSizeDistance;
                private float minSizeDistance;
                
            #endregion
            
            #region InteractQuest
                private SerializedProperty interactor;
                private SerializedProperty interactableState;
            #endregion
            
            #region CollectQuest

            private SerializedProperty collectables;
            
            #endregion
        
        #endregion

        #region splitsPart
        
            bool showGeneral = false;
            bool showEdit = false;
            bool events = false;
            
        #endregion
    #endregion

    private void OnEnable() {
        questName = serializedObject.FindProperty("questName");
        questDescription = serializedObject.FindProperty("questDescription");
        questType = serializedObject.FindProperty("questType");
        onFinish = serializedObject.FindProperty("eventsOnFinish");
        onStart = serializedObject.FindProperty("eventsOnStart");
        
        // moveVars
        waypoints = serializedObject.FindProperty("waypoints");
        colorGizmos = serializedObject.FindProperty("gizmosColor");
        drawGizmos = serializedObject.FindProperty("displayOnEditor");
        
        // talkVars
        talkTo = serializedObject.FindProperty("talkTo");
        
        // collectVars
        enemy = serializedObject.FindProperty("enemy");
        
        // interactQuest
        interactor = serializedObject.FindProperty("interactable");
        interactableState = serializedObject.FindProperty("stateButton");
        
        // collectQuest
        collectables = serializedObject.FindProperty("collectables");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        Quest quest = (Quest)target;
        EditorGUILayout.BeginHorizontal();
        var style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 20
        };
        EditorGUILayout.LabelField("Quest Editor", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(20);
        showGeneral = EditorGUILayout.BeginFoldoutHeaderGroup(showGeneral, "General Information");
        if (showGeneral) {
            EditorGUILayout.PropertyField(questName);
            EditorGUILayout.PropertyField(questDescription);
            EditorGUILayout.PropertyField(questType);
            serializedObject.ApplyModifiedProperties();
        }
        EditorGUI.EndFoldoutHeaderGroup();

        events = EditorGUILayout.BeginFoldoutHeaderGroup(events, "Events");
        if (events) {
            EditorGUILayout.HelpBox("Events are called when the quest is started or finished", MessageType.Info);
            EditorGUILayout.PropertyField(onStart);
            EditorGUILayout.HelpBox("Events are called when the quest is finished", MessageType.Info);
            EditorGUILayout.PropertyField(onFinish);
            serializedObject.ApplyModifiedProperties();
        }
        EditorGUI.EndFoldoutHeaderGroup();

        EditorGUILayout.Space(5);
        showEdit = EditorGUILayout.BeginFoldoutHeaderGroup(showEdit, "Edit Quest");
        EditorGUI.EndFoldoutHeaderGroup();
        if (showEdit) {
            switch (quest.GetQuestType())
            {
                case QuestType.Collect:
                    DisplayCollectEditor(quest);
                    break;
                case QuestType.Destruct:
                    DisplayDestructEditor();
                    break;
                case QuestType.Kill:
                    DisplayKillEditor();
                    break;
                case QuestType.Move:
                    DisplayMoveEditor(quest);
                    break;
                case QuestType.Talk:
                    DisplayTalkEditor();
                    break;
                case QuestType.Interact:
                    DisplayInteractable();
                    break;
                default:
                    DisplayOtherEditor();
                    break;
            }
        }
        EditorGUILayout.Space(5);
        serializedObject.ApplyModifiedProperties();
    }
    
    private void DisplayCollectEditor(Quest quest)
    {
        var style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 15,
        };
        EditorGUILayout.LabelField("Collect", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space(15);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(collectables, true);
        EditorGUILayout.EndHorizontal();
        
    }
    
    private void DisplayKillEditor() {
        var style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 15,
        };
        EditorGUILayout.LabelField("Kill", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space(15);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(enemy);
        EditorGUILayout.EndHorizontal();
    }
    
    private void DisplayTalkEditor() {
        var style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 15,
        };
        EditorGUILayout.LabelField("Talk", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space(15);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("You", GUILayout.Width(100));
        EditorGUILayout.PropertyField(talkTo);
        EditorGUILayout.EndHorizontal();
    }
    
    private void DisplayDestructEditor() {
        
    }
    
    private void DisplayMoveEditor(Quest quest) {
        
        var style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 15,
        };
        EditorGUILayout.LabelField("Move", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space(15);
        EditorGUILayout.PropertyField(waypoints);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(drawGizmos);
        EditorGUILayout.PropertyField(colorGizmos);
        EditorGUILayout.EndHorizontal();
    }
    
    private void DisplayOtherEditor() {
        
    }

    private void DisplayInteractable()
    {
        var style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 15,
        };
        EditorGUILayout.LabelField("Interact", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space(15);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(interactor);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("State of button needed: ", GUILayout.Width(200));
        EditorGUILayout.PropertyField(interactableState);
        EditorGUILayout.EndHorizontal();
    }
}

#endif
