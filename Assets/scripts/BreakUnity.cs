using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine.Serialization;

public class BreakUnity : MonoBehaviour
{
    public class Cell
    {
    }

    public Vector2 offset;
    public Vector2 size;

    public GameObject island;
    [SerializeField] private GameObject location;
    List<Cell> board;

    // Start is called before the first frame update
    void Generate()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];
                Instantiate(island, new Vector3(i * offset.x, 0, -j * offset.y), quaternion.identity,
                    location.transform);
            }
        }
    }

    [Button]
    void Board()
    {
        board = new List<Cell>();
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        Generate();
    }
}