using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScale
    : MonoBehaviour,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerEnterHandler,
        IPointerExitHandler
{
    [SerializeField]
    private float hoverScale = 1.1f;

    [SerializeField]
    private float clickScale = 0.9f;

    [SerializeField]
    private float duration = 0.09f;

    //public bool soundEffect = true;
    [SerializeField]
    private Vector3 baseScale = new Vector3(1, 1, 1);

    //public AudioId audioId = AudioId.ButtonClick;

    //private void Start()
    //{
    //       baseScale = transform.localScale;
    //   }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(hoverScale * baseScale, duration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(baseScale, duration);
        // if (soundEffect)
        // {
        //     Systems.serviceManager.Get<AudioService>().PlaySound(audioId);
        // }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(clickScale * baseScale, duration);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //transform.DOScale(hoverScale, duration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(baseScale, duration);
    }

    private void OnDestroy()
    {
        //transform.DOKill(soundEffect)
        // {
        //     Systems.serviceManager.Get<AudioService>().PlaySound(audioId);
        // }
    }
}
