using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform player;

    Vector3 offset;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = player.position.x + offset.x;
        // if (player.position.y >= 1.5)
        // {
            pos.y = player.position.y + offset.y;
        // }
        transform.position = pos;
    }
}
