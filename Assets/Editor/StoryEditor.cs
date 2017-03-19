using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class StoryEditor : Editor {

    [MenuItem("Assets/Create/FreeMindNode Info")]
    static void Create()
    {
        // get current selected directory
        string assetName = "FreeMindNode Database";

        string assetPath = "Assets";
        if (Selection.activeObject)
        {
            assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (Path.GetExtension(assetPath) != "")
            {
                assetPath = Path.GetDirectoryName(assetPath);
            }
        }

        //
        bool doCreate = true;
        string path = Path.Combine(assetPath, assetName + ".asset");
        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Exists)
        {
            doCreate = EditorUtility.DisplayDialog(assetName + " already exists.",
                                                    "Do you want to overwrite the old one?",
                                                    "Yes", "No");
        }
        if (doCreate)
        {
            FreeMindNodeScriptable FreeMindNodeInfo = FreeMindNodeUtility.Create(assetPath, assetName);
            Selection.activeObject = FreeMindNodeInfo;
            // EditorGUIUtility.PingObject(border);
        }
    }


    public static class FreeMindNodeUtility
    {
        public static FreeMindNodeScriptable Create(string _path, string _name)
        {
            //
            if (new DirectoryInfo(_path).Exists == false)
            {
                Debug.LogError("can't create asset, path not found");
                return null;
            }
            if (string.IsNullOrEmpty(_name))
            {
                Debug.LogError("can't create asset, the name is empty");
                return null;
            }
            string assetPath = Path.Combine(_path, _name + ".asset");

            //
            FreeMindNodeScriptable newAbilityInfo = ScriptableObject.CreateInstance<FreeMindNodeScriptable>();

            newAbilityInfo = LoadData(newAbilityInfo);

            AssetDatabase.CreateAsset(newAbilityInfo, assetPath);
            Selection.activeObject = newAbilityInfo;
            return newAbilityInfo;
        }

        public static FreeMindNodeScriptable LoadData(FreeMindNodeScriptable nodeScript)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            string path = Application.dataPath + "\\Test\\Resources\\TE1：替身.xml";
            FreeMindeReader reader = new FreeMindeReader(path);
            List<FreeMindNode> nodes = reader.SelectNodes();

            UnityEngine.Debug.Log(nodes[0].Text);

            nodeScript.nodes = nodes;
            return nodeScript;
        }
    }


   
}
