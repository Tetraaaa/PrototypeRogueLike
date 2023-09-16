using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile
{
    public int x;
    public int y;
    private int tilesetX;
    private int tilesetY;
    public Vector3 worldPos;
    public GameObject? entity = null;
    public bool hasCollision = false;
    public bool ignoreAllCollisions = false;
    public int gCost;
    public int hCost;


    public GameTile Parent = null;

    public GameTile(int x, int y, int tilesetX, int tilesetY, bool hasCollision = false)
    {
        this.x = x;
        this.y = y;
        this.tilesetX = tilesetX;
        this.tilesetY = tilesetY;
        this.hasCollision = hasCollision;
        this.worldPos = new Vector3(x, y);
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public bool IsWalkable()
    {
        if (ignoreAllCollisions) return true;
        return !hasCollision && entity == null;
    }

    public override string ToString()
    {
        return "T( " + x + ";" + y + ")" + ",walkable:" + IsWalkable();
    }



}
