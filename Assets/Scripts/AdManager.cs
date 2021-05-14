using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Events;
public class AdManager : MonoBehaviour
{
    public static AdManager instance;

    private string appID = "ca-app-pub-6351610926806883~7616644332";

// **************Banner Ad Id & var****************
    // private BannerView bannerView;
    // private string bannerID = "ca-app-pub-6351610926806883/7339427609";

// **************Interstitial Ad Id & var****************
    private InterstitialAd fullscreenAd;
    private string fullscreenAdID = "ca-app-pub-6351610926806883/2219195032";


// *****************************************************************
    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
    }
    private void Start() {
        RequestFullScreenAd();
        // RequestBanner();
    }
    

    // ********************Interstitial Ad********************
    public void RequestFullScreenAd(){
        fullscreenAd = new InterstitialAd(fullscreenAdID);

        AdRequest request = new AdRequest.Builder().Build();

        fullscreenAd.LoadAd(request);
    }

    // for interstitial we make another fn and check that ad is loaded or not
    public void ShowFullScreenAd(){
        if(fullscreenAd.IsLoaded()){
            fullscreenAd.Show();
        }else{
            RequestFullScreenAd();
        }
    }

}
