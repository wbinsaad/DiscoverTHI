using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackingController : MonoBehaviour
{
    public TMP_Text infoBox;
    public GameObject[] ArPrefabs;
    private ARTrackedImageManager trackedImages;
    List<GameObject> ARObjects = new List<GameObject>();
    private ARPlaneManager planeManager;
    


    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        planeManager = FindObjectOfType<ARPlaneManager>();
    }

    void Update()
    {
        // Check if a surface is detected
        if (IsSurfaceDetected())
        {
            infoBox.text = "Surface detected. Tracking objects...\n";
            outputTracking(); // Continue tracking and updating prefab status

            if (CheckWinCondition())
            {
                Handheld.Vibrate();
                SceneManager.LoadScene(1);
            }
        }
        else
        {
            infoBox.text = "Please scan a surface to begin.\n";

            // Optionally disable tracking logic while waiting for surface detection
            DisableTracking();
        }
    }

    bool IsSurfaceDetected()
    {
        return true; //  planeManager.trackables != null && planeManager.trackables.count > 0;
    }


    void DisableTracking()
    {
        foreach (var obj in ARObjects)
        {
            obj.SetActive(false); // Hide all prefabs until surface is scanned
        }
    }


    void outputTracking()
    {

        infoBox.text = "Tracking Data: \n";

        int i = 0;
        foreach (var trackedImage in trackedImages.trackables)
        {
            infoBox.text += "Image: " + trackedImage.referenceImage.name + " " + trackedImage.trackingState + " position" + trackedImage.transform.position + "\n";

            if (trackedImage.trackingState == TrackingState.Limited)
            {
                ARObjects[i].SetActive(false);
            }
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                ARObjects[i].SetActive(true);
            }
            i++;

        }
    }

    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        if (trackedImages == null)
        {
            trackedImages = GetComponent<ARTrackedImageManager>();
        }
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }


    void OnDisable()
    {
        if (trackedImages != null)
        {
            trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    void OnDestroy()
    {
        foreach (var obj in ARObjects)
        {
            Destroy(obj);
        }
        ARObjects.Clear();
    }


    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //Create object based on image tracked
        foreach (var trackedImage in eventArgs.added)
        {
            foreach (var arPrefab in ArPrefabs)
            {
                if (trackedImage.referenceImage.name == arPrefab.name && !ARObjects.Any(arObject => arObject.name.Contains(trackedImage.referenceImage.name)))
                {
                    Debug.LogWarning("DETECT");
                    ARObjects.Add(Instantiate(arPrefab, trackedImage.transform));
                }
            }
        }

        //Update tracking position
        foreach (var trackedImage in eventArgs.updated)
        {
            foreach (var gameObject in ARObjects)
            {
                if (gameObject.name == trackedImage.name)
                {
                    Debug.LogWarning("LOST");
                    gameObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);
                }
            }
        }
    }

    private bool CheckWinCondition()
    {
        // Thresholds for alignment
        const float positionThreshold = 0.05f; // Allowed deviation (adjust as needed)


        // Collect active ARObjects
        var activeObjects = ARObjects.Where(obj => obj.activeSelf).ToList();
        if (activeObjects.Count != 4)
        {
            infoBox.text += "Error: Not all 4 pieces are active. Active pieces: ";
            activeObjects.ForEach(x => infoBox.text += x.name + ", ");
            return false; // Ensure exactly 4 pieces are active
        }

        Debug.LogWarning("CHECK");

        // Map piece names to their expected offsets
        Dictionary<string, Vector3> expectedOffsets = new Dictionary<string, Vector3>()
        {
            { "part1", new Vector3(0, 0, 0) },
            { "part2", new Vector3(0.05f, 0, 0) },
            { "part3", new Vector3(0, 0, -0.05f) },
            { "part4", new Vector3(0.05f, 0, -0.05f) },
        };

        // Get the base position (Part1)
        var baseObject = activeObjects.FirstOrDefault(obj => obj.name.StartsWith("part1"));
        if (baseObject == null)
        {
            infoBox.text += "Error: Base piece not found.\n";
            activeObjects.ForEach(x => infoBox.text += x.name + ", ");
            return false; // Base piece not found
        }
        Vector3 basePosition = baseObject.transform.position;

        // Variable to track if all pieces are aligned correctly
        bool allAligned = true;

        // Check alignment for each piece
        foreach (var obj in activeObjects)
        {
            string pieceName = obj.name.Substring(0, 5); // Assuming piece name starts with "partX"
            if (!expectedOffsets.ContainsKey(pieceName))
            {
                infoBox.text += "Error: Unknown piece " + pieceName + ".\n";
                allAligned = false; // Unknown piece
                continue;
            }

            Vector3 expectedOffset = expectedOffsets[pieceName];
            Vector3 actualOffset = obj.transform.position - basePosition;

            // Compare offsets
            if (Mathf.Abs(actualOffset.x - expectedOffset.x) > positionThreshold ||
                // Mathf.Abs(actualOffset.y - expectedOffset.y) > yThreshold ||
                Mathf.Abs(actualOffset.z - expectedOffset.z) > positionThreshold)
            {
                infoBox.text += "Misaligned: " + pieceName + ".\n";
                allAligned = false; // Misaligned piece
            }
            else
            {
                infoBox.text += "Aligned: " + pieceName + ".\n";
            }
        }

        return allAligned;
    }
}