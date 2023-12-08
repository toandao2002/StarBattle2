using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node
{
    public int val;
    public List<Node> nodeChilds;
    public int color;
    public bool visitted;
    public Node(int val)
    {
        this.val = val;
        nodeChilds = new List<Node>();
        color = 0;
        visitted = false;
    }
    public void AddNodeChild(Node node)
    {
        nodeChilds.Add(node);
    }
    public bool CheckNodeChildHasColor(int color)
    {
        for(int i = 0;i<nodeChilds.Count; i++)
        {
            if(nodeChilds[i].color == color)
            {
                return true;
            }
        }
        return false;   
    }
    public static Node GetNodeByVal(List<Node> nodes, int val)
    {
        Node node = null;
        if (nodes != null)
            for (int i = 0; i < nodes.Count; i++)
            {
                if(nodes[i].val == val)
                {
                    node = nodes[i];
                    break;
                }
            }
        if(node == null)
        {
            node = new Node(val);
            nodes.Add(node);
        }
        return node;
    }
    public override string ToString()
    {
        return "val:"+ val +"; " +"color: " + color+ "; " ;
    }
}
public class ColorGraph 
{
    List<Node> nodes; 
    public void DrawColor(List<Node> nodes)
    {
        nodes.Sort((a, b) => a.nodeChilds.Count.CompareTo(b.nodeChilds.Count));
        int color = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            Node nodeCur = nodes[i];
            if (nodeCur.visitted) continue;
            color += 1;
            for (int j = i; j< nodes.Count; j ++)
            {
                if (!nodes[j].visitted && !nodeCur.nodeChilds.Contains(nodes[j]) && !nodes[j].CheckNodeChildHasColor(color))
                {
                    nodes[j].color = color;
                    nodes[j].visitted = true;
                }
               

            }
           
        }
    }
    public void Draw(Node node, int color)
    {
        foreach(Node i in node.nodeChilds)
        {
            if (!i.visitted && !i.CheckNodeChildHasColor(color))
            {
                i.color = color;
                i.visitted = true;
            }
        }
    }
    

}
