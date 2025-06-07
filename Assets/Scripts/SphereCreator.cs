using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SphereCreator : MonoBehaviour
{
    [Header("Sphere Settings")]
    public GameObject spherePrefab;          // Sphere Prefab
    public float distanceBetweenSpheres = 20f;
    private float currentXPosition = 0f;    // Starting X position
    public GameObject lastCreatedSphere;

    [Header("UI References")]
    public GameObject messageBox;            // “Ortamınız oluşturuldu…” panel
    public Button yesButton;                 // “Evet” butonu
    public Button noButton;                  // “Hayır” butonu
    public GameObject editPanel;      // PanelEdit GameObject
    public Slider rotationSlider;     // RotationSlider
    public Button flipButton;         // FlipButton
    public Button closeEditButton;    // CloseEditButton
    public Transform rigTransform;


    void Start()
    {
        // Buton event’lerini bağla
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);

        rotationSlider.onValueChanged.AddListener(OnRotationSliderChanged);
        flipButton.onClick.AddListener(OnFlipButtonClicked);
        closeEditButton.onClick.AddListener(CloseEditPanel);

        // Panel başlangıçta kapalı
        editPanel.SetActive(false);

    }

    void Update()
    {
        // H tuşuyla sphere oluştur ve paneli göster
        if (Input.GetKeyDown(KeyCode.H))
            CreateSphere();

        // Eğer panel açık ise N/Y tuşlarıyla da seçim yap
        if (messageBox.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.N))
                noButton.onClick.Invoke();

            if (Input.GetKeyDown(KeyCode.Y))
                yesButton.onClick.Invoke();
        }
    }

    public void CreateSphere()
    {
        if (messageBox.activeSelf) return;

        // Yeni sphere oluştur
        lastCreatedSphere = Instantiate(
            spherePrefab,
            new Vector3(currentXPosition, 0, 0),
            Quaternion.identity
        );

        lastCreatedSphere.transform.localScale = Vector3.one * 5f;
        currentXPosition += distanceBetweenSpheres;
        Vector3 spherePos = lastCreatedSphere.transform.position;
        rigTransform.position = spherePos;

        // Mesaj kutusunu aktif et
        messageBox.SetActive(true);
    }

    void OnNoClicked()
    {
        // Hayır: sphere sil ve paneli kapat
        if (lastCreatedSphere != null)
        {
            Destroy(lastCreatedSphere);
            lastCreatedSphere = null;
        }
        messageBox.SetActive(false);
    }

    void OnYesClicked()
    {
#if UNITY_EDITOR
        ShowEditorFilePicker();
#else
        Debug.LogWarning("Bu platformda dosya seçimi desteklenmiyor.");
#endif
    }
#if UNITY_EDITOR
    void ShowEditorFilePicker()
    {
        // Unity Editor içindeki Open File Panel
        string path = EditorUtility.OpenFilePanel(
            "360° Fotoğraf Seç",
            Application.dataPath,
            "png,jpg,jpeg"
        );

        if (!string.IsNullOrEmpty(path))
            StartCoroutine(LoadTextureFromPath(path));
    }
#endif

    //IEnumerator LoadTextureFromPath(string filePath)
    //{
    //    // Yerel dosyadan texture yükle
    //    var www = new WWW("file:///" + filePath);
    //    yield return www;

    //    var tex = www.texture;
    //    if (lastCreatedSphere != null && tex != null)
    //    {
    //        var rend = lastCreatedSphere.GetComponent<Renderer>();
    //        if (rend != null)
    //            rend.material.SetTexture("_MainTex", tex);
    //    }

    //    messageBox.SetActive(false);
    //    OpenEditPanel();
    //}
    IEnumerator LoadTextureFromPath(string filePath)
    {
        // 1) Dosyayı yükle
        var www = new WWW("file:///" + filePath);
        yield return www;

        // 2) Ham byte dizisini al
        byte[] bytes = www.bytes;

        // 3) Yüksek kalite Texture2D oluştur (RGBA32 + mipMap)
        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, true);

        // 4) Byte dizisini yükle ve mipMap zincirini güncelle
        tex.LoadImage(bytes);
        tex.Apply(updateMipmaps: true, makeNoLongerReadable: false);

        // 5) Filtreleme ayarları
        tex.filterMode = FilterMode.Trilinear;
        tex.anisoLevel = 9;

        // 6) Sphere’e ata
        if (lastCreatedSphere != null)
        {
            var rend = lastCreatedSphere.GetComponent<Renderer>();
            if (rend != null)
                rend.material.SetTexture("_MainTex", tex);
        }

        // 7) UI akışını sürdür
        messageBox.SetActive(false);
        OpenEditPanel();
    }


    void OpenEditPanel()
    {
        if (lastCreatedSphere == null) return;
        // Slider’ı sıfır konuma getir:
        rotationSlider.value = 0;
        // Paneli göster:
        editPanel.SetActive(false);                                         //ERTELENDİ
    }

    void OnRotationSliderChanged(float angle)
    {
        if (lastCreatedSphere != null)
        {
            Vector3 e = lastCreatedSphere.transform.localEulerAngles;
            lastCreatedSphere.transform.localEulerAngles = new Vector3(angle, e.y, e.z);
        }
    }


    void OnFlipButtonClicked()
    {
        if (lastCreatedSphere != null)
        {
            Vector3 e = lastCreatedSphere.transform.localEulerAngles;
            lastCreatedSphere.transform.localEulerAngles = new Vector3(e.x, e.y, e.z + 180f);
        }
    }


    void CloseEditPanel()
    {
        editPanel.SetActive(false);
    }


}
