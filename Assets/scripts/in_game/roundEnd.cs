using UnityEngine;
using System.Collections;

public class roundEnd : MonoBehaviour
{
    static gameScript gameScript;
    private SpriteRenderer fondoMenu;
    public GUISkin newSkin;

    private int points, total, record, round;
    private float porcentaje;
    private bool end = false;

    public Texture2D Equis;
    public Texture2D OK;
    public Texture2D Replay;

    public bool alert = false;
    public string alertMsg = "";
    public bool videoOK = false;

    private Graficos graficos = new Graficos();

    void Start()
    {
        gameScript = GameObject.Find("gameScript").GetComponent<gameScript>();
        fondoMenu = GameObject.Find("round_menu").GetComponent<SpriteRenderer>();
    }

    //public void Set(int lpoints, int ltotal, int lrecord, bool lend, bool lmulti, int[] lmpoints, int lplayer, int)
    public void Set(int lpoints, int ltotal, int lrecord, float lporcentaje, bool lend, int lround)
    {
		points = lpoints;
		total = ltotal;
		record = lrecord;
		end = lend;
        porcentaje = lporcentaje;
        round = lround+1;
    }
	
	void thePauseMenu()
	{
        float centerx = gameScript.ScreenX / 2;
        float centery = gameScript.ScreenY / 2;

        //background
        fondoMenu.enabled = true;

        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.skin.label.fontSize = 30;
        GUI.Label(new Rect(20, centery-20, 170, 40), "Avr: " + System.Math.Round(porcentaje, 2).ToString() + "%");


        GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUI.skin.label.fontSize = 40;

        if (end)
            GUI.Label(new Rect(centerx - 150, centery - 25 - 65, 300, 50), "Game Over");
        else
        {
            GUI.Label(new Rect(centerx - 150, centery - 25 - 65, 300, 50), "Round " + round.ToString());
        }

        GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.skin.label.fontSize = 30;
		//GUI.Label(new Rect(centerx -150 -20, centery -25 -10, 300, 50), "Round score: " + points);
		//GUI.Label(new Rect(centerx -150 -20, centery -25 +30, 300, 50), "Total score: " + total);
        GUI.Label(new Rect(centerx - 150 - 50, centery - 25 - 10, 300, 50), "Round score: " + points);
        GUI.Label(new Rect(centerx - 150 - 50, centery - 25 + 30, 300, 50), "Total score: " + total);

        //GUI.Label(new Rect(centerx + 150 + 20, centery - 25 - 10, 300, 50), "Record score:");
        //GUI.Label(new Rect(centerx + 150 + 20, centery - 25 + 30, 300, 50), "" + record);
        GUI.Label(new Rect(centerx + 150 - 50, centery - 25 - 10, 300, 50), "Record score:");
        GUI.Label(new Rect(centerx + 150 - 50, centery - 25 + 30, 300, 50), "" + record);

        GUI.skin.button.fontSize = 28;
		GUI.skin.button.alignment = TextAnchor.MiddleCenter;

        GUIContent btn = new GUIContent("Main Menu", Equis);
        if (GUI.Button(new Rect(centerx -170 -100, centery -10 + 110, 220, 50), btn))
		{
            //if(end)
            gameScript.FinalPartida();

            fondoMenu.enabled = false;			
			this.enabled = false;
            GameObject.Find("AutoFade").GetComponent<AutoFade>().LoadLevel(1, 0.5f,0.5f); //level 1 main menu
        }	
		
        if(end) //game over
        {
            //string text = multi && player == 0 ? "Next Player" : "Restart";
            btn = new GUIContent("Restart", Replay);
            if (GUI.Button(new Rect(centerx -170 +200, centery -10 + 110, 220, 50), btn))
            {
                gameScript.FinalPartida();

				fondoMenu.enabled = false;
				this.enabled = false; 	
				//if(!multi)
					GameObject.Find("gameScript").GetComponent<gameScript>().Restart();
				//else
				//	GameObject.Find("gameScript").GetComponent<gameScript>().MultiRestart(1);
			}

            if (gameScript.primerVidasExtra)
            {
                btn = new GUIContent("Play video and get 3 lifes", OK); //poner icono video
                if (GUI.Button(new Rect(centerx - 200, centery - 10 + 180, 400, 50), btn))
                {
                    //publicidad en game over €€€

                    //GameObject.Find("ChartBoostManager").GetComponent<CBads>().ShowAd("game_over");

                    //https://support.google.com/admob/answer/6201350?hl=es&ref_topic=2745287
                    //Se recomienda que los anuncios intersticiales aparezcan antes de la página de pausa, y no después. Las páginas de pausa suelen solicitar al usuario que toque un botón del tipo Siguiente.
                    //Los anuncios entre niveles o después del botón Siguiente pueden sorprender. Es preferible que los anuncios intersticiales entre niveles o fases se muestren explícitamente antes de cualquier botón del tipo Continuar o Siguiente nivel. De este modo, será menos probable que los usuarios hagan clics accidentales en los anuncios cuando intenten hacer clic en el botón Siguiente.

                    GameObject.Find("AdsManager").GetComponent<AdsManager>().VerAnuncioVideoGameOver();

                    //VideoVidasOK("test"); //es una prueba, en el movil quitarlo !!!

                    //fondo.enabled = false;
                    //this.enabled = false;
                    //GameObject.Find("gameScript").GetComponent<gameScript>().Restart();
                }
            }

        }
		else
        {
            btn = new GUIContent("Next Round", OK);
            if (GUI.Button(new Rect(centerx -170 +200, centery -10 + 110, 220, 50), btn) ){			
				fondoMenu.enabled = false;
				this.enabled = false; 	
				//if(!multi)
					GameObject.Find("gameScript").GetComponent<gameScript>().NextRound();
				//else
				//	GameObject.Find("gameScript").GetComponent<gameScript>().MultiNextRound();
			}
		}
		
	}

    public void VideoVidasOK(string msg)
    {
        alert = true;
        alertMsg = msg;
        videoOK = true;

        //gameScript.Sound(1);

        gameScript.primerVidasExtra = false; //gastadas
    }

    public void VideoVidasKO(string msg)
    { 
        alert = true;
        alertMsg = msg;
        videoOK = false;

        //gameScript.Sound(5);
    }

    public void AlertAceptar()
    {
        //recarga video
        //GameObject.Find("AdsManager").GetComponent<AdsManager>().AnuncioVideoGameOver();

        if (videoOK)
        {
            //sube 3 vidas
            GameObject.Find("Controller").GetComponent<scoreScript>().balls += 3;

            fondoMenu.enabled = false;
            this.enabled = false;

            //if(termina ronda)
            //GameObject.Find("gameScript").GetComponent<gameScript>().NextRound();
            //else
            GameObject.Find("Controller").GetComponent<scoreScript>().ContinuePlay();
        }
        else
        {
            //al menu ppal (habria que poner record !!!)
            fondoMenu.enabled = false;
            this.enabled = false;

            //GameObject.Find("gameScript").GetComponent<gameScript>().Restart();
            GameObject.Find("AutoFade").GetComponent<AutoFade>().LoadLevel(1, 0.5f, 0.5f); //level 1 main menu
        }

    }

    void OnGUI ()		
	{
        Pantalla.AutoResize(gameScript.ScreenX, gameScript.ScreenY);
        //AutoResize(800, 470);
		Cursor.visible = true;

        if (alert)
        {
            //alerta tras el video
            float centerx = gameScript.ScreenX / 2;
            float centery = gameScript.ScreenY / 2;

            graficos.InitStyles(Color.white); //recuadro de color solido
            GUI.Box(new Rect(centerx - 300, centery - 100, 600, 200), "", graficos.currentStyle);

            GUI.skin.label.fontSize = 24; //34
            GUI.color = Color.black;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(centerx - 250, centery - 50, 500, 50), alertMsg);

            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            GUIContent btn = new GUIContent("OK", OK);
            if (GUI.Button(new Rect(centerx - 50, centery + 20, 100, 40), btn))
            {
                AlertAceptar();
                alert = false;
            }

        }
        else
        {
            GUI.skin = newSkin;
            thePauseMenu();
        }
	}	
	
}