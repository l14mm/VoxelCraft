using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modify : MonoBehaviour
{
    public Camera camera;
    public Animator anim;
    public GameObject item_grass;
    public GameObject item_sand;
    public GameObject item_wood;
    public GameObject _arrow;
    public Transform arrowFirePosition;

    private Block currentBlock = new BlockGrass();

    void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            currentBlock = new BlockGrass();
            GetComponent<InventoryManager>().SelectItem(1);
        }
        else if (Input.GetKeyDown("2"))
        {
            currentBlock = new BlockSand();
            GetComponent<InventoryManager>().SelectItem(2);
        }
        else if (Input.GetKeyDown("3"))
        {
            currentBlock = new BlockWood();
            GetComponent<InventoryManager>().SelectItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject temp;
            if (currentBlock is BlockGrass && GetComponent<InventoryManager>().item1count > 0)
            {
                temp = Instantiate(item_grass, transform.position + transform.forward, transform.rotation);
                temp.GetComponent<Rigidbody>().AddForce(transform.forward * 3 + new Vector3(0, 1, 0), ForceMode.Impulse);
                GetComponent<InventoryManager>().item1count--;
            }
            else if (currentBlock is BlockSand && GetComponent<InventoryManager>().item2count > 0)
            {
                temp = Instantiate(item_sand, transform.position + transform.forward, transform.rotation);
                temp.GetComponent<Rigidbody>().AddForce(transform.forward * 3 + new Vector3(0, 1, 0), ForceMode.Impulse);
                GetComponent<InventoryManager>().item2count--;
            }
            else if (currentBlock is BlockWood && GetComponent<InventoryManager>().item3count > 0)
            {
                temp = Instantiate(item_wood, transform.position + transform.forward, transform.rotation);
                temp.GetComponent<Rigidbody>().AddForce(transform.forward * 3 + new Vector3(0, 1, 0), ForceMode.Impulse);
                GetComponent<InventoryManager>().item3count--;
            }
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
                if (currentBlock is BlockGrass && GetComponent<InventoryManager>().item1count > 0)
                {
                    EditTerrain.SetSideBlock(hit, currentBlock, false, true);
                    GetComponent<InventoryManager>().item1count--;
                }
                else if (currentBlock is BlockSand && GetComponent<InventoryManager>().item2count > 0)
                {
                    EditTerrain.SetSideBlock(hit, currentBlock, false, true);
                    GetComponent<InventoryManager>().item2count--;
                }
                else if (currentBlock is BlockWood && GetComponent<InventoryManager>().item3count > 0)
                {
                    EditTerrain.SetSideBlock(hit, currentBlock, false, true);
                    GetComponent<InventoryManager>().item3count--;
                }
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