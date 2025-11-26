using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Vector2 worldSize = new Vector2(5, 5);
    Room[,] rooms;
    List<Vector2> fullPositions = new List<Vector2>();
    int gridSizeX, gridSizeY;
    public int roomCount = 2;

    public GameObject roomTestObj;
    public GameObject doorTestObj;

    public GameObject[] wallObjects;
    public GameObject[] floorObjects;
    public GameObject bossRoom;

    public GameObject doorPrefab;

    private void Start()
    {
        if (roomCount >= worldSize.x /4 * worldSize.y /8)
        {
            roomCount = Mathf.RoundToInt(worldSize.x / 4 * worldSize.y / 8);
        }
        gridSizeX = Mathf.RoundToInt(worldSize.x/16);
        gridSizeY = Mathf.RoundToInt(worldSize.y/8);
        CreateRooms();
        SetRoomDoors();
        SpawnPlaceholders();
        SpawnFloors();
        
        //SpawnDoors();   

    }

    void CreateRooms()
    {
        //generating the first room in the middle of the grid
        rooms = new Room[gridSizeX * 2, gridSizeY * 2];
        rooms[gridSizeX, gridSizeY] = new Room(Vector2.zero, 0);
        fullPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero;

        //change these to change generation
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
        //add new rooms
        for (int i = 0; i < roomCount - 1; i++)
        {
            float randomPerc = (float)i / ((float)roomCount - 1);
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);

            checkPos = NewPosition();

            if (NumberOfNeighbors(checkPos, fullPositions) > 1 && Random.value > randomCompare)
            {
                int iterations = 1;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                } while (NumberOfNeighbors(checkPos, fullPositions) > 1 && iterations < 100);
            }
            if(i == roomCount -2) rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, 1);
            else rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, 0);
            fullPositions.Insert(0, checkPos);
        }
    }

    Vector2 NewPosition()
    {
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            int index = Mathf.RoundToInt(Random.value * (fullPositions.Count - 1));
            x = (int)fullPositions[index].x;
            y = (int)fullPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive) y += 1;
                else y -= 1;
            }
            else
            {
                if (positive) x += 1;
                else x -= 1;
            }
            checkingPos = new Vector2(x, y);

        } while (fullPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        return checkingPos;

    }

    Vector2 SelectiveNewPosition()
    {
        int x = 0, y = 0, inc = 0, index = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            inc = 0;
            do
            {
                index = Mathf.RoundToInt(Random.value * (fullPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbors(fullPositions[index], fullPositions) > 1 && inc < 100);

            x = (int)fullPositions[index].x;
            y = (int)fullPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive) y += 1;
                else y -= 1;
            }
            else
            {
                if (positive) x += 1;
                else x -= 1;
            }
            checkingPos = new Vector2(x, y);

        } while (fullPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        return checkingPos;
    }

    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        int ret = 0;
        if (usedPositions.Contains(checkingPos + Vector2.right))
        {
            ret++;
        }
        if(usedPositions.Contains(checkingPos + Vector2.left))
        {
            ret++;
        }
        if(usedPositions.Contains(checkingPos + Vector2.up))
        {
            ret++;
        }
        if(usedPositions.Contains(checkingPos + Vector2.down))
        {
            ret++;
        }
        return ret;
    }

    void SetRoomDoors()
    {
        for(int x = 0; x < gridSizeX*2; x++)
        {
            for (int y = 0; y < gridSizeY * 2; y++)
            {
                if (rooms[x, y] == null) continue;
                Vector2 gridPosition = new Vector2(x, y);
                if(y - 1 < 0)//check down
                {
                    rooms[x, y].doorDown = false;
                }
                else
                {
                    rooms[x, y].doorDown = (rooms[x, y - 1] != null);
                }
                if ((y + 1) >= gridSizeY*2)//check above
                {
                    rooms[x, y].doorTop = false;
                }
                else
                {
                    rooms[x, y].doorTop = (rooms[x, y + 1] != null);
                }
                if (x - 1 < 0)//check left
                {
                    rooms[x, y].doorLeft = false;
                }
                else
                {
                    rooms[x, y].doorLeft = (rooms[x - 1, y] != null);
                }
                if ((x + 1) >= gridSizeX * 2)//check right
                {
                    rooms[x, y].doorRight = false;
                }
                else
                {
                    rooms[x, y].doorRight = (rooms[x + 1,y] != null);
                }

            }
                
        }
    }

    void SpawnPlaceholders()
    {
        for (int x = 0; x < gridSizeX * 2; x++)
        {
            for (int y = 0; y < gridSizeY * 2; y++)
            {
                if (rooms[x, y] == null) continue;
                //Instantiate(roomTestObj, new Vector2(2*x, y), Quaternion.identity);

                int wallIndex = 0;
                if (rooms[x, y].doorTop) wallIndex += 1;
                if (rooms[x, y].doorRight) wallIndex += 2;
                if (rooms[x, y].doorDown) wallIndex += 4;
                if (rooms[x, y].doorLeft) wallIndex += 8;

                Instantiate(wallObjects[wallIndex - 1], new Vector2(16 * x, 8*y), Quaternion.identity);





            }
        }
    }

    void SpawnFloors()
    {
        for (int x = 0; x < gridSizeX * 2; x++)
        {
            for (int y = 0; y < gridSizeY * 2; y++)
            {
                if (rooms[x, y] == null) continue;

                if(rooms[x,y].type == 0)
                {
                    int index = Random.Range(0, floorObjects.Length);

                    var obj = Instantiate(floorObjects[index], new Vector2(16 * x, 8 * y), Quaternion.identity);
                    if (x == gridSizeX && y == gridSizeY)
                    {
                        obj.gameObject.GetComponent<RoomBehavior>().spawned = true;
                    }
                    SpawnDoors(x, y);
                }
                else
                {
                    var obj = Instantiate(bossRoom, new Vector2(16 * x, 8 * y), Quaternion.identity);
                    SpawnDoors(x, y);
                }

                
                


            }
        }
    }

    void SpawnDoors(int x, int y)
    {
        List<Door> doorList = new List<Door>();
        if (rooms[x, y] == null) return;


        if (rooms[x, y].doorTop)
        {
            Door newDoor = Instantiate(doorPrefab, new Vector2(x * 16, y * 8 + 3.95f), Quaternion.identity).GetComponent<Door>();
            newDoor.SetPositions(new Vector2(16 * x, 8 * y), new Vector2(16 * x, 8 * (y+1)));
            GameManager.instance.AddDoor(newDoor);

        }
        if (rooms[x, y].doorRight)
        {
            Door newDoor = Instantiate(doorPrefab, new Vector2(x * 16 + 7.95f, y * 8), Quaternion.Euler(0,0,-90f)).GetComponent<Door>();
            newDoor.SetPositions(new Vector2(16 * x, 8 * y), new Vector2(16 * (x+1), 8 * y ));
            GameManager.instance.AddDoor(newDoor);
        }
        if (rooms[x, y].doorDown)
        {
            Door newDoor = Instantiate(doorPrefab, new Vector2(x * 16, y * 8 - 3.95f), Quaternion.Euler(0, 0, 180f)).GetComponent<Door>();
            newDoor.SetPositions(new Vector2(16 * x, 8 * y), new Vector2(16 * x, 8 * (y - 1)));
            GameManager.instance.AddDoor(newDoor);
        }
        if (rooms[x, y].doorLeft)
        {
            Door newDoor = Instantiate(doorPrefab, new Vector2(x * 16 - 7.95f, y * 8), Quaternion.Euler(0, 0, 90f)).GetComponent<Door>();
            newDoor.SetPositions(new Vector2(16 * x, 8 * y), new Vector2(16 * (x - 1), 8 * y));
            GameManager.instance.AddDoor(newDoor);
        }
       


            
    }
}
