using System;
using UnityEngine;

// Librerias de Google Ads
using GoogleMobileAds;
using GoogleMobileAds.Api;

using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    static gameScript gameScript;

    public static AdsManager manager;    // Patron Singleton
    private InterstitialAd interstitialMP; //menu ppal
    //private BannerView bannerViewIJ; //inferior juego
    //private InterstitialAd interstitialGO; //game over video pantalla completa
    //private RewardBasedVideoAd interstitialGO; //game over video recompensa

    private bool primerMenu = true;

    private string zoneId = "rewardedVideo"; //unity ads zone Id del rewarded video game over
    private string interstitialId = "ca-app-pub-6945259204820562/1591208735"; //intersticial Id antes menu principal

    void Awake()
    {
        if (manager == null)
        {
            // Persistente entre escenas
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if (manager != this)
        {
            // Solo puede haber 1
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameScript = GameObject.Find("gameScript").GetComponent<gameScript>();

        //inicializa anuncios
        InicializaAnuncioImagenMenuPrincipal();
        AnuncioImagenMenuPrincipal();
        //InicializaVideoGameOver();
        //AnuncioVideoGameOver(); 

    }

    public void InicializaAnuncioImagenMenuPrincipal()
    {
        // Initialize an InterstitialAd 
        interstitialMP = new InterstitialAd(interstitialId);

        interstitialMP.OnAdClosed += HandleInterstitialClosed;
    }

    public void AnuncioImagenMenuPrincipal()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitialMP.LoadAd(request);
    }

    public void VerAnuncioImagenMenuPrincipal()
    {
        if (primerMenu)
        {
            //no pone publicidad la primera vez
            primerMenu = false;
        }
        else
        {
            if (interstitialMP.IsLoaded())
            {
                interstitialMP.Show();

            }
            else
                Debug.Log("No esta cargado el anuncio imagen Menu Principal");
        }
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    { 
        Debug.Log("HandleInterstitialClosed event received");
        //prepara anuncio para proxima vez
        interstitialMP.Destroy();
        AnuncioImagenMenuPrincipal();
    }


    public void VerAnuncioVideoGameOver()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show(zoneId, options); //rewarded video
    }

    private void HandleShowResult(ShowResult result)
    {
        string alertMsg;

        switch (result)
        {
            case ShowResult.Finished:
                alertMsg = "Video completed. User rewarded 3 lifes.";
                Debug.Log(alertMsg);
                GameObject.Find("Controller").GetComponent<roundEnd>().VideoVidasOK(alertMsg);
                break;
            case ShowResult.Skipped:
                alertMsg = "Video was skipped.";
                Debug.LogWarning(alertMsg);
                GameObject.Find("Controller").GetComponent<roundEnd>().VideoVidasKO(alertMsg);
                break;
            case ShowResult.Failed:
                alertMsg = "Video failed to show.";
                Debug.LogError(alertMsg);
                GameObject.Find("Controller").GetComponent<roundEnd>().VideoVidasKO(alertMsg);
                break;
        }
    }


    /*
    //no funcionan los banners ???
    public void AnuncioBannerInferiorJuego()
    {
        // Creacion del banner "Banner inferior juego"
        bannerViewIJ = new BannerView("ca-app-pub-6945259204820562/7916943932", AdSize.Banner, AdPosition.Top);
        // Creacion de la peticion de anuncio
        AdRequest request = new AdRequest.Builder().Build();
        // Carga del anuncio
        bannerViewIJ.LoadAd(request);
    }

    public void QuitarAnuncioBannerInferiorJuego()
    {
        bannerViewIJ.Hide();
    }
    

    public void InicializaVideoGameOver()
    {
        //Rewarded video "game over"

        interstitialGO = RewardBasedVideoAd.Instance;

        interstitialGO.OnAdLoaded += HandleRewardBasedVideoLoaded;
        interstitialGO.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        interstitialGO.OnAdOpening += HandleRewardBasedVideoOpened;
        interstitialGO.OnAdStarted += HandleRewardBasedVideoStarted;
        interstitialGO.OnAdRewarded += HandleRewardBasedVideoRewarded;
        interstitialGO.OnAdClosed += HandleRewardBasedVideoClosed;
        interstitialGO.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
    }

    public void AnuncioVideoGameOver()
    {
        // Initialize an InterstitialAd "Video en game over"
        //interstitialGO = new InterstitialAd("ca-app-pub-6945259204820562/5881807539");
        // Create an empty ad request.
        //AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        //interstitialGO.LoadAd(request);


        //Rewarded video "game over"

        AdRequest request = new AdRequest.Builder().Build();
        interstitialGO.LoadAd(request, "ca-app-pub-6945259204820562/8835273934"); //rewarded 
        //interstitialGO.LoadAd(request, "ca-app-pub-6945259204820562/5881807539"); //video normal !!!

    }

    public void VerAnuncioVideoGameOver()
    {
        if (interstitialGO.IsLoaded())
        {
            interstitialGO.Show();
        }
        else
            Debug.Log("No esta cargado el anuncio video Game Over");
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        print("HandleRewardBasedVideoLoaded event received.");

        //gameScript.Sound(1);
        //GameObject.Find("Controller").GetComponent<roundEnd>().VideoVidasKO("loaded");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);

        //gameScript.Sound(5);

        GameObject.Find("Controller").GetComponent<roundEnd>().VideoVidasKO("load failed " + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        print("HandleRewardBasedVideoOpened event received");

        //GameObject.Find("Controller").GetComponent<roundEnd>().VideoVidasKO("opened");
        //gameScript.Sound(2);
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        print("HandleRewardBasedVideoStarted event received");

        //GameObject.Find("Controller").GetComponent<roundEnd>().VideoVidasKO("opened");
        //gameScript.Sound(3);
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        print("HandleRewardBasedVideoClosed event received");

        //gameScript.Sound(4);

        //prepara anuncio para proxima vez
        AnuncioVideoGameOver();

        GameObject.Find("Controller").GetComponent<roundEnd>().VideoVidasKO("closed");
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        print("HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);

        //gameScript.Sound(1);

        GameObject.Find("Controller").GetComponent<roundEnd>().VideoVidasOK("rewarded " + amount.ToString() + " " + type);
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        print("HandleRewardBasedVideoLeftApplication event received");

        //gameScript.Sound(2);

        GameObject.Find("Controller").GetComponent<roundEnd>().VideoVidasOK("left app");
    }


    //public void OnDestroy()
    //{
    //    //Un-Subscribe to Ad event once
    //    interstitialGO.OnAdLoaded -= HandleRewardBasedVideoLoaded;
    //    interstitialGO.OnAdFailedToLoad -= HandleRewardBasedVideoFailedToLoad;
    //    interstitialGO.OnAdOpening -= HandleRewardBasedVideoOpened;
    //    interstitialGO.OnAdStarted -= HandleRewardBasedVideoStarted;
    //    interstitialGO.OnAdRewarded -= HandleRewardBasedVideoRewarded;
    //    interstitialGO.OnAdClosed -= HandleRewardBasedVideoClosed;
    //    interstitialGO.OnAdLeavingApplication -= HandleRewardBasedVideoLeftApplication;
    //}
    */
}
