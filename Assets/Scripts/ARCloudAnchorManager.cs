using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Google.XR.ARCoreExtensions;

public class AnchorCreatedEvent : UnityEvent<Transform> { }

/* TODO 1. Enable ARCore Cloud Anchors API */
public class ARCloudAnchorManager : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera = null;

    [SerializeField]
    private float resolveAnchorPassedTimeout = 20.0f;

    private ARAnchorManager arAnchorManager = null;
    private ARAnchor pendingHostAnchor = null;
    private ARCloudAnchor cloudAnchor = null;
    private string anchorIdToResolve;
    private bool anchorHostInProgress = false;
    private bool anchorResolveInProgress = false;
    private float safeToResolvePassed = 0;
    private AnchorCreatedEvent anchorCreatedEvent = null;
    public static ARCloudAnchorManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        anchorCreatedEvent = new AnchorCreatedEvent();
        anchorCreatedEvent.AddListener((t) => CloudAnchorObjectPlacement.Instance.RecreatePlacement(t));
    }

    private Pose GetCameraPose()
    {
        return new Pose(arCamera.transform.position, arCamera.transform.rotation);
    }
    public void QueueAnchor(ARAnchor arAnchor)
    {
        pendingHostAnchor = arAnchor;
    }

    public void HostAnchor()
    {
        Debug.Log("HostAnchor call in progress");

        /* TODO 3.1 Get FeatureMapQuality */ 
        FeatureMapQuality quality = new FeatureMapQuality();

        Debug.Log(string.Format("Feature Map Quality: {0}", quality));
        
        /* TODO 3.2 Save in cloudAnchor variable the result of the hosting process */

        if (cloudAnchor == null)
        {
            Debug.Log("Unable to host cloud anchor");
        }
        else
        {
            anchorHostInProgress = true;
        }
    }

    public void Resolve()
    {
        Debug.Log("Resolve call in progress");

        /* TODO 5 Save in cloudAnchor variable the result of the resolve process */
        cloudAnchor = arAnchorManager.ResolveCloudAnchorId(anchorIdToResolve);

        if (cloudAnchor == null)
        {
            Debug.Log(string.Format("Unable to resolve cloud anchor {0}", anchorIdToResolve));
        }
        else
        {
            safeToResolvePassed = resolveAnchorPassedTimeout;
            anchorResolveInProgress = true;
        }
    }

    private void CheckHostingProgress()
    {
        /* TODO 3.3 Implement CheckHostingProgress logic */
    }

    private void CheckResolveProgress()
    {
        CloudAnchorState cloudAnchorState = cloudAnchor.cloudAnchorState;

        if (cloudAnchorState == CloudAnchorState.Success)
        {
            anchorResolveInProgress = false;
            anchorCreatedEvent?.Invoke(cloudAnchor.transform);
        }
        else
        {
            if (cloudAnchorState != CloudAnchorState.TaskInProgress)
            {
                Debug.Log(string.Format("Error while resolving cloud anchor: {0}", cloudAnchorState));
                anchorResolveInProgress = false;
            }
        }
    }

    void Update()
    {
        /* Check host result */
        if (anchorHostInProgress)
        {
            CheckHostingProgress();
            return;
        }

        /*Check resolve result */
        if (anchorResolveInProgress && safeToResolvePassed >= 0)
        {
            if (!string.IsNullOrEmpty(anchorIdToResolve))
            {
                Debug.Log(string.Format("Resolving Anchor Id: {0}", anchorIdToResolve));
                CheckResolveProgress();
                safeToResolvePassed -= Time.deltaTime * 1.0f;
            }
        }
    }
}
