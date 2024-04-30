using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class mapGenerator : MonoBehaviour
{
    public int sizeX;
    public int sizeY;
    public Tile[] tilePrefabs;
    Dictionary<Tile, int> tileWeightss; //weights of each tile so we can have certain tiles being placed more often
    public int[] weightValues;
    List<Tile> placedTiles = new List<Tile>();
    Tile[,] world;
    List<Vector2Int> prevLocation = new List<Vector2Int>();
    bool back;
    public void Start()
    {
        tileWeightss = populateTileWeights();
        makeGrid();
        drawMap();

    }
    Dictionary<Tile,int> populateTileWeights() // create a dictionary from the list of weights and tile prefabs
    {
        Dictionary<Tile, int> tileWeights = new Dictionary<Tile, int>();
        for (int i = 0; i< tilePrefabs.Length; i++)
        {
            tileWeights.Add(tilePrefabs[i], weightValues[i]);
        }
        return tileWeights;
    }
    public void makeGrid()
    {
        world = new Tile[sizeX, sizeY]; // make world 2d array
        
        Tile startTile = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
        world[0,0] = startTile; // random start tile placed
        placedTiles.Add(startTile);
        generateTiles(world, 0, 0, startTile);
        
    }
    
   bool generateTiles(Tile[,] world, int x, int y, Tile tilePrfb)
    {
       for(int i = 0; i < sizeX; i++)
        
  
    }
    void drawMap()
    {
        for(int i = 0; i < sizeY; i++)
        {
            for( int j = 0; j < sizeX; j++)
            {
                Instantiate(world[i, j].gameObject, new Vector3(i, j, 0), Quaternion.identity);
            }
        }
        Debug.Log("placed tiles: "+placedTiles.Count);
    }

        bool canBePlaced(Tile[,] world, int x, int y, Tile tile)
        {
            if (x < 0 || x >= sizeX || y < 0 || y >= sizeY || world[x, y] != null)
            {
                Debug.Log("Cannot place tile at: " + x + ", " + y + ". Out of bounds or tile already exists.");
                return false;
            }

            

            bool canConnectUp = canConnectToNeigh(world, x, y + 1, tile.upNeighbors);
            if (canConnectUp == false)
            {
                Debug.Log("tried but couldnt 1");
                return false;

            }
            bool canConnectDown = canConnectToNeigh(world, x, y - 1, tile.downNeighbors);
            if (canConnectDown == false)
            {
                Debug.Log("tried but couldnt 2");
                return false;
            }
            bool canConnectLeft = canConnectToNeigh(world, x-1, y, tile.leftNeighbors);
            if (canConnectLeft == false)
            {
                Debug.Log("tried but couldnt 3");
                return false;
            }
            bool canConnectRight = canConnectToNeigh(world, x + 1, y, tile.rightNeighbors);
            if(canConnectRight == false)
            {
                Debug.Log("tried but couldnt 4");
                return false;
            }
            else
            {
                return true;
            }
        

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
                    if (world[x,y] == null ||  world[x,y] == neighborPrefab)
                    {
                        return true;
                        
                    }
                }
                return false;
            }
            return true;
        }

        Tile[]getPossibleNeighbors(Tile [,] world, int x, int y, Vector2Int direction)
        {
            Tile t = world[x, y];
            if(t == null)
            {
                return new Tile[0];
               

            }
           
            if(direction == new Vector2Int(0,1))
            {
                return t.upNeighbors;
            }
           else if (direction == new Vector2Int(0, -1))
            {
                return t.downNeighbors;
            }
            else if (direction == new Vector2Int(-1, 0))
            {
                return t.leftNeighbors;
            }
            else if (direction == new Vector2Int(1, 0))
            {
                return t.rightNeighbors;
            }
            else 
            {
                return new Tile[0];
            }

        }
        Tile[] weightedSelect(Tile[] neighbors)
        {
           List<Tile> weightedSelect = new List<Tile>();
            
           foreach(Tile neighbor in neighbors)
            {
                int weight;
                if(tileWeightss.TryGetValue(neighbor, out weight))
                {
                    for(int i = 0; i < weight; i++)
                    {
                        weightedSelect.Add(neighbor);
                    }
                }
            }
            Tile[] shuffle = weightedSelect.ToArray();
            for (int i = 0; i < shuffle.Length; i++)
            {
                int randomIndex = Random.Range(i, shuffle.Length);
                Tile temp = shuffle[i];
                shuffle[i] = shuffle[randomIndex];
                shuffle[randomIndex] = temp;
            }
            return shuffle;

        } 





    
}