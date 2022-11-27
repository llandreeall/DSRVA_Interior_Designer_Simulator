using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Lean.Touch;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObject : MonoBehaviour
{
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    ARRaycastManager m_raycastManager;

    // The first-person camera being used to render the passthrough camera image (i.e. AR background)
    public Camera m_firstPersonCamera;

    // The object instantiated as a result of a successful raycast intersection with a plane
    public GameObject m_currentSelection = null;

    bool m_doubleTap = false;
    float m_lastTapTime;

    List<GameObject> m_addedObjects;
    
    [SerializeField]
    public Dictionary<string, GameObject> m_nameToPrefab;
    //nu e frumos asa..
    public GameObject prefabCube;

    public Vector2 m_touchPosition;

    public Canvas m_canvas;
    public GameObject m_inventoryParent;
    public GameObject m_newUI;

    public GameObject m_movingObject;

    //TODO dinamic puse eventual, nu direct din inspector. 
    public List<GameObject> m_menuButtons;


    public TextMeshProUGUI m_debugText;
    public TextMeshProUGUI m_debugTextPermanent;
    private bool m_isDeleteHovered = false;

    void Awake()
    {
        m_raycastManager = GetComponent<ARRaycastManager>();
        m_addedObjects = new List<GameObject>();
        m_debugText.text = "Debug";
        m_nameToPrefab = new Dictionary<string, GameObject>();

        //add from inventar
        //m_nameToPrefab["Cube"] = prefabCube;
        
    }

    private void Start()
    {
        m_nameToPrefab = ObjectsUIMenu.instance.GetItemDictionary();
    }

    // Only the selected object should be able to be scaled, rotated or translated
    void SelectObject(GameObject selected)
    {
        DeselectObject();

        m_currentSelection = selected;

        // Add the translation, rotation and scaling scripts to the current objects
        m_currentSelection.AddComponent<LeanDragTranslate>();
        m_currentSelection.AddComponent<LeanPinchScale>();
        m_currentSelection.AddComponent<LeanTwistRotateAxis>();

        // Pinch and twist gestures require two fingers on screen
        m_currentSelection.GetComponent<LeanPinchScale>().Use.RequiredFingerCount = 2;
        m_currentSelection.GetComponent<LeanTwistRotateAxis>().Use.RequiredFingerCount = 2;
    }

    // Remove translation, rotation and scaling scripts for previously selected object
    void DeselectObject()
    {
        if (m_currentSelection != null)
        {
            // Destroy LeanDragTranslate, LeanPinchScale and LeanTwistRotateAxis components for previously selected object
            if (m_currentSelection.GetComponent<LeanDragTranslate>())
            {
                Destroy(m_currentSelection.GetComponent<LeanDragTranslate>());
            }
            if (m_currentSelection.GetComponent<LeanPinchScale>())
            {
                Destroy(m_currentSelection.GetComponent<LeanPinchScale>());
            }
            if (m_currentSelection.GetComponent<LeanTwistRotateAxis>())
            {
                Destroy(m_currentSelection.GetComponent<LeanTwistRotateAxis>());
            }

            m_currentSelection = null;
        }
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

    // Check if the user has tapped on screen. Check if an object was selected
    bool HandleTouch(out Vector2 touchPosition)
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

            Ray ray = m_firstPersonCamera.ScreenPointToRay(touchPosition);
            RaycastHit hitObject = new RaycastHit();

            // Check if a 3D object was hit
            // Check if 3D object was tapped
            //if (true)
            if (Physics.Raycast(ray, out hitObject))
            {
                m_debugText.text = "\n Hit Tag " + hitObject.transform.tag + ", name: " + hitObject.transform.name;
                if (hitObject.transform.tag == "Manipulated")
                {
                    m_lastTapTime = Time.time;

                    if (hitObject.transform.gameObject != m_currentSelection)
                    {
                        m_doubleTap = false;
                    }
                    else
                    {
                        // Check if a small amount of time has passed since the last tap on the same object
                        if (Time.time < m_lastTapTime + 0.3f)
                        {
                            m_doubleTap = true;
                        }
                    }

                    // If double tap event occured, change translation time
                    if (m_doubleTap)
                    {
                        m_doubleTap = false;
                        // Call function which adds the scripts for object manipulation
                        SelectObject(hitObject.transform.gameObject);

                        m_debugText.text = "DoubleTap" + m_debugText.text;
                    }

                    touchPosition = default;
                    return false;
                }
            }
            return true;
        }

        touchPosition = default;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
        m_touchPosition = default;
        if (Input.touchCount < 1)
        {
            m_touchPosition = default;
        }
        else
        {
            m_touchPosition = Input.GetTouch(0).position;
        }

        if (m_currentSelection == null)
        {
            if (m_touchPosition != default && m_movingObject != null)
            {
                DragMenuButton();
            }
        }

        m_debugTextPermanent.text = m_isDeleteHovered.ToString();
        //m_debugTextPermanent.text = "cS: " + ((m_currentSelection != null) ? m_currentSelection.name : "") +
        //    "\nmo: " + m_movingObject + "\ntp: " + m_touchPosition;

        if (m_currentSelection != null)
        {
            if (!HandleTouch(out Vector2 m222222222222_touchPosition))
            {
                return;
            }
        }

        //TODO. Sau altceva care sa evidentieze obiectul selectat.
        DrawBoundingBox();

        
    }

    // Delete the currently selected object
    public void Delete()
    {
        Destroy(m_currentSelection);
        m_addedObjects.Remove(m_currentSelection);
        m_currentSelection = null;
        m_debugText.text = "Deleted";
    }

    public void DeleteAll()
    {
        foreach (var obj in m_addedObjects)
            Destroy(obj);

        m_addedObjects.Clear();
        m_movingObject = null;
        m_currentSelection = null;

    }

    public void StartAddingObject(BaseEventData data)
    {
        if (m_currentSelection != null)
            return;

        m_debugText.color = Color.red;
        GameObject pressedButton = data.selectedObject;

        // Here we add the preview prefab (not the actual object, just a preview)
        // TODO. Este un Image, cu un copil Text. Nu stiu cum sa fac sa apara obiectul 3D ca parte din UI. 
        m_movingObject = Instantiate(m_newUI, pressedButton.transform.position, pressedButton.transform.rotation);
        m_movingObject.transform.parent = m_inventoryParent.transform;

        m_movingObject.AddComponent<LeanDragTranslate>();
    }
    public void StopAddingObject(BaseEventData data)
    {
        if (m_currentSelection != null)
            return;

        m_debugText.color = Color.green;
        if (m_movingObject.GetComponent<LeanDragTranslate>())
        {
            Destroy(m_movingObject.GetComponent<LeanDragTranslate>());
        }

        if(!m_isDeleteHovered)
        {
            var auxName = data.selectedObject.GetComponent<ObjectButton>().objectReference.name; Debug.Log(auxName);
            var prefabToAdd = m_nameToPrefab[auxName]; //data.selectedObject.name
            AddObject(prefabToAdd);
        }
        // else currentSelection remains null, nothing is added.
        
        Destroy(m_movingObject);
        m_movingObject = null;
        
        //TODO Inventar Inactiv here
    }

    public void AddObject(GameObject prefabToAdd)
    {
        Vector3 position;
        Quaternion rotation;

        m_debugText.text = "Addding new";
        m_debugText.text += "\ntp " + m_touchPosition;
        m_debugText.text += "\nmo.p " + m_movingObject.transform.position;

        if (m_raycastManager.Raycast(m_touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            position = s_Hits[0].pose.position;
            rotation = s_Hits[0].pose.rotation;
        }
        else
        {
            //If the destination (last position of drag & drop) is not valid (on an AR plane) we will not instantiate an object
            // TODO Ar fi fost frumos sa il pot adauga in mijlocul ecranului sau intr-un punct valid.. idk how..
           
            //le las totusi aici ca sa putem testa ca se adauga ceva in Unity, dar pozitiile nu sunt valide pe telefon
            position = m_movingObject.transform.position;
            rotation = Quaternion.identity;
        }

        m_debugText.text += "\nfinal p " + position;

        // Add a new object in scene
        var spawnedObject = Instantiate(prefabToAdd, position, rotation);
       
        m_addedObjects.Add(spawnedObject);

        SelectObject(spawnedObject);
    }

    void DrawBoundingBox()
    {
        if (m_currentSelection != null)
            ; // TODO
    }

    public void ConfirmChanges()
    {
        DeselectObject();
        //TODO Inventar buton activ here

    }

    public void DragMenuButton()
    {
        m_debugText.color = Color.yellow;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)m_canvas.transform,
            m_touchPosition,
            m_canvas.worldCamera,
            out position
            );

        m_movingObject.transform.position = m_canvas.transform.TransformPoint(position);
        m_debugText.text = "pos pentru " + m_movingObject.transform.name + " este " + m_movingObject.transform.position;
    }


    public void DeleteEnterPointer()
    {
        m_isDeleteHovered = true;
    }

    public void DeleteExitPointer()
    {
        m_isDeleteHovered = false;
    }

}
