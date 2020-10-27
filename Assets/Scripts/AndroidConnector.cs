using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidConnector : IConnector
{
    public void LoadAnotherApplication(string bundleId)
    {
        bool fail = false;

        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject launchIntent = null;

        try {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
        }
        catch (System.Exception e) {
            fail = true;
        }

        if (fail) {
            Application.OpenURL("https://google.com");
        }
        else
            ca.Call("startActivity", launchIntent);

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
        Application.Quit();
    }
}
