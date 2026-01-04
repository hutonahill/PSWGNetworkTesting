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
            throw new InvalidOperationException("You may not set a parent if it already exits");
        }
        
        ParentNode = parent;
    }
    
    private void ClearParent() {
        if(ParentNode != null){
            ParentNode._childNodes.Remove(this);
            ParentNode = null;
        }
    }
    
    private readonly List<NetworkNode> _childNodes = new List<NetworkNode>();

    public IReadOnlyList<NetworkNode> ChildNodes => _childNodes;
    
    // TODO: add RemoveChild method. needs to be virtual and AdminTerminal needs an override.
    
    public virtual void AddChild(NetworkNode node) {
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
        
        FindRoot().FindDataCaches();
    }
    
    /// <summary>
    /// Gets weather a node is a child of this node.
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
    
    public bool Powered { get; private set; } = false;
    
    // These are separate methods so we can attach things to them later if we need to.
    public void Power() {
        Powered = true;
    }
    
    public void CutPower() {
        Powered = false;
    }
    
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
    
    /// <summary>
    /// Maximum number of jumps to get to a DataCache node.
    /// <br/> if null node does not care.
    /// </summary>
    public abstract uint? MaximumJumpsToDataCache { get; protected init; }
    
    private uint _stepsToDataCache = uint.MaxValue;
    
    /// <summary>
    /// Sets the _stepsToDataCache values for all child nodes to maximum value.
    /// </summary>
    private void ClearDataCacheSteps() {
        
        _stepsToDataCache = uint.MaxValue;
        
        foreach (NetworkNode child in _childNodes) {
            child.ClearDataCacheSteps();
        }
        
    }
    
    public void FindDataCaches() {
        if (ParentNode == null) {
            ClearDataCacheSteps();
        }
        
        // point of the next two blocks is to only call SetDataCacheDistance on DataCaches.
        foreach (NetworkNode child in _childNodes) {
            child.FindDataCaches();
        }
        
        if (GetType().IsAssignableTo(typeof(DataCache))) {
            SetDataCacheDistance(0);
        }
    }
    
    private void SetDataCacheDistance(uint value) {
        if (value == uint.MaxValue) {
            throw new InvalidOperationException("May not exceed uint maximum value.");
        }
        
        // only accept an update if the value is closer.
        if (_stepsToDataCache <= value) {
            return;
        }
        
        _stepsToDataCache = value;
        
        uint newValue = value + 1;
        
        if (ParentNode != null) {
            ParentNode.SetDataCacheDistance(newValue);
        }
        
        foreach (NetworkNode child in _childNodes) {
            child.SetDataCacheDistance(newValue);
        }
    }
    
    private NetworkNode FindRoot() {
        NetworkNode current = this;
        
        while (current.ParentNode != null) {
            current = current.ParentNode;
        }
        
        return current;
    }
    
    public override string ToString() {
        return $"{GetType().Name} {{ DataCache Distance: {_stepsToDataCache}, Powered: {Powered}}}";
    }
}