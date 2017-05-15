using System;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            FreeMindeReader reader;
            List<FreeMindNode> data;
            Dictionary<string, FreeMindNode> dic = new Dictionary<string, FreeMindNode>();

            Dictionary<string, Block> blockDic = new Dictionary<string, Block>();
            Dictionary<string, Command> CommandDic = new Dictionary<string, Command>();

            //Init Flowchart
            //!\todo Migration
            Flowchart flowchart;
            GameObject go = new GameObject("Flowchart");
            flowchart = go.AddComponent<Flowchart>();

            //Read Data
            var path = EditorUtility.OpenFilePanelWithFilters("选择.mm或者.xml文件", "", new[] { "Xmind Export Fole", "mm,xml", "All files", "*" });
            reader = new FreeMindeReader(path);
            //data = reader.SelectNodes(dic);
            //flowchart.name = data[0].Text;
            ////Add Block
            //for (int i = 0; i < data[0].Nodes.Count; i++)
            //{
            //    Block block = FlowchartWindow.CreateBlock(flowchart, new Vector2(250 * i / 5, 150 * (i % 5)));
            //    block.BlockName = data[0].Nodes[i].Text;
            //    blockDic.Add(data[0].Nodes[i].Id, block);
            //}
            //Block firstBlock = blockDic.ElementAt(0).Value;

            //ChangeBlockEventHandler(firstBlock, typeof(GameStarted));


            ////对每个Block生成Command
            //foreach (var blockNode in data[0].Nodes)
            //{
            //    CommandTextDic commandTextDic = new CommandTextDic(blockDic[blockNode.Id]);
            //    Queue<String> commands = reader.CommandQueue(blockNode);
            //    //CommandGenerate.AddCommand(, blockNode);
            //    while (commands.Any())
            //    {
            //        //Debug.Log(commands.Dequeue());
            //        commandTextDic.Translate(commands.Dequeue(), TranslateMode.Single);
            //    }

            //}
            ////CommandTextDic commandTextDic = new CommandTextDic(blockDic[data[0].Nodes[1].Id]);
            ////Queue<String> commands = reader.CommandQueue(data[0].Nodes[1]);
            //////CommandGenerate.AddCommand(, blockNode);
            ////while (commands.Any())
            ////{
            ////    //Debug.Log(commands.Dequeue());
            ////    commandTextDic.Translate(commands.Dequeue(), TranslateMode.Single);
            ////}



            ////连接各个Block
            //foreach (var node in data[0].Nodes)
            //{
            //    List<FreeMindArrowLink> blockLink = node.Link.Where(p => blockDic.ContainsKey(p.DestinationId)).ToList();
            //    if (blockLink.Count > 1)
            //    {
            //        foreach (var link in blockLink)
            //        {
            //            Fungus.Menu command = CommandGenerate.AddCommand(blockDic[node.Id], typeof(Fungus.Menu)) as Fungus.Menu;
            //            CommandGenerate.SetCommandField(command, "targetBlock", blockDic[link.DestinationId]);
            //        }
            //    }
            //    else if (blockLink.Count == 1)
            //    {
            //        Call command = CommandGenerate.AddCommand(blockDic[node.Id], typeof(Call)) as Call;

            //        CommandGenerate.SetCommandField(command, "targetBlock", blockDic[blockLink[0].DestinationId]);
            //    }
            //}

            ////CommandGenerate.GetCommand("");
            TreeData tree = FindObjectOfType<TreeData>();
            if (tree == null)
            {
                tree=new GameObject("TreeData").AddComponent<TreeData>();
            }

        }




        public static void ChangeBlockEventHandler(Block block, Type eventHandlerType)
        {
            EventHandler newHandler = Undo.AddComponent(block.gameObject, eventHandlerType) as EventHandler;
            newHandler.ParentBlock = block;
            block._EventHandler = newHandler;
        }


    }


}
