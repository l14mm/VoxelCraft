using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isReady = false;
    public bool isTool = false;
    public Items type;
    public Sprite sprite;
    public string name;

    public enum Items
    {
        grass, sand, wood, pick, bow
    }

    void Start()
    {
        StartCoroutine(SetLanded());
    }

    private IEnumerator SetLanded()
    {
        yield return new WaitForSeconds(1.0f);
        isReady = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player" && isReady)
        {
            col.gameObject.GetComponent<InventoryManager>().PickupItem(type);
            Destroy(gameObject);
        }
    }
}
