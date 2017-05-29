using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BlockSand : Block {
    
    public BlockSand()
        : base()
    {
        isSand = true;
    }
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 3;
        tile.y = 2;
        return tile;
    }
    public void CheckBelow()
    {
        //RaycastHit hit;
        //if(Physics.Raycast())
    }
}
