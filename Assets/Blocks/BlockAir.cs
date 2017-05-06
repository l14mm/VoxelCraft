﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAir : Block
{
    public BlockAir()
        : base()
    {
        isAir = true;
    }
    public override MeshData Blockdata
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        return meshData;
    }
    public override bool IsSolid(Block.Direction direction)
    {
        return false;
    }
}
