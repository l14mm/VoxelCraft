using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modify : MonoBehaviour
{
    public Camera camera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 100))
            {
                EditTerrain.SetBlock(hit, new BlockAir());
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 100))
            {
                //EditTerrain.SetBlock(hit, new BlockSand());
                // Get block to the side which faces the player
                EditTerrain.SetSideBlock(hit, new BlockSand());
            }
        }
    }
}