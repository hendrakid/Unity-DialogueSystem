using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    Transform player;
    float angle = 25;
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    void LateUpdate()
    {
        var newTransform = new Vector3(player.position.x, player.position.y, player.position.z);
        transform.position = newTransform;

        var rotationVector = transform.rotation.eulerAngles;
        rotationVector.x = angle;

        transform.rotation = Quaternion.Euler(rotationVector);
    }
}
