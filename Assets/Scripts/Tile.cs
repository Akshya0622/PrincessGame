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

