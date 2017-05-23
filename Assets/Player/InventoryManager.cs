using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private Text item1counttext;
    private Image item1selector;
    public int item1count;
    private Text item2counttext;
    private Image item2selector;
    public int item2count;
    private Text item3counttext;
    private Image item3selector;
    public int item3count;

    void Start()
    {
        item1count = 5;
        item2count = 5;
        item3count = 5;

        item1counttext = GameObject.Find("Item1Count").GetComponent<Text>();
        item2counttext = GameObject.Find("Item2Count").GetComponent<Text>();
        item3counttext = GameObject.Find("Item3Count").GetComponent<Text>();

        item1selector = GameObject.Find("Item1Selector").GetComponent<Image>();
        item1selector.enabled = false;
        item2selector = GameObject.Find("Item2Selector").GetComponent<Image>();
        item2selector.enabled = false;
        item3selector = GameObject.Find("Item3Selector").GetComponent<Image>();
        item3selector.enabled = false;
        SelectItem(1);
    }

    public void SelectItem(int item)
    {
        item1selector.enabled = false;
        item2selector.enabled = false;
        item3selector.enabled = false;
        if (item == 1)
            item1selector.enabled = true;
        else if (item == 2)
            item2selector.enabled = true;
        else if (item == 3)
            item3selector.enabled = true;
    }

    void Update()
    {
        item1counttext.text = item1count.ToString();
        item2counttext.text = item2count.ToString();
        item3counttext.text = item3count.ToString();
    }
}
