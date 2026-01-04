// See https://aka.ms/new-console-template for more information

using System.ComponentModel;
using PSWGNetworkTesting.Nodes;

public static class Program {
    public static void Main(string[] args) {
        AdminTerminal root = new AdminTerminal();
        
        root.AddChild(new DataCache());
        root.AddChild(new Door());
        
        PrintTree(root);
    }
    
    public static void PrintTree(NetworkNode root) {
        PrintInternal(root, "", true);
    }
    
    private static void PrintInternal(
        NetworkNode node,
        string indent,
        bool last)
    {
        string marker = last ? "└─ " : "├─ ";
        Console.WriteLine(indent + marker + node);
        
        indent += last ? "   " : "│  ";
        
        var children = node.ChildNodes;
        for (int i = 0; i < children.Count; i++) {
            bool isLast = (i == children.Count - 1);
            PrintInternal(children[i], indent, isLast);
        }
    }
}