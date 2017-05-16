using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using FreeMinLib.MindInfo;
using MindStory;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class TreeData : MonoBehaviour
{
    public GameObject buttonGroup;

    public GameObject button;
   

    Stack<List<TreeNode>> RightTreeNode=new Stack<List<TreeNode>>();
    // Use this for initialization
    void Start ()
    {
        string Xmlpath = "Assets\\Resources\\Story\\隐形玩家.asset";
        XmlData xmlData =AssetDatabase.LoadAssetAtPath(Xmlpath, typeof(XmlData)) as XmlData;
        Debug.Log(xmlData.fileName);
        MemoryStream ms = new MemoryStream(xmlData.Content);
        XmlDocument dataDocument=new XmlDocument();
        dataDocument.Load(ms);
        FreeMindeReader reader=new FreeMindeReader(dataDocument);

        Dictionary<string, string> allData;
        StartCoroutine(visitOneNode(reader.GenerateTreeNode(out allData)));
    }
	
	// Update is called once per frame
	void Update () {
	 
	}

    public IEnumerator visitListNode(List<TreeNode> treeNode)
    {
        yield return null;
        foreach (var node in treeNode)
        {
            Debug.Log(node.Text);
        }

        GameObject returnBtn = Instantiate(button);
        returnBtn.transform.SetParent(buttonGroup.transform);
        returnBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (treeNode.First().ParentNode != null && treeNode.First().ParentNode.ParentNode != null)
            {
                TreeNode indexNode = treeNode.First().ParentNode.ParentNode;
                StartCoroutine(visitOneNode(indexNode));
            }
            else
            {
                Debug.Log("开始");
            }
           
        });
        for (int i = 0; i < treeNode.Count; i++)
        {
            GameObject go = Instantiate(button);
            go.transform.SetParent(buttonGroup.transform);
            ButtonData btnData = go.AddComponent<ButtonData>();
            btnData.index = i;
            Button btn = go.GetComponent<Button>();
            btn.gameObject.transform.Find("Text").GetComponent<Text>().text = treeNode[i].Text;
            btn.onClick.AddListener(() =>
            {
                TreeNode indexNode = treeNode[btn.gameObject.GetComponent<ButtonData>().index];
                StartCoroutine(visitOneNode(indexNode));
            });
        }
    }

    public IEnumerator visitOneNode(TreeNode tree)
    {
        for (int i = 0; i < buttonGroup.transform.childCount; i++)
        {
            GameObject go = buttonGroup.transform.GetChild(i).gameObject;
            Destroy(go);
        }
        yield return null;
        if (tree.RightChildNode.Any())
            RightTreeNode.Push(tree.RightChildNode);
        if (tree.LeftChildNode.Any())
        {
            StartCoroutine(visitListNode(tree.LeftChildNode));          
        }
        else
        {
            if (RightTreeNode.Any())
                StartCoroutine(visitListNode(RightTreeNode.Pop()));
            else
            {
                Debug.Log("游戏结束");
            }
        }
    }



}
