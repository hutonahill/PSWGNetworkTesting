// See https://aka.ms/new-console-template for more information

using PSWGNetworkTesting.Nodes;

namespace PSWGNetworkTesting;

public static class Program {
    public static void Main(string[] args) {
        AdminTerminal root = new AdminTerminal();
        
        root.AddChild(new DataCache());
        
        NetworkNode door = new Door();
        
        door.AddChild(new SecuredChest{ Contents = new string("why hello there.")});
        
        NetworkNode power = new PowerNode {Producing = 16};
        
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
    
    public static string ToColor(this string target, ConsoleColor color){
        string code = color switch {
            ConsoleColor.Black => "30",
            ConsoleColor.DarkRed => "31",
            ConsoleColor.DarkGreen => "32",
            ConsoleColor.DarkYellow => "33",
            ConsoleColor.DarkBlue => "34",
            ConsoleColor.DarkMagenta => "35",
            ConsoleColor.DarkCyan => "36",
            ConsoleColor.Gray => "37",
            
            ConsoleColor.DarkGray => "90",
            ConsoleColor.Red => "91",
            ConsoleColor.Green => "92",
            ConsoleColor.Yellow => "93",
            ConsoleColor.Blue => "94",
            ConsoleColor.Magenta => "95",
            ConsoleColor.Cyan => "96",
            ConsoleColor.White => "97",
            
            _ => "0"
        };
        
        return $"\u001b[{code}m{target}\u001b[0m";
    }
}