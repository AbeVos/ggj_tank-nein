using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
[CustomEditor(typeof(SetSortingLayer))]
[CanEditMultipleObjects]
[ExecuteInEditMode]
public class SetSortingLayerEditor : Editor
{

    //[HideInInspector]
    //private bool snapToGrid = true;

    //public override void OnInspectorGUI();
    //public void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    //public void OnGUI()
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        //DrawDefaultInspector();

        /* Only for the Actor, not for its other classes */
        //if (	! (	Selection.activeGameObject.GetType() == typeof(Actor)	)	)
        //	return;

        //bool previousSnap = snapToGrid;
        //this.snapToGrid = EditorGUILayout.Toggle("Snap to grid", this.snapToGrid);

        if (GUILayout.Button("Sort now"))
        {
            SetSortingLayer sorting = target as SetSortingLayer;
            sorting.SortLayer();
            EditorUtility.SetDirty(sorting);
        }
    }
}