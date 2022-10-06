using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RingShaderProperties))]
public class EditorRingShader : Editor
{

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        base.DrawDefaultInspector();

        RingShaderProperties selected = target as RingShaderProperties;

        if (selected != null)
        {
            selected.UpdateRingProperties();

            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            view.Repaint();
        }

    }
}
