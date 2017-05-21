using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private Text item1counttext;
    public int item1count;

    void Start()
    {
        item1counttext = GameObject.Find("Item1Count").GetComponent<Text>();
    }

    void Update()
    {
        item1counttext.text = item1count.ToString();
    }
}
