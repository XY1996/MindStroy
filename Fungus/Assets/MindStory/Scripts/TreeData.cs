using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FreeMinLib.MindInfo;
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
        FreeMindeReader reader=new FreeMindeReader(Application.dataPath+ "\\隐形玩家.xml");
        StartCoroutine(visitOneNode(reader.GenerateTreeNode()));
    }
	
	// Update is called once per frame
	void Update () {
	 
	}

    public IEnumerator  visitListNode(List<TreeNode> treeNode)
    {
        yield return null;
        foreach (var node in treeNode)
        {
            Debug.Log(node.Text);
        }
       
        
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
