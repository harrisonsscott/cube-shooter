using System;
using UnityEngine;
using UnityEngine.Advertisements;

[Serializable]
public class Credentials {
    public string iosGameID;
    public string androidGameID;
}

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    public TextAsset credentialJson; // not included in the repo
    [SerializeField]
    Credentials credentials;
    [SerializeField]
    bool isTesting = true;
    private string selectedID;
    private void Awake() {
        // load game ids
        credentials = JsonUtility.FromJson<Credentials>(credentialJson.text);
        InitializeAds();
    }

    public void InitializeAds(){
        // set game id based on device type
        #if UNITY_IOS
            selectedID = credentials.iosGameID;
        #elif UNITY_ANDROID
            selectedID = credentials.androidGameID;
        #elif UNITY_EDITOR
            selectedID = credentials.androidGameID;
        #endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(selectedID, isTesting, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }
 
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
    }
}
