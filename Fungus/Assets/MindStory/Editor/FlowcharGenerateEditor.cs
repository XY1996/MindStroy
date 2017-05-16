using System;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FreeMinLib.Reader;
using Fungus.EditorUtils;
using UnityEditor;
using UnityEngine;
using EventHandler = Fungus.EventHandler;

namespace MindStory
{

    public class FlowcharGenerateEditor : Editor
    {

        [ExecuteInEditMode]
        [MenuItem("MindStory/Generate")]
        public static void menu()
        {
            ITreeNodeGenerate reader;
            Dictionary<string, string> allData;
            //Read Data
            var path = EditorUtility.OpenFilePanelWithFilters("选择.mm或者.xml文件", "", new[] { "Xmind Export Fole", "mm,xml", "All files", "*" });

            XmlData xml= ScriptableObject.CreateInstance<XmlData>();
            xml.fileName = Path.GetFileNameWithoutExtension(path);
            xml.Content = File.ReadAllBytes(path);

            string saveAssestPath = "Assets/Resources/story/" + xml.fileName + ".asset";
            string saveBundlePath = "Assets/Resources/story/" + xml.fileName + ".unity3d";
            AssetDatabase.CreateAsset(xml, saveAssestPath);

            //AssetBundleBuild[] buildMap = new AssetBundleBuild[2];

            //buildMap[0].assetBundleName = "resources";//打包的资源包名称 随便命名
            //string[] resourcesAssets = new string[2];//此资源包下面有多少文件
            //resourcesAssets[0] = "resources/1.prefab";
            //resourcesAssets[1] = "resources/MainO.cs";
            //buildMap[0].assetNames = resourcesAssets;

            //BuildPipeline.BuildAssetBundles("Assets/ABs", buildMap);


            //reader = new FreeMindeReader(path);
            //reader.GenerateTreeNode(out allData);
        }


    }

  

}
