using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Item[] inventory = new Item[12];
    public List<HUDInventorySlot> slots = new List<HUDInventorySlot>();
    private int currentInventoryIndex;
    public GameObject currentTool = null;
    public int selectedIndex = 0;
    public bool isStorageEnabled;
    public GameObject inventoryStorage;
    public List<InventorySlot> inv = new List<InventorySlot>();

    private Image dragImage = null;
    private HUDInventorySlot sourceSlot = null;

    void Start()
    {
        for(int i = 0; i < 12; i++)
        {
            InventorySlot temp = new InventorySlot();
            temp.itemType = inventory[i];
            temp.count = 1;
            temp.HUDSlot = slots[i];
            inv.Add(temp);
        }
        currentInventoryIndex = 0;
        SelectItem(currentInventoryIndex);

        inventoryStorage.SetActive(false);
        isStorageEnabled = false;


        inventoryStorage.SetActive(false);
        isStorageEnabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GetComponent<UnityStandardAssets.Characters.FirstPerson.Player>().enabled = true;

        //Serialization.Load(this);
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = true;
        //Cursor.SetCursor(cursorTex, new Vector2(Screen.width / 2, Screen.height / 2), CursorMode.Auto);
    }

    public void SelectItem(int index)
    {
        if (currentTool != null)
            Destroy(currentTool);
        // Deselect all other slots
        foreach (HUDInventorySlot slot in slots)
        {
            slot.selector.enabled = false;
        }
        // Select one
        slots[index].selector.enabled = true;

        if (inv[index].itemType && inv[index].itemType.isTool)
        {
            //Debug.Log("instantiated: " + inventory[index].name);
            currentTool = Instantiate(inv[index].itemType.gameObject, Camera.main.transform);
        }
        else if (inv[index].itemType)
        {
            currentTool = Instantiate(inv[index].itemType.gameObject, Camera.main.transform);
            currentTool.GetComponent<Rigidbody>().isKinematic = true;
            currentTool.GetComponent<BoxCollider>().enabled = false;
            selectedIndex = index;
        }
    }

    public void PickupItem(Item.Items type)
    {
        // Find first item of same type in inventory
        int index = -1;
        for(int i = 0; i < inv.Count; i++)
        {
            if(inv[i].itemType != null && inv[i].itemType.type == type)
            {
                index = i;
                break;
            }
        }
        //index = -1;
        if(index > -1)
        {
            inv[index].count++;
        }
        else
        {
            // Else find first empty slot and add new item
            for (int i = 0; i < inv.Count; i++)
            {
                if (inv[i].itemType == null)
                {
                    index = i;
                    break;
                }
                if(index > -1)
                {
                    inv[index].itemType = new Item();
                    inv[index].itemType.type = type;
                    inv[index].count = 1;
                }
            }
        }
    }

    void Update()
    {
        //item1counttext.text = item1count.ToString();
        for (int i = 0; i < inv.Count; i++)
        {
            if (inv[i].itemType)
            {
                inv[i].HUDSlot.icon.enabled = true;
                //slots[i].icon.sprite = inventory[i].sprite;
                inv[i].HUDSlot.icon.sprite = inv[i].itemType.sprite;
                inv[i].HUDSlot.count.text = inv[i].count.ToString();
                inv[i].HUDSlot.icon.enabled = true;
                inv[i].HUDSlot.count.enabled = true;
                // If item count is less then 1, remove item.
                if (inv[i].count < 1)
                {
                    inv[i].HUDSlot.icon.sprite = null;
                    inv[i].HUDSlot.count.text = null;
                    inv[i].HUDSlot.icon.enabled = false;
                    inv[i].itemType = null;
                    if(i == selectedIndex)
                    {
                        // If we are holding null block, remove from hand
                        Destroy(currentTool);
                    }
                }
            }
            else
            {
                inv[i].HUDSlot.icon.enabled = false;
                inv[i].HUDSlot.count.enabled = false;
            }
        }
        if(Input.GetMouseButtonDown(0) && isStorageEnabled)
        {
            // Get array of icon images and find the closest one to the mouse click
            GameObject[] images = GameObject.FindGameObjectsWithTag("InventorySlot");
            List<Image> icons = new List<Image>();
            foreach(GameObject image in images)
            {
                icons.Add(image.GetComponent<HUDInventorySlot>().icon);
            }
            float closestDistance = Mathf.Infinity;
            Image closestIcon = null;
            foreach(Image icon in icons)
            {
                float distance = Vector2.Distance(Input.mousePosition, icon.GetComponent<RectTransform>().position);
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIcon = icon;
                }
            }
            dragImage = closestIcon;
            sourceSlot = closestIcon.GetComponentInParent<HUDInventorySlot>();
        }
        if (dragImage && Input.GetMouseButton(0) && isStorageEnabled)
        {
            dragImage.GetComponent<RectTransform>().Translate(Input.mousePosition - dragImage.GetComponent<RectTransform>().position);
        }
        if (dragImage && Input.GetMouseButtonUp(0) && isStorageEnabled)
        {
            // Get array of icon images and find the closest one to the mouse click
            GameObject[] images = GameObject.FindGameObjectsWithTag("InventorySlot");
            List<Image> icons = new List<Image>();
            foreach (GameObject image in images)
            {
                icons.Add(image.GetComponent<HUDInventorySlot>().selector);
            }
            float closestDistance = Mathf.Infinity;
            Image closestIcon = null;
            foreach (Image icon in icons)
            {
                float distance = Vector2.Distance(Input.mousePosition, icon.GetComponent<RectTransform>().position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIcon = icon;
                }
            }
            // Check if icon has been dragged inside the closest icon
            if((dragImage.GetComponent<RectTransform>().position.x < closestIcon.GetComponent<RectTransform>().position.x + closestIcon.GetComponent<RectTransform>().sizeDelta.x)
                && (dragImage.GetComponent<RectTransform>().position.x > closestIcon.GetComponent<RectTransform>().position.x - closestIcon.GetComponent<RectTransform>().sizeDelta.x)
                && (dragImage.GetComponent<RectTransform>().position.y < closestIcon.GetComponent<RectTransform>().position.y + closestIcon.GetComponent<RectTransform>().sizeDelta.y)
                && (dragImage.GetComponent<RectTransform>().position.y > closestIcon.GetComponent<RectTransform>().position.y - closestIcon.GetComponent<RectTransform>().sizeDelta.y)
                )
            {
                /*
                // Put drag icon in new slot
                dragImage.GetComponent<RectTransform>().position = closestIcon.GetComponent<RectTransform>().position;
                dragImage.transform.parent = closestIcon.transform.parent;
                // Put new icon in old slot (swap)
                closestIcon.GetComponent<RectTransform>().position = sourceSlot.selector.GetComponent<RectTransform>().position;
                closestIcon.transform.parent.GetComponent<HUDInventorySlot>().icon.transform.parent = sourceSlot.transform;
                */
                //dragImage.GetComponent<RectTransform>().Translate(Input.mousePosition - dragImage.GetComponent<RectTransform>().position);
                dragImage.GetComponent<RectTransform>().position = sourceSlot.selector.GetComponent<RectTransform>().position;
                int i = slots.IndexOf(dragImage.transform.parent.GetComponent<HUDInventorySlot>());
                int j = slots.IndexOf(closestIcon.transform.parent.GetComponent<HUDInventorySlot>());

                // Swap i and j
                InventorySlot temp = new InventorySlot();
                if (inv[i].itemType == inv[j].itemType)
                {
                    // If both slots are the same item type, combine the items and the counts
                    inv[j].count += inv[i].count;
                    inv[i].itemType = null;
                    inv[i].count = 0;
                }
                else
                {
                    if (inv[i].itemType)
                    {
                        temp.itemType = inv[i].itemType;
                        temp.count = inv[i].count;
                    }
                    else
                    {
                        temp.itemType = null;
                        temp.count = 0;
                    }
                    if (inv[j].itemType)
                    {
                        inv[i].itemType = inv[j].itemType;
                        inv[i].count = inv[j].count;
                    }
                    else
                    {
                        inv[i].itemType = null;
                        inv[i].count = 0;
                    }
                    inv[j].itemType = temp.itemType;
                    inv[j].count = temp.count;
                }
            }
            else
            {
                // Else return to original position
                dragImage.GetComponent<RectTransform>().position = sourceSlot.selector.GetComponent<RectTransform>().position;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!isStorageEnabled)
            {
                inventoryStorage.SetActive(true);
                isStorageEnabled = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                GetComponent<UnityStandardAssets.Characters.FirstPerson.Player>().enabled = false;
            }
            else
            {
                inventoryStorage.SetActive(false);
                isStorageEnabled = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                GetComponent<UnityStandardAssets.Characters.FirstPerson.Player>().enabled = true;
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
