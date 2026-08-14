using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DebugUIClickBlocker : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
                Debug.Log("UI blocking click: " + result.gameObject.name);
        }
    }
}