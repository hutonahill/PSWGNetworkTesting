using System;
using System.Collections.Generic;


namespace PSWGNetworkTesting;

public class Commands
{
    public void AskForCommand()
    {
        Dictionary<string, string> commandList = new Dictionary<string, string>();

        // prefix: admin 
        commandList.Add("log", "ADMIN_LOG");

        // prefix: simulate
        commandList.Add("blockBreak", "SIM_BLOCK_BREAK");

        commandList.Add("dataTerminal", "SIM_DATA_TERMINAL_BREACH");
        // EXTRA COMMAND, door has multiple commands with multiple prefixes, simulate | add | remove
        //
        // simulate door open/close 
        //
        
        // prefix: add | remove
            // IO DEVICES
        commandList.Add("door", "DOOR");
        commandList.Add("chest", "CHEST");

            // NETWORK
        commandList.Add("adminTerminal", "ADMIN_TERMINAL");
        commandList.Add("dataCache", "DATA_CACHE");
        commandList.Add("powerNode", "POWER_NODE");
        
        Console.WriteLine();

        // Dictionary<string, string>.KeyCollection keyColl = commandList.Keys;
        //
        // foreach (string s in keyColl)
        // {
        //     Console.WriteLine("Key = {0}", s);
        // }
        //
        // Dictionary<string, string>.ValueCollection valueColl = commandList.Values;
        //
        // Console.WriteLine();
        //
        // foreach (string s in valueColl)
        // {
        //     Console.WriteLine("Value = {0}", s);
        // }
        string[] parts;

        string prefix;

        string objectType;

        string? optionalArg1 = null; 

        string? optionalArg2 = null; 

        int id;

        while (true)
        {
            Console.WriteLine();

            Console.WriteLine("Enter Command: ");
            
            string? input = Console.ReadLine();

            input = input.Trim();
            
            if (input == "exit")
            {
                break;
            }
            
            parts = input.Split(' ');
            prefix = parts[0];
            objectType = parts[1];

            //id = int.Parse(parts.Last()); 
            if (parts.Length >= 3)
            {
                optionalArg1 = parts[2];
            } 
            
            if (parts.Length >= 4)
            {
                optionalArg2 = parts[3];
            }
            
            {
                if (prefix == "add" && parts.Length == 2)
                {
                    if (commandList.ContainsKey(objectType))
                    {
                        string value;

                        value = commandList[objectType];

                        // IO DEVICES
                        if (value == "DOOR")
                        {
                            Console.WriteLine("Added Door");
                        }

                        if (value == "CHEST")
                        {
                            Console.WriteLine("Added Chest");
                        }

                        // NETWORK DEVICES

                        if (value == "ADMIN_TERMINAL")
                        {
                            Console.WriteLine("Added AdminTerminal");
                        }

                        if (value == "DATA_CACHE")
                        {
                            Console.WriteLine("Added DataCache");
                        }

                        if (value == "POWER_NODE")
                        {
                            Console.WriteLine("Added PowerNode");
                        }
                    }
                }
                else if (prefix == "remove" && parts.Length == 2)
                {
                    if (commandList.ContainsKey(objectType))
                    {
                        string value;

                        value = commandList[objectType];

                        // IO DEVICES
                        if (value == "DOOR")
                        {
                            Console.WriteLine("Removed Door");
                        }

                        if (value == "CHEST")
                        {
                            Console.WriteLine("Removed Chest");
                        }

                        // NETWORK DEVICES

                        if (value == "ADMIN_TERMINAL")
                        {
                            Console.WriteLine("Removed AdminTerminal");
                        }

                        if (value == "DATA_CACHE")
                        {
                            Console.WriteLine("Removed DataCache");
                        }

                        if (value == "POWER_NODE")
                        {
                            Console.WriteLine("Removed PowerNode");
                        }
                    }
                }
                else if (prefix == "clear")
                {
                    if (commandList.ContainsKey(objectType))
                    {
                        if (parts.Length >= 2 && parts.Last() == "all")
                        {
                            Console.WriteLine("Cleared all Entries");
                        }
                    }

                    Console.WriteLine("Cleared All");
                }
                else if (prefix == "admin" && parts.Length == 2)
                {
                    if (commandList.ContainsKey(objectType))
                    {
                        string value;

                        value = commandList[objectType];

                        // IO DEVICES
                        if (value == "ADMIN_LOG")
                        {
                            Console.WriteLine("Admin Log Command");
                        }
                    }
                }
                else if (prefix == "simulate")
                {
                    if (commandList.ContainsKey(objectType))
                    {
                        string value;
                        value = commandList[objectType];

                        // IO DEVICES
                        if (value == "SIM_BLOCK_BREAK")
                        {
                            Console.WriteLine("Block has been broken!");
                        }
                        if (value == "SIM_DATA_TERMINAL_BREACH")
                        {
                            Console.WriteLine("Data Terminal has been breached!");
                        }
                        if (value == "DOOR" && optionalArg1 == "open" && parts.Length == 3)
                        {
                            Console.WriteLine("Door has been opened!");
                        }
                        if (value == "DOOR" && optionalArg1 == "close" && parts.Length == 3)
                        {
                            Console.WriteLine("Door has been closed!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("⢰⣶⣤⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠀\n" +
                                      "⠀⣿⣿⣿⣷⣤⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣤⣶⣾⣿⠀\n" +
                                      "⠀⠘⢿⣿⣿⣿⣿⣦⣀⣀⣀⣄⣀⣀⣠⣀⣤⣶⣿⣿⣿⣿⣿⠇⠀\n" +
                                      "⠀⠀⠈⠻⣿⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠋⠀⠀\n" +
                                      "⠀⠀⠀⠀⣰⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣟⠋⠀⠀⠀⠀\n" +
                                      "⠀⠀⠀⢠⣿⣿⡏⠆⢹⣿⣿⣿⣿⣿⣿⠒⠈⣿⣿⣿⣇⠀⠀⠀⠀\n" +
                                      "⠀⠀⠀⣼⣿⣿⣷⣶⣿⣿⣛⣻⣿⣿⣿⣶⣾⣿⣿⣿⣿⡀⠀⠀⠀\n" +
                                      "⠀⠀⠀⡁⠀⠈⣿⣿⣿⣿⢟⣛⡻⣿⣿⣿⣟⠀⠀⠈⣿⡇⠀⠀⠀\n" +
                                      "⠀⠀⠀⢿⣶⣿⣿⣿⣿⣿⡻⣿⡿⣿⣿⣿⣿⣶⣶⣾⣿⣿⠀⠀⠀\n" +
                                      "⠀⠀⠀⠘⣿⣿⣿⣿⣿⣿⣿⣷⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡆⠀⠀\n" +
                                      "⠀⠀⠀⠀⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠀⠀");
                    throw new Exception($"Unknown Command: {input}");
                    
                }
            }
        }
    }
}

