using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInterstitial : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener {
    [SerializeField] string android = "Interstitial_Android";
    [SerializeField] string ios = "Interstitial_iOS";
    string id;

    private void Awake() {
        id = (Application.platform == RuntimePlatform.IPhonePlayer) ? ios : android;
    }

    public void Load(){
        Advertisement.Load(id, this);
    }

    public void Show(){
        Advertisement.Show(id, this);
    }

    public void OnUnityAdsAdLoaded(string id){} 
    public void OnUnityAdsFailedToLoad(string id, UnityAdsLoadError error, string message){}
    public void OnUnityAdsShowFailure(string id, UnityAdsShowError error, string message){}
    public void OnUnityAdsShowStart(string id){}
    public void OnUnityAdsShowClick(string id) {}
    public void OnUnityAdsShowComplete(string id, UnityAdsShowCompletionState showCompletionState) {}
}