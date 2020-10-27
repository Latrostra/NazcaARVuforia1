using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AplicationController : MonoBehaviour
{

    private IConnector connector;

    String bundleId;

    public void Start() {
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            connector = new IOSConnector();
            bundleId = "visca://";
        }
        else if (Application.platform == RuntimePlatform.Android) {
            connector = new AndroidConnector();
            bundleId = "pl.apagroup.andvisca";
        }
    }
    
    public void LoadApp() {
        connector.LoadAnotherApplication(bundleId);
    }
}

