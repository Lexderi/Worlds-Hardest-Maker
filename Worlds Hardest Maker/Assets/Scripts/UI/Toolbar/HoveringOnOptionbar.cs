using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class HoveringOnOptionbar : MonoBehaviour
{
    public GameObject optionBar;
    Animator anim;
    MouseOverUI mo;
    private void Start()
    {
        mo = GetComponent<MouseOverUI>();
        anim = optionBar.GetComponent<Animator>();
    }
    private void Update()
    {
        anim.SetBool("Hovered", mo.over && !GameManager.Instance.Playing);
    }
}
