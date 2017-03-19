using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class ReaderTest : MonoBehaviour
{
    [SerializeField]
    private List<FreeMindNode> nodes;

    private Dictionary<string, FreeMindNode> id2Node = new Dictionary<string, FreeMindNode>();

    const string path1 = "\\Test\\Resources\\章节1.xml";
    const string path2 = "\\Test\\Resources\\TE1：替身.xml";
    // Use this for initialization
    IEnumerator Start()
    {
        yield return StartCoroutine(LoadData());
        yield return StartCoroutine(ShowPath());
    }

    IEnumerator LoadData()
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        yield return null;
        string path = Application.dataPath + path2;
        FreeMindeReader reader = new FreeMindeReader(path);
        nodes = reader.SelectNodes();
        id2Node = reader.Dic;

        UnityEngine.Debug.Log(nodes[0].Text);

        stopwatch.Stop();
        UnityEngine.Debug.Log(stopwatch.Elapsed.Seconds);

    }

    IEnumerator ShowPath()
    {
        yield return null;
        ShowText(nodes);
    }

    void ShowText(FreeMindNode node)
    {
        UnityEngine.Debug.Log(node.Text);
        if (node.Link.Count == 0)
            if (node.Nodes.Count == 0)
                return;
            else
            {
                ShowText(node.Nodes);
            }
        else
        {
            List<FreeMindNode> linkedNode = new List<FreeMindNode>();
            for (int i = 0; i < node.Link.Count; i++)
            {
                if(id2Node.ContainsKey(node.Link[i].Id))
                    linkedNode.Add(id2Node[node.Link[i].Id]);
            }
               
            ShowText(linkedNode);
        }

    }
    void ShowText(List<FreeMindNode> nodes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            ShowText(nodes[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}