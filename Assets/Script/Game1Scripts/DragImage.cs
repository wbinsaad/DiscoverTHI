using UnityEngine;
using UnityEngine.EventSystems;

public class DragImage : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    public Vector2 correctPosition; // correct position
    public GameObject correctObject;
    private const float threshold = 100f; // Permissible distance

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        correctPosition = correctObject.GetComponent<RectTransform>().anchoredPosition;
        originalPosition = rectTransform.anchoredPosition; // Record the original position
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / CanvasScale();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        

        // Detect if the puzzle pieces are placed correctly
        if (Vector2.Distance(rectTransform.anchoredPosition, correctPosition) < threshold)
        {
            rectTransform.anchoredPosition = correctPosition; // Set to the correct position
            GameManager.Instance.CheckGameComplete();
            // Puzzle Completion Logic
            correctObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition; // Return to original position
            UIManager.Instance.ShowFailPanel();
        }
    }

    private float CanvasScale()
    {
        return GetComponentInParent<Canvas>().scaleFactor;
    }
}