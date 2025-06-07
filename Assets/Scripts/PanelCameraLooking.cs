using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCameraLooking : MonoBehaviour
{



    public Camera mainCamera;

    void LateUpdate()
    {
        if (mainCamera == null)
            mainCamera = Camera.main; // Kamerayı bul

        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 0, 0); // Menü düzgün görünsün diye döndürme
    }
}
