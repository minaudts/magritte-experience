using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Pigeon : MonoBehaviour, IPointerClickHandler
{
    private bool isKeyPigeon = false;
    [SerializeField] private TMP_Text keyPressedText;
    public void MakeKeyPigeon()
    {
        transform.SetParent(null, true);
        isKeyPigeon = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isKeyPigeon)
        {
            keyPressedText.gameObject.SetActive(true);
            Debug.Log("Pigeon clicked");
        }
    }
}
