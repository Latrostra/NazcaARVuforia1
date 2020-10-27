using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utillity
{
    // Shot raycast and call action
    public static void ShotRaycast(Action<RaycastHit> RaycastAction, Camera camera) {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            RaycastAction?.Invoke(hit);
        }   
    }
}
