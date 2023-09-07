using System;
using UnityEngine;
using System.Collections;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class AdvertisementManager : MonoBehaviour {
	
	#region - singleton -
	static AdvertisementManager m_Instance; public static AdvertisementManager Instance{get{return m_Instance;}}
    #endregion
    #region - serialized -
    [SerializeField] Slider loadingBar;
    float showAdPeriodHour = 0.2f;// / (60f * 60f);
    #endregion
    #region - init -
    BannerView banner;
    InterstitialAd interstitial;
    InterstitialAd interstitial_Mid;

    void Awake()
	{
		#region - singleton -
		m_Instance = this;
        #endregion

        DontDestroyOnLoad(gameObject);
        loadingBar.transform.parent.gameObject.SetActive(false);

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
		MobileAds.Initialize("ca-app-pub-8674279891927404~6294399179");

        //banner
        banner = new BannerView("ca-app-pub-8674279891927404/1862038371", AdSize.Banner, AdPosition.Top);
        AdRequest request = new AdRequest.Builder().Build();
 
        banner.LoadAd(request);
        banner.Hide();
        
        //interstitial
        interstitial = new InterstitialAd("ca-app-pub-8674279891927404/9247865578");
        AdRequest requestInterstitial = new AdRequest.Builder().Build();
        interstitial.LoadAd(requestInterstitial);
        interstitial.OnAdClosed += HandleInterstitialClosed;

        interstitial_Mid = new InterstitialAd("ca-app-pub-8674279891927404/5136383594");
        AdRequest requestInterstitial_Mid = new AdRequest.Builder().Build();
        //interstitial_Mid.LoadAd(requestInterstitial_Mid);
#endif
    }
    #endregion
    #region - call back -
    float pausedTime = 0f;
    private void OnApplicationPause(bool pause)
    {
        Debug.Log("AdvertisementManager:: OnApplicationPause: pause = " + pause);

        if (pause == true)
            pausedTime = Time.realtimeSinceStartup;
        else
        {
            float elapsedTime = Time.realtimeSinceStartup - pausedTime;
            if (elapsedTime > showAdPeriodHour * 3600f)
            {
                Debug.Log("AdvertisementManager:: OnApplicationPause: pausing time = " + elapsedTime);

                Show_Interstitial_Mid();
            }
        }
    }

    private void OnDisable()
    {
        Debug.Log("AdvertisementManager:: OnDisable: ");
    }
    #endregion
    #region - public -
    public void Show_Banner()
	{
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
		banner.Show();
#endif
    }

    public void Close_Banner()
	{
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
		banner.Hide();
#endif
    }

    void OnDestroy()
	{
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        banner.Destroy();
#endif
    }

    public void Show_Interstitial()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        Debug.Log("interstitial.IsLoaded() == " + interstitial.IsLoaded());
        if (interstitial.IsLoaded())
        {
            Debug.Log("interstitial.IsLoaded() == true");
            interstitial.Show();
        }
#endif
    }

    public void Show_Interstitial_Mid()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        StartCoroutine(_Show_Interstitial_Mid_CR());
#endif
    }

    IEnumerator _Show_Interstitial_Mid_CR()
    {
        interstitial_Mid.LoadAd(new AdRequest.Builder().Build());

        loadingBar.transform.parent.gameObject.SetActive(true);
        loadingBar.value = 0f;

        float time = 0f;
        const float waitTime = 7f;
        while (true)
        {
            yield return null;

            loadingBar.value = time / waitTime;

            if (interstitial_Mid.IsLoaded() == true)
            {
                while (true)
                {
                    yield return null;

                    loadingBar.value += 0.2f;
                    if (loadingBar.value >= 1f)
                        break;
                }

                interstitial_Mid.Show();
                Debug.Log("AdvertisementManager:: _Show_Interstitial_Mid_CR: interstitial_Mid.Show()");
                break;
            }
            else
            {
                time += Time.deltaTime * 2f;
                if (time > waitTime)
                {
                    Debug.Log("AdvertisementManager:: _Show_Interstitial_Mid_CR: interstitial_Mid loading FAILED");
                    break;
                }
            }
        }
        
        loadingBar.transform.parent.gameObject.SetActive(false);
    }

    public bool Check_InterstitialIsLoaded()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        return interstitial.IsLoaded();
#endif
        return true;
    }
    #endregion
    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received.");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");
    }

    void HandleAdClosing(object sender, EventArgs args)
    {
        print("HandleAdClosing event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        print("HandleAdLeftApplication event received");
    }

    #endregion
    #region Interstitial callback handlers
    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        AdRequest requestInterstitial = new AdRequest.Builder().Build();
        interstitial.LoadAd(requestInterstitial);
    }
    #endregion
}