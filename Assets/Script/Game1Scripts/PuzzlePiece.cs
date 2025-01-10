
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    private Vector2 correctPosition;
    public GameObject targetGameObject;
    private bool isDragging = false;
    private Vector2 offset;
    private Vector3 originalPosition;
    private bool isPlaced = false;
    
    // Touch ID for Multi-Touch
    private int fingerId = -1;

    void Start()
    {
        originalPosition = transform.position;
        correctPosition = targetGameObject.transform.position;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Debug.Log($"touch count:{Input.touchCount}");
            Touch touch = Input.GetTouch(0);
            
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            // Collider2D hit = Physics2D.OverlapPoint(touchPosition);
            // Debug.Log($"hit: {hit != null}");
            // if (hit != null)
            // {
            //     Debug.Log($"hit" +"   " + hit.transform.gameObject.name);
            // }
            
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);

            if (hit)
            {
                // Handle touched objects here
                Debug.Log("Touched: " + hit.transform.name);
                
                if (hit.transform != null && hit.transform == transform)
                {
                    Debug.Log($"hit" + touch.phase +"   " + hit.transform.gameObject.name);
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            if (!isPlaced && fingerId == -1)
                            {
                                isDragging = true;
                                fingerId = touch.fingerId;
                                offset = touchPosition - (Vector2)transform.position;
                                Debug.Log($"TouchPhase.Began");
                            }
                            break;

                        case TouchPhase.Moved:
                            Debug.Log($"fingerId:{fingerId}");
                            if (isDragging && touch.fingerId == fingerId)
                            {
                                transform.position = touchPosition - offset;
                            }
                            break;

                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            if (touch.fingerId == fingerId)
                            {
                                isDragging = false;
                                fingerId = -1;
                                //CheckPosition();
                            }
                            break;
                    }
                }
                
            }

        }
    }

    void CheckPosition()
    {
        float distance = Vector2.Distance((Vector2)transform.position, correctPosition);
        
        if (distance < 0.5f)
        {
            transform.position = correctPosition;
            isPlaced = true;
            // Add Vibration Feedback
            Handheld.Vibrate();
            GameManager.Instance.CheckGameComplete();
        }
        else
        {
            transform.position = originalPosition;
            // UIManager.Instance.ShowMessage("Incorrect position, please try again");
        }
    }
}