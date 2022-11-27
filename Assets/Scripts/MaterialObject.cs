using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Material", menuName = "Material Object")]
public class MaterialObject : ScriptableObject
{
    public Sprite icon;
    public Material objectMaterial;
}
