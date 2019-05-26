﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{

    // Use this for initialization

    public PostProcessingProfile blurProfile;
    public PostProcessingProfile normalProfile;
    public PostProcessingBehaviour cameraPostProcess;

    public void EnableCameraBlur(bool state)
    {
        if (cameraPostProcess != null && blurProfile != null && normalProfile != null)
        {
            cameraPostProcess.profile = (state) ? blurProfile : normalProfile;
        }
    }
    
}
