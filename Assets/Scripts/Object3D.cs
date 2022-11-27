using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Object", menuName = "3D Object")]
public class Object3D : ScriptableObject
{
    public Sprite icon;
    public GameObject objectPrefab;
    public string roomName;

    
}
