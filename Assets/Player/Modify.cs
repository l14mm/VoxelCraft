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
    private float arrowCharge = 0;

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
        if (Input.GetMouseButton(0))
        {
            //Mine();
            ChargeArrow();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Mine();
        }
        if (Input.GetMouseButtonUp(0))
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

    void ChargeArrow()
    {
        arrowCharge += 0.05f;
    }

    void FireArrow()
    {
        // Rotate rotation to match forward of bow
        GameObject arrow = Instantiate(_arrow, arrowFirePosition.position, arrowFirePosition.rotation * Quaternion.Euler(0, 180, 0));
        
        float force = 5 * arrowCharge;
        arrowCharge = 0;
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
            Vector3 pos = new Vector3(EditTerrain.GetBlockPos(hit).x, EditTerrain.GetBlockPos(hit).y, EditTerrain.GetBlockPos(hit).z);
            Block block = EditTerrain.GetBlock(hit);
            EditTerrain.SetBlock(hit, new BlockAir(), false, true);
            if(block is BlockGrass)
                Instantiate(item_grass, pos, Quaternion.identity);
            else if (block is BlockSand)
                Instantiate(item_sand, pos, Quaternion.identity);
            else if (block is BlockWood)
                Instantiate(item_wood, pos, Quaternion.identity);
        }
    }
}