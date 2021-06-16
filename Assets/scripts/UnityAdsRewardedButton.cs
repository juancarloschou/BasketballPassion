using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdsRewardedButton : MonoBehaviour
{
    private string zoneId = "rewardedVideo";
    private int rewardQty = 3;

    private string alertMsg;
    Graficos graficos = new Graficos();

    void OnGUI()
    {
        if (string.IsNullOrEmpty(zoneId)) zoneId = null;

        Rect buttonRect = new Rect(10, 10, 150, 50);
        string buttonText = Advertisement.IsReady(zoneId) ? "Show Ad" : "Waiting...";

        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        if (GUI.Button(buttonRect, buttonText))
        {
            Advertisement.Show(zoneId, options);
        }

        //alerta tras el video
        float centerx = Screen.width / 2;
        float centery = Screen.height / 2;

        graficos.InitStyles(Color.white); //recuadro de color solido
        GUI.Box(new Rect(centerx - 300, centery - 100, 600, 200), "", graficos.currentStyle);

        GUI.skin.label.fontSize = 20; //34
        GUI.color = Color.black;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(centerx - 250, centery - 50, 500, 50), alertMsg);
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                alertMsg = "Video completed. User rewarded " + rewardQty + " lifes.";
                Debug.Log(alertMsg);
                break;
            case ShowResult.Skipped:
                alertMsg = "Video was skipped.";
                Debug.LogWarning(alertMsg);
                break;
            case ShowResult.Failed:
                alertMsg = "Video failed to show.";
                Debug.LogError(alertMsg);
                break;
        }
    }
}
