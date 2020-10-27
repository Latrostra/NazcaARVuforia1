using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour, IInput
{
    [HideInInspector]
    public Camera camera;

    public void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Utillity.ShotRaycast(ClickAction, camera);
        }
    }

    public void ClickAction(RaycastHit hit) {
        Debug.Log("Got Mouse Button (0)");
        if (hit.collider.gameObject.GetComponent<IClickAction>() != null) {
            hit.collider.gameObject.GetComponent<IClickAction>().ClickAction();
        }
    }
}   
