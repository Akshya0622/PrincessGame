using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell : MonoBehaviour
{
    public bool tilePlaced;
    public Tile[] tileOptions;

    public void NewCell(bool currentState, Tile[] currentOptions)
    {
        tilePlaced = currentState;
        tileOptions = currentOptions;
    }
}
