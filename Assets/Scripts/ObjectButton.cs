using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectButton : MonoBehaviour
{
    public Object3D objectReference = null;
    public Image icon;
    public int index;

    private void Awake()
    {
        icon = GetComponent<Image>();
    }
    public void ActionOnClick()
    {
        //TODO
        if(objectReference != null)
        {
            Debug.Log(objectReference.name);
        }
    }

    public void SetReference(Object3D reff, int id)
    {
        objectReference = reff;
        icon.sprite = reff != null ? reff.icon : null;
        index = id;
        Color c = icon.color;
        c.a = reff != null ? 1 : 0;
        icon.color = c;
    }
}
