using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCanvasVisibility : MonoBehaviour
{
    public GameObject canvasObject; // Canvas'ı referans alacağımız GameObject

    public void ToggleCanvas()
    {
        // Canvas'ı aktif veya inaktif yapıyoruz
        canvasObject.SetActive(!canvasObject.activeSelf);
    }
}
