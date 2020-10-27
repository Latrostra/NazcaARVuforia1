using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFactory : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    private void Awake() {
        if (Application.platform == RuntimePlatform.Android) {
            var obj = gameObject.AddComponent<TouchInput>();
            obj.camera = this.camera;
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
            var obj = gameObject.AddComponent<MouseInput>();
            obj.camera = this.camera;
        }
    }
}
