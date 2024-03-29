﻿using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization
{
    public static string saveFolderName = "voxelGameSaves";

    public static string SaveLocation(string worldName)
    {
        string saveLocation = saveFolderName + "/" + worldName + "/";

        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        return saveLocation;
    }

    public static string FileName(WorldPos chunkLocation)
    {
        string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
        return fileName;
    }

    public static void SaveChunk(Chunk chunk)
    {
        //chunk.SetBlocksUnmodified();
        Save save = new Save(chunk);
        if (save.blocks.Count == 0)
        {
            Debug.Log("no blocks");
            return;
        }

        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.pos);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, save);
        stream.Close();
        //Debug.Log("saved chunk");
    }

    public static void SavePlayer(InventoryManager player)
    {
        //chunk.SetBlocksUnmodified();
        Save save = new Save(player);
        if (save.inventory.Count == 0)
        {
            Debug.Log("no inventory");
            return;
        }

        string saveFile = SaveLocation("player");
        saveFile += player.name + ".bin";

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, save);
        stream.Close();
        //Debug.Log("saved chunk");
    }

    public static bool Load(Chunk chunk)
    {
        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.pos);

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        Save save = (Save)formatter.Deserialize(stream);
        foreach (var block in save.blocks)
        {
            chunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
        }
        stream.Close();

        //Once set new loaded blocks, reload the chunk to display them
        chunk.update = true;

        return true;
    }

    public static bool Load(InventoryManager player)
    {
        string saveFile = SaveLocation("player");
        saveFile += player.name + ".bin";

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        Save save = (Save)formatter.Deserialize(stream);
        //player.item1count = save.inventory[0];
        //player.item2count = save.inventory[1];
        //player.item3count = save.inventory[2];
        stream.Close();

        return true;
    }
}