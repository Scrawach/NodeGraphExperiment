﻿using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueNodeViewFactory
    {
        private readonly DialogueGraphView _canvas;
        private readonly DialoguePersonDatabase _personDatabase;

        public DialogueNodeViewFactory(DialogueGraphView canvas, DialoguePersonDatabase personDatabase)
        {
            _canvas = canvas;
            _personDatabase = personDatabase;
        }

        public RedirectNode CreateRedirectNode(Vector2 position, Edge target, EventCallback<MouseDownEvent> onMouseDown = null)
        {
            target.input.Disconnect(target);
            target.output.Disconnect(target);
            target.Clear();
            _canvas.RemoveElement(target);

            var redirectNode = new RedirectNode
            {
                title = ""
            };
            
            redirectNode.styleSheets.Add(Resources.Load<StyleSheet>("Styles/RedirectNode"));

            var (input, output) = CreatePortsFor(redirectNode);
            redirectNode.SetPosition(new Rect(position, Vector2.zero));
            
            var leftEdge = CreateEdge(target.output, input, onMouseDown);
            var rightEdge = CreateEdge(target.input, output, onMouseDown);
                        
            _canvas.AddElement(redirectNode);
            _canvas.AddElement(leftEdge);
            _canvas.AddElement(rightEdge);
            return redirectNode;
        }

        public DialogueNodeView From(DialogueNodeViewData data)
        {
            var dialogue = new DialogueNode(data.PersonName, data.Title, data.Description);
            var node = new DialogueNodeView(dialogue);
            
            dialogue.Guid = node.viewDataKey;
            node.Update(data);
            
            dialogue.PersonName.Changed += () =>
            {
                var updatedData = _personDatabase.FindByName(dialogue.PersonName.Value);
                node.ChangePerson(updatedData);
            };

            CreatePortsFor(node);
            return node;
        }
        
        private static Edge CreateEdge(Port a, Port b, EventCallback<MouseDownEvent> onMouseDown = null)
        {
            var edge = a.ConnectTo(b);
            
            if (onMouseDown != null)
                edge.RegisterCallback(onMouseDown);
            
            return edge;
        }

        private static (Port input, Port output) CreatePortsFor(Node node)
        {
            var input = node.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            input.portName = "";
            node.inputContainer.Add(input);
            var output = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            node.outputContainer.Add(output);
            output.portName = "";
            
            node.RefreshPorts();
            node.RefreshExpandedState();
            return (input, output);
        }

        public void CreatePersonNode(DialoguePersonData data, Vector2 position)
        {
            var viewData = new DialogueNodeViewData()
            {
                PersonName = data.Name,
                HeaderColor = data.Color,
                Title = "none",
                Description = "none",
                Icon = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(data.Icon))
            };

            var dialogueNode = From(viewData);
            dialogueNode.SetPosition(new Rect(_canvas.contentViewContainer.WorldToLocal(position), Vector2.zero));
            _canvas.AddNode(dialogueNode);
        }
    }
}