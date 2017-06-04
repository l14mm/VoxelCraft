using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Item[] inventory = new Item[6];
    public HUDInventorySlot[] slots = new HUDInventorySlot[6];
    private int currentInventoryIndex;
    public GameObject currentTool = null;
    public bool isStorageEnabled;
    public GameObject inventoryStorage;

    void Start()
    {
        for(int i = 0; i < 6; i++)
        {
            //inventory[i] = null;
        }
        currentInventoryIndex = 0;
        SelectItem(currentInventoryIndex);

        inventoryStorage.SetActive(false);
        isStorageEnabled = false;

        //Serialization.Load(this);
    }

    public void SelectItem(int index)
    {
        // Deselect all other slots
        foreach(HUDInventorySlot slot in slots)
        {
            slot.selector.enabled = false;
        }
        // Select one
        slots[index].selector.enabled = true;

        if(inventory[index] && inventory[index].isTool)
        {
            if (currentTool != null)
                Destroy(currentTool);
            Debug.Log("instantiated: " + inventory[index].name);
            currentTool = Instantiate(inventory[index].gameObject, Camera.main.transform);
        }
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //item1counttext.text = item1count.ToString();
        for (int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i] != null)
            {
                slots[i].icon.sprite = inventory[i].sprite;
                slots[i].icon.enabled = true;
            }
            else
                slots[i].icon.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!isStorageEnabled)
            {
                inventoryStorage.SetActive(true);
                isStorageEnabled = true;
                //Cursor.visible = true;
                //Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                inventoryStorage.SetActive(false);
                isStorageEnabled = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Serialization.SavePlayer(this);
        }

        // Player can change current item with scroll wheel, with bounds of 0 and 5
        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d < 0f)
        {
            if(currentInventoryIndex < 5)
                currentInventoryIndex++;
            SelectItem(currentInventoryIndex);
        }
        else if (d > 0f)
        {
            if (currentInventoryIndex > 0)
                currentInventoryIndex--;
            SelectItem(currentInventoryIndex);
        }
    }
}
