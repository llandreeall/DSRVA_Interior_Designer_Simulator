using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsUIMenu : MonoBehaviour
{
    #region Singleton ObjectsMenu
    public static ObjectsUIMenu instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public RoomObjectsUI bathroomUI;
    public RoomObjectsUI bedroomUI;
    public RoomObjectsUI livingroomUI;
    public RoomObjectsUI kitchenUI;
    public GameObject parentRoom;

    public List<Object3D> displayedObjects;
    public string selectedRoom;

    public ObjectButton bttn1;
    public ObjectButton bttn2;
    public ObjectButton bttn3;
    public ObjectButton bttn4;
    public GameObject leftBttn, rightBttn;

    int index = 0;
    bool clicked = false;
    GameObject containerObject;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        RoomObjectsUI[] rooms = parentRoom.transform.GetComponentsInChildren<RoomObjectsUI>();
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].roomName == "Bedroom")
            { bedroomUI = rooms[i]; bedroomUI.parent = gameObject; }
            if (rooms[i].roomName == "Bathroom")
            { bathroomUI = rooms[i]; bathroomUI.parent = gameObject; }
            if (rooms[i].roomName == "LivingRoom")
            { livingroomUI = rooms[i]; livingroomUI.parent = gameObject; }
            if (rooms[i].roomName == "Kitchen")
            { kitchenUI = rooms[i]; kitchenUI.parent = gameObject; }
        }

        displayedObjects = kitchenUI.objectsList;
        selectedRoom = "Kitchen";
        kitchenUI.Select();
        DisplayList();
        containerObject = transform.GetChild(0).gameObject;
        containerObject.SetActive(false);
        anim = gameObject.GetComponent<Animator>();
        StartCoroutine("Close");
    }

    public void SelectRoom(string room)
    {
        if (room != selectedRoom)
        {
            switch (room){ 
            case"Bedroom":
                displayedObjects = bedroomUI.objectsList;
                break;
            case"Bathroom":
                displayedObjects = bathroomUI.objectsList;
                break;
             case "LivingRoom":
                    displayedObjects = livingroomUI.objectsList;
                break;
            default: 
                    displayedObjects = kitchenUI.objectsList;
                break;
            }
            switch (selectedRoom)
            {
                case "Bedroom":
                    bedroomUI.Deselect();
                    break;
                case "Bathroom":
                    bathroomUI.Deselect();
                    break;
                case "LivingRoom":
                    livingroomUI.Deselect();
                    break;
                default:
                    kitchenUI.Deselect();
                    break;
            }
            DisplayList();
            selectedRoom = room;
        }
    }

    public void DisplayList()
    {
        index = 0;
        if (displayedObjects == null || displayedObjects.Count == 0)
        {
            bttn1.SetReference(null, -1);
            bttn2.SetReference(null, -1);
            bttn3.SetReference(null, -1);
            bttn4.SetReference(null, -1);

            leftBttn.SetActive(false);
            rightBttn.SetActive(false);
        }
        else
        {
            if (displayedObjects.Count > 0 && displayedObjects[0] != null)
            {
                bttn1.SetReference(displayedObjects[0], index);
                index++;
            }
            else
                bttn1.SetReference(null, -1);
            if (displayedObjects.Count > 1 && displayedObjects[1] != null)
            {
                bttn2.SetReference(displayedObjects[1], index);
                index++;
            }
            else
                bttn2.SetReference(null, -1);
            if (displayedObjects.Count > 2 && displayedObjects[2] != null)
            {
                bttn3.SetReference(displayedObjects[2], index);
                index++;
            }
            else
                bttn3.SetReference(null, -1);
            if (displayedObjects.Count > 3 && displayedObjects[3] != null)
            {
                bttn4.SetReference(displayedObjects[3], index);
                index++;
            }
            else
                bttn4.SetReference(null, -1);

            if(displayedObjects.Count > 4)
            {
                leftBttn.SetActive(true);
                rightBttn.SetActive(true);
            } else
            {
                leftBttn.SetActive(false);
                rightBttn.SetActive(false);
            }
        }
    }

    public void GoLeft()
    {
        if (displayedObjects.Count > 4)
        {
            int leftId = bttn1.index;

            int newId = leftId + 1;
            if (newId >= displayedObjects.Count) newId = 0;

            Object3D i = displayedObjects[newId];
            bttn1.SetReference(i, newId);

            if (newId + 1 >= displayedObjects.Count) newId = 0;
            else newId += 1;
            i = displayedObjects[newId];
            bttn2.SetReference(i, newId);

            if (newId + 1 >= displayedObjects.Count) newId = 0;
            else newId += 1;
            i = displayedObjects[newId];
            bttn3.SetReference(i, newId);

            if (newId + 1 >= displayedObjects.Count) newId = 0;
            else newId += 1;
            i = displayedObjects[newId];
            bttn4.SetReference(i, newId);
        }
    }

    public void GoRight()
    {
        if (displayedObjects.Count > 4)
        {
            int rightId = bttn4.index;

            int newId = rightId - 1;
            if (newId < 0) newId = displayedObjects.Count - 1;

            Object3D i = displayedObjects[newId];
            bttn4.SetReference(i, newId);

            if (newId - 1 < 0) newId = displayedObjects.Count - 1;
            else newId -= 1;
            i = displayedObjects[newId];
            bttn3.SetReference(i, newId);

            if (newId - 1 < 0) newId = displayedObjects.Count - 1;
            else newId -= 1;
            i = displayedObjects[newId];
            bttn2.SetReference(i, newId);

            if (newId - 1 < 0) newId = displayedObjects.Count - 1;
            else newId -= 1;
            i = displayedObjects[newId];
            bttn1.SetReference(i, newId);
        }
    }

    IEnumerator Open()
    {
        anim.SetBool("isOpen", true);
        yield return new WaitForSeconds(0.33f);
        containerObject.SetActive(true);

    }

    IEnumerator Close()
    {
        containerObject.SetActive(false);
        anim.SetBool("isOpen", false);
        yield return new WaitForSeconds(0.33f);
    }

    public void DisplayMenu()
    {
        if(clicked == false) //open menu
        {
            StartCoroutine("Open");
        } else //hide menu
        {
            StartCoroutine("Close");
        }
        clicked = !clicked;
    }

    private void OnDisable()
    {
        if (clicked == true)  //hide menu
        {
            StartCoroutine("Close");
        }
    }

    public Dictionary<string, GameObject> GetItemDictionary()
    {
        Dictionary<string, GameObject> aux;
        aux = new Dictionary<string, GameObject>();

        Object3D[] objectsArray = Resources.LoadAll<Object3D>("ScriptableObjects");
        for (int i = 0; i < objectsArray.Length; i++)
        {
            aux[objectsArray[i].name] = objectsArray[i].objectPrefab;
        }

        return aux;
    }
}
