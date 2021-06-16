using UnityEngine;
using System.Collections;

public class pauseMenu : MonoBehaviour
{
    static gameScript gameScript;
    static scoreScript scoreScript;
    private GameObject background;

    public GUISkin newSkin;
    public GUIStyle StyleButton;
    public Texture2D Equis;
    public Texture2D OK;

    float porcentaje;

    void Start()
    {
        gameScript = GameObject.Find("gameScript").GetComponent<gameScript>();
        scoreScript = gameObject.GetComponent<scoreScript>();
        background = GameObject.Find("pause_menu");
    }
	
	void Update()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
            QuitarPausa();
        }
	}

    public void QuitarPausa()
    {
        this.enabled = false;
        Time.timeScale = 1.0f;
        background.GetComponent<Renderer>().enabled = false;
    }

    public void PonerPausa()
    {
        //no estamos en pausa ni en la pantalla de final de fase
        if (!this.enabled && !GameObject.Find("Controller").GetComponent<roundEnd>().enabled)
        {
            //desactiva la preparación del lanzamiento
            scoreScript = gameObject.GetComponent<scoreScript>();
            scoreScript.CurrentBall.GetComponent<ballShot>().aiming = false;

            Time.timeScale = 0.0f;
            //LoadAchivments();
            this.enabled = true;

            porcentaje = 100f * (float)gameScript.canastasHechas / gameScript.canastasIntentos;
        }
    }

    void thePauseMenu()
	{			
        float centerx = gameScript.ScreenX / 2;
        float centery = gameScript.ScreenY / 2;

        //background
        background.GetComponent<SpriteRenderer>().enabled = true;

        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.skin.label.fontSize = 30;
        GUI.Label(new Rect(20, centery-20, 170, 40), "Avr: " + System.Math.Round(porcentaje, 2).ToString() + "%");


        GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUI.skin.label.fontSize = 40;
        //GUI.Label(new Rect(centerx - 150, centery + 30 - 65, 300, 50), "Pause menu");
        GUI.Label(new Rect(centerx - 150, centery - 70, 300, 50), "Pause menu");

        GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.skin.label.fontSize = 28;

        scoreScript = gameObject.GetComponent<scoreScript>();
        int points = scoreScript.score;
        GUI.Label(new Rect(centerx - 150, centery + 10, 300, 45), "Round score: " + points);

        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
        GUIContent btn = new GUIContent("Main Menu", Equis);
        if (GUI.Button(new Rect(centerx - 150 - 90, centery - 10 + 110, 220, 50), btn))
        {
			Time.timeScale = 1.0f;
            GameObject.Find("AutoFade").GetComponent<AutoFade>().LoadLevel(1, 0.5f,0.5f); //level 1 main menu
		}

        btn = new GUIContent("Resume", OK);
        if (GUI.Button(new Rect(centerx - 150 + 180, centery - 10 + 110, 220, 50), btn))
        {
            Time.timeScale = 1.0f;
			this.enabled = false; 
			background.GetComponent<SpriteRenderer>().enabled = false;
		}	
	}
		
	void OnGUI ()		
	{
        Pantalla.AutoResize(gameScript.ScreenX, gameScript.ScreenY);
        //AutoResize(800, 470); //470x800->0,587

        GUI.skin = newSkin;
		Cursor.visible = true;	
		thePauseMenu();
	}

}