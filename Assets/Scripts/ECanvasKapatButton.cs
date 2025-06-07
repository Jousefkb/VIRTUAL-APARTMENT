using UnityEngine;
using UnityEngine.UI; // UI öğeleriyle çalışabilmek için

public class ECanvasKapatButton : MonoBehaviour
{
    public Button button;  // Butonun referansı
    public GameObject canvasObject; // Canvas'ı referans alacağımız GameObject

    private bool isButtonClicked = false; // Butonun tıklanıp tıklanmadığını takip edecek değişken

    void Update()
    {
        // E tuşuna basıldığında butonu tetikleyelim
        if (Input.GetKeyDown(KeyCode.E)) // "E" tuşuna basılınca
        {
            button.onClick.Invoke(); // Butonun OnClick() event'ini tetikle
            ToggleCanvasVisibility(); // Canvas'ın görünürlüğünü değiştir
            isButtonClicked = !isButtonClicked; // Butonun durumunu değiştir
            Debug.Log("Buton E tuşu ile tıklanmış oldu!");
        }
    }

    // Canvas'ı aktif veya inaktif yapacak fonksiyon
    void ToggleCanvasVisibility()
    {
        canvasObject.SetActive(!canvasObject.activeSelf);  // Canvas'ı aktif veya inaktif yap
    }
}
