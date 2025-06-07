using UnityEngine;
using UnityEngine.UI; // UI öğeleriyle çalışabilmek için

public class ButtonTrigger : MonoBehaviour
{
    public Button button;   // Butonun referansı
    public Text textObject; // Text objesinin referansı

    void Update()
    {
        // H tuşuna basıldığında butonu tetikleyelim ve text'i aktif / inaktif yapalım
        if (Input.GetKeyDown(KeyCode.H)) // "H" tuşuna basılınca
        {
            button.onClick.Invoke(); // Butonun OnClick() event'ini tetikle
            Debug.Log("Buton H tuşu ile tıklanmış oldu!");
        }
    }

    // Text objesinin aktiflik durumunu değiştiren fonksiyon
}
