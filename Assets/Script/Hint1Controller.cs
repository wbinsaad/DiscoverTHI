using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint1Controller : MonoBehaviour
{
    public GameObject BagObject;
    public GameObject EnterGamePopup;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-8, 8), 0, UnityEngine.Random.Range(-8, 8));
        Vector3 cameraPosition = Camera.main.transform.position;

        Instantiate(BagObject, (randomOffset + cameraPosition), Quaternion.identity);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mourse Clicked");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, 100))
            {

                if (hit.transform.tag == "YellowBag")
                {
                    EnterGamePopup.SetActive(true);
                }
            }
        }
    }
}