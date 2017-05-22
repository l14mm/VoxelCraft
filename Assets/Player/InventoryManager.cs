using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private Text item1counttext;
    public int item1count;
    private Text item2counttext;
    public int item2count;
    private Text item3counttext;
    public int item3count;

    void Start()
    {
        item1counttext = GameObject.Find("Item1Count").GetComponent<Text>();
        item2counttext = GameObject.Find("Item2Count").GetComponent<Text>();
        item3counttext = GameObject.Find("Item3Count").GetComponent<Text>();
    }

    void Update()
    {
        item1counttext.text = item1count.ToString();
        item2counttext.text = item2count.ToString();
        item3counttext.text = item3count.ToString();
    }
}
