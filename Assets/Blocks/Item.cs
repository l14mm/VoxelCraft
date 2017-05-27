using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isReady = false;
    public int type;

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
            if(type == 1)
                col.gameObject.GetComponent<InventoryManager>().item1count++;
            else if (type == 2)
                col.gameObject.GetComponent<InventoryManager>().item2count++;
            else if (type == 3)
                col.gameObject.GetComponent<InventoryManager>().item3count++;
            Destroy(gameObject);
        }
    }
}
