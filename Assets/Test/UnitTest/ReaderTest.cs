using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class ReaderTest : MonoBehaviour
{
    [SerializeField]
    private List<FreeMindNode> nodes;

    [SerializeField]
    private FreeMindNode node;

    const string path1 = "\\Test\\Resources\\章节1.xml";
    const string path2 = "\\Test\\Resources\\TE1：替身.xml";
    // Use this for initialization
    void Start()
    {
        //StartCoroutine(LoadData());

    }

    IEnumerator LoadData()
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        yield return null;
        string path = Application.dataPath + path2;
        FreeMindeReader reader = new FreeMindeReader(path);
        nodes = reader.SelectNodes();

        UnityEngine.Debug.Log(nodes[0].Text);

        stopwatch.Stop();
        UnityEngine.Debug.Log(stopwatch.Elapsed.Seconds);

    }

    // Update is called once per frame
    void Update()
    {

    }
}