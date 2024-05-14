using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System;
using Random = System.Random;
using System.Linq;

public class Tile : MonoBehaviour
{
    public Tile[] upNeighbors;
    public Tile[] downNeighbors;
    public Tile[] rightNeighbors;
    public Tile[] leftNeighbors;
    public int number;
}

/* public List<Tile> possibilities;
    public Dictionary <int, Tile> MyNeighbors; // like actual placed tiles not like oh this tile could work as a neighbor
    Dictionary<Tile, double> tileWeights;
    int entropy;
    bool reduced;
    public void Start()
    {
       entropy = possibilities.Count; // number of possibilities for current tile
    }
    public void addNeighbors(int direction, Tile neigh)
    {
        MyNeighbors[direction] = neigh; 
    }
    public Tile getNeighbors(int direction)
    {
        return MyNeighbors[direction];
    }
    public List<int> getDirections()
    {
        List<int> dirWneighbor = new List<int>(MyNeighbors.Keys);
        return dirWneighbor; 
    }
    public List<Tile> getPossibilities() {
        return possibilities;
    }
    public void collapse()
    {
        Random r = new Random();
        double ran = r.NextDouble();
        double cumulativeWeight = 0;
        List<Tile> actualTile = tileWeights.Keys.ToList();
        List<double> weights = tileWeights.Values.ToList();
        for (int i = 0; i < tileWeights.Count; i++)
        {
            cumulativeWeight += weights[i];
            if(ran <  cumulativeWeight && possibilities.Contains(actualTile[i]))
            {
                possibilities = new List<Tile>();
                possibilities.Add(actualTile[i]);
            }
        }
        entropy = 0;
    }
    public void constrain()
    {
        reduced = false;
    }
}
*/