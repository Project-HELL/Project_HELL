using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Camera camera;

    [Range(0.01f, 20)]
    public float moveSpeed = 1f;

    public float maxPos, minPos;

    // Update is called once per frame
    void Update()
    {
        Vector3 fixedPos = camera.transform.position; 
        fixedPos.y = Math.Min(Math.Max(transform.position.y, minPos), maxPos);

        camera.transform.position = Vector3.Lerp(camera.transform.position, fixedPos, Time.deltaTime * moveSpeed);
    }
}
