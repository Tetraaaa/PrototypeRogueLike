using System.Drawing;
using System.Collections.Generic;
using UnityEngine;

public class HomeMadePathfinder : MonoBehaviour
{
    private static GameBoard Board;

    public static void Init(GameBoard _Board)
    {
        Board = _Board ;
    }

    public static List<GameTile> FindPath(GameTile startTile, GameTile endTile)
    {
        endTile.ignoreAllCollisions = true;
        foreach (KeyValuePair<Point, GameTile> entry in Board.board)
        {
            entry.Value.Parent = null;
        }
        List<GameTile> result = ProcessPath(startTile, endTile);
        endTile.ignoreAllCollisions = false;
        return result;
    }

    private static List<GameTile> ProcessPath(GameTile startTile, GameTile endTile)
    {
        List<GameTile> openSet = new List<GameTile>();
        HashSet<GameTile> closedSet = new HashSet<GameTile>();
        openSet.Add(startTile);

        while(openSet.Count > 0)
        {
            GameTile currentNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endTile)
            {
                return RetracePath(startTile, endTile);
            }

            foreach (GameTile neighbor in Board.GetNeighbors(currentNode))
            {
                if(!neighbor.IsWalkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistanceBetweenTiles(currentNode, neighbor);
                if(newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistanceBetweenTiles(neighbor, endTile);
                    neighbor.Parent = currentNode;
                    if(!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    static List<GameTile> RetracePath(GameTile start, GameTile end)
    {
        List<GameTile> path = new List<GameTile>();
        GameTile currentTile = end;

        while(currentTile != start)
        {
            path.Add(currentTile);
            currentTile = currentTile.Parent;
        }

        path.Reverse();
        return path;
    }

    private static int GetDistanceBetweenTiles(GameTile a, GameTile b)
    {
        int distanceX = Mathf.Abs(a.x - b.x);
        int distanceY = Mathf.Abs(a.y - b.y);

        return 10 * distanceX + 10 * distanceY;
    }
}
