﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{

    LocalCamera _localCamera;

    public Transform followAt;
    public Vector3 offset;


    private void Awake() {
        _localCamera = Object.FindObjectOfType<LocalCamera>().GetComponent<LocalCamera>();
        transform.position = followAt.transform.position - offset;

    }



    void Update()
    {
        transform.position = followAt.transform.position - offset;
    }
}
