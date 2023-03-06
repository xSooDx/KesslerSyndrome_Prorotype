using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchInputHandler : MonoBehaviour
{
    bool isClicked = false;
    Vector2 start;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        isClicked = true;
        start = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("OnMouseDown");
    }

    private void OnMouseDrag()
    {
        if (!isClicked) return;
        Vector3 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        end.z = 0; 
        Debug.DrawLine(transform.position, end);
        Debug.Log("OnMouseDrag");
    }

    private void OnMouseUp()
    {
        isClicked = false;
        Vector3 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 delta = transform.position - end;
        RocketLaunchPlatform.Instance.LaunchRocket(delta);
        Debug.Log("OnMouseUp");
    }
}
