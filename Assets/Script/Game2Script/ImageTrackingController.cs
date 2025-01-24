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
    public GameObject[] ArPrefabs;
    private ARTrackedImageManager trackedImages;
    List<GameObject> ARObjects = new List<GameObject>();

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Update()
    {
        if (CheckWinCondition())
        {
            Handheld.Vibrate();
            SceneManager.LoadScene("Hint3Scene");
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
            return false; // Ensure exactly 4 pieces are active
        }

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
                allAligned = false; // Unknown piece
                continue;
            }

            Vector3 expectedOffset = expectedOffsets[pieceName];
            Vector3 actualOffset = obj.transform.position - basePosition;

            // Compare offsets
            if (Mathf.Abs(actualOffset.x - expectedOffset.x) > positionThreshold ||
                Mathf.Abs(actualOffset.z - expectedOffset.z) > positionThreshold)
            {
                allAligned = false; // Misaligned piece
            }
        }

        return allAligned;
    }
}
