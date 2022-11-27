using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialUIMenu : MonoBehaviour
{
    public List<MaterialObject> materialList = new List<MaterialObject>();
    
    public MaterialButton bttn1;
    public MaterialButton bttn2;
    public MaterialButton bttn3;
    public MaterialButton bttn4;

    public GameObject leftBttn, rightBttn;

    int index = 0;
    bool clicked = false;
    GameObject containerObject;
    Animator anim;
    private void Awake()
    {
        MaterialObject[] objectsArray = Resources.LoadAll<MaterialObject>("Materials");

        for (int i = 0; i < objectsArray.Length; i++)
        {
            materialList.Add(objectsArray[i]);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        DisplayList();
        containerObject = transform.GetChild(0).gameObject;
        containerObject.SetActive(false);
        anim = gameObject.GetComponent<Animator>();
    }

    
    public void DisplayList()
    {
        index = 0;
        if (materialList == null || materialList.Count == 0)
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
            if (materialList.Count > 0 && materialList[0] != null)
            {
                bttn1.SetReference(materialList[0], index);
                index++;
            }
            else
                bttn1.SetReference(null, -1);
            if (materialList.Count > 1 && materialList[1] != null)
            {
                bttn2.SetReference(materialList[1], index);
                index++;
            }
            else
                bttn2.SetReference(null, -1);
            if (materialList.Count > 2 && materialList[2] != null)
            {
                bttn3.SetReference(materialList[2], index);
                index++;
            }
            else
                bttn3.SetReference(null, -1);
            if (materialList.Count > 3 && materialList[3] != null)
            {
                bttn4.SetReference(materialList[3], index);
                index++;
            }
            else
                bttn4.SetReference(null, -1);

            if (materialList.Count > 4)
            {
                leftBttn.SetActive(true);
                rightBttn.SetActive(true);
            }
            else
            {
                leftBttn.SetActive(false);
                rightBttn.SetActive(false);
            }
        }
    }

    public void GoLeft()
    {
        if (materialList.Count > 4)
        {
            int leftId = bttn1.index;

            int newId = leftId + 1;
            if (newId >= materialList.Count) newId = 0;

            MaterialObject i = materialList[newId];
            bttn1.SetReference(i, newId);

            if (newId + 1 >= materialList.Count) newId = 0;
            else newId += 1;
            i = materialList[newId];
            bttn2.SetReference(i, newId);

            if (newId + 1 >= materialList.Count) newId = 0;
            else newId += 1;
            i = materialList[newId];
            bttn3.SetReference(i, newId);

            if (newId + 1 >= materialList.Count) newId = 0;
            else newId += 1;
            i = materialList[newId];
            bttn4.SetReference(i, newId);
        }
    }

    public void GoRight()
    {
        if (materialList.Count > 4)
        {
            int rightId = bttn4.index;

            int newId = rightId - 1;
            if (newId < 0) newId = materialList.Count - 1;

            MaterialObject i = materialList[newId];
            bttn4.SetReference(i, newId);

            if (newId - 1 < 0) newId = materialList.Count - 1;
            else newId -= 1;
            i = materialList[newId];
            bttn3.SetReference(i, newId);

            if (newId - 1 < 0) newId = materialList.Count - 1;
            else newId -= 1;
            i = materialList[newId];
            bttn2.SetReference(i, newId);

            if (newId - 1 < 0) newId = materialList.Count - 1;
            else newId -= 1;
            i = materialList[newId];
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
        if (clicked == false) //open menu
        {
            StartCoroutine("Open");
        }
        else //hide menu
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
}
