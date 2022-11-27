using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Touch touch;
    private float speedModifier;

    public GameObject a;
    // Start is called before the first frame update
    void Start()
    {
        speedModifier = 0.001f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            // This line moves the object up-down, left-right and front-back 
            transform.position += new Vector3(0, touch.deltaPosition.y * speedModifier, 0);
            transform.position += new Vector3(touch.deltaPosition.x * speedModifier, 0,  0);
        }
    }
}
