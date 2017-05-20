using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isReady = false;

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

            Destroy(gameObject);
        }
    }
}
