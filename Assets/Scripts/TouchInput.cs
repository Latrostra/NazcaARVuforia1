using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour, IInput
{
    [HideInInspector]
    public Camera camera;

    public void Update() {
        for (var i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                Utillity.ShotRaycast(ClickAction, camera);
            }
        }
    }

    public void ClickAction(RaycastHit hit)
    {
        Debug.Log("Got Touch Input");
        if (hit.collider.gameObject.GetComponent<IClickAction>() != null) {
            hit.collider.gameObject.GetComponent<IClickAction>().ClickAction();
        }
    }
}
