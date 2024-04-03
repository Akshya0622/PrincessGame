using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class mapGenerator : MonoBehaviour
{
    public int sizeX = 50;
    public int sizeY = 50;
    public Tile[] tilePrefabs;
    public void Start()
    {
        makeGrid();
    }
    public void makeGrid()
    {
        GameObject[,] world = new GameObject[sizeX, sizeY];
        Vector2Int startPos = new Vector2Int(sizeX / 2, sizeY / 2);
        generateTiles(world, startPos.x, startPos.y);
    }
    void generateTiles(GameObject[,] world, int x, int y)
    {
        if (x < 0 || x >= sizeX || y < 0 || y >= sizeY)
        {
            return;
        }
        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(0, 1),
            new Vector2Int(0,-1),
            new Vector2Int(-1,0),
            new Vector2Int(1,0)
        };
        Shuffle(directions);

        foreach (Vector2Int direction in directions)
        {
            int newX = x + direction.x;
            int newY = y + direction.y;

            if (newX >= 0 && newX < sizeX && newY >= 0 && newY < sizeY && world[newX, newY] == null)
            {
                GameObject[] possibleNeighbors = getPossibleNeighbors(world, newX, newY, direction);
                possibleNeighbors = weightedSelect(possibleNeighbors);

                foreach (GameObject neighbor in possibleNeighbors)
                {
                    if (canBePlaced(world, newX, newY, neighbor))
                    {
                        world[newX, newY] = neighbor;
                        generateTiles(world, newX, newY);
                        break;
                    }
                }
            }
        }

        bool canBePlaced(GameObject[,] world, int x, int y, GameObject tile)
        {
            if (x < 0 || x >= sizeX || y < 0 || y >= sizeY || world[x, y] != null)
            {
                return false;
            }

            Tile tileSc = tilePrefabs.GetComponent<Tile>();

            bool canConnectUp = canConnectToNeigh(world, x, y + 1, tileSc.downNeighbors);
            bool canConnectDown = canConnectToNeigh(world, x, y - 1, tileSc.upNeighbors);
            bool canConnectLeft = canConnectToNeigh(world, x-1, y, tileSc.rightNeighbors);
            bool canConnectRight = canConnectToNeigh(world, x+1, y, tileSc.leftNeighbors);

            if(!canConnectUp || !canConnectDown || !canConnectLeft || !canConnectRight)
            {
                return false;
            }
            return true;
        }
        bool canConnectToNeigh(GameObject[,] world, int x, int y, GameObject[] possibleNeighbors)
        {
            if( possibleNeighbors == null || possibleNeighbors.Length == 0 )
            {
                return true;
            }
            if(x>= 0 && x<sizeX && y>= 0 && y<sizeY)
            {
                foreach(GameObject neighborPrefab in possibleNeighbors)
                {
                    if (world[x,y] != null && world[x,y].gameObject == neighborPrefab)
                    {
                        return true;
                    }
                }
            }
            return false;
        }





    }
}