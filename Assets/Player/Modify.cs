using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modify : MonoBehaviour
{
    public Camera camera;
    public Animator anim;
    public GameObject item_grass;
    public GameObject _arrow;
    public Transform arrowFirePosition;

    private Block currentBlock = new BlockGrass();

    void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            currentBlock = new BlockGrass();
        }
        else if (Input.GetKeyDown("2"))
        {
            currentBlock = new BlockSand();
        }
        else if (Input.GetKeyDown("3"))
        {
            currentBlock = new BlockWood();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetComponent<InventoryManager>().item1count--;
            GameObject temp = Instantiate(item_grass, transform.position, transform.rotation);
            temp.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 1));
        }
        if (Input.GetMouseButtonDown(0))
        {
            //Mine();
            FireArrow();
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 5))
            {
                //EditTerrain.SetBlock(hit, new BlockSand());
                // Get block to the side which faces the player
                //EditTerrain.SetSideBlock(hit, new BlockSand(), false, true);
                EditTerrain.SetSideBlock(hit, currentBlock, false, true);
            }
        }
    }

    void FireArrow()
    {
        // Rotate rotation to match forward of bow
        GameObject arrow = Instantiate(_arrow, arrowFirePosition.position, arrowFirePosition.rotation * Quaternion.Euler(0, 180, 0));

        //arrow.transform.forward = arrowFirePosition.forward;
        float force = 10;
        arrow.GetComponent<Rigidbody>().AddForce(arrowFirePosition.forward * force, ForceMode.Impulse);
    }

    void Mine()
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
}