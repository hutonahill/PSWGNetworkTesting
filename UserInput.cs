using System;
using System.Collections.Generic;


namespace PSWGNetworkTesting;

    public class Commands
    {
        public void AskForCommand()
        {
            Dictionary<string, string> commandList = new Dictionary<string, string>();
            
            commandList.Add("admin log", "ADMIN_LOG");
            
            commandList.Add("simulate blockBreak", "SIM_BLOCK_BREAK");
            
            commandList.Add("simulate dataTerminal hack", "SIM_DATA_TERMINAL_BREACH");
            
            commandList.Add("simulate dataTerminal door open", "SIM_DATA_TERMINAL_OPEN");
            commandList.Add("simulate dataTerminal door close", "SIM_DATA_TERMINAL_CLOSE");
            
            Console.WriteLine();
            
            Dictionary<string, string>.KeyCollection keyColl = commandList.Keys;
            
            foreach ( string s in keyColl )
            {
                Console.WriteLine("Key = {0}", s);
            }
            
            Dictionary<string, string>.ValueCollection valueColl = commandList.Values;
            
            Console.WriteLine();
            
            foreach ( string s in valueColl )
            {
                Console.WriteLine("Value = {0}", s);
            }
            
            Console.WriteLine();
            
            Console.WriteLine("Enter Command: ");
            
            string? input = Console.ReadLine();

            input = input.Trim();
            
            if (commandList.ContainsKey(input))
            {
                string value;
                
                value = commandList[input];

                if (value == "ADMIN_LOG")
                {
                    Console.WriteLine("Admin Log Command");
                } else if (value == "SIM_BLOCK_BREAK") 
                {
                    Console.WriteLine("Block Break Command");
                }
            }
        }
    }