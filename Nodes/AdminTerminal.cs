// -----------------------------------------------------------------------------
// Project: PSWG Network Testing
// Copyright (c) 2026
// Author: Evan RIker
// GitHub Account: hutonahill
// Email: evan.k.riker@gmail.com
// 
// License: GNU General Public License
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License version 3 as published by
// the Free Software Foundation.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// -----------------------------------------------------------------------------

using System.Diagnostics;

namespace PSWGNetworkTesting.Nodes;

public class AdminTerminal : NetworkNode {
    public AdminTerminal() {
        AddedParent += _ => isRoot = false;
    }
    
    public override bool IsKeyNode { get; } = true;
    
    public override uint PowerCost { get; protected init; } = 3;
    
    
    public override uint? MaximumJumpsToDataCache { get; protected init; } = 1;
    
    
    public bool isRoot = true;
    
    /// <summary>
    /// The total amount of power available to the network.
    /// </summary>
    public uint NetworkPowerSupply { get; private set; }
    
    /// <summary>
    /// The total amount of unallocated power available to the network.
    /// </summary>
    public uint NetworkPowerAvailable { get; private set; }
    
    public void AddPower(uint amount) {
        if (!isRoot) {
            throw new InvalidOperationException($"You may only call {nameof(AddPower)} on the root node.");
        }
        
        NetworkPowerSupply += amount;
        
        List<NetworkNode> needsPower = new ();
        
        // If its likely we need new power, and we are getting new power, Find all nodes that need power.
        if (NetworkPowerAvailable == 0 && amount > 0) {
            Queue<NetworkNode> queue = new ();
            queue.Enqueue(this);
            
            while (queue.Count > 0){
                NetworkNode current = queue.Dequeue();
                
                // check the next node.
                if (!current.Powered){
                    needsPower.Add(current);
                }
                
                // add children to the queue.
                foreach (NetworkNode child in current.ChildNodes){
                    queue.Enqueue(child);
                }
            }
        }
        
        NetworkPowerAvailable += amount;
        
        // for each node that needs power, try to allocate power.
        foreach (NetworkNode node in needsPower) {
            Debug.Assert(node.Powered == false);
            if (node.PowerCost <= NetworkPowerAvailable) {
                node.Power();
                
                if (NetworkPowerAvailable == 0) {
                    break;
                }
            }
        }
    }
    
    public void RemovePower(uint amount) {
        if (!isRoot) {
            throw new InvalidOperationException($"You may only call {nameof(RemovePower)} on the root node.");
        }
        
        if (NetworkPowerSupply < amount) {
            throw new InvalidOperationException("You may not reduce power my more than the supply.");
        }
        
        NetworkPowerSupply -= amount;
        
        if (NetworkPowerAvailable >= amount) {
            NetworkPowerAvailable -= amount;
        }
        
        // We need to reclaim power from powered nodes.
        uint deficit = amount - NetworkPowerAvailable;
        NetworkPowerAvailable = 0;
        
        List<(NetworkNode Node, int Depth)> poweredNodes = GetPoweredNodesByDepthDescending();
        
        for (int i = 0; i < poweredNodes.Count && deficit > 0; i++){
            NetworkNode node = poweredNodes[i].Node;
            
            // don't do anything if the node isn't powered.
            if (node.Powered){
                uint reclaimed = node.PowerCost;
                
                node.CutPower();
                
                if (reclaimed >= deficit) {
                    // if we reclaim more power than we need to meet the deficit, we return it to the network.
                    NetworkPowerAvailable += reclaimed - deficit;
                        
                    deficit = 0;
                }
                else{
                    deficit -= reclaimed;
                }
            }
        }
        
        if (deficit > 0){
            // This *should* be impossible as we are checking that the amount we remove is greater than the amount we supply.
            throw new InvalidOperationException("Insufficient powered load to cut to satisfy the requested power reduction.");
        }
    }
    
    private List<(NetworkNode Node, int Depth)> GetPoweredNodesByDepthDescending(){
        List<(NetworkNode Node, int Depth)> output = new ();
        Queue<(NetworkNode Node, int Depth)> queue = new ();
        
        queue.Enqueue((this, 0));
        
        while (queue.Count > 0){
            (NetworkNode Node, int Depth) current = queue.Dequeue();
            
            if (current.Node.Powered){
                output.Add(current);
            }
            
            IReadOnlyList<NetworkNode> children = current.Node.ChildNodes;
            foreach (NetworkNode t in children) {
                queue.Enqueue((t, current.Depth + 1));
            }
        }
        
        
        return output.OrderByDescending(x => x.Depth)      // farthest from root first
           .ThenBy(x => x.Node.IsKeyNode)         // non-key first, key last
           .ToList();
    }
    
    private List<PowerNode> _powerNodes = new ();
    
    private List<DataCache> _dataNodes = new ();
    
    public override void AddChild(NetworkNode node) {
        if (node is PowerNode powerNode) {
            _powerNodes.Add(powerNode);
            
            
            
        }
        else if (node is DataCache dataNode) {
            _dataNodes.Add(dataNode);
        }
        
        base.AddChild(node);
    }
    
    public void RecalculatePower() {
        
    }
    
    public void PullPower(NetworkNode node) {
        
    }
}