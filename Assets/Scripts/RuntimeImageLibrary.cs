using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Networking;

/* TODO 1 Create in Unity an image database with minimum 3 images */
/* TODO 2 Augment the database */
public class RuntimeImageLibrary : MonoBehaviour
{
    private ARTrackedImageManager trackImageManager;

    void Start()
    {
        /* TODO 3.1 Download minimum one image from the internet */
        var url = "your_link_for_the_image";
        StartCoroutine(DownloadImage(url));
    }

    /* Function which doawnloads and creates and image database */
    IEnumerator DownloadImage(string url)
    {
        Texture2D imageToAdd;

        /* We will use UnityWebRequest API to download the image */
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
    
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            /* Downloaded image */
            imageToAdd = ((DownloadHandlerTexture)request.downloadHandler).texture;

            /*TODO 3.2 Attach a new ARTrackedImageManager component */
            trackImageManager = new ARTrackedImageManager();

            /* TODO 3.3 Create a new runtime library */
            var library = trackImageManager.CreateRuntimeLibrary();

            /* TODO 3.4 Add the image to the database*/
            
            /* Set maximum number of moving images */
            trackImageManager.maxNumberOfMovingImages = 3;

            /* TODO 3.5 Set the new library as the reference library */

            /* TODO 3.6 Enable the new ARTrackedImageManager component */

            /* TODO 3.7 Disable the previous ARTrackedImageManager component */

            /* Attach event handling */
            trackImageManager.trackedImagesChanged += OnTrackedImagesChanged;            
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            /* TODO 3.8 Instantiate a new object in scene */
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            /* TODO 3.9 Update the object parameters */
        }
    }

    XRImageTrackingSubsystem CreateImageTrackingSubsystem()
    {
        // Get all available plane subsystems
        var descriptors = new List<XRImageTrackingSubsystemDescriptor>();
        Debug.Log(descriptors.Count);
        SubsystemManager.GetSubsystemDescriptors(descriptors);

        // Find one that supports boundary vertices
        foreach (var descriptor in descriptors)
        {
            if (descriptor.supportsMutableLibrary)
            {
                // Create this plane subsystem
                return descriptor.Create();
            }
        }

        return null;
    }
}
