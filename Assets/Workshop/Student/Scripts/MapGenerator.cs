using System;
using System.Collections.Generic;
using UnityEngine;

namespace Workshop.Student
{
    public class MapGenerator : MonoBehaviour
    {
        public int columns = 10;
        public int rows = 10;
        public GameObject[] players;
        public GameObject exitPrefeb;

        public GameObject[] floorTiles;
        public GameObject[] wallTiles;
        public GameObject[] foodTiles;
        public HashSet<Vector3> obstaclePos = new HashSet<Vector3>();

        public string[,] saveItemMap = new string[3, 3] {
            { " ", "Soda", " "},
            { " ", " ", " "},
            { "Food", " ", " "},
        };


        public void Start()
        {
            // 1. random player at the position <0, 0> map
            int playerIndex = UnityEngine.Random.Range(0, players.Length);
            GameObject player = Instantiate(players[playerIndex], new Vector3(0, 0), Quaternion.identity);

            // 2. create obstacles
            for (int y = 1; y < rows - 1; y++)
            {
                for (int x = 1; x < columns - 1; x++)
                {
                    int r = UnityEngine.Random.Range(0, wallTiles.Length);
                    GameObject obstacle = Instantiate(wallTiles[r], new Vector3(x, y), Quaternion.identity);
                    obstacle.name = $"Obstacle({x},{y})";
                    obstaclePos.Add(obstacle.transform.position);
                }
            }

            // 3. create floor
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    int r = UnityEngine.Random.Range(0, floorTiles.Length);
                    GameObject floor = Instantiate(floorTiles[r], new Vector3(x, y), Quaternion.identity);
                    floor.name = $"Floor({x},{y})";
                }
            }

            // 4. create walls
            for (int y = -1; y < rows + 1; y++)
            {
                for (int x = -1; x < columns + 1; x++)
                {
                    if (x == -1 || x == columns || y == -1 || y == rows)
                    {
                        int r = UnityEngine.Random.Range(0, wallTiles.Length);
                        GameObject floor = Instantiate(wallTiles[r], new Vector3(x, y), Quaternion.identity);
                        floor.name = $"Wall({x},{y})";
                    }
                }
            }

            // 7. place exit
            GameObject exit = Instantiate(exitPrefeb, new Vector3(9, 9), Quaternion.identity);

            // 5. random foods
            int numberOfFood = UnityEngine.Random.Range(1, 6);
            for (int i = 0; i < numberOfFood; i++)
            {
                bool isCanSpawnFood = true;
                while (isCanSpawnFood)
                {
                    int randomX = UnityEngine.Random.Range(0, columns);
                    int randomY = UnityEngine.Random.Range(0, rows);
                    int f = UnityEngine.Random.Range(0, foodTiles.Length);
                    Vector3 foodPos = new Vector3(randomX, randomY);
                    if (!obstaclePos.Contains(foodPos) && foodPos != player.transform.position && foodPos != exit.transform.position)
                    {
                        GameObject food = Instantiate(foodTiles[f], foodPos, Quaternion.identity);
                        food.name = $"Food({randomX},{randomY})";
                        isCanSpawnFood = false;
                    }
                }
            }

            // 6. generate item along with the saveItemMap
            for (int y = 0; y < saveItemMap.GetLength(0); y++)
            {
                for (int x = 0; x < saveItemMap.GetLength(1); x++)
                {
                    string item = saveItemMap[y, x];
                    int foodIndex = -1;
                    for (int i = 0; i < foodTiles.Length; i++)
                    {
                        if (foodTiles[i].name == item)
                        {
                            foodIndex = i;
                        }
                        if (foodIndex > -1)
                        {
                            Instantiate(foodTiles[foodIndex], new Vector2(x, y), Quaternion.identity);
                        }
                    }
                }
            }
        }
    }

}