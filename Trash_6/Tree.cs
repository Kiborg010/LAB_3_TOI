using System;
using System.Collections.Generic;

public class Node
{
    public string Data { get; set; }
    public List<Node> Children { get; set; }

    public Node(string data)
    {
        Data = data;
        Children = new List<Node>();
    }

    public void AddChild(Node child)
    {
        Children.Add(child);
    }
}

public class Tree
{
    public Node Root { get; set; }

    public Tree(string rootData)
    {
        Root = new Node(rootData);
    }
}

