using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        yield return new WaitForSeconds(1);
        bool mapOK = false;
        int attempts = 0;
        while (!mapOK)
        {
            RefreshMaze();
            GenerateRandomPaths();
            ForcedWalls();
            mapOK = RunCompleteCheck();
            attempts++;
            if(attempts > 10){
                Debug.Log("Failed to generate a valid map after 10 attempts");
                break;
            }
        }
        DebugMaze();
    }
    void RefreshMaze(){
        maze = new bool[30, 30];

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                maze[i, j] = true;
            }
        }
    }
    void GenerateRandomPaths(){
        for (int i = 0; i < 60; i++)
        {
            //pick a random point in the maze
            Vector2Int randomPoint = new Vector2Int(Random.Range(1, 29), Random.Range(1, 29));
            //pick a random 90 degree direction
            Vector2Int direction = new Vector2Int(0, 0);
            if (Random.Range(0, 2) == 0)
            {
                direction.x = Random.Range(-1, 2);
            }
            else
            {
                direction.y = Random.Range(-1, 2);
            }
            int distance = Random.Range(5, 15);
            //from the random point set all cells in the direction to false
            for (int j = 0; j < distance; j++)
            {
                if (randomPoint.x + direction.x * j >= 0 && randomPoint.x + direction.x * j < 30 && randomPoint.y + direction.y * j >= 0 && randomPoint.y + direction.y * j < 30)
                {
                    maze[randomPoint.x + direction.x * j, randomPoint.y + direction.y * j] = false;
                }
            }
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
    bool RunCompleteCheck(){

        //ensure that the start and goal are connected
        bool connected = false;
        List<Vector2Int> openList = new List<Vector2Int>();
        List<Vector2Int> closedList = new List<Vector2Int>();
        openList.Add(startCell);
        while (openList.Count > 0)
        {
            Vector2Int currentCell = openList[0];
            openList.RemoveAt(0);
            closedList.Add(currentCell);
            if (currentCell == goalCell)
            {
                connected = true;
                break;
            }
            if (currentCell.x + 1 < 30 && !maze[currentCell.x + 1, currentCell.y] && !closedList.Contains(new Vector2Int(currentCell.x + 1, currentCell.y)))
            {
                openList.Add(new Vector2Int(currentCell.x + 1, currentCell.y));
            }
            if (currentCell.x - 1 >= 0 && !maze[currentCell.x - 1, currentCell.y] && !closedList.Contains(new Vector2Int(currentCell.x - 1, currentCell.y)))
            {
                openList.Add(new Vector2Int(currentCell.x - 1, currentCell.y));
            }
            if (currentCell.y + 1 < 30 && !maze[currentCell.x, currentCell.y + 1] && !closedList.Contains(new Vector2Int(currentCell.x, currentCell.y + 1)))
            {
                openList.Add(new Vector2Int(currentCell.x, currentCell.y + 1));
            }
            if (currentCell.y - 1 >= 0 && !maze[currentCell.x, currentCell.y - 1] && !closedList.Contains(new Vector2Int(currentCell.x, currentCell.y - 1)))
            {
                openList.Add(new Vector2Int(currentCell.x, currentCell.y - 1));
            }
        }
        Debug.Log($"Map is connected: {connected}");
        return connected;
    }

    void DebugMaze()
    {
        Vector3 backLeft = transform.position - new Vector3(15, 0, 15);
        for (int i = 0; i < 30; i++) {
            for (int j = 0; j < 30; j++) {
                if (maze[i, j]) {
                    Debug.DrawRay(backLeft + new Vector3(i, 0, j), Vector3.up * 2, Color.red, 200f);


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
}
