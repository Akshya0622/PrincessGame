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
        world[0,0] = startTile; // 
        placedTiles.Add(startTile);
        generateTiles(world);
        
    }

    bool generateTiles(Tile[,] world)
    {
        Vector2Int emptyPosition = FindEmpty(world);

        // map done
        if (emptyPosition.x == -1 && emptyPosition.y == -1)
        {
          
            
            return true;
        }

        int row = emptyPosition.x;
        int col = emptyPosition.y;
        Tile[] weightedTiles = weightedSelect(tilePrefabs);
       
        foreach (Tile tilePrefab in weightedTiles)
        {
           
            if (canBePlaced(world, row, col, tilePrefab))
            {
               
                world[row, col] = tilePrefab;
               

               
                if (generateTiles(world))
                {
                    return true;
                }
                   

                
                world[row, col] = null;
                
            }
        }

       
        return false;
    }
    Vector2Int FindEmpty(Tile[,] world)
    {
        for(int row = 0; row < sizeY; row++)
        {
            for(int col = 0; col < sizeX; col++)
            {
                if (world[row,col] == null)
                {
                    return new Vector2Int(row,col);
                }
            }
        }

        
        return new Vector2Int(-1,-1);
        
    }
    void drawMap()
    {
        for(int row = 0; row < sizeY; row++)
        {
            for( int col = 0; col < sizeX; col++)
            {
                Instantiate(world[row, col].gameObject, new Vector3(row, col, 0), Quaternion.identity);
            }
        }
      
    }

        bool canBePlaced(Tile[,] world, int x, int y, Tile tile)
        {
           

            

            bool canConnectUp = canConnectToNeigh(world, x, y+1, tile.upNeighbors);
            if (canConnectUp == false)
            {
                Debug.Log("tried but couldnt 1");
                return false;

            }
            bool canConnectDown = canConnectToNeigh(world, x, y-1, tile.downNeighbors);
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
            bool canConnectRight = canConnectToNeigh(world, x+1, y, tile.rightNeighbors);
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
        bool canConnectToNeigh(Tile[,] world, int row, int col, Tile[] possibleNeighbors)
        {
           if( possibleNeighbors == null || possibleNeighbors.Length == 0 )
            {
                return true;
            }
            if(row>= 0 && row<sizeY && col>= 0 && col<sizeX)
            {
               
                foreach(Tile neighborPrefab in possibleNeighbors)
                {
                    if (world[row,col] == null ||  world[row,col] == neighborPrefab)
                    {
                        return true;
                        
                    }
                }
                return false;
            }
            return true;
        }

        Tile[]getPossibleNeighbors(Tile [,] world, int row, int col, Vector2Int direction)
        {
            Tile t = world[row, col];
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