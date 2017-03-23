using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMindAPI.LIB;
using System.IO;

public partial class TestXmindAPI : MonoBehaviour
{
    const string path2 = "C:\\Users\\xy\\Desktop\\隐形玩家剧本（第3版）.xmind";
    void Start()
    {

        XMindWorkBook xwb = new XMindWorkBook( path2, true);

        // Read workbook content and format into a report containing parent and child by sheet:
        List<XMindSheet> sheets = xwb.GetSheetInfo();



    }


}

