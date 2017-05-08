using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditTerrain
{
    public static WorldPos GetBlockPos(Vector3 pos)
    {
        WorldPos blockPos = new WorldPos(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y),
            Mathf.RoundToInt(pos.z)
            );

        return blockPos;
    }
    public static WorldPos GetBlockPos(RaycastHit hit, bool adjacent = false)
    {
        // Returns world hit point on block
        //Debug.DrawRay(hit.point, hit.normal * 1, Color.red, 5);
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
            );
        return GetBlockPos(pos);
    }
    static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
    {
        if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
        {
            if (adjacent)
            {
                pos += (norm / 2);
            }
            else
            {
                pos -= (norm / 2);
            }
        }

        return (float)pos;
    }
    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;

        WorldPos pos = GetBlockPos(hit, adjacent);

        chunk.world.SetBlock(pos.x, pos.y, pos.z, block);

        return true;
    }
    public static bool SetSideBlock(RaycastHit hit, Block block, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;

        WorldPos pos = GetBlockPos(hit, adjacent);

        // Check if side block is air, then set
        if (chunk.world.GetBlock(pos.x + (int)hit.normal.x, pos.y + (int)hit.normal.y, pos.z + (int)hit.normal.z) is BlockAir)
            chunk.world.SetBlock(pos.x + (int)hit.normal.x, pos.y + (int)hit.normal.y, pos.z + (int)hit.normal.z, block);
        

        return true;
    }
    public static Block GetBlock(RaycastHit hit, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;

        WorldPos pos = GetBlockPos(hit, adjacent);

        Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);

        return block;
    }
}
