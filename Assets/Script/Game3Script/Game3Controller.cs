using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Game3Controller : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject PrinterPrefab;         // The Printer prefab
    public GameObject StudentCardPrefab;     // The StudentCard prefab

    private GameObject instantiatedPrinter;      // The instantiated Printer
    private GameObject instantiatedStudentCard;  // The instantiated StudentCard
    private ARTrackedImageManager trackedImageManager;

    private Vector3 lastPosition;            // Store the last position
    private float positionThreshold = 0.03f; // Threshold for position changes

    [Header("Card Movement")]
    public float speed = 1.0f;   // Speed of the movement
    public float range = 3.0f;   // Range of movement along the X-axis

    [Header("Game Object")]
    public GameObject EndPopup;

    private void Start()
    {
        // Prevent device from going to sleep
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    private void Update()
    {
        // Only proceed if both the printer and the card have been instantiated
        if (instantiatedPrinter != null && instantiatedStudentCard != null)
        {
            // Calculate a small side-to-side movement on X plus an 8-unit offset on Z
            float offsetX = Mathf.Sin(Time.time * speed) * range;
            Vector3 localOffset = new Vector3(offsetX, 0, 8);

            // Convert the local offset to world space
            Vector3 worldPosition = instantiatedPrinter.transform.TransformPoint(localOffset);

            // Apply position/rotation updates to the card
            instantiatedStudentCard.transform.position = worldPosition;
            instantiatedStudentCard.transform.rotation = instantiatedPrinter.transform.rotation;

            // Check for user input
            if (Input.GetMouseButtonDown(0))
            {
                // Now we call our revised "IsCardOnSameX" method
                if (IsCardOnSameX(0.05f, 2.0f))  // tweak threshold & distance as needed
                {
                    Debug.Log("Student card is within X threshold of the printer! ...");
                    EndPopup.SetActive(true);
                }
                else
                {
                    Debug.Log("Student card is NOT within X threshold of the printer!");
                }
            }
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // Handle newly detected images
        foreach (var trackedImage in args.added)
        {
            Debug.Log($"Tracked image added: {trackedImage.referenceImage.name}");

            if (instantiatedPrinter == null)
            {
                // Create a new Printer at the location of the tracked image
                instantiatedPrinter = Instantiate(PrinterPrefab, trackedImage.transform);
                // Create the student card
                CreateStudentCard();
            }
        }

        // Handle updated images
        foreach (var trackedImage in args.updated)
        {
            bool isTracking = (trackedImage.trackingState == TrackingState.Tracking);

            if (instantiatedPrinter != null)
                instantiatedPrinter.SetActive(isTracking);

            if (instantiatedStudentCard != null)
                instantiatedStudentCard.SetActive(isTracking);

            // If the image is tracked, update card position if needed
            if (isTracking && instantiatedStudentCard != null)
            {
                UpdateStudentCard(trackedImage.transform.position);
            }
        }
    }

    private void CreateStudentCard()
    {
        Vector3 localOffset = new Vector3(0, 0, 5);
        Vector3 worldPosition = instantiatedPrinter.transform.TransformPoint(localOffset);

        instantiatedStudentCard = Instantiate(StudentCardPrefab, worldPosition, Quaternion.identity);

        // Make the card face the same direction as the printer
        instantiatedStudentCard.transform.rotation =
            Quaternion.LookRotation(instantiatedPrinter.transform.forward, Vector3.up);

        Debug.Log($"Card Created at: {instantiatedStudentCard.transform.position}, " +
                  $"Rotation: {instantiatedStudentCard.transform.eulerAngles}");
    }

    private void UpdateStudentCard(Vector3 position)
    {
        // Only update if the position change exceeds the threshold
        if (Vector3.Distance(lastPosition, position) > positionThreshold)
        {
            Vector3 localOffset = new Vector3(0, 0, 8);
            Vector3 worldPosition = instantiatedPrinter.transform.TransformPoint(localOffset);

            instantiatedStudentCard.transform.SetPositionAndRotation(
                worldPosition,
                instantiatedPrinter.transform.rotation
            );

            lastPosition = position; // Update the last known position
        }
    }

    private bool IsCardOnSameX(float xThreshold, float maxDistance)
    {
        if (instantiatedPrinter == null || instantiatedStudentCard == null)
        {
            Debug.LogWarning("Printer or Card is null. Can't check X-axis alignment.");
            return false;
        }

        float distance =
            Vector3.Distance(instantiatedPrinter.transform.position, instantiatedStudentCard.transform.position);

        if (distance > maxDistance)
        {
            Debug.Log($"Card is too far from the printer. Distance: {distance} > {maxDistance}");
            return false;
        }

        // Get each object's X coordinate in world space
        float printerX = instantiatedPrinter.transform.position.x;
        float cardX = instantiatedStudentCard.transform.position.x;
        float diffX = Mathf.Abs(cardX - printerX);

        Debug.Log($"Printer X: {printerX}, Card X: {cardX}, diffX: {diffX}, threshold: {xThreshold}");

        if (diffX <= xThreshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
