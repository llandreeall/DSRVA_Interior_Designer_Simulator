using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class CloudAnchorObjectPlacement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    GameObject previousSelection = null;
    GameObject currentSelection = null;
    bool doubleTap = false;
    float doubleTapTime;
    private ARAnchorManager arAnchorManager = null;
    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    /// <summary>
    /// The first-person camera being used to render the passthrough camera image (i.e. AR
    /// background).
    /// </summary>
    public Camera FirstPersonCamera;

    /// <summary>
    /// A prefab to place when a raycast from a user touch hits a plane.
    /// </summary>
    public GameObject prefab;

    public static CloudAnchorObjectPlacement Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        m_RaycastManager = GetComponent<ARRaycastManager>();
        arAnchorManager = GetComponent<ARAnchorManager>();
        spawnedObject = null;
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            touchPosition = default;
            return false;
        }

        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        /* Add only one cube on scene */
        if (spawnedObject != null)
            return;
         
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;

            /* TODO 2.1. Instantiate a new prefab on scene */

            /* TODO 2.2 Attach an anchor to the prefab (Hint! arAnchorManager) */
            ARAnchor anchor = new ARAnchor();
            spawnedObject.transform.parent = anchor.transform;
            
            /* Send anchor to ARCloudAnchorManager */
            ARCloudAnchorManager.Instance.QueueAnchor(anchor);
        }
    }

    /* Add object on scene after the anchor has been resolved */
    public void RecreatePlacement(Transform transform)
    {
        spawnedObject = Instantiate(placedPrefab, transform.position, transform.rotation);
        spawnedObject.transform.parent = transform;
    }

    public void RemovePlacement()
    {
        /* TODO 4 Remove cube from screen */

    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    ARRaycastManager m_RaycastManager;
}
