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
        // Eventueel implementatie aanpassen
        if(isKeyPigeon)
        {
            Debug.Log("Pigeon clicked");
            keyPressedText.gameObject.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
