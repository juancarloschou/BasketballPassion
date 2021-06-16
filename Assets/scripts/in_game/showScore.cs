using UnityEngine;
using System.Collections;

public class showScore : MonoBehaviour {

    static gameScript gameScript;

	public int balls, round, ball, limit, score, record, tempscore; //player 
	//public int[] mpoints = new int[2];

    public GUISkin newSkin;
	public Texture ball_img;

    public Texture2D PausaButton;
    public Texture2D AyudaButton;
    //private GUIContent content;
    public GUIStyle StyleButton;

    private float life_cooldown = 2f; //tiempo dura (vidas)
    public float life_count;
    private int life = 0; //texto para sumar o restar vidas

    public Texture2D VidaMas1;
    public Texture2D VidaMas2;
    public Texture2D VidaMas3;
    public Texture2D VidaMas4;
    public Texture2D VidaMas5;
    public Texture2D VidaMenos1;
    public Texture2D VidaMenos2;
    public Texture2D VidaMenos3;
    public Texture2D VidaMenos4;
    public Texture2D VidaMenos5;


    // Use this for initialization
    void Start()
    {
        gameScript = GameObject.Find("gameScript").GetComponent<gameScript>();

        //se inicializan en el Set llamado por el Star de scoreScript
        //balls = ball = round = score = record = player = 0;
        //multi = false;

        //imagen boton pausa
        //imageButton = GameObject.Find("btn_pausa").GetComponent<SpriteRenderer>();
        //imageButton = (Sprite)Resources.Load("Pausa");
        //content = new GUIContent();
        //content.image = image;
    }

    public void Set(int balls, int ball, int limit, int round, int score, int record) 
    {
		this.balls = balls;
		this.ball = ball;
        this.limit = limit;
		this.round = round+1;
		this.score = score;
		this.record = record;
	}

    public void SetVidas(int balls)
    {
        this.balls = balls;
    }

    public void PonerVidas(int vidas)
    {
        life = vidas;

        life_count = life_cooldown;
    }

    public void Update()
    {
        if (life != 0 && life_count > 0)
            life_count -= Time.deltaTime;
        else if (life_count <= 0)
            life = 0;
    }

    // Use this for initialization
    void OnGUI()
	{
        Pantalla.AutoResize(gameScript.ScreenX, gameScript.ScreenY);
        //AutoResize(800, 470);

        //boton de pausa
        if (GUI.Button(new Rect(170, 0, 40, 40), new GUIContent(PausaButton), StyleButton))
        {
            GameObject.Find("Controller").GetComponent<pauseMenu>().PonerPausa();
        }

        //boton de ayuda
        if (GUI.Button(new Rect(453, 0, 40, 40), new GUIContent(AyudaButton), StyleButton))
        {
            GameObject.Find("Controller").GetComponent<tutorialScript>().PonerTutorial(false);
        }

        GUI.skin = newSkin;
        //GUI.skin.button.fontSize = 24; //prueba


        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        GUI.Box(new Rect(10, 0, 85, 40), ""); //, styleLabel);
        GUI.DrawTexture(new Rect(15, 4f, 32f, 32f), ball_img, ScaleMode.ScaleToFit);
        GUI.skin.label.fontSize = 26;
        GUI.Label(new Rect(43, -2, 60, 38), "x" + balls);

        GUI.skin.label.fontSize = 16;
        GUI.skin.box.alignment = TextAnchor.MiddleLeft;
		
		GUI.Box(new Rect(230, 0, 90, 30), "Round:  "+round);
        GUI.Box(new Rect(230+90+1, 0, 110, 30), "Ball:  " + ball + " / " + limit);

        GUI.Box(new Rect(515, 0, 140, 30), "Score:  "+score);
		
		GUI.Box(new Rect(515+140+1, 0, 140, 30), "Record:  "+record);


        //ganar o perder vidas
        if (life != 0)
        {
            if (life == 1)
            {
                GUI.DrawTexture(new Rect(100, 8, 43, 24), VidaMas1, ScaleMode.ScaleToFit, true);
            }
            else if (life == 2)
            {
                GUI.DrawTexture(new Rect(100, 8, 43, 24), VidaMas2, ScaleMode.ScaleToFit, true);
            }
            else if (life == 3)
            {
                GUI.DrawTexture(new Rect(100, 8, 43, 24), VidaMas3, ScaleMode.ScaleToFit, true);
            }
            else if (life == 4)
            {
                GUI.DrawTexture(new Rect(100, 8, 43, 24), VidaMas4, ScaleMode.ScaleToFit, true);
            }
            else if (life == 5)
            {
                GUI.DrawTexture(new Rect(100, 8, 43, 24), VidaMas5, ScaleMode.ScaleToFit, true);
            }
            else if (life == -1)
            {
                GUI.DrawTexture(new Rect(100, 8, 43, 24), VidaMenos1, ScaleMode.ScaleToFit, true);
            }
            else if (life == -2)
            {
                GUI.DrawTexture(new Rect(100, 8, 43, 24), VidaMenos2, ScaleMode.ScaleToFit, true);
            }
            else if (life == -3)
            {
                GUI.DrawTexture(new Rect(100, 8, 43, 24), VidaMenos3, ScaleMode.ScaleToFit, true);
            }
            else if (life == -4)
            {
                GUI.DrawTexture(new Rect(100, 8, 43, 24), VidaMenos4, ScaleMode.ScaleToFit, true);
            }
            else if (life == -5)
            {
                GUI.DrawTexture(new Rect(100, 8, 43, 24), VidaMenos5, ScaleMode.ScaleToFit, true);
            }
        }

    }

}