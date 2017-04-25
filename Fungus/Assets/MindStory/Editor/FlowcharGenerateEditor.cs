using Fungus;
using System.Collections;
using System.Collections.Generic;
using Fungus.EditorUtils;
using UnityEditor;
using UnityEngine;

public class FlowcharGenerateEditor : Editor {

    [ExecuteInEditMode]
    [MenuItem("MindStory/Generate")]
    public static void menu()
    {
        Flowchart flowchart = FindObjectOfType<Flowchart>();
        if (flowchart == null)
        {
            GameObject go = new GameObject("Flowchart");
            flowchart = go.AddComponent<Flowchart>();
        }
        else
        {
            DestroyImmediate(flowchart.gameObject);
            GameObject go = new GameObject("Flowchart");
            flowchart = go.AddComponent<Flowchart>();
        }
        FlowchartWindow.CreateBlock(flowchart, Vector2.zero);




    }
}
