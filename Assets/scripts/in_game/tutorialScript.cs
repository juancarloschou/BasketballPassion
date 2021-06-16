using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tutorialScript : MonoBehaviour {

    public GameObject basketball; //tipo de basketball A (balon)
    public GameObject CurrentBall;
    static scoreScript scoreScript;
    static gameScript gameScript;

    private int MaxDots = 25;
    //private int MinDots = 8;
    private int dots;
    private float interval;
    private GameObject Dot;
    public List<GameObject> ind; //Dots
    private Vector3 startPos;

    private GameObject mano;
    private GameObject circulo;
    private SpriteRenderer fondoMenu;
    private GUIStyle currentStyle = null;

    public int parte;
    public float time;
    public float escala;
    float power;

    public float ballX, ballY;
    public float manoX, manoY;
    private float manoIniX, manoIniY;
    private bool menuPrincipal; //si es tutorial de menu principal

    private float tutorial_cooldown = 2.5f;
    private float tutorial_count = 0;
    private int tutorial_mensaje = 0;
    private float zonaCancel_cooldown = 2f;
    private float zonaCancel_count = 0;
    private bool tutorialMenu = false;

    private bool hecho;

    public Texture2D Replay;
    //public Texture2D Equis;
    public Texture2D OK;

    void StartTutorial()
    {
        this.enabled = true;

        //if (!menuPrincipal)
        //{
            //oculta y desactiva el balón
            scoreScript.CurrentBall.GetComponent<SpriteRenderer>().enabled = false;
            //desactiva la preparación del lanzamiento
            scoreScript.HidePath();
            scoreScript.CurrentBall.GetComponent<ballShot>().permitirTiro = false;
            //ocultar los mensajes de acciones
            scoreScript.esTutorial = true;
            scoreScript.LimpiarPantalla();
        //}

        interval = 1 / 20f; //lo lejos que llegan dots en la trayectoria, cuando menos interval haya

        dots = MaxDots; //van disminuyendo los dots

        Dot = GameObject.Find("Dot");
        //ind = Dots.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);
        ind = new List<GameObject>();
        for (int i = 0; i < dots; i++)
        {
            ind.Add((GameObject)Instantiate(Dot, new Vector3(0, 0, 0), Quaternion.identity));
            ind[i].GetComponent<Renderer>().enabled = false;
        }

        mano = GameObject.Find("mano");
        circulo = GameObject.Find("circulo");
        //zonaCancel = GameObject.Find("zona_cancel");
        fondoMenu = GameObject.Find("round_menu").GetComponent<SpriteRenderer>();

        //empieza tutorial
        parte = 0;
        time = 0;


        //parte = 21; //!!! pruebas
        //manoIniX = 0;
        //manoIniY = 0;
        //mano.transform.position = new Vector3(manoIniX, manoIniY, 0);
        //mano.GetComponent<SpriteRenderer>().enabled = true;
        //ballX = 0;
        //ballY = -2;


        //posicion balon
        ballX = 0;
        ballY = 0;
        CurrentBall = (GameObject)Instantiate(basketball, new Vector3(ballX, ballY, 0), Quaternion.identity);
        CurrentBall.GetComponent<ballShot>().permitirTiro = false;
        power = CurrentBall.GetComponent<ballShot>().power;

        tutorial_count = tutorial_cooldown;
        tutorial_mensaje = 0;
    }

    public void PonerTutorial(bool menuPPal)
    {
        if (!this.enabled)
        {
            menuPrincipal = menuPPal;
            scoreScript = GameObject.Find("Controller").GetComponent<scoreScript>();
            gameScript = GameObject.Find("gameScript").GetComponent<gameScript>();
            if (!menuPPal) //estas en medio de una partida (quizas deberia caparlo y solo desde pausa o menu ppal)
            {
                if (scoreScript.BalonParado())
                {
                    //deberia poner el boton en gris cuando no se pueda pulsar !!!
                    StartTutorial();
                }
            }
            else
                StartTutorial();
        }
    }

    void Update()
    {
        //muestra el mensaje tutorial
        tutorial_count -= Time.deltaTime;
        if (tutorial_count < 0)
            tutorial_count = 0;

        //muestra el zona cancel
        zonaCancel_count -= Time.deltaTime;
        if (zonaCancel_count < 0)
            zonaCancel_count = 0;

        if (parte == 0)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                //tras 1 sg crear mano
                parte = 1;

                manoIniX = 2;
                manoIniY = -8;
                mano.transform.position = new Vector3(manoIniX, manoIniY, 0); //pos inicial
                mano.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        if (parte == 1)
        {
            //mover mano a pos inicial
            manoX = 4;
            manoY = -2;
            mano.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime);
            if (mano.transform.position.y >= manoY)
            {
                parte = 2;

                time = 0;
                mano.transform.position = new Vector3(manoX, manoY, 0); //mano touch
            }
        }

        if (parte == 2)
        {
            //espera 1 sg crear touch
            time += Time.deltaTime;
            if (time > 1)
            {
                parte = 3;

                circulo.transform.position = new Vector3(manoX - 0.9f, manoY + 4, 0); //circulo touch
                escala = 3;
                circulo.transform.localScale = new Vector3(escala, escala, 0);
                circulo.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        if (parte == 3)
        {
            //circulo se empequeñece al pulsar
            escala -= (Time.deltaTime * 4f);
            circulo.transform.localScale = new Vector3(escala, escala, 0);
            if (escala < 0.35)
            {
                parte = 4;

                escala = 0.35f;
                circulo.transform.localScale = new Vector3(escala, escala, 0);

                //arma el tiro
                startPos = circulo.transform.position;
                CalculatePath(circulo.transform.position);
                ShowPath();

                //mueve la mano y el circulo
                manoIniX = manoX;
                manoIniY = manoY; //4, -2
            }
        }

        if (parte == 4)
        {
            //mueve la mano y el circulo
            manoX = manoIniX + 1.54f;
            manoY = manoIniY - 5f; //5.54, -7
            mano.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime * 0.4f);
            circulo.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime * 0.4f);

            //muestra el tiro
            CalculatePath(circulo.transform.position);

            if (mano.transform.position.y <= manoY)
            {
                parte = 5;

                time = 0;
                mano.transform.position = new Vector3(manoX, manoY, 0); //posicion mano suelta
                circulo.transform.position = new Vector3(manoX - 0.9f, manoY + 4, 0); //circulo suelta
            }
        }

        if (parte == 5)
        {
            //circulo se agranda al soltar
            escala += (Time.deltaTime * 4f);
            circulo.transform.localScale = new Vector3(escala, escala, 0);
            if (escala >= 3)
            {
                parte = 6;

                hecho = false;
                escala = 3;
                circulo.GetComponent<SpriteRenderer>().enabled = false;

                //lanza el tiro
                CurrentBall.transform.GetComponent<Rigidbody2D>().isKinematic = false; //deja de flotar y es afectada por la gravedad
                CurrentBall.transform.GetComponent<Collider2D>().enabled = true;
                Vector2 force = GetForce(circulo.transform.position) * 45.5f;
                CurrentBall.transform.GetComponent<Rigidbody2D>().AddForce(force);
                //CurrentBall.shot = true;
                //CurrentBall.aiming = false;
                //scoreScript.Shot();
                //scoreScript.HidePath();
                HidePath();

                time = 0;
            }
        }

        if (parte == 6)
        {
            time += Time.deltaTime;
            if (time > 1.2f)
            {
                if (!hecho)
                {
                    mano.GetComponent<SpriteRenderer>().enabled = false;
                    scoreScript.ShowSprite(true, false, false, false, false, 0, 1f);
                    hecho = true;
                }
            }

            if (time > 2.5f)
            //if (CurrentBall.GetComponent<ballShot>().hit_ground)
            {
                //pasar a siguiente tutorial
                parte = 10;

                tutorial_count = tutorial_cooldown;
                tutorial_mensaje = 1;

                CurrentBall.GetComponent<ballShot>().life = 0f;

                //crear nuevo balon
                ballX = 0;
                ballY = -2f;
                CurrentBall = (GameObject)Instantiate(basketball, new Vector3(ballX, ballY, 0), Quaternion.identity);
                CurrentBall.GetComponent<ballShot>().permitirTiro = false;

                time = 0;
            }
        }


        //------------------------------------
        //siguiente tutiorial
        //------------------------------------
        if (parte == 10)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                //tras 1 sg crear mano
                parte = 11;

                manoIniX = 2;
                manoIniY = -8;
                mano.transform.position = new Vector3(manoIniX, manoIniY, 0);
                mano.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        if (parte == 11)
        {
            //mover mano a balon
            manoX = 0.9f;
            manoY = -6;
            mano.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime);
            if (mano.transform.position.y >= manoY)
            {
                parte = 12;

                mano.transform.position = new Vector3(manoX, manoY, 0); //mano touch

                circulo.transform.position = new Vector3(manoX - 0.9f, manoY + 4, 0); //circulo touch
                escala = 3;
                circulo.transform.localScale = new Vector3(escala, escala, 0);
                circulo.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        if (parte == 12)
        {
            //circulo se empequeñece al pulsar
            escala -= (Time.deltaTime * 4f);
            circulo.transform.localScale = new Vector3(escala, escala, 0);
            if (escala < 0.35)
            {
                parte = 13;

                escala = 0.35f;
                circulo.transform.localScale = new Vector3(escala, escala, 0);

                //arma el tiro
                startPos = circulo.transform.position;
                CalculatePath(circulo.transform.position);
                ShowPath();

                //mueve la mano y el circulo
                manoIniX = manoX;
                manoIniY = manoY; //0.9, -6
            }
        }

        if (parte == 13)
        {
            //mueve la mano y el circulo
            manoX = manoIniX + 2f;
            manoY = manoIniY - 4.5f; //2.9, -10.5
            mano.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime * 0.6f);
            circulo.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime * 0.6f);

            //muestra el tiro
            CalculatePath(circulo.transform.position);

            if (mano.transform.position.y <= manoY)
            {
                parte = 14;

                mano.transform.position = new Vector3(manoX, manoY, 0); //mano touch
                circulo.transform.position = new Vector3(manoX - 0.9f, manoY + 4, 0); //circulo touch

                //mueve la mano y el circulo
                manoIniX = manoX;
                manoIniY = manoY; //0.9, -6
            }
        }

        if (parte == 14)
        {
            //mueve la mano y el circulo a la derecha
            manoX = manoIniX + 2f;
            manoY = manoIniY;
            mano.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime);
            circulo.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime);

            //muestra el tiro
            CalculatePath(circulo.transform.position);

            if (mano.transform.position.x >= manoX)
            {
                parte = 15;

                mano.transform.position = new Vector3(manoX, manoY, 0); //mano touch
                circulo.transform.position = new Vector3(manoX - 0.9f, manoY + 4, 0); //circulo touch

                //mueve la mano y el circulo
                manoIniX = manoX;
                manoIniY = manoY; //2.9, -6
            }
        }

        if (parte == 15)
        {
            //mueve la mano y el circulo a la izquierda
            manoX = manoIniX - 5f;
            manoY = manoIniY;
            mano.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime);
            circulo.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime);

            //muestra el tiro
            CalculatePath(circulo.transform.position);

            if (mano.transform.position.x <= manoX)
            {
                parte = 16;

                mano.transform.position = new Vector3(manoX, manoY, 0); //mano touch
                circulo.transform.position = new Vector3(manoX - 0.9f, manoY + 4, 0); //circulo touch

                time = 0;
            }
        }

        if (parte == 16)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                //muestra la zona de cancelacion
                parte = 17;

                zonaCancel_count = zonaCancel_cooldown;
            }
        }

        if (parte == 17)
        {
            if (zonaCancel_count <= 0)
            {
                parte = 18;

                //mover mano a zona cancel
                manoIniX = manoX;
                manoIniY = manoY; //-0.1, -10.5
            }
        }

        if (parte == 18)
        {
            manoX = -10f;
            manoY = -7;
            mano.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime * 0.7f);
            circulo.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime * 0.7f);

            //muestra el tiro
            CalculatePath(circulo.transform.position);

            if (mano.transform.position.x <= manoX)
            {
                parte = 19;

                mano.transform.position = new Vector3(manoX, manoY, 0); //mano touch
                circulo.transform.position = new Vector3(manoX - 0.9f, manoY + 4, 0); //circulo touch
            }
        }

        if (parte == 19)
        {
            //circulo se agranda al soltar
            escala += (Time.deltaTime * 4f);
            circulo.transform.localScale = new Vector3(escala, escala, 0);
            if (escala >= 3)
            {
                parte = 20;

                escala = 3;
                circulo.GetComponent<SpriteRenderer>().enabled = false;

                //cancela el tiro
                HidePath();
                time = 0;

                manoIniX = manoX;
                manoIniY = manoY;
            }
        }

        if (parte == 20)
        {
            //espera 1 seg
            time += Time.deltaTime;
            if (time > 1)
            {
                parte = 21;
            }
        }

        if (parte == 21)
        {
            //luego tira a tablero
            manoX = 3.5f;
            manoY = -3;
            mano.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime);

            if (mano.transform.position.x >= manoX)
            {
                parte = 22;

                mano.transform.position = new Vector3(manoX, manoY, 0);
                circulo.transform.position = new Vector3(manoX - 0.9f, manoY + 4, 0); //circulo touch
                circulo.GetComponent<SpriteRenderer>().enabled = true;
                escala = 3;
            }
        }

        if (parte == 22)
        {
            //circulo se empequeñece al pulsar
            escala -= (Time.deltaTime * 4f);
            circulo.transform.localScale = new Vector3(escala, escala, 0);
            if (escala < 0.35)
            {
                parte = 23;

                escala = 0.35f;
                circulo.transform.localScale = new Vector3(escala, escala, 0);

                //arma el tiro
                startPos = circulo.transform.position;
                CalculatePath(circulo.transform.position);
                ShowPath();

                //mueve la mano y el circulo
                manoIniX = manoX;
                manoIniY = manoY; 
            }
        }

        if(parte == 23)
        {
            //mueve mano para tirar
            manoX = manoIniX + 2.3f;
            manoY = manoIniY - 5.2f; 
            mano.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime * 0.5f);
            circulo.transform.Translate(new Vector3(manoX - manoIniX, manoY - manoIniY, 0) * Time.deltaTime * 0.5f);

            //muestra el tiro
            CalculatePath(circulo.transform.position);

            if (mano.transform.position.y <= manoY)
            {
                parte = 24;

                mano.transform.position = new Vector3(manoX, manoY, 0); //mano touch
                circulo.transform.position = new Vector3(manoX - 0.9f, manoY + 4, 0); //circulo touch

            }
        }

        if (parte == 24)
        {
            //circulo se agranda al soltar
            escala += (Time.deltaTime * 4f);
            circulo.transform.localScale = new Vector3(escala, escala, 0);
            if (escala >= 3)
            {
                parte = 25;

                hecho = false;
                escala = 3;
                circulo.GetComponent<SpriteRenderer>().enabled = false;

                //lanza el tiro
                CurrentBall.transform.GetComponent<Rigidbody2D>().isKinematic = false; //deja de flotar y es afectada por la gravedad
                CurrentBall.transform.GetComponent<Collider2D>().enabled = true;
                Vector2 force = GetForce(circulo.transform.position) * 45.5f;
                CurrentBall.transform.GetComponent<Rigidbody2D>().AddForce(force);
                //CurrentBall.shot = true;
                //CurrentBall.aiming = false;
                //scoreScript.Shot();
                //scoreScript.HidePath();
                HidePath();

                time = 0;
            }
        }

        if (parte == 25)
        {
            time += Time.deltaTime;
            if (time > 1.2f)
            {
                if (!hecho)
                {
                    mano.GetComponent<SpriteRenderer>().enabled = false;
                    scoreScript.ShowSprite(true, true, false, false, false, 0, 1f);
                    hecho = true;
                }
            }

            if (CurrentBall.GetComponent<ballShot>().hit_ground)
            {
                parte = 26;

                //finaliza tutorial
                tutorialMenu = true;
            }
        }

        if(parte == 26)
        {
            if (!tutorialMenu)
            {
                this.enabled = false;
            }

        }

    }

    void OnGUI()
    {
        Pantalla.AutoResize(gameScript.ScreenX, gameScript.ScreenY);
        //AutoResize(800, 470);

        if (tutorial_count > 0)
        {
            //mensajes sobre el tutorial
            float centerX = gameScript.ScreenX / 2;
            float centerY = gameScript.ScreenY / 2;

            GUI.Box(new Rect(centerX-150, 80, 300, 100), ""); //, styleLabel);
            GUI.skin.label.fontSize = 34;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.color = Color.white;
            GUI.Label(new Rect(centerX-140, 85, 280, 40), "Tutorial");

            GUI.skin.label.fontSize = 22;
            if (tutorial_mensaje == 0)
            {
                GUI.Label(new Rect(centerX - 140, 135, 280, 40), "How to shoot");
            }
            else if(tutorial_mensaje == 1)
            {
                GUI.Label(new Rect(centerX - 140, 135, 280, 40), "How to cancel the shoot");
            }
        }

        if(zonaCancel_count > 0)
        {
            InitStyles(new Color(1f,0.4f,1f,0.5f)); //zona cancel violeta transparente
            //GUI.Box(new Rect(0, 0, 70, Screen.height), "", currentStyle);
            GUI.Box(new Rect(0, 0, Screen.width / 10f, Screen.height), "", currentStyle);

            //GUI.Box(new Rect(70, 200, 260, 50), ""); //mensaje
            GUI.Box(new Rect(Screen.width / 10f, 200, 250, 50), ""); //mensaje
            GUI.skin.label.fontSize = 34;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.color = new Color(1f, 0.8f, 1f); //violeta claro
            GUI.Label(new Rect(Screen.width / 10, 205, 250, 40), "Cancel shoot");
        }

        if(tutorialMenu)
        {
            //GUI.skin = newSkin;
            Cursor.visible = true;
            theTutorialMenu();
        }
    }

    private void InitStyles(Color color)
    {
        if (currentStyle == null)
        {
            currentStyle = new GUIStyle(GUI.skin.box);
            //currentStyle.normal.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.5f));
            currentStyle.normal.background = MakeTex(2, 2, color);
        }
    }


    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }


    void theTutorialMenu()
    {
        float centerx = gameScript.ScreenX / 2;
        float centery = gameScript.ScreenY / 2;

        //background
        fondoMenu.enabled = true;

        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.skin.label.fontSize = 40;

        //GUI.Label(new Rect(centerx - 150, centery - 25 - 65, 300, 50), "Tutorial End");
        GUI.Label(new Rect(centerx - 150, centery - 40, 300, 50), "Tutorial End");

        GUI.skin.button.fontSize = 28;
        GUI.skin.button.alignment = TextAnchor.MiddleCenter;

        GUIContent btn = new GUIContent("Replay", Replay);
        if (GUI.Button(new Rect(centerx - 170 - 100, centery - 10 + 110, 220, 50), btn))
        {
            tutorialMenu = false;
            fondoMenu.enabled = false;
            StartTutorial();
        }

        btn = new GUIContent("Continue", OK);
        if (GUI.Button(new Rect(centerx - 170 + 200, centery - 10 + 110, 220, 50), btn))
        {
            tutorialMenu = false;
            fondoMenu.enabled = false;
            //this.enabled = false;

            if (menuPrincipal)
            {
                GameObject.Find("AutoFade").GetComponent<AutoFade>().LoadLevel(1, 0.5f, 0.5f); //level 1 menu
            }
            else
            {
                //oculta y desactiva el balón
                scoreScript.CurrentBall.GetComponent<SpriteRenderer>().enabled = true;
                //desactiva la preparación del lanzamiento
                //scoreScript.HidePath();
                scoreScript.CurrentBall.GetComponent<ballShot>().permitirTiro = true;
                //ocultar los mensajes de acciones
                scoreScript.esTutorial = false;
                //scoreScript.LimpiarPantalla();
            }
        }
    }


    //
    /*  Path and dots */
    //
    public Vector2 GetForce(Vector2 toPos)
    {
        Vector2 force = new Vector2(startPos.x, startPos.y) - new Vector2(toPos.x, toPos.y);

        return force * power;
    }

    public void CalculatePath(Vector2 posicion)
    {
        //Vector2 vel = GetForce(posicion) / 30;
        //Vector2 vel = GetForce(posicion) * 50/30;
        Vector2 vel = GetForce(posicion) * 1.5f;
        float t;
        for (int i = 0; i < dots; i++)
        {
            ind[i].GetComponent<Renderer>().enabled = true;
            t = (float)i * interval; // tiempo
            Vector3 point = PathPoint(CurrentBall.transform.position, vel, t);
            point.z = -1.0f;
            ind[i].transform.position = point;
        }
    }

    //Get point position
    Vector2 PathPoint(Vector2 startP, Vector2 startVel, float t)
    {
        //t tiempo
        return startP + startVel * t + 0.5f * Physics2D.gravity * t * t;
        //return startP + startVel * t + 0.5f * GetComponent<Rigidbody2D>().gravityScale * Physics2D.gravity * t * t;
    }

    //Hide all used dots
    public void HidePath()
    {
        for (int i = 0; i < dots; i++)
            ind[i].GetComponent<Renderer>().enabled = false;
    }

    //Show all used dots
    public void ShowPath()
    {
        for (int i = 0; i < dots; i++)
            ind[i].GetComponent<Renderer>().enabled = true;
    }

}
