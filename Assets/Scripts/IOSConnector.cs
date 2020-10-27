using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOSConnector : IConnector
{
    public void LoadAnotherApplication(string bundleId) {
        Application.OpenURL(bundleId);
        Application.Quit();
    }
}
