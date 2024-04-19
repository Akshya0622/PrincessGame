using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class mapGenerator : MonoBehaviour
{
    public int sizeX = 10;
    public int sizeY = 10;
    public Tile[] tilePrefabs;
    Dictionary<Tile, int> tileWeightss; //weights of each tile so we can have certain tiles being placed more often
    public int[] weightValues;
    //Stack<Vector2Int> backtrackStack = new Stack<Vector2Int>();
    Tile[,] world; 

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
        Vector2Int startPos = new Vector2Int(sizeX / 2, sizeY / 2); // start pos in the middle
        Tile startTile = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
        world[startPos.x, startPos.y] = startTile; // random start tile placed
        generateTiles(world, startPos.x, startPos.y, startTile);
    }
    
   void generateTiles(Tile[,] world, int x, int y, Tile tilePrfb)
    {
        Debug.Log("Generating tile at: " + x + ", " + y);
        
        
        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(0, 1),
            new Vector2Int(0,-1),
            new Vector2Int(-1,0),
            new Vector2Int(1,0)
        };


        for (int i = 0; i < 4; i++) // shuffling list of directions
        {
            int randomIndex = Random.Range(i, 4);
            Vector2Int temp = directions[i];
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }


        bool placed = false;
        int count = 0;
        foreach (Vector2Int direction in directions) // take the current tile and get the random direction, then check if any of the tiles that are connectable to the current tile are valid options to be placed into the map. If none of the tiles in one direction work, foreach(direction in directions) will select next direction...
        {
            int newX = x + direction.x;
            int newY = y + direction.y;

            if (newX >= 0 && newX < sizeX && newY >= 0 && newY < sizeY && world[newX, newY] == null)
            {
               
                Tile[] possibleNeighbors = getPossibleNeighbors(world, x, y, direction); // access from tile script
                Debug.Log("Number of possible neighbors: " + possibleNeighbors.Length + "direction " + direction);
                possibleNeighbors = weightedSelect(possibleNeighbors); // remake neighbors array with weights

                foreach (Tile neighbor in possibleNeighbors) // checking neighbors in selected direction
                {
                    Debug.Log("Checking placement for neighbor: " + neighbor.name);
                    if (canBePlaced(world, newX, newY, neighbor))
                    {
                        Debug.Log("Placing tile at: " + newX + ", " + newY);
                        placed = true;
                        world[newX, newY] = neighbor;
                     
                        generateTiles(world, newX, newY, neighbor); 
                        break;

                      
                     
                    }
                    else
                    {
                        Debug.Log("cant be placed looking through neighbors");
                    }
                }
                if (placed) // If a tile is successfully placed, break out of the loop and continue with the next position.
                {
                    break;
                }

            }
            if(placed == false)
            {
                count++;
                Debug.Log("none worked in " + direction);
            }
           

        }
        if(count == 4) // if we couldnt place a tile because there was no option in any of the 4 directions
        {
            Debug.Log("used to be " + world[x, y]);
            world[x, y] = null; // then make the current location null and have the previous tile check a different direction to place a tile in 
            Debug.Log("made null");
        }
      

       
        // tile found no possible neighbors
        
        
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