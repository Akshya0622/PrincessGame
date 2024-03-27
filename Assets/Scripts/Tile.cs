using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Tile[] upNeighbors;
    public Tile[] downNeighbors;
    public Tile[] rightNeighbors;
    public Tile[] leftNeighbors;
    public Tile[] possibilities;
    public Dictionary <int, Tile> MyNeighbors;

    public void Start()
    {
        int entropy = possibilities.Length;
    }
    public void addNeighbors(int direction, Tile neigh)
    {
        MyNeighbors[direction] = neigh;
    }
    public Tile getNeighbors(int direction)
    {
        return MyNeighbors[direction];
    }
}
