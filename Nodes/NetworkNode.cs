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

namespace PSWGNetworkTesting.Nodes;

public abstract class NetworkNode {
    /// <summary>
    /// How hard it is to get into the network at this node.
    /// </summary>
    public uint ExternalStrength { get; init; }
    
    /// <summary>
    /// How hard it is to get into this node from inside the network.
    /// </summary>
    public uint InternalStrength { get; init; }
    
    /// <summary>
    /// The currently equiped security chip.
    /// </summary>
    public SecurityChip? Chip = null;
    
    public NetworkNode? ParentNode { get; private set; }
    
    
    private void SetParent(NetworkNode parent){
        if(ParentNode != null) {
            throw new InvalidOperationException("You may not set a parent if it alread exits");
        }
    }
    
    private void ClearParent() {
        if(ParentNode != null){
            ParentNode._childNodes.Remove(this);
            ParentNode = null;
        }
    }
    
    private readonly List<NetworkNode> _childNodes = new List<NetworkNode>();

    public IReadOnlyList<NetworkNode> ChildNodes => _childNodes;
    
    public void AddChild(NetworkNode node) {
        if (node == this) {
            throw new ArgumentException("You may not be a child of yourself.");
        }

        if (Contains(node)) {
            throw new ArgumentException("a node may not be a child twice.");
        }

        if(node.ParentNode != null) {
            throw new ArgumentException("target node is already a child of another node.");
        }
        
        node.SetParent(this);
        
        _childNodes.Add(node);
    }
    
    /// <summary>
    /// Gets wether a node is a child of this node.
    /// <br/> recursively checks child nodes.
    /// </summary>
    /// <param name="node">Node to look for.</param>
    /// <returns></returns>
    public bool Contains(NetworkNode node){
        if (_childNodes.Contains(node)) {
            return true;
        }
        
        foreach (NetworkNode child in _childNodes) {
            if (child.Contains(node)) {
                return true;
            }
        }
        
        return false;
    }
    
    /// <summary>
    /// How much power this node costs to run.
    /// </summary>
    public abstract uint PowerCost { get; protected init; }
    
    /// <summary>
    /// Calculates how much power this node and its children cost.
    /// </summary>
    /// <returns>number of power points it costs.</returns>
    public uint TotalPowerCost() {
        uint output = PowerCost;
        foreach (NetworkNode child in ChildNodes) {
            output += child.TotalPowerCost();
        }
        
        return output;
    }


}