using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Fungus;
using Fungus.EditorUtils;
using UnityEditor;
using UnityEngine;

namespace MindStory
{
    public class AddCommandOperation
    {
        public Block block;
        public Type commandType;
        public int index;
    }

    public class CommandGenerate
    {

        static List<System.Type> menuTypes = EditorExtensions.FindDerivedTypes(typeof(Command)).ToList();
        static List<KeyValuePair<System.Type, CommandInfoAttribute>> filteredAttributes = GetFilteredCommandInfoAttribute(menuTypes);

        public static Command GetCommand(string Text)
        {
            foreach (var c in menuTypes)
            {
                Debug.Log(c);
            }
            return new Comment();
        }


        public static List<KeyValuePair<System.Type, CommandInfoAttribute>> GetFilteredCommandInfoAttribute(List<System.Type> menuTypes)
        {
            Dictionary<string, KeyValuePair<System.Type, CommandInfoAttribute>> filteredAttributes = new Dictionary<string, KeyValuePair<System.Type, CommandInfoAttribute>>();

            foreach (System.Type type in menuTypes)
            {
                object[] attributes = type.GetCustomAttributes(false);
                foreach (object obj in attributes)
                {
                    CommandInfoAttribute infoAttr = obj as CommandInfoAttribute;
                    if (infoAttr != null)
                    {
                        string dictionaryName = string.Format("{0}/{1}", infoAttr.Category, infoAttr.CommandName);

                        int existingItemPriority = -1;
                        if (filteredAttributes.ContainsKey(dictionaryName))
                        {
                            existingItemPriority = filteredAttributes[dictionaryName].Value.Priority;
                        }

                        if (infoAttr.Priority > existingItemPriority)
                        {
                            KeyValuePair<System.Type, CommandInfoAttribute> keyValuePair = new KeyValuePair<System.Type, CommandInfoAttribute>(type, infoAttr);
                            filteredAttributes[dictionaryName] = keyValuePair;
                        }
                    }
                }
            }
            return filteredAttributes.Values.ToList<KeyValuePair<System.Type, CommandInfoAttribute>>();
        }


        public static Command AddCommand(Block block, Type commandType, int index = -1)
        {
            AddCommandOperation commandOperation = new AddCommandOperation();

            commandOperation.block = block;
            commandOperation.commandType = commandType;
            if (index != -1)
            {
                commandOperation.index = index;
            }
            else
            {
                commandOperation.index = block.CommandList.Count;
            }

            Debug.Assert(block != null);


            var flowchart = (Flowchart)block.GetFlowchart();

            flowchart.ClearSelectedCommands();

            var newCommand = block.gameObject.AddComponent(commandOperation.commandType) as Command;
            block.GetFlowchart().AddSelectedCommand(newCommand);
            newCommand.ParentBlock = block;
            newCommand.ItemId = flowchart.NextItemId();

            // Let command know it has just been added to the block
            newCommand.OnCommandAdded(block);


            if (commandOperation.index < block.CommandList.Count - 1)
            {
                block.CommandList.Insert(commandOperation.index, newCommand);
            }
            else
            {
                block.CommandList.Add(newCommand);
            }

            // Because this is an async call, we need to force prefab instances to record changes
            //PrefabUtility.RecordPrefabInstancePropertyModifications(block);
            return newCommand;
        }

        public static Command SetCommandField(Command command, string propertyName, object value)
        {
            FieldInfo[] fieldInfos = command.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo fieldInfo = fieldInfos.First(p => p.Name == propertyName);
            fieldInfo.SetValue(command, value);
            return command;
        }

        //public static Block AddCommand(Block block, FreeMindNode node)
        //{
        //    //找到所有的CommandNode
        //    List<FreeMindNode> commandsNode = node.Nodes;
        //    //Debug.LogFormat("Block:{0} CommandCount{1}", blockNode.Text, commandsNode.Count);
        //    CommandTextDic commandTextDic = new CommandTextDic(block);
        //    //放进Stack里面的都是被访问过的
        //    Stack<FreeMindNode> accessStack = new Stack<FreeMindNode>();

        //    commandTextDic.Translate(node.Text, TranslateMode.Single);
        //    accessStack.Push(node);
        //    while (accessStack.Any())
        //    {
        //        List<FreeMindArrowLink> selfLink = accessStack.Pop().Link
        //            .Where(p => commandsNode.Any(o => p.DestinationId == o.Id))
        //            .ToList();
        //        if (!selfLink.Any())
        //        {                 
        //            continue;
        //        }
        //        else if (selfLink.Count == 1)
        //        {                   
        //            FreeMindNode templeFreeMindNode = commandsNode.First(p => p.Id == selfLink.First().DestinationId);
        //            commandTextDic.Translate(templeFreeMindNode.Text, TranslateMode.Single);
        //            accessStack.Push(templeFreeMindNode);
        //        }
        //        else
        //        {
        //            foreach (var tempArrowLink in selfLink)
        //            {
        //                FreeMindNode templeFreeMindNode = commandsNode.First(p => p.Id == tempArrowLink.DestinationId);
        //                commandTextDic.Translate(templeFreeMindNode.Text, TranslateMode.Multi);
        //                accessStack.Push(templeFreeMindNode);
        //            }
        //        }

        //    }
        //    return block;
        //}


    }

    public class CommandTextDic
    {
        private Block m_block;
        public CommandTextDic(Block block)
        {
            m_block = block;
        }

        public Command Translate(string data, TranslateMode mode)
        {
            switch (mode)
            {
                case TranslateMode.Multi:
                    {
                        Fungus.Menu command =CommandGenerate.AddCommand(m_block, typeof(Fungus.Menu)) as Fungus.Menu;
                        //CommandGenerate.SetCommandField(command, "text", data);
                        break;
                    }
                case TranslateMode.Single:
                    {
                        Comment comment= CommandGenerate.AddCommand(m_block, typeof(Fungus.Comment)) as Comment;
                        //CommandGenerate.SetCommandField(comment, "commentText", data);
                        break;
                    }

            }
            return new Comment();
        }

        //private Command TranslateName()
        //{

        //}
    }
    public enum TranslateMode
    {
        Single,
        Multi
    }
}

