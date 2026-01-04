// See https://aka.ms/new-console-template for more information

using PSWGNetworkTesting.Nodes;

namespace PSWGNetworkTesting;

public static class Program {
    public static void Main(string[] args) {
        AdminTerminal root = new AdminTerminal();
        
        root.AddChild(new DataCache());
        
        NetworkNode door = new Door();
        
        door.AddChild(new SecuredChest{ Contents = new string("why hello there.")});
        
        NetworkNode power = new PowerNode();
        
        power.AddChild(new DataCache());
        
        door.AddChild(power);
        
        root.AddChild(door);
        
        PrintTree(root);
    }
    
    public static void PrintTree(NetworkNode root){
        Console.WriteLine(root);
        
        var children = root.ChildNodes;
        if(children.Count > 0){
            Console.WriteLine("│");
        }
        
        for(int i = 0; i < children.Count; i++){
            bool isLast = (i == children.Count - 1);
            PrintInternal(children[i], "", isLast);
            
            if(!isLast){
                Console.WriteLine("│");
            }
        }
    }
    
    private static void PrintInternal(NetworkNode node, string indent, bool last){
        string marker = last ? "└─ " : "├─ ";
        Console.WriteLine(indent + marker + node);
        
        string childIndent = indent + (last ? "   " : "│  ");
        
        var children = node.ChildNodes;
        if(children.Count > 0){
            Console.WriteLine(childIndent + "│"); // <-- keep indentation
        }
        
        for(int i = 0; i < children.Count; i++){
            bool isLastChild = (i == children.Count - 1);
            PrintInternal(children[i], childIndent, isLastChild);
            
            if(!isLastChild){
                Console.WriteLine(childIndent + "│"); // <-- keep indentation
            }
        }
    }

}