using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class GoogleMobileAdsDemoScript : MonoBehaviour
{
    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            CreateBannerView();
            LoadAd();

            // This callback is called once the MobileAds SDK is initialized.
        });
    }

    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/6300978111";//ca-app-pub-3940256099942544/6300978111
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
    private string _adUnitId = "unused";
#endif

    BannerView _bannerView;

    /// <summary>
    /// Creates a 320x50 banner view at top of the screen.
    /// </summary>
    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyAd();
        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create a 320x50 banner views at coordinate (0,50) on screen.
        //_bannerView = new BannerView(_adUnitId, AdSize.Banner, 60, 755);

        // Use the AdSize argument to set a custom size for the ad.
        AdSize adSize = new AdSize(50, 50);
        //_bannerView = new BannerView(_adUnitId, adSize, AdPosition.Bottom);
        ListenToAdEvents();
    }
    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public void LoadAd()
    {
        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }
    /// <summary>
    /// listen to events the banner view may raise.
    /// </summary>
    private void ListenToAdEvents()
    {
        // Raised when an ad is loaded into the banner view.
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }
    /// <summary>
    /// Destroys the banner view.
    /// </summary>
    public void DestroyAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    public void ShowAd()
    {
        _bannerView.Show();
    }

    public GameObject paymentPanel;
    public GameObject blackPanel;
    [SerializeField] private AudioSource audioSource;
    public void HideAd()
    {
        // Hide the ad banner view (replace _bannerView.Hide() with appropriate method call)
        // _bannerView.Hide();

        // Open the payment panel
        if (paymentPanel != null)
        {
            paymentPanel.SetActive(true);

            // Activate the black panel
            if (blackPanel != null)
            {
                blackPanel.SetActive(true);
            }
            else
            {
                Debug.LogError("Black panel reference is not set!");
            }
        }
        else
        {
            Debug.LogError("Payment panel reference is not set!");
        }
    }

    public void Cancel()
{
    // Close the payment panel
    if (paymentPanel != null)
    {
        paymentPanel.SetActive(false);

        // Deactivate the black panel
        if (blackPanel != null)
        {
            blackPanel.SetActive(false);

            // Play audio
            if (audioSource != null)
            {
                audioSource.enabled = true;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("Audio source reference is not set!");
            }
        }
        else
        {
            Debug.LogError("Black panel reference is not set!");
        }
    }
    else
    {
        Debug.LogError("Payment panel reference is not set!");
    }
}
}
