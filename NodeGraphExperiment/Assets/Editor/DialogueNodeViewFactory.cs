﻿using System;
using UnityEditor.Experimental.GraphView;

namespace Editor
{
    public class DialogueNodeViewFactory
    {
        public DialogueNodeView From(DialogueNodeViewData data)
        {
            var node = new DialogueNodeView(data);
            node.Guid = Guid.NewGuid().ToString();

            var input = node.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            input.portName = "";
            node.inputContainer.Add(input);

            var output = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            output.portName = "";
            node.outputContainer.Add(output);
            
            node.RefreshPorts();
            node.RefreshExpandedState();
            return node;
        }
        
        public DialogueNodeView CreateDialogueNode()
        {
            var node = new DialogueNodeView(NodeFactory.Elena("test", "description"));
            node.Guid = Guid.NewGuid().ToString();

            var input = node.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            input.portName = "";
            node.inputContainer.Add(input);

            var output = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            output.portName = "";
            node.outputContainer.Add(output);
            
            node.RefreshPorts();
            node.RefreshExpandedState();
            return node;
        }
    }
}