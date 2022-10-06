using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameras : MonoBehaviour
{
    CinemachineStateDrivenCamera statesCamera;
    CinemachineVirtualCamera runCamera;
    CinemachineVirtualCamera idleCamera;
    CinemachineVirtualCamera climbingCamera;
    CinemachineVirtualCamera deathCamera;
    Collider2D bounds;

    GameObject player;
    Animator playerAnimator;

    List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    List<CinemachineConfiner> confiners = new List<CinemachineConfiner>();

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
        statesCamera = FindObjectOfType<CinemachineStateDrivenCamera>();
        statesCamera.m_AnimatedTarget = playerAnimator;
        runCamera = GameObject.Find("Run Camera").GetComponent<CinemachineVirtualCamera>();
        idleCamera = GameObject.Find("Idle Camera").GetComponent<CinemachineVirtualCamera>();
        climbingCamera = GameObject.Find("Climbing Camera").GetComponent<CinemachineVirtualCamera>();
        deathCamera = GameObject.Find("Death Camera").GetComponent<CinemachineVirtualCamera>();
        bounds = GameObject.Find("Background Tilemap").GetComponent<Collider2D>();
        cameras.Add(runCamera);
        cameras.Add(idleCamera);
        cameras.Add(climbingCamera);
        cameras.Add(deathCamera);
    }

    void Update()
    {
        for (int i = 0; i < cameras.Count - 1; i++)
        {
            cameras[i].Follow = player.transform;
        }
        for (int i = 0; i < cameras.Count; i++)
        {
            confiners.Add(cameras[i].GetComponent<CinemachineConfiner>());
            confiners[i].m_BoundingShape2D = bounds;
        }
    }
}
