using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppLovinMax;

public class ApplovinMaxManager : MonoBehaviour
{
    private string adUnitIdInterstitial = "08a4c2ee787148e7";
    private string adUnitIdRewarded = "cfce02e40fcc205d";
    private string adUnitIdBanner = "867edadc74688fae";

    void Start() {
        // Khởi tạo SDK
        MaxSdkCallbacks.OnSdkInitializedEvent += (config) => {
            Debug.Log("MAX SDK Initialized");

            // Load ads
            LoadInterstitial();
            LoadRewarded();
            InitBanner();
        };

        MaxSdk.SetSdkKey("lzy1D43gpRPW9tUi5NWKHgdzaEOHtF7TwA6i9VTFBwSD_dPNImwq28W9Gc2dAx9esZW4uiX14oOUbg-RlCtXlT");
        MaxSdk.InitializeSdk();
    }

    // Interstitial
    void LoadInterstitial() {
        MaxSdk.LoadInterstitial(adUnitIdInterstitial);
    }

    public void ShowInterstitial() {
        #if UNITY_EDITOR
                Debug.Log("ShowInterstitial() chỉ hoạt động trên thiết bị thật.");
        #else
            if (MaxSdk.IsInterstitialReady(adUnitIdInterstitial))
            {
                MaxSdk.ShowInterstitial(adUnitIdInterstitial);
            }
        #endif
    }

    // Rewarded
    void LoadRewarded() {
        MaxSdk.LoadRewardedAd(adUnitIdRewarded);
    }

    public void ShowRewarded() {
        #if UNITY_EDITOR
                Debug.Log("ShowRewarded() chỉ hoạt động trên thiết bị thật.");
        #else
                if (MaxSdk.IsRewardedAdReady(adUnitIdRewarded)) {
                    MaxSdk.ShowRewardedAd(adUnitIdRewarded);
                }
        #endif
    }

    // Banner
    void InitBanner() {
        #if UNITY_EDITOR
                Debug.Log("InitBanner() chỉ hoạt động trên thiết bị thật.");
        #else
                MaxSdk.CreateBanner(adUnitIdBanner, MaxSdkBase.BannerPosition.BottomCenter);
                MaxSdk.ShowBanner(adUnitIdBanner);
        #endif
    }
}
