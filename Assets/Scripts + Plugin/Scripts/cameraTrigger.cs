﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class cameraTrigger : MonoBehaviour
{
    
    Camera m_mainCamera;
    public GameObject cameraDolly;

    private CameraPathPosition m_cameraPathPosition;

    List<LocalCamera> m_LocalCameras;

    public float nearPlane;

    public float dollyEaseTime;
    public Ease dollyEaseType;

    public float zoomEaseTime;
    public Ease zoomEaseType;



    private void Awake() {

        m_mainCamera = Object.FindObjectOfType<Camera>().GetComponent<Camera>();

        m_cameraPathPosition = Object.FindObjectOfType<CameraPathPosition>().GetComponent<CameraPathPosition>();

        LocalCamera[] LocalCameras = FindObjectsOfType<LocalCamera>() as LocalCamera[];
        m_LocalCameras = LocalCameras.ToList();

        foreach (LocalCamera localCamera in m_LocalCameras) {
            if (localCamera.localCameraID == 0) {
                localCamera.onAir = true;
            }
            else {
                localCamera.onAir = false;
            }
        }

        Debug.Log(m_LocalCameras);
    }

    [SerializeField]
    private int _cameraID;
    public int CameraID { get { return _cameraID; }}

    [SerializeField]
    private Vector3 _offset;
    public Vector3 Offset { get { return _offset; } set { _offset = value; } }
    
    [SerializeField]
    private float _rotationX;
    public float RotationX { get { return _rotationX; } }

    [SerializeField]
    private float _rotationY;
    public float RotationY { get { return _rotationY; } }

    [SerializeField]
    private float _rotationZ;
    public float RotationZ { get { return _rotationZ; } }

    [SerializeField]
    private float _localFOV;
    public float LocalFOV { get { return _localFOV; } }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Player") {
            foreach (LocalCamera localCamera in m_LocalCameras) {
                if (!localCamera.onAir && localCamera.localCameraID == CameraID) {
                    StartCoroutine(TwiningManager());
                    cameraDolly.GetComponent<cameraFollow>().offset = Offset;
                    CameraZoom();
                    SetNearPlane();
                    cameraDolly.GetComponent<cameraFollow>().transform.rotation = Quaternion.Euler(RotationX , RotationY , RotationZ); 
                    //m_mainCamera.transform.rotation = Quaternion.Euler(RotationX, RotationY, RotationZ);
                }
            }
        }   
    }

    private void resetOtherCameras(int n) {
        foreach (LocalCamera localCamera in m_LocalCameras) {
            if (localCamera.onAir && localCamera.localCameraID != n) {
                localCamera.onAir = false;
            }
        }
    }



    IEnumerator TwiningManager(){
        foreach (LocalCamera localCamera in m_LocalCameras) {
            Debug.Log("TRIGGERED");
            if (localCamera.localCameraID == CameraID) {
                resetOtherCameras(CameraID);
                m_cameraPathPosition.cameraBond = false;
                m_cameraPathPosition.easeType = dollyEaseType;
                m_cameraPathPosition.easeTime = dollyEaseTime;
                localCamera.onAir = true;
                
                yield return new WaitForSeconds(m_cameraPathPosition.delay + dollyEaseTime);
                m_cameraPathPosition.cameraBond = true;
            }
            
        }
    }

    public void CameraZoom() {
        m_mainCamera.DOFieldOfView(LocalFOV, zoomEaseTime).SetEase(zoomEaseType);
    }

    public void SetNearPlane()
    {
        m_mainCamera.nearClipPlane = nearPlane;
    }
    

}
