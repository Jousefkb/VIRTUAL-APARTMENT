using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereChanger : MonoBehaviour {



    //This object should be called 'Fader' and placed over the camera
    GameObject m_Fader;

    //This ensures that we don't mash to change spheres
    bool changing = false;


    void Awake()
    {

        //Find the fader object
        m_Fader = GameObject.Find("Fader");

        //Check if we found something
        if (m_Fader == null)
            Debug.LogWarning("No Fader object found on camera.");

    }


    public void ChangeSphere(Transform nextSphere)
    {

        //Start the fading process
        StartCoroutine(FadeCamera(nextSphere));

    }


    IEnumerator FadeCamera(Transform nextSphere)
    { 
        if (m_Fader != null)
        {
            var mat = m_Fader.GetComponent<Renderer>().material;

            // Fade in, then wait
            yield return StartCoroutine(FadeIn(0.75f, mat));

            // Move camera rig
            Camera.main.transform.parent.position = nextSphere.position;

            // Fade out, then wait
            yield return StartCoroutine(FadeOut(0.75f, mat));
        }
        else
        {
            Camera.main.transform.parent.position = nextSphere.position;
        }

    }
     
    IEnumerator FadeOut(float time, Material mat)
    {
        // Gradually decrease alpha until ≤0.0
        while (mat.color.a > 0.0f)
        {
            Color c = mat.color;
            float a = c.a - (Time.deltaTime / time);
            mat.color = new Color(c.r, c.g, c.b, Mathf.Max(a, 0.0f));
            yield return null;
        }
        // Ensure exact 0.0
        Color finalOut = mat.color;
        mat.color = new Color(finalOut.r, finalOut.g, finalOut.b, 0.0f);
    }

     
    IEnumerator FadeIn(float time, Material mat)
    {
        // Gradually increase alpha until ≥1.0
        while (mat.color.a < 1.0f)
        {
            Color c = mat.color;
            float a = c.a + (Time.deltaTime / time);
            mat.color = new Color(c.r, c.g, c.b, Mathf.Min(a, 1.0f));
            yield return null;
        }
        // Ensure exact 1.0
        Color finalIn = mat.color;
        mat.color = new Color(finalIn.r, finalIn.g, finalIn.b, 1.0f);
    }



}
