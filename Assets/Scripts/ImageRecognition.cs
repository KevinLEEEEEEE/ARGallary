using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageRecognition : MonoBehaviour
{
    public GameObject mountedObject;
    private ARTrackedImageManager _arTrackedImageManager;

    private void Awake()
    {
        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        _arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach(var trackedImage in args.added)
        {
            Debug.Log("[ImageRecognition] Target image detected, mount image");

            mountedObject.transform.parent = trackedImage.transform;
            mountedObject.transform.localPosition = new Vector3(0, 0, 0);
            mountedObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
