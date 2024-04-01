using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionPanelUI : MonoBehaviour
{
    RectTransform canvasRectTransform;

    void Start()
    {
        canvasRectTransform = GetComponentInParent<Canvas>().transform as RectTransform;
    }

    void Update()
    {
        //get mouse local canvas coordinates
        Vector3 mousePosition = Input.mousePosition;
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, Camera.main, out localMousePosition);

        //if pivot isn't relative to new mouse position, update pivot location
        float pivotX = localMousePosition.x < 0 ? -0.05f : 1.05f;
        float pivotY = localMousePosition.y < 0 ? -0.05f : 1.05f;

        GetComponent<RectTransform>().pivot = new Vector2(pivotX, pivotY);

        transform.position = Input.mousePosition;
    }
}
