using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class mapGenerator : MonoBehaviour
{
    public int sizeX;
    public int sizeY;
    public List<Tile> tilePrefabs;
    Dictionary<Tile, int> tileWeightss; //weights of each tile so we can have certain tiles being placed more often
    Dictionary<int, Tile> identifiers;
    public int[] weightValues;
    List<int> weightedTiles;
    List<Tile> placedTiles = new List<Tile>();
    int[,] world;
   
    public void Start()
    {
        tileWeightss = populateTileWeights();
        identifiers = populateIdentifiers();
        weightedTiles = weightedSelect(tilePrefabs);
        makeGrid();
        drawMap();

    }
    Dictionary<Tile,int> populateTileWeights() // create a dictionary from the list of weights and tile prefabs
    {
        Dictionary<Tile, int> tileWeights = new Dictionary<Tile, int>();
        for (int i = 0; i< tilePrefabs.Count; i++)
        {
            tileWeights.Add(tilePrefabs[i], weightValues[i]);
        }
        return tileWeights;
    }
    Dictionary<int,Tile> populateIdentifiers()
    {
        Dictionary<int, Tile> iden = new Dictionary<int, Tile>();
        for (int i = 0; i < tilePrefabs.Count; i++)
        {
            iden.Add(i+1, tilePrefabs[i]);
        }
        Debug.Log(iden.Count);
        return iden;
    }
    public void makeGrid()
    {
        world = new int[sizeX, sizeY]; // make world 2d array
        
        Tile startTile = tilePrefabs[Random.Range(0, tilePrefabs.Count)];
        world[0,0] = startTile.number; // 
       
        generateTiles(world);
        
    }

    bool generateTiles(int[,] world)
    {
        Vector2Int emptyPosition = FindEmpty(world);

        // map done
        if (emptyPosition.x == -1 && emptyPosition.y == -1)
        {
          
            
            return true;
        }

        int row = emptyPosition.x;
        int col = emptyPosition.y;

        List<int> copy = new List<int>(weightedTiles);

        shuffle(copy);
        foreach (int tilePrefab in copy)
        {
           
            if (canBePlaced(world, row, col, tilePrefab))
            {
               
                world[row, col] = tilePrefab;
               

               
                if (generateTiles(world))
                {
                    return true;
                }
                   

                
                world[row, col] = 0;
                
            }
        }

       
        return false;
    }
    Vector2Int FindEmpty(int[,] world)
    {
        for(int row = 0; row < sizeY; row++)
        {
            for(int col = 0; col < sizeX; col++)
            {
                if (world[row,col] == 0)
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
                identifiers.TryGetValue(world[row, col], out Tile t);
                Instantiate(t, new Vector3(row, col, 0), Quaternion.identity);
            }
        }
      
    }

        bool canBePlaced(int[,] world, int x, int y, int tileNum)
        {
        Tile tile;
        if(!identifiers.TryGetValue(tileNum, out tile))
        {
            
            Debug.Log("ahhhhh");
        }

            

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
        bool canConnectToNeigh(int[,] world, int row, int col, Tile[] possibleNeighbors)
        {
           if( possibleNeighbors == null || possibleNeighbors.Length == 0 )
            {
                return true;
            }
            if(row>= 0 && row<sizeY && col>= 0 && col<sizeX)
            {
               
                foreach(Tile neighborPrefab in possibleNeighbors)
                {
                    
                    if (world[row,col] == 0 ||  world[row,col] == neighborPrefab.number)
                    {
                        return true;
                        
                    }
                }
                return false;
            }
            return true;
        }

        /*Tile[]getPossibleNeighbors(Tile [,] world, int row, int col, Vector2Int direction)
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

        }*/
        List<int> weightedSelect(List<Tile> neighbors)
        {
           List<int> weightedSelect = new List<int>();
            
           foreach(Tile neighbor in neighbors)
            {
                int weight;
                if(tileWeightss.TryGetValue(neighbor, out weight))
                {
                    for(int i = 0; i < weight; i++)
                    {
                        weightedSelect.Add(neighbor.number);
                    }
                }
            }
        return weightedSelect;
           
        }
    void shuffle(List<int> shuffle)
    {
        int x = shuffle.Count;
        while(x>1)
        {
            x--;
            int k = Random.Range(0, x + 1);
            int temp = shuffle[k];
            shuffle[k] = shuffle[x];
            shuffle[x] = temp;
        }
       
    }






}