using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path
    /// Note that you will probably need to add some helper functions
    /// from the startPos to the endPos
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        if(!InGrid(startPos) || !InGrid(endPos))
            return null;

        Node current = null;
        var openList = new List<Node>()
        {
            new Node(startPos, null, 0, HScore(startPos))
        };
        var closedList = new List<Node>();
        int GScore = 0;

        while(openList.Count > 0)
        {
            // TODO: improve these 2 functions.
            var lowest = openList.Min(element => element.FScore);
            current = openList.First(element => element.FScore == lowest);

            closedList.Add(current);
            openList.Remove(current);

            if(InList(closedList, endPos))
                break;

            // If we have not reached the end, branch out
            ++GScore;

            var (x, y) = (current.position.x, current.position.y);
            var cell = grid[x, y];

            if(!cell.HasWall(Wall.UP)) TryNewLocation(x, y + 1);
            if(!cell.HasWall(Wall.RIGHT)) TryNewLocation(x + 1, y);
            if(!cell.HasWall(Wall.DOWN)) TryNewLocation(x, y - 1);
            if(!cell.HasWall(Wall.LEFT)) TryNewLocation(x - 1, y);
        }

        var path = new List<Vector2Int>();
        while(current != null)
        {
            path.Insert(0, current.position);
            current = current.parent;
        }

        return path;

        // Inlined Methods
        bool InList(List<Node> nodeList, Vector2Int pos) => nodeList.Any(element => element.position == pos);
        bool InGrid(Vector2Int pos) => pos.x >= 0 && pos.x < grid.GetLength(0) && pos.y >= 0 && pos.y < grid.GetLength(1);
        int HScore(Vector2Int pos)
        {
            var difference = endPos - pos;
            return Mathf.Abs(difference.x) + Mathf.Abs(difference.y);
        }

        void TryNewLocation(int x, int y)
        {
            var newPos = new Vector2Int(x, y);
            if(!InGrid(newPos) || InList(closedList, newPos))
                return;

            var newHScore = HScore(newPos);
            if(!InList(openList, newPos) || GScore + newHScore < openList.Find(element => element.position == newPos).FScore)
                openList.Insert(0, new Node(newPos, current, GScore, newHScore));
        }
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public int FScore => GScore + HScore;
        public int GScore; //Current Travelled Distance
        public int HScore; //Distance estimated based on Heuristic

        public Node(Vector2Int position, Node parent, int GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
