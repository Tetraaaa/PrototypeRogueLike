using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PathFinder
{
    private static List<GameTile> Tiles;

    public static void Init(List<GameTile> tileList)
    {
        Tiles = tileList;
    }

    public static List<GameTile> FindPath(GameTile startTile, GameTile endTile)
    {
        Tiles.ForEach(x => x.Parent = null);
        List<GameTile> result = ProcessPath(startTile, endTile);
        return result;
    }

    private static List<GameTile> ProcessPath(GameTile startTile, GameTile endTile)
    {
        // Vérification des tuiles de départ et d'arrivée
        if (!startTile.IsWalkable() || !endTile.IsWalkable())
            return null;

        Queue<GameTile> queue = new Queue<GameTile>();
        HashSet<GameTile> visited = new HashSet<GameTile>();

        queue.Enqueue(startTile);
        visited.Add(startTile);

        while (queue.Count > 0)
        {
            GameTile currentTile = queue.Dequeue();

            if (currentTile == endTile)
                return ReconstructPath(currentTile);

            foreach (var neighbor in GetNeighbors(currentTile))
            {
                if (!neighbor.IsWalkable() || visited.Contains(neighbor))
                    continue;

                neighbor.Parent = currentTile;
                queue.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }

        return null; // Aucun chemin trouvé
    }

    public static IEnumerable<GameTile> GetAvailableWalkingTiles(GameTile tile, int movePoints)
    {
        List<GameTile> availableTiles = new List<GameTile>();

        List<GameTile> openList = new List<GameTile>();
        HashSet<GameTile> closedSet = new HashSet<GameTile>();

        openList.Add(tile);

        while (openList.Count > 0)
        {
            GameTile currentTile = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (GetDistance(openList[i], tile) < GetDistance(currentTile, tile))
                {
                    currentTile = openList[i];
                }
            }

            openList.Remove(currentTile);
            closedSet.Add(currentTile);

            if (GetDistance(tile, currentTile) <= movePoints)
            {
                availableTiles.Add(currentTile);
            }

            foreach (GameTile neighbor in GetNeighbors(currentTile))
            {
                if (!neighbor.IsWalkable() || closedSet.Contains(neighbor))
                {
                    continue;
                }

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
            }
        }

        return availableTiles;
    }

    public static List<GameTile> FindPathToRunAwayFromTiles(IEnumerable<GameTile> tilesToRunAway, GameTile currentTile, int movePoints, int idealDistance)
    {
        GameTile targetTile = null;
        List<GameTile> result = null;

        IEnumerable<GameTile> availableWalkingTiles = GetAvailableWalkingTiles(currentTile, movePoints);

        int closestDistanceToEnemy = int.MaxValue;

        foreach (GameTile availableWalkingTile in availableWalkingTiles)
        {
            int minDistanceToEnemy = int.MaxValue;

            foreach (GameTile enemyTile in tilesToRunAway)
            {
                int distanceToEnemy = GetDistance(availableWalkingTile, enemyTile);
                minDistanceToEnemy = Math.Min(minDistanceToEnemy, distanceToEnemy);
            }

            if (Math.Abs(minDistanceToEnemy - idealDistance) < Math.Abs(closestDistanceToEnemy - idealDistance))
            {
                closestDistanceToEnemy = minDistanceToEnemy;
                targetTile = availableWalkingTile;
            }
        }

        if (targetTile != null)
            result = FindPath(currentTile, targetTile);

        return result;
    }

    public static List<GameTile> FindPathToGoToClosestEnnemy(IEnumerable<GameTile> ennemyTiles, GameTile currentTile, int movePoints)
    {
        int closestDistance = int.MaxValue;
        GameTile targetTile = null;
        List<GameTile> result = null;

        IEnumerable<GameTile> availableWalkingTiles = GetAvailableWalkingTiles(currentTile, movePoints);

        foreach (GameTile availableWalkingTile in availableWalkingTiles)
        {
            foreach (GameTile ennemyTile in ennemyTiles)
            {
                int distance = GetDistance(availableWalkingTile, ennemyTile);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetTile = availableWalkingTile;
                }

                if (availableWalkingTile == currentTile && distance == closestDistance)
                {
                    closestDistance = distance;
                    targetTile = availableWalkingTile;
                }
            }
        }

        if (targetTile != null)
            result = FindPath(currentTile, targetTile);

        return result;
    }

    private static List<GameTile> ReconstructPath(GameTile currentTile)
    {
        List<GameTile> path = new List<GameTile> { currentTile };

        while (currentTile.Parent != null)
        {
            currentTile = currentTile.Parent;
            path.Insert(0, currentTile);
        }

        if (path.Count > 0)
            path.RemoveAt(0);

        return path;
    }

    public static int GetDistance(GameTile tileA, GameTile tileB)
    {
        int distanceX = Mathf.Abs(tileA.x - tileB.x);
        int distanceY = Mathf.Abs(tileA.y - tileB.y);
        return distanceX + distanceY;
    }

    private static List<GameTile> GetNeighbors(GameTile tile)
    {
        List<GameTile> neighbors = new List<GameTile>();

        // Recherche des voisins en haut, en bas, à gauche et à droite (4 directions)
        int[] xOffset = { 0, 0, -1, 1 };
        int[] yOffset = { -1, 1, 0, 0 };

        for (int i = 0; i < xOffset.Length; i++)
        {
            int neighborX = tile.x + xOffset[i];
            int neighborY = tile.y + yOffset[i];

            GameTile neighbor = Tiles.Find(t => t.x == neighborX && t.y == neighborY);
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}
