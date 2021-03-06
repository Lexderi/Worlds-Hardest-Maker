using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolbarManager : MonoBehaviour
{
    public GameObject toolbarBackground;
    public GameObject infobarBackground;

    public static GameObject[] tools;
    void Start()
    {
        tools = GameObject.FindGameObjectsWithTag("Tool");

        // editmode wall when starting
        transform.GetChild(0).GetComponent<Tool>().SwitchGameMode();
    }
    private void Update()
    {
        // check if toolbar background is hovered
        GameManager.Instance.UIHovered = toolbarBackground.GetComponent<MouseUIHoverDetection>().IsPointerOverUIElement() ||
                                         infobarBackground.GetComponent<MouseUIHoverDetection>().IsPointerOverUIElement();
    }

    public static void DeselectAll()
    {
        for(int i = 0; i < tools.Length; i++)
        {
            tools[i].GetComponent<Tool>().Selected(false);
        }
    }
}
