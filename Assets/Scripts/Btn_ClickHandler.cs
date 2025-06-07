using UnityEngine;
using UnityEngine.UI;

public class Btn_ClickHandler : MonoBehaviour
{
    public Button buttonYes;  // Evet butonu
    public Button buttonNo;   // Hayır butonu

    void Start()
    {
        // Evet butonuna tıklama işlemi için listener ekleyelim
        buttonYes.onClick.AddListener(OnYesClicked);

        // Hayır butonuna tıklama işlemi için listener ekleyelim
        buttonNo.onClick.AddListener(OnNoClicked);
    }

    // Evet butonuna tıklanması
    void OnYesClicked()
    {
        Debug.Log("Evet butonuna tıklandı!");
    }

    // Hayır butonuna tıklanması
    void OnNoClicked()
    {
        Debug.Log("Hayır butonuna tıklandı!");
    }
}
