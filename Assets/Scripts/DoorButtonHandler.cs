using UnityEditor;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DoorButtonHandler : MonoBehaviour
{
    [Header("Prefabs & Buttons")]
    public GameObject square;                // Inspector’dan atayacağınız square prefab’ı
    public GameObject spherePrefab;          // sphere prefab
    public Button doorButton;                // “I” tuşunu simüle eden veya tıklanan buton
    public GameObject PanelConfirmDoor;      // “Kapınız koyuldu…” paneli
    public Button confirmSureButton;         // “Evet” onay butonu
    public Button confirmNotSureButton;      // “Hayır” iptal butonu
    private GameObject currentSphere;       // Mevcut içinde olduğumuz sphere
    private GameObject previousSphere;      // Bir sonraki sphere yaratılırken eskiyi tutacak
    public SphereCreator sphereCreator;  // Inspector’da atayacağın referans

    [Header("Camera Tracking")]
    public Transform cameraTransform;        // MainCamera Transform

    private GameObject spawnedSquare;        // Son oluşturulan square instance’ı
    private bool lastConfirmed = false;      // Önceki karenin onay durumu

    private GameObject newSphere;            // Son oluşturulan sphere

    void Start()
    {
        // Door button tıklayınca kare koyma akışı
        if (currentSphere == null && sphereCreator.lastCreatedSphere != null)
            currentSphere = sphereCreator.lastCreatedSphere;

        doorButton.onClick.AddListener(OnDoorClicked);

        // Panel butonları
        confirmSureButton.onClick.AddListener(OnSureClicked);
        confirmNotSureButton.onClick.AddListener(OnNotSureClicked);

        // Başlangıçta panel kapalı
        PanelConfirmDoor.SetActive(false);
    }

    void Update()
    {
        // “I” tuşu da doorButton tıklaması gibi çalışsın
        if (Input.GetKeyDown(KeyCode.I))
            doorButton.onClick.Invoke();

        if (Input.GetKeyDown(KeyCode.V))
            confirmNotSureButton.onClick.Invoke();

        if (Input.GetKeyDown(KeyCode.C))
            OnCreateSphereWithPhoto();


    }

    void OnCreateSphereWithPhoto()
    {
        // Önce eski panelden çık
        PanelConfirmDoor.SetActive(false);

#if UNITY_EDITOR
        ShowEditorFilePicker();
#else
        Debug.LogWarning("Bu platformda dosya seçimi desteklenmiyor.");
#endif
    }
#if UNITY_EDITOR
    void ShowEditorFilePicker()
    {
        string path = EditorUtility.OpenFilePanel(
            "360° Fotoğraf Seç",
            Application.dataPath,
            "png,jpg,jpeg"
        );
        if (!string.IsNullOrEmpty(path))
            StartCoroutine(LoadTextureFromPath(path));
    }
#endif
    IEnumerator LoadTextureFromPath(string filePath)
    {
        previousSphere = currentSphere;

        Transform origRigTransform = sphereCreator.rigTransform;
        sphereCreator.rigTransform = this.transform;
        // Sphere oluştur
        sphereCreator.CreateSphere();
        sphereCreator.messageBox.SetActive(false);
        newSphere = sphereCreator.lastCreatedSphere;
        currentSphere = newSphere;
        sphereCreator.rigTransform = origRigTransform;


        // Texture yükle
        var www = new WWW("file:///" + filePath);
        yield return www;
        byte[] bytes = www.bytes;
        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, true);
        tex.LoadImage(bytes);
        tex.Apply(true, false);
        tex.filterMode = FilterMode.Trilinear;
        tex.anisoLevel = 9;
        var rend = newSphere.GetComponent<Renderer>();
        if (rend != null)
            rend.material.SetTexture("_MainTex", tex);

        if (spawnedSquare != null)
        {
            var forwardPortal = spawnedSquare.GetComponent<PortalSquare>();
            forwardPortal.targetSphere = newSphere.transform;
        }

        // --- Eski dünyaya otomatik kapı oluştur ---
        CreateReturnPortal();
    }
    void CreateReturnPortal()
    {
        if (previousSphere == null)
            return;

        Vector3 returnPos = newSphere.transform.position + newSphere.transform.forward * 2.38f;
        GameObject returnSquare = Instantiate(
            square, returnPos,
            Quaternion.LookRotation(-newSphere.transform.forward),
            newSphere.transform
        );

        float parentScale = newSphere.transform.localScale.x;
        Vector3 originalScale = square.transform.localScale;
        returnSquare.transform.localScale = new Vector3(
            originalScale.x / parentScale,
            originalScale.y / parentScale,
            originalScale.z / parentScale
        );
        var portal = returnSquare.GetComponent<PortalSquare>();
        portal.targetSphere = previousSphere.transform;
        Debug.Log(previousSphere.name);

    }


    void OnDoorClicked()
    {
        if (PanelConfirmDoor.activeSelf) return;

        currentSphere = sphereCreator.lastCreatedSphere;

        //if (spawnedSquare != null && !lastConfirmed)
        //    Destroy(spawnedSquare); 

        // Yeni kare spawn
        Vector3 spawnPos = cameraTransform.position + cameraTransform.forward * 2.3f;
        spawnedSquare = Instantiate(square, spawnPos, Quaternion.LookRotation(cameraTransform.forward));
        lastConfirmed = false;
        // Onay panelini göster
        PanelConfirmDoor.SetActive(true);

        // Kareyi siyah ve opak yap
        var spr = spawnedSquare.GetComponent<SpriteRenderer>();
        if (spr != null)
            spr.color = new Color(0f, 0f, 0f, 1f);
    }

    void OnSureClicked()
    {
        // Kareyi sabitle
        lastConfirmed = true;
        PanelConfirmDoor.SetActive(false);
        currentSphere = sphereCreator.lastCreatedSphere;

    }

    void OnNotSureClicked()
    {
        // Kareyi sil
        if (spawnedSquare != null)
            Destroy(spawnedSquare);
        spawnedSquare = null;
        lastConfirmed = false;
        PanelConfirmDoor.SetActive(false);
    }
}
