using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNode
{
    // Position of the node in the world
    private Vector3 v3_Position;
    
    // Where the node should take the character
    private MovementNode mn_WhereToGo;
    // Where the character came from before this node
    private MovementNode mn_Parent;

    // A* Stuff
    // G comes from node plus 1. First is 0
    private int i_G;
    // Heuristic - estimated distance from the current node to the end node
    private int i_H;
    // Total cost of the node (H + G)
    private int i_F;

    /// <summary>
    /// Constructs a MovementNode.
    /// </summary>
    /// <param name="_pos_">Position of the node.</param>
    public MovementNode(Vector3 _pos_)
    {
        v3_Position = _pos_;
        mn_WhereToGo = null;
        mn_Parent = null;
        i_G = 0;
        i_H = 0;
        i_F = 0;
    }

    // Getters
    public MovementNode GetWhereToGo() { return mn_WhereToGo; }
    public MovementNode GetParent() { return mn_Parent; }
    public int GetG() { return i_G; }
    public int GetH() { return i_H; }
    public int GetF() { return i_F; }

    // Setters
    public void SetWhereToGo(MovementNode _go_) { mn_WhereToGo = _go_; }
    public void SetParent(MovementNode _parent_) { mn_Parent = _parent_; }
    public void SetG(int _g_) { i_G = _g_; }
    public void SetH(int _h_) { i_H = _h_; }

    /// <summary>
    /// Adds G and H and sets it to F.
    /// </summary>
    public void CalculateF() { i_F = i_G + i_H; }
}
