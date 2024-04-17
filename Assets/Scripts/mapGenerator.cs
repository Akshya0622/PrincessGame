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
    Dictionary<Tile, int> tileWeightss;
    public int[] weightValues;
    public void Start()
    {
        tileWeightss = populateTileWeights();
        makeGrid();

    }
    Dictionary<Tile,int> populateTileWeights()
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
        Tile[,] world = new Tile[sizeX, sizeY];
        Vector2Int startPos = new Vector2Int(sizeX / 2, sizeY / 2);
        Tile startTile = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
        world[startPos.x, startPos.y] = startTile;
        Tile tile = Instantiate(startTile, new Vector3(startPos.x, startPos.y, 0), Quaternion.identity);
        generateTiles(world, startPos.x, startPos.y, startTile);
    }
    bool generateTiles(Tile[,] world, int x, int y, Tile tilePrfb)
    {
        Debug.Log("Generating tile at: " + x + ", " + y);
        //Tile tile = Instantiate(tilePrfb, new Vector3(x, y, 0), Quaternion.identity);
        
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

        bool result = false;

        foreach (Vector2Int direction in directions)
        {
            int newX = x + direction.x;
            int newY = y + direction.y;

            if (newX >= 0 && newX < sizeX && newY >= 0 && newY < sizeY && world[newX, newY] == null)
            {
                result = true;
                Tile[] possibleNeighbors = getPossibleNeighbors(world, x, y, direction); // access from tile
                Debug.Log("Number of possible neighbors: " + possibleNeighbors.Length + "direction " + direction);
                possibleNeighbors = weightedSelect(possibleNeighbors); // u need to makesomething to remake the array with weights

                foreach (Tile neighbor in possibleNeighbors)
                {
                    Debug.Log("Checking placement for neighbor: " + neighbor.name);
                    if (canBePlaced(world, newX, newY, neighbor))
                    {
                        Debug.Log("Placing tile at: " + newX + ", " + newY);
                        Tile tile = Instantiate(neighbor, new Vector3(newX, newY, 0), Quaternion.identity);
                        world[newX, newY] = tile;
                        
                        result = generateTiles(world, newX, newY, tile);

                        if (!result) Destroy(tile.gameObject);
                     
                    }
                    else
                    {
                        Debug.Log("cant be placed loking thru neighbors rn");
                    }
                }

            }
        }

        if (result) return false;
        else return true;


        // tile found no possible neighbors
        
        
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
                        Debug.Log("yes");
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