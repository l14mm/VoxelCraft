using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Item[] inventory = new Item[6];
    public HUDInventorySlot[] slots = new HUDInventorySlot[6];
    private int currentInventoryIndex;

    void Start()
    {
        currentInventoryIndex = 0;
        SelectItem(currentInventoryIndex);

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
    }

    void Update()
    {
        //item1counttext.text = item1count.ToString();
        for(int i = 0; i < 6; i++)
        {
            slots[i].icon.sprite = inventory[i].sprite;
        }

        if(Input.GetKeyDown(KeyCode.S))
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
