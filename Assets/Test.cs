using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CleverTap;
using CleverTap.Utilities;
public class Test : MonoBehaviour
{
    public void testc()
    {
        Debug.Log("Hello");
        // CleverTapUnity c = new CleverTapUnity();
        // Debug.Log(c);
    }
    public void LaunchInbox(Dictionary<string, object> styleConfig) {
        var styleConfigString = Json.Serialize(styleConfig);
        CleverTapBinding.ShowAppInbox(styleConfigString);
        Debug.Log("Appinbox show");
    }
}
