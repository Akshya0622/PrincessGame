using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
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
    HashSet<int> up = new HashSet<int>();
    HashSet<int> down = new HashSet<int>();
    HashSet<int> left = new HashSet<int>();
    HashSet<int> right = new HashSet<int>();
    public List<int> tiles1to35;
    public GameObject[] weapons; 
   public Vector3 centerize = new Vector3(-40, -30, 0);
    public void Start()
    {
        
        for(int i = 1; i<=35; i++)
        {
            tiles1to35.Add(i);
        }
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
            iden.Add(tilePrefabs[i].number, tilePrefabs[i]);
        }
        
        return iden;
    }
    public void makeGrid()
    {
        world = new int[sizeX, sizeY]; // make world 2d array
        
        Tile startTile = tilePrefabs[Random.Range(0, tilePrefabs.Count)];
        world[0,0] = startTile.number; // 
        
        generateTiles(world,0,0);
        
    }
    public void makeUpSet(int row, int col)
    {

        up.Clear();
        if (row > 0 && world[row - 1, col] !=0)
        {
            
            int upTileNumber = world[row - 1, col];// check the tile  above where we are
            Tile t = null;
            if (upTileNumber > 0)
            {
                identifiers.TryGetValue(upTileNumber, out t);
                for (int i = 0; i < t.downNeighbors.Count();i++) // look thru that tiles down neighbors (cause those are the ones we can place)
                {
                    //Tile x;
                    //if (!identifiers.TryGetValue(t.downNeighbors[i].number, out x)) throw new ArgumentException("Bad neighbor number");

                    up.Add(t.downNeighbors[i].number); // add its number to the up set
                }

            }

        }
        else
        {
         
            up.UnionWith(tiles1to35);
        }
    }
    public void makeDownSet(int row, int col)
    {

        down.Clear();
        if (row < sizeY -1 && world[row+1,col] !=0)
        {
           
            int downTileNumber = world[row + 1, col];// check the tile below where we are
            Tile t = null;
            if (downTileNumber > 0)
            {
                identifiers.TryGetValue(downTileNumber, out t);
                for (int i = 0; i < t.upNeighbors.Count(); i++) // look thru that tiles up neighbors (cause those are the ones we can place)
                {
                    Tile x;
                    if (!identifiers.TryGetValue(t.upNeighbors[i].number, out x)) throw new ArgumentException("Bad neighbor number");
                    down.Add(t.upNeighbors[i].number); // add its number to the down set
                }

            }

        }
        else
        {
            
            down.UnionWith(tiles1to35);
        }
    }
    public void makeLeftSet(int row, int col)
    {
        left.Clear();
        if (col > 0 && world[row,col-1] != 0)
        {
            
            int leftTileNumber = world[row, col-1];// check the tile to the left of where we are
            Tile t = null;
            if (leftTileNumber > 0)
            {
                identifiers.TryGetValue(leftTileNumber, out t);
                for (int i = 0; i < t.rightNeighbors.Count(); i++) // look thru that tiles right neighbors (cause those are the ones we can place)
                {
                    Tile x;
                    if (!identifiers.TryGetValue(t.rightNeighbors[i].number, out x)) throw new ArgumentException("Bad neighbor number");
                    left.Add(t.rightNeighbors[i].number); // add its number to the left set
                }

            }

        }
        else
        {
            
            left.UnionWith(tiles1to35);
        }
    }
    public void makeRightSet(int row, int col)
    {
        right.Clear();
        if (col< sizeX -1 && world[row, col + 1] !=0)
        {
            
            int rightTileNumber = world[row, col+1];// check the tile to the right of where we are
            Tile t = null;
            if (rightTileNumber > 0)
            {
                identifiers.TryGetValue(rightTileNumber, out t);
                for (int i = 0; i < t.leftNeighbors.Count(); i++) // look thru that tiles left neighbors (cause those are the ones we can place)
                {
                    Tile x;
                    if (!identifiers.TryGetValue(t.leftNeighbors[i].number, out x)) throw new ArgumentException("Bad neighbor number");

                    right.Add(t.leftNeighbors[i].number); // add its number to the up set
                }

            }

        }
        else
        {

            right.UnionWith(tiles1to35);
        }
    }


    bool generateTiles(int[,] world, int row, int col)
    {
        makeUpSet(row, col);
        makeDownSet(row, col);
        makeLeftSet(row, col);
        makeRightSet(row, col);

        HashSet<int> s = new HashSet<int>(up);
        s.IntersectWith(left);
        s.IntersectWith(right);
        s.IntersectWith(down);

        
        if (!s.Any())
        {
            return false;
        }

        

        while(s.Count>0)
        {
            world[row, col] = select(s);
            Debug.Log(world[row, col] + " : (" + row + ", " + col + ") : " + String.Join(", ", s));
            // Map done
            if (row == sizeY - 1 && col == sizeX - 1)
            {
                return true;
            }

            int nextRow = row;
            int nextCol = col + 1;
            if (nextCol >= sizeX)
            {
                nextCol = 0;
                nextRow++;
            }

            if (generateTiles(world, nextRow, nextCol))
            {
                return true;
            }
            s.Remove(world[row, col]);
           
            // Backtrack
            world[row, col] = 0;
         
        }

        return false;
    }
    void drawMap()
    {
        

        for (int i = 0; i<sizeX; i++)
        {
            Debug.Log("\n");
            for(int j = 0; j<sizeY; j++)
            {
                Debug.Log(world[i, j] + " ");
            }
        }
        for(int row = 0; row < sizeY; row++)
        {
            for( int col = 0; col < sizeX; col++)
            {
                identifiers.TryGetValue(world[row, col], out Tile t);
                
                    Instantiate(t, new Vector3(col, sizeY - row, 0) + centerize , Quaternion.identity);
                
                
            }
        } 
        for(int x = 0; x < 5; x++)
        {
            int randRow = Random.Range(0, sizeY);
            int randCol = Random.Range(0, sizeX);
            int randIndex = Random.Range(0, weapons.Length);
            Instantiate(weapons[randIndex], new Vector3(randCol, sizeY - randRow, 0) + centerize , Quaternion.identity);
        }
      
    }

      /*  bool canBePlaced(int[,] world, int x, int y, int tileNum)
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
        

        } */
     /*   bool canConnectToNeigh(int[,] world, int row, int col, Tile[] possibleNeighbors)
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
        } */

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
    public int select(HashSet<int> ps)
    {
        List<int> paw = new List<int>(ps);
        int x = 0;
        foreach(int a in ps)
        {
            x += weightedTiles[a - 1];
        }
        int w = 0;
        x = Random.Range(0, x);
        for(int i = 0; i < paw.Count; i++)
        {
            w += weightedTiles[paw[i]-1];
            if(w >= x)
            {
                return paw[i];
            }
        }
        return - 1;
       
    }






}