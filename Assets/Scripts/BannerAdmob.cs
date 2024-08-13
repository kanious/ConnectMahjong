using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GoogleMobileAds.Api;

public class BannerAdmob : MonoBehaviour
{
    //private BannerView bannerView;

    void Start()
    {
        //MobileAds.Initialize(initStatus => { });
        //RequestBanner();
    }

    private void RequestBanner()
    {
        //if (bannerView != null)
        //    bannerView.Destroy();

        //AdSize size = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        //bannerView = new BannerView("ca-app-pub-9486867262428667/5545926755", size, AdPosition.Bottom);

        //AdRequest request = new AdRequest.Builder().Build();
        //bannerView.LoadAd(request);



        //string deviceId = "ca42667b2c23b2f1";
        //List<string> deviceIds = new List<string>() { deviceId };

        //RequestConfiguration requestconfig = new RequestConfiguration.Builder().SetTestDeviceIds(deviceIds).build();
        //MobileAds.SetRequestConfiguration(requestconfig);
    }

    void Update()
    {
        
    }

    public static string GetDeviceID()
    {
        //// Get Android ID
        //AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
        //AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
        //AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");

        //string android_id = clsSecure.CallStatic<string>("getString", objResolver, "android_id");

        //// Get bytes of Android ID
        //System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        //byte[] bytes = ue.GetBytes(android_id);

        //// Encrypt bytes with md5
        //System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        //byte[] hashBytes = md5.ComputeHash(bytes);

        //// Convert the encrypted bytes back to a string (base 16)
        //string hashString = "";

        //for (int i = 0; i < hashBytes.Length; i++)
        //{
        //    hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        //}

        //string device_id = hashString.PadLeft(32, '0');

        //return device_id;
        return "";
    }
}
