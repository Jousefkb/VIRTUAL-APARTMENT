using UnityEngine;
using UnityEngine.UI;  // UI bileşenleri ile çalışabilmek için

public class MessageBoxSphere : MonoBehaviour
{
    public GameObject messageBox; // Mesaj kutusu paneli
    public Button ButtonYes; // Evet butonu
    public Button ButtonNo;  // Hayır butonu
    public GameObject blockScreen;  // Ekranı engelleyen panel
    public Camera playerCamera;  // Kamera


    void Start()
    {
        // Başlangıçta mesaj kutusunu gizle
        messageBox.SetActive(false);
        blockScreen.SetActive(false);  // Ekran engelleme paneli gizli

        // Evet butonuna tıklandığında yapılacak işlem
        ButtonYes.onClick.AddListener(OnYesClicked);

        // Hayır butonuna tıklandığında yapılacak işlem
        ButtonNo.onClick.AddListener(OnNoClicked);
    }

    // Buton ile mesaj kutusunu gösterme
    public void ShowMessageBox()
    {
        // Mesaj kutusunu göster
        messageBox.SetActive(true);
        blockScreen.SetActive(true); // Ekranı engelleyen paneli aktif et

        foreach (Transform child in messageBox.transform)
        {
            child.gameObject.SetActive(true);  // Her alt objeyi aktif et
        }


    }

    // Evet butonuna tıklandığında yapılacak işlem
    void OnYesClicked()
    {
        messageBox.SetActive(false); // Mesaj kutusunu kapat
        blockScreen.SetActive(false); // Ekranı engelleyen paneli kaldır
        Debug.Log("Evet tıklandı");
        // Burada resmi eklemek için başka bir fonksiyon tetiklenebilir
    }

    // Hayır butonuna tıklandığında yapılacak işlem
    void OnNoClicked()
    {
        messageBox.SetActive(false); // Mesaj kutusunu kapat
        blockScreen.SetActive(false); // Ekranı engelleyen paneli kaldır
        Debug.Log("Hayır tıklandı");
    }
    void Update()
    {
        // Mesaj kutusunu her zaman kameranın önüne yerleştir
        //if (messageBox.activeSelf)
        //{
        //    messageBox.transform.position = playerCamera.transform.position + playerCamera.transform.forward * 3.0f;  // Kameranın 3 birim önüne yerleştir
        //    messageBox.transform.LookAt(playerCamera.transform);  // Kameraya doğru bakmasını sağla
        //}
    }

}
