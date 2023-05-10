using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Pigeon : MonoBehaviour, IPointerClickHandler
{
    private bool _isKeyPigeon = false;
    [SerializeField] private TMP_Text keyPressedText;
    public void MakeKeyPigeon()
    {
        transform.SetParent(null, true);
        _isKeyPigeon = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Eventueel implementatie aanpassen
        if(_isKeyPigeon)
        {
            Debug.Log("Pigeon clicked");
            keyPressedText.gameObject.SetActive(true);
            PortalManager.Instance.OnPigeonPortalComplete();
            Destroy(this.gameObject);
        }
        else
        {
            // Geluidje maken?
            // Animatietje spelen?
        }
    }
}
