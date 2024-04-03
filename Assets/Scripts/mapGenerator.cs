using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
        Tile[,] world = new Tile[sizeX, sizeY];
        Vector2Int startPos = new Vector2Int(sizeX / 2, sizeY / 2);
        generateTiles(world, startPos.x, startPos.y);
    }
    void generateTiles(Tile[,] world, int x, int y)
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

        
        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(i, 4);
            Vector2Int temp = directions[i];
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }

        foreach (Vector2Int direction in directions)
        {
            int newX = x + direction.x;
            int newY = y + direction.y;

            if (newX >= 0 && newX < sizeX && newY >= 0 && newY < sizeY && world[newX, newY] == null)
            {
                Tile[] possibleNeighbors = getPossibleNeighbors(world, newX, newY, direction); // access from tile
                possibleNeighbors = weightedSelect(possibleNeighbors); // u need to makesomething to remake the array with weights

                foreach (Tile neighbor in possibleNeighbors)
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

        bool canBePlaced(Tile[,] world, int x, int y, Tile tile)
        {
            if (x < 0 || x >= sizeX || y < 0 || y >= sizeY || world[x, y] != null)
            {
                return false;
            }

            

            bool canConnectUp = canConnectToNeigh(world, x, y + 1, tile.downNeighbors);
            bool canConnectDown = canConnectToNeigh(world, x, y - 1, tile.upNeighbors);
            bool canConnectLeft = canConnectToNeigh(world, x-1, y, tile.rightNeighbors);
            bool canConnectRight = canConnectToNeigh(world, x+1, y, tile.leftNeighbors);

            if(!canConnectUp || !canConnectDown || !canConnectLeft || !canConnectRight)
            {
                return false;
            }
            return true;
        }
        bool canConnectToNeigh(Tile[,] world, int x, int y, Tile[] possibleNeighbors)
        {
            if( possibleNeighbors == null || possibleNeighbors.Length == 0 )
            {
                return true;
            }
            if(x>= 0 && x<sizeX && y>= 0 && y<sizeY)
            {
                foreach(Tile neighborPrefab in possibleNeighbors)
                {
                    if (world[x,y] != null && world[x,y].gameObject == neighborPrefab)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        List<Tile> getPossibleNeighbors(Tile [,] world, int x, int y, Vector2Int direction)
        {
            Tile t = world[x, y];
        }





    }
}