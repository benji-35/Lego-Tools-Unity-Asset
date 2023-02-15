#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using kap35.lego;

[CanEditMultipleObjects]
[CustomEditor(typeof(Responable))]
public class ResponableEditor : Editor {
    #region vars

    

    #endregion
    
    #region methods

    private void OnEnable() {
            
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }

    #endregion
}

#endif