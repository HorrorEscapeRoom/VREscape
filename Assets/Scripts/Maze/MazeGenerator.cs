using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] GameObject wallPrefab, floorPrefab;
    Vector2Int startCell = new Vector2Int(15, 0);
    Vector2Int goalCell = new Vector2Int(16, 29);
    bool[,] maze;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateMazeDelayed());
    }
    IEnumerator GenerateMazeDelayed(){
        yield return new WaitForSeconds(3);
        MapProcess();
    }
    async Task MapProcess(){
        bool mapOK = false;
        int attempts = 0, maxAttempts = 40;
        List<float> generationTimes = new List<float>();
        while (!mapOK)
        {
            float startTime = Time.realtimeSinceStartup;
            RefreshMaze();
            await Task.Delay(3);
            await GenerateRandomPaths();
            ForcedWalls();
            await Task.Delay(3);
            mapOK = RunCompleteCheck();
            attempts++;
            generationTimes.Add(Time.realtimeSinceStartup - startTime);
            if(attempts > maxAttempts){
                Debug.Log($"Failed to generate a valid maze after {maxAttempts} attempts");
                break;
            }   
        }
        float totalTime = 0;
        foreach (float time in generationTimes)
        {
            totalTime += time;
        }
        float averageTime = totalTime / generationTimes.Count;
        Debug.Log($"Average generation time: {averageTime} seconds");
        if(mapOK){
            Debug.Log($"Maze generated in {attempts} attempts");
        }else{
            //use this preset map if the map generation fails
            int[,] presetMap = new int[30, 30]{
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1},
                {1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1},
                {1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1},
                {1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1},
                {1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1},
                {1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1},
                {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1},
                {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                {0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0},
                {1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1},
                {1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };
            maze = FromInt(presetMap);
        }
        DebugMaze();
    }
    void RefreshMaze(int size = 30){
        maze = new bool[size, size];
        for (int i = 0; i < size; i++) { for (int j = 0; j < 30; j++) { maze[i, j] = true; } }
    }
    async Task GenerateRandomPaths(){
        for (int i = 0; i < 60; i++)
        {
            // Pick a random point in the maze
            Vector2Int randomPoint = new Vector2Int(Random.Range(1, 29), Random.Range(1, 29));

            // Pick a random 90 degree direction
            Vector2Int direction = Random.Range(0, 2) == 0  ? new Vector2Int(Random.Range(-1, 2), 0)  : new Vector2Int(0, Random.Range(-1, 2));

            int distance = Random.Range(5, 15);

            // From the random point set all cells in the direction to false
            for (int j = 0; j < distance; j++)
            {
                int newX = randomPoint.x + direction.x * j;
                int newY = randomPoint.y + direction.y * j;

                if (newX >= 0 && newX < 30 && newY >= 0 && newY < 30)
                {
                    maze[newX, newY] = false;
                }
                
            }
            await Task.Delay(1);
        }
    }
    void ForcedWalls(){
        //set all edges to true (except for the start and goal)
        for (int x = 0; x < 30; x++)
        {
            for (int y = 0; y < 30; y++)
            {
                if (x == 0 || x == 29 || y == 0 || y == 29)
                {
                    maze[x, y] = true;
                }
            }
        }
        maze[startCell.x, startCell.y] = false;
        maze[startCell.x, startCell.y + 1] = false;
        maze[startCell.x, startCell.y + 2] = false;


        maze[goalCell.x, goalCell.y] = false;
        maze[goalCell.x, goalCell.y - 1] = false;
        maze[goalCell.x, goalCell.y - 2] = false;
    }
    bool RunCompleteCheck()
    {
        // Ensure that the start and goal are connected
        bool connected = false;
        Queue<Vector2Int> openList = new Queue<Vector2Int>();
        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();
        openList.Enqueue(startCell);
        while (openList.Count > 0)
        {
            Vector2Int currentCell = openList.Dequeue();
            closedList.Add(currentCell);
            if (currentCell == goalCell)
            {
                connected = true;
                break;
            }
            // Check the four possible directions
            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(1, 0),  // Right
                new Vector2Int(-1, 0), // Left
                new Vector2Int(0, 1),  // Up
                new Vector2Int(0, -1)  // Down
            };
            foreach (var direction in directions)
            {
                Vector2Int neighbor = new Vector2Int(currentCell.x + direction.x, currentCell.y + direction.y);
                if (neighbor.x >= 0 && neighbor.x < 30 && neighbor.y >= 0 && neighbor.y < 30 && 
                    !maze[neighbor.x, neighbor.y] && !closedList.Contains(neighbor))
                {
                    openList.Enqueue(neighbor);
                }
            }
        }
        return connected;
    }
    void DebugMaze()
    {
        Vector3 backLeft = transform.position - new Vector3(15, 0, 15);
        for (int i = 0; i < 30; i++) {
            for (int j = 0; j < 30; j++) {
                if (maze[i, j]) {
                    //Debug.DrawRay(backLeft + new Vector3(i, 0, j), Vector3.up * 2, Color.red, 200f);


                    //if there are walls on all sides, instantiate a wall
                    if (!IsSurrounded(i, j)){
                        Instantiate(wallPrefab, backLeft + new Vector3(i * 2, 0, j * 2), wallPrefab.transform.rotation);
                    }
                }else{
                    Instantiate(floorPrefab, backLeft + new Vector3(i * 2, 0, j * 2), floorPrefab.transform.rotation);
                }
            }
        }
        Destroy(wallPrefab);
        Destroy(floorPrefab);
    }
    bool IsSurrounded(int x, int y)
    {
        if (x == 0 || x == 29 || y == 0 || y == 29)
        {
            return false;
        }
        if (maze[x - 1, y] && maze[x + 1, y] && maze[x, y - 1] && maze[x, y + 1])
        {
            return true;
        }
        return false;
    }
    bool[,] FromInt(int[,] input){
        bool[,] output = new bool[input.GetLength(0), input.GetLength(1)];
        for (int x = 0; x < input.GetLength(0); x++)
        {
            for (int y = 0; y < input.GetLength(1); y++)
            {
                output[x, y] = input[x, y] == 1;
            }
        }
        return output;
    }
}
