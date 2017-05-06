using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modify : MonoBehaviour
{

    Vector2 rot;
    public Camera camera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 100))
            {
                EditTerrain.SetBlock(hit, new BlockAir());
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 100))
            {
                EditTerrain.SetBlock(hit, new BlockSand());
            }
        }
        /*
        rot = new Vector2(
            rot.x + Input.GetAxis("Mouse X") * 3,
            rot.y + Input.GetAxis("Mouse Y") * 3);

        transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);

        transform.position += transform.forward * 3 * Input.GetAxis("Vertical");
        transform.position += transform.right * 3 * Input.GetAxis("Horizontal");
        */
    }
}