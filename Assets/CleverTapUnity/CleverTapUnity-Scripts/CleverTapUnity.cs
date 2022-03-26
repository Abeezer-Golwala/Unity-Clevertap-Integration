using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using CleverTap;
using CleverTap.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CleverTapUnity: MonoBehaviour {

    public String CLEVERTAP_ACCOUNT_ID = "TEST-468-W87-546Z";
    public String CLEVERTAP_ACCOUNT_TOKEN = "TEST-ab0-b64";
    public String CLEVERTAP_ACCOUNT_REGION = "eu1";
    public int CLEVERTAP_DEBUG_LEVEL = 3;
    public bool CLEVERTAP_ENABLE_PERSONALIZATION = true;
    public bool CLEVERTAP_DISABLE_IDFV;

    void Awake(){
        #if (UNITY_IPHONE && !UNITY_EDITOR)
        DontDestroyOnLoad(gameObject);
        CleverTapBinding.SetDebugLevel(CLEVERTAP_DEBUG_LEVEL);
        CleverTapBinding.LaunchWithCredentialsForRegion(CLEVERTAP_ACCOUNT_ID, CLEVERTAP_ACCOUNT_TOKEN, CLEVERTAP_ACCOUNT_REGION);
        if (CLEVERTAP_ENABLE_PERSONALIZATION) {
            CleverTapBinding.EnablePersonalization();
        }
        #endif
        #if (UNITY_ANDROID && !UNITY_EDITOR)
        DontDestroyOnLoad(gameObject);
        CleverTapBinding.SetDebugLevel(CLEVERTAP_DEBUG_LEVEL);
        CleverTapBinding.Initialize(CLEVERTAP_ACCOUNT_ID, CLEVERTAP_ACCOUNT_TOKEN, CLEVERTAP_ACCOUNT_REGION);
        Dictionary<string, string> newProps = new Dictionary<string, string>();
            newProps.Add("email", "test@test.com");
            newProps.Add("Identity", "123456");
        CleverTapBinding.OnUserLogin(newProps);
        CleverTapBinding.CreateNotificationChannel("abtest", "abtest", "CT test channel", 5, true);
        CleverTapBinding.RecordEvent("Send Basic Push");
        if (CLEVERTAP_ENABLE_PERSONALIZATION) {
            CleverTapBinding.EnablePersonalization();
        }
        #endif
        CleverTapBinding.InitializeInbox();
        Debug.Log("InboxInit started");
        LaunchInbox();
    }
    void Start() {}
    void CleverTapDeepLinkCallback(string url) {
        Debug.Log("unity received deep link: " + (!String.IsNullOrEmpty(url) ? url : "NULL"));
    }
    void CleverTapProfileInitializedCallback(string message) {
        Debug.Log("unity received profile initialized: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        if (String.IsNullOrEmpty(message)) {
            return;
        }
        try {
            JSONClass json = (JSONClass)JSON.Parse(message);
            Debug.Log(String.Format("unity parsed profile initialized {0}", json));
        } catch {
            Debug.LogError("unable to parse json");
        }
    }
    void CleverTapProfileUpdatesCallback(string message) {
        Debug.Log("unity received profile updates: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));

        if (String.IsNullOrEmpty(message)) {
            return;
        }

        try {
            JSONClass json = (JSONClass)JSON.Parse(message);
            Debug.Log(String.Format("unity parsed profile updates {0}", json));
        } catch {
            Debug.LogError("unable to parse json");
        }
    }	
		
    // returns the data associated with the push notification
    void CleverTapPushOpenedCallback(string message) {
        Debug.Log("unity received push opened: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));

        if (String.IsNullOrEmpty(message)) {
            return;
        }

        try {
            JSONClass json = (JSONClass)JSON.Parse(message);
            Debug.Log(String.Format("push notification data is {0}", json));
        } catch {
            Debug.LogError("unable to parse json");
        }
    }

    // returns a unique CleverTap identifier suitable for use with install attribution providers.
    void CleverTapInitCleverTapIdCallback(string message) {
        Debug.Log("unity received clevertap id: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // returns the custom data associated with an in-app notification click
    void CleverTapInAppNotificationDismissedCallback(string message) {
        Debug.Log("unity received inapp notification dismissed: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // returns when an in-app notification is dismissed by a call to action with custom extras
    void CleverTapInAppNotificationButtonTapped(string message) {
        Debug.Log("unity received inapp notification button tapped: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }
    void LaunchInbox()
    {
        CleverTapBinding.ShowAppInbox(new Dictionary<string, object>());
        Debug.Log("Appinbox show");
    }
    // returns callback for InitializeInbox
    void CleverTapInboxDidInitializeCallback(){
        Debug.Log("unity received inbox initialized");
    }

    void CleverTapInboxMessagesDidUpdateCallback(){
        Debug.Log("unity received inbox messages updated");
    }

    // returns on the click of app inbox message with a map of custom Key-Value pairs
    void CleverTapInboxCustomExtrasButtonSelect(string message) {
        Debug.Log("unity received inbox message button with custom extras select: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // returns native display units data
    void CleverTapNativeDisplayUnitsUpdated(string message) {
        Debug.Log("unity received native display units updated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // invoked when Product Experiences - Product Config are fetched 
    void CleverTapProductConfigFetched(string message) {
        Debug.Log("unity received product config fetched: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // invoked when Product Experiences - Product Config are activated
    void CleverTapProductConfigActivated(string message) {
        Debug.Log("unity received product config activated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // invoked when Product Experiences - Product Config are initialized
    void CleverTapProductConfigInitialized(string message) {
        Debug.Log("unity received product config initialized: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // invoked when Product Experiences - Feature Flags are updated 
    void CleverTapFeatureFlagsUpdated(string message) {
        Debug.Log("unity received feature flags updated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }
    #if UNITY_EDITOR
    private void OnValidate() {
        EditorPrefs.SetBool("CLEVERTAP_DISABLE_IDFV", CLEVERTAP_DISABLE_IDFV);
    }
    #endif
}
