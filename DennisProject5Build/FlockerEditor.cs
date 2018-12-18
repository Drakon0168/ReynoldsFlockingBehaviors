using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Flocker))]
public class FlockerEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Flocker flocker = (Flocker)target;

        float totalValue = flocker.cohesionWeight + flocker.separationWeight + flocker.alignmentWeight + flocker.seekWeight;

        if(totalValue == 0)
        {
            totalValue = 1;
        }

        flocker.cohesionWeight /= totalValue;
        flocker.separationWeight /= totalValue;
        flocker.alignmentWeight /= totalValue;
        flocker.seekWeight /= totalValue;

        flocker.cohesionWeight = EditorGUILayout.Slider("Cohesion Weight", flocker.cohesionWeight, 0, 1);
        flocker.separationWeight = EditorGUILayout.Slider("Separation Weight", flocker.separationWeight, 0, 1);
        flocker.alignmentWeight = EditorGUILayout.Slider("AlignmentWeight", flocker.alignmentWeight, 0, 1);
        flocker.seekWeight = EditorGUILayout.Slider("SeekWeight", flocker.seekWeight, 0, 1);
    }
}
