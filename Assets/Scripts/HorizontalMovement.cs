using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalMovement : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] Image img;
    [SerializeField] GameObject left, right;

    Collider2D button;

    private void Awake()
    {
        button.transform.SetParent(btn.transform, false);
       
    }

    public void OnPressedButton()
    {
        
    }

    private void OnMouseDown()
    {
        if(img.transform.position.x > right.transform.position.x)
            img.transform.position = right.transform.position;
        else if(img.transform.position.x < left.transform.position.x)
            img.transform.position = left.transform.position;
        else
            img.transform.position = new Vector3(Input.GetTouch(0).position.x,0,0);
    }
}
