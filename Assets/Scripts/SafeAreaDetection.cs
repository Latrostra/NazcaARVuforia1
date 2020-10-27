using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SafeAreaDetection : MonoBehaviour
{
    public static Action<Rect> OnSafeAreaChanged;
    private Rect _safeArea;
    private void Awake() {
        _safeArea = Screen.safeArea;
    }

    public void Update() {
        if (_safeArea != Screen.safeArea) {
            _safeArea = Screen.safeArea;
            OnSafeAreaChanged?.Invoke(_safeArea);
        }
    }
}
