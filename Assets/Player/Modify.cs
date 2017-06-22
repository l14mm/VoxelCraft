using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modify : MonoBehaviour
{
    public Camera myCam;
    public Animator anim;
    public GameObject item_grass;
    public GameObject item_sand;
    public GameObject item_wood;
    public GameObject _arrow;
    public Transform arrowFirePosition;
    private float arrowCharge = 0;

    private Block currentBlock = new BlockGrass();

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown("1"))
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
            if (GetComponent<InventoryManager>().currentTool.GetComponent<Item>().type == Item.Items.grass)
            {
                temp = Instantiate(item_grass, transform.position + transform.forward, transform.rotation);
                temp.GetComponent<Rigidbody>().AddForce(transform.forward * 3 + new Vector3(0, 1, 0), ForceMode.Impulse);
                //GetComponent<InventoryManager>().
            }
            else if (GetComponent<InventoryManager>().currentTool.GetComponent<Item>().type == Item.Items.sand)
            {
                temp = Instantiate(item_sand, transform.position + transform.forward, transform.rotation);
                temp.GetComponent<Rigidbody>().AddForce(transform.forward * 3 + new Vector3(0, 1, 0), ForceMode.Impulse);
            }
            else if (GetComponent<InventoryManager>().currentTool.GetComponent<Item>().type == Item.Items.wood)
            {
                temp = Instantiate(item_wood, transform.position + transform.forward, transform.rotation);
                temp.GetComponent<Rigidbody>().AddForce(transform.forward * 3 + new Vector3(0, 1, 0), ForceMode.Impulse);
            }
        }
        if (Input.GetMouseButton(0))
        {
            if(GetComponent<InventoryManager>().currentTool && GetComponent<InventoryManager>().currentTool.GetComponent<Item>().name == "bow")
                ChargeArrow();
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (GetComponent<InventoryManager>().currentTool.GetComponent<Item>().name == "pick")
                Mine();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (GetComponent<InventoryManager>().currentTool.GetComponent<Item>().name == "bow")
                FireArrow();
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            if (Physics.Raycast(myCam.transform.position, myCam.transform.forward, out hit, 5))
            {
                //EditTerrain.SetBlock(hit, new BlockSand());
                // Get block to the side which faces the player
                //EditTerrain.SetSideBlock(hit, new BlockSand(), false, true);
                
                if (GetComponent<InventoryManager>().currentTool.GetComponent<Item>().type == Item.Items.grass)
                {
                    EditTerrain.SetSideBlock(hit, new BlockGrass(), false, true);
                }
                else if (GetComponent<InventoryManager>().currentTool.GetComponent<Item>().type == Item.Items.sand)
                {
                    EditTerrain.SetSideBlock(hit, new BlockSand(), false, true);
                }
                else if (GetComponent<InventoryManager>().currentTool.GetComponent<Item>().type == Item.Items.wood)
                {
                    EditTerrain.SetSideBlock(hit, new BlockWood(), false, true);
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
        arrowFirePosition = GetComponent<InventoryManager>().currentTool.transform.FindChild("ArrowFirePosition");
        GameObject arrow = Instantiate(_arrow, arrowFirePosition.position, arrowFirePosition.rotation * Quaternion.Euler(0, 180, 0));
        
        float force = 10 * arrowCharge;
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
        if (Physics.Raycast(myCam.transform.position, myCam.transform.forward, out hit, 5))
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