using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modify : MonoBehaviour
{
    public Camera camera;
    public Animator anim;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (anim)
                anim.SetTrigger("Mine");
               // anim.Play("Mining");
            RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 5))
            {
                EditTerrain.SetBlock(hit, new BlockAir(), false, true);
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
                EditTerrain.SetSideBlock(hit, new BlockSand(), false, true);
            }
        }
    }
}