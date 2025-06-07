using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSquare : MonoBehaviour
{

    public Transform targetSphere; // Hedef sphere (dönülecek oda)

    private void OnMouseDown() { OnPortalTriggered(); }

    public void OnPortalTriggered()
    {
        if (targetSphere != null)
        {
            // SphereChanger bulup, hedef sphere'a fade geçiş yap
            SphereChanger changer = FindObjectOfType<SphereChanger>();
            if (changer != null)
            {
                changer.ChangeSphere(targetSphere);
            }
            else
            {
                // Yedek: Eğer bulamazsa yine de kamera rig'i doğrudan taşı
                Camera.main.transform.parent.position = targetSphere.position;
            }
        }
    }

}
