using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBoosterManager : MonoBehaviour
{
    GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            target.GetComponent<PlayerController>().isDashBoosted = true;
        }
    }
}
