using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour {

    public Block[,,] blocks = new Block[chunkSize, chunkSize, chunkSize];
    public static int chunkSize = 16;
    public bool update = false;
    //private bool updating = false;
    public World world;
    public WorldPos pos;
    public bool rendered;

    MeshFilter filter;
    MeshCollider coll;

    public GameObject item_grass;
    public GameObject item_sand;
    public GameObject item_wood;

    private float lastTimeUpdated = 0;
    private float updateInterval = 0.5f;

    void OnApplicationQuit()
    {
    }
    public void SaveChunk()
    {
        Serialization.SaveChunk(this);
    }
    //Use this for initialization
    void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
    }
    void Update()
    {
        if (Time.time < lastTimeUpdated + updateInterval)
            return;
        if (Time.deltaTime < 0.035)
            return;
        if (update)
        {
            update = false;
            UpdateChunk();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            //SaveChunk();
        }
        lastTimeUpdated = Time.time; 
    }
    public Block GetBlock(int x, int y, int z)
    {
        if (InRange(x) && InRange(y) && InRange(z))
            return blocks[x, y, z];
        return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
    }
    public static bool InRange(int index)
    {
        if (index < 0 || index >= chunkSize)
            return false;

        return true;
    }
    public void SetBlockItemDrop(int x, int y, int z, Block block, bool changed = false)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            blocks[x, y, z] = block;

            // Drop item of block destroyed
            //Debug.Log(x + " " + y + " " + z);
            //Instantiate(item_grass)

            if (changed)
            {
                blocks[x, y, z].changed = changed;
            }
        }
        else
        {
            world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
        }
    }
    public void SetBlock(int x, int y, int z, Block block, bool changed = false)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            blocks[x, y, z] = block;

            if (changed)
            {
                blocks[x, y, z].changed = changed;
            }
        }
        else
        {
            world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
        }
    }
    
    IEnumerator MoveSand(Block sand, int x, int y, int z)
    {
        if (sand.moving)
            yield break;
        sand.moving = true;
        yield return new WaitForSeconds(0.001f);
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.015f);
            sand.y_offset -= 0.1f;
            update = true;
        }
        // When sand have visually hit the ground, actually move the block
        SetBlock(x, y, z, new BlockAir());
        SetBlock(x, y - 1, z, new BlockSand());
        sand.y_offset = 0;
        sand.moving = false;
        update = true;
    }
    // Updates the chunk based on its contents
    void UpdateChunk()
    {
        //updating = true;
        rendered = true;
        MeshData meshData = new MeshData();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    if (blocks[x, y, z].isSand)
                    {
                        //Vector3 chunkLocation = new Vector3(pos.x, pos.y, pos.z);
                        //Vector3 blockLocation = chunkLocation + new Vector3(x, y, z);

                        // If block below sand is air, fall down
                        if (blocks[x, y - 1, z].isAir && !blocks[x, y - 1, z].moving)
                        {
                            StartCoroutine(MoveSand(blocks[x, y, z], x, y, z));
                        }
                    }

                    meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                }
            }
        }
        RenderMesh(meshData);
        //updating = false;
    }
    // Sends the calculated mesh information
    // to the mesh and collision components
    void RenderMesh(MeshData meshData)
    {
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();

        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.RecalculateNormals();

        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        coll.sharedMesh = mesh;
    }
    public void SetBlocksUnmodified()
    {
        foreach (Block block in blocks)
        {
            block.changed = false;
        }
    }
}
