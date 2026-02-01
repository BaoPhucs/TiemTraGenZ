using UnityEngine;
using TMPro;

public class InteractUI : MonoBehaviour
{
    public TMP_Text interactText;
    public PlayerInteractor playerInteractor;

    void Update()
    {
        if (playerInteractor.currentTarget != null)
        {
            interactText.text = playerInteractor.currentTarget.interactText;
            interactText.gameObject.SetActive(true);
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }
}
