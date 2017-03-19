using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class ReaderTest : MonoBehaviour
{

    const string path1 = "\\Test\\Resources\\章节1.xml";
    const string path2 = "\\Test\\Resources\\TE1：替身.xml";
    // Use this for initialization
    void Start()
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        string path = Application.dataPath + path2;
        FreeMindeReader reader = new FreeMindeReader(path);
        List<FreeMindNode> nodes= reader.SelectNodes();
        UnityEngine.Debug.Log(nodes[0].Text);
        stopwatch.Stop();
        UnityEngine.Debug.Log(stopwatch.Elapsed.Seconds);

    }

    // Update is called once per frame
    void Update()
    {

    }
}