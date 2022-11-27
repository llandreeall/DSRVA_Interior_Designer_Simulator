using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomObjectsUI : MonoBehaviour
{
    public List<Object3D> objectsList = new List<Object3D>();
    public string roomName;
    public bool selected = false;
    public Button roomButton;
    public Color baseColor;
    public Color highlightColor;

    public GameObject parent;
    // Start is called before the first frame update
    void Awake()
    {
        roomButton = GetComponent<Button>();
        Object3D[] objectsArray = Resources.LoadAll<Object3D>("ScriptableObjects");
        
        for(int i = 0; i < objectsArray.Length; i++){
            if (objectsArray[i].roomName == this.roomName)
                objectsList.Add(objectsArray[i]);
        }
        baseColor = roomButton.GetComponent<Image>().color;
    }

    public void Select()
    {
        ObjectsUIMenu reff = parent.transform.GetComponentInParent<ObjectsUIMenu>();
        reff.SelectRoom(roomName);

        //Highlight
        Vector3 aux = transform.localScale;
        aux.y = 1.4f;
        transform.localScale = aux;
        roomButton.GetComponent<Image>().color = highlightColor;
    }

    public void Deselect()
    {
        Vector3 aux = transform.localScale;
        aux.y = 1f;
        transform.localScale = aux;
        roomButton.GetComponent<Image>().color = baseColor;
    }

}
