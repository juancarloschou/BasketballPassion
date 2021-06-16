using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scoreScript : MonoBehaviour
{
	public int balls, max_balls; //vidas
	public int ball = 0, limit = 10; //lanzamientos de la fase
	public int score = 0, default_score = 100; //puntos de la fase
    public int combo = 0; //canastas seguidas
	public int cleans = 0, banks = 0, longs = 0, uphighs = 0, tricks = 0; //cleans seguidos
    public bool down = false, end_it = false;
    //private string achv = ""; //si hay texto de logro conseguido se muestra

    //private float ball_cooldown = 2f; //tiempo dura (desde tira hasta aparece balon)
    private float shoot_cooldown = 2f;
    //private float achv_cooldown = 5f; //tiempo dura (logro)
    //private float ball_count = 1f; //debe inicializarse a 1 o aparecen 2 balones al principio
    private float clean_count, bank_count, long_count, uphigh_count, trick_count;

    //private float life_cooldown = 2f; //tiempo dura (vidas)
    //private float achv_count, life_count, combo_count;
    //private float achv_count, combo_count;
    private float combo_count;

    static gameScript gameScript;
    static coachScript coachScript;
    static showScore showScore;
    static pauseMenu pauseMenu;
    static roundEnd roundEnd;
    static objetivoScript objetivoScript;
    public GameObject basketball; //tipo de basketball A (balon)
    public GameObject CurrentBall;
    private GameObject clean_sprite, bank_sprite, long_sprite, uphigh_sprite, trick_sprite;
    private ballShot ballShot;

    private int MaxDots = 25;
    private int MinDots = 10;
    private int dots;
    private float interval;
    private GameObject Dot;
    public List<GameObject> ind; //Dots

    public bool esTutorial = false;
    public int round = 0;

    private Graficos graficos = new Graficos();

    void Awake()
    {
        //DontDestroyOnLoad(gameObject); //el Script se mantiene en todas las escenas
    }

    void Start()
    {
        gameScript = GameObject.Find("gameScript").GetComponent<gameScript>();
        coachScript = gameObject.GetComponent<coachScript>();
        showScore = gameObject.GetComponent<showScore>();
        pauseMenu = gameObject.GetComponent<pauseMenu>();
        roundEnd = gameObject.GetComponent<roundEnd>();
        objetivoScript = GameObject.Find("objetivoScript").GetComponent<objetivoScript>();

        round = gameScript.round;

        limit = objetivoScript.Objetivos[round].Bolas; //limite de bolas de la ronda

        //primero a coach, luego StartScript
        coachScript.enabled = true;
    }

    public void StartScript()
    {
        clean_sprite = GameObject.Find("clean_shot");
        bank_sprite = GameObject.Find("bank_shot");
        long_sprite = GameObject.Find("long_shot");
        uphigh_sprite = GameObject.Find("uphigh_shot");
        trick_sprite = GameObject.Find("trick_shot");

        if (round == 0)
            interval = 1 / 18f; //lo lejos que llegan dots en la trayectoria, cuando menos interval haya
        else if (round == 1)
            interval = 1 / 20f; //cada dot muestra la posicion en el tiempo segun intervalo
        else if (round == 2)
            interval = 1 / 22f; //cada dot muestra la posicion en el tiempo segun intervalo
        else if (round == 3)
            interval = 1 / 23f; //22 dots / 26, no llega a un segundo de trayectoria
        else if (round <= 10)
            interval = 1 / 24f; //22 dots / 26, no llega a un segundo de trayectoria
        else if(round <= 25)
            interval = 1 / 25f; //22 dots / 26, no llega a un segundo de trayectoria
        else if(round <= 50)
            interval = 1 / 26f; //22 dots / 26, no llega a un segundo de trayectoria
        else
            interval = 1 / 27f; //22 dots / 26, no llega a un segundo de trayectoria

        dots = (int)System.Math.Truncate(MaxDots - round / 2f); //van disminuyendo los dots poco a poco
        if (dots < MinDots)
            dots = MinDots;

        Dot = GameObject.Find("Dot");
        //ind = Dots.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);
        ind = new List<GameObject>();
        for (int i = 0; i < dots; i++)
        {
            ind.Add((GameObject)Instantiate(Dot, new Vector3(0, 0, 0), Quaternion.identity));
            ind[i].GetComponent<Renderer>().enabled = false;
        }

        showScore.enabled = true;
        combo = gameScript.combo;
        balls = gameScript.balls;
        //balls = 3; //!!!prueba
        max_balls = gameScript.max_balls;

        NewBall();
        //UpdateScore(); //cuando ball es cero, newBall llama a UpdateScore

        if (gameScript.ponerTutorial)
        {
            //tutorial desde menu ppal
            GameObject.Find("Controller").GetComponent<tutorialScript>().PonerTutorial(true); 
            gameScript.ponerTutorial = false;
        }


        if (objetivoScript.Objetivos[round].Obstaculos > 0)
        {
            //definir posicion, tamaño, movimiento, velocidad
            GameObject.Find("obstaculo").GetComponent<SpriteRenderer>().transform.position = new Vector3(-1, 2, 0);
            GameObject.Find("obstaculo").GetComponent<SpriteRenderer>().enabled = true;
            GameObject.Find("obstaculo").GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            GameObject.Find("obstaculo").GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find("obstaculo").GetComponent<BoxCollider2D>().enabled = false;
        }


        //Banner inferior juego €€€ (NO FUNCIONA EL BANNER ADMOB? PUBLI PANTALLA COMPLETA SI)

        //GameObject.Find("AdsManager").GetComponent<AdsManager>().AnuncioBannerInferiorJuego();

    }


    //
    /*  Path and dots */
    //
    public void CalculatePath()
    {
        ballShot ballShot = CurrentBall.GetComponent<ballShot>();
        Vector2 vel = ballShot.GetForce(Input.mousePosition) / 30;
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


    //
    /* Score and ball stuff */
    //
    void Update()
    {
        if (!coachScript.enabled)
        {
            /*
            if(down && ball_count > 0)
                ball_count -= Time.deltaTime;
            else if(ball_count <= 0)
            {
                NewBall();
                ball_count = ball_cooldown;
            }
            */

            if (combo_count > 0)
                combo_count -= Time.deltaTime;

            if (clean_count > 0)
                clean_count -= Time.deltaTime;
            else
                clean_sprite.GetComponent<Renderer>().enabled = false;

            if (bank_count > 0)
                bank_count -= Time.deltaTime;
            else
                bank_sprite.GetComponent<Renderer>().enabled = false;

            if (long_count > 0)
                long_count -= Time.deltaTime;
            else
                long_sprite.GetComponent<Renderer>().enabled = false;

            if (uphigh_count > 0)
                uphigh_count -= Time.deltaTime;
            else
                uphigh_sprite.GetComponent<Renderer>().enabled = false;

            if (trick_count > 0)
                trick_count -= Time.deltaTime;
            else
                trick_sprite.GetComponent<Renderer>().enabled = false;

            //if (achv != "" && achv_count > 0)
            //    achv_count -= Time.deltaTime;
            //else if (achv_count <= 0)
            //    achv = "";

            //if (life != "" && life_count > 0)
            //    life_count -= Time.deltaTime;
            //else if (life_count <= 0)
            //    life = "";
        }
    }

    /*
    public void AchivmentDone(string texto)
    {
        achv = texto;
        achv_count = achv_cooldown;
    }
    */

    public void ContinuePlay()
    {
        //si obtiene vidas extra continua jugando
        NewBall();
        showScore.enabled = true;
    }

    void NewBall()
    {
        float ballX, ballY;

        //down = false;
		if (ball >= limit || balls == 0)
        {
			end_it = true; //no genera un nuevo balon
			return;
		}
		else
			end_it = false;
				
		ball++;
        ballX = 0; ballY = 0;
        if (objetivoScript.Objetivos[round].Longs > 0)
        {
            if (round <= 10)
                ballX = Random.Range(7.5f, 10f); //lejos
            else 
                ballX = Random.Range(7.5f, 12f); //muy lejos
        }
        else if(objetivoScript.Objetivos[round].Obstaculos > 0)
        {
            if (round <= 10)
                ballX = Random.Range(5f, 10f); //lejos del obstaculo
            else
                ballX = Random.Range(5f, 12f);
        }
        else
        {
            if (round == 0)
                ballX = Random.Range(-4f, 2f); //primera fase tiros cercanos
            else if (round == 1)
                ballX = Random.Range(-4f, 4f); //mas lejos
            else if (round <= 3)
                ballX = Random.Range(-4f, 6f); //mas lejos
            else if (round <= 5)
                ballX = Random.Range(-4f, 8f); //lejos
            else if (round <= 10)
                ballX = Random.Range(-3f, 10f); //muy lejos
            else
                ballX = Random.Range(-2f, 12f); //toda la cancha
        }

        if (round == 0)
            ballY = Random.Range(0, 1.5f); //mas alto el balon
        else if (round <= 5)
            ballY = Random.Range(-1f, 1.5f); //mas bajo el balon
        else if (round <= 10)
            ballY = Random.Range(-2f, 1.5f); //mas bajo
        else if (round > 25)
            ballY = Random.Range(-3f, 1f); //muy bajo
        else
            ballY = Random.Range(-4f, 0.5f); //muy bajo

        CurrentBall = (GameObject)Instantiate(basketball, new Vector3(ballX, ballY, 0), Quaternion.identity);
        CurrentBall.GetComponent<ballShot>().ball = ball;
		
		//if(ball == 1)
		UpdateScore();
	}
	
	//Balon lanzado
	public void Shot()
    {
        if (!esTutorial)
		    WastedBall();
	}

	public void WastedBall()
    {
		//down = true; // aviso nuevo balon

        UpdateScore();
		//ball_count = ball_cooldown; //inicia conteo siguiente balon
	}
	
	public void BreakCombo() //si falla
    {
        if (!esTutorial)
        {
            combo = 0;
            cleans = 0;
            banks = 0;
            longs = 0;
            uphighs = 0;
            tricks = 0;

            //pierdes vida
            CambiarVidas(-1);

            gameScript.canastasIntentos++;

            objetivoScript.Objetivos[round].Falla();

            NewBall();
        }

        UpdateScore();
        ShowSprite(false, false, false, false, false, 0);
    }

    public void CambiarVidas(int vidas)
    {
        balls += vidas;
        showScore.SetVidas(balls);
        showScore.PonerVidas(-1);
    }

    /*
    //el balon ha llegado al suelo
    public void BalonLanzadoSuelo(bool score)
    {
        if(!score)
        {
            //pierdes vida
            balls--;
            life = "-1";
            life_count = life_cooldown;
        }
    }
    */

    public void Score(bool clean_shot, bool bank_shot, bool long_shot, bool uphigh_shot, bool trick_shot)
    {
        if (!esTutorial)
        {
            gameScript.canastasIntentos++;
            gameScript.canastasHechas++;

            NewBall();

            int extraLife = 0; //cuenta vidas extra
            float multi = 1.0f; //multiplicador puntos

            if (clean_shot)
            {
                //extraLife++;
                multi += 1f;
                cleans++;
            }
            if (bank_shot)
            {
                multi += .25f;
                banks++;
            }
            if (long_shot)
            {
                multi += 1.5f;
                longs++;
            }
            if (uphigh_shot)
            {
                multi += .5f;
                uphighs++;
            }
            if (trick_shot)
            {
                extraLife++;
                multi += 3f;
                tricks++;
            }
            combo++;

            ShowSprite(clean_shot, bank_shot, long_shot, uphigh_shot, trick_shot, combo);
            if (extraLife > 0)
            {
                CambiarVidas(extraLife);
            }

            objetivoScript.Objetivos[round].Score(clean_shot, bank_shot, long_shot, uphigh_shot, trick_shot);

            //balls += 1;
            if (balls > max_balls)
                balls = 10;

            score += (int)Mathf.Ceil(multi * (default_score + (combo - 1) * 30 + (round - 1) * 15));
            UpdateScore();

            if (end_it)
                NewBall();

            /*
            string achv = GameObject.Find("gameScript").GetComponent<aManager>().Check(combo, cleans, banks, longs, uphighs, tricks);
            if (achv != "")
            {
                AchivmentDone(achv);
            }
            */
        }
	}
	
	public void DestroyedBall(GameObject gBall)
    {
        //vidas 0 o bola 10 de la ronda
        //if (gBall == lastball || balls == 0)
        //if (balls <= 0 || ball >= limit)
        if (balls <= 0 || gBall.GetComponent<ballShot>().ball >= limit)
        {
            //gameObject.GetComponent<showScore>().enabled =  false;

            //showScore.enabled = false; //no hay que quitar score en el coach final, ni game over, que ponen y quitan vidas!!!

            if (balls > 0)
            {
                //primero coach, luego va a round end
                coachScript.FinRonda();
            }
            else
            {
                //game over
                FinRonda();
                //gameScript.FinishRound(score, combo, balls);
            }
		}
	}

    public void FinRonda()
    {
        //va a round end
        gameScript.FinishRound(score, combo, balls);
    }
	
    public void LimpiarPantalla()
    {
        //borra los mensajes de accion
        combo_count = 0;
        clean_count = 0;
        bank_count = 0;
        uphigh_count = 0;
        long_count = 0;
        trick_count = 0;
        //achv_count = 0;
        showScore.life_count = 0;
        //ball_count = 0; //nuevo balon
    }

    public bool BalonParado()
    {
        //indica si el balon esta parado y podemos arrancar tutorial

        //si no estamos lanzando o el balon se está moviendo o esta en el suelo
        ballShot = CurrentBall.GetComponent<ballShot>();
        if (ballShot.aiming)
            return false;
        if (CurrentBall.GetComponent<Rigidbody2D>().velocity.x != 0 || CurrentBall.GetComponent<Rigidbody2D>().velocity.y != 0)
            return false;
        if (ballShot.hit_ground && ballShot.life > 0)
            return false;
        return true;
    }

	
    void OnGUI()
    {
        Pantalla.AutoResize(gameScript.ScreenX, gameScript.ScreenY);
        //AutoResize(800, 470);

        if ((!pauseMenu.enabled) && (!roundEnd.enabled) && (!coachScript.enabled))
        {
            //combo x2
            if (combo_count > 0)
            {
                int PosX = gameScript.ScreenX / 2 - 40;
                if (combo >= 10)
                    PosX -= 20;
                if (combo >= 20)
                    PosX -= 10;

                /*
                GUI.skin.label.fontSize = 80;
                GUI.contentColor = Color.black;
                GUI.Label(new Rect(PosX, gameScript.ScreenY / 2, 200, 80), "x" + combo.ToString());

                GUI.skin.label.fontSize = 80;
                GUI.contentColor = Color.yellow;
                GUI.Label(new Rect(PosX - 5, gameScript.ScreenY / 2 - 5, 200, 80), "x" + combo.ToString());
                */

                GUI.skin.label.fontSize = 90;
                GUI.contentColor = Color.black;
                GUI.Label(new Rect(PosX-5, gameScript.ScreenY / 2-5, 210, 90), "x" + combo.ToString());

                GUI.skin.label.fontSize = 80;
                GUI.contentColor = Color.yellow;
                GUI.Label(new Rect(PosX, gameScript.ScreenY / 2, 200, 80), "x" + combo.ToString());
            }

            //objetivos
            int totalObjetivos = objetivoScript.Objetivos[round].Objetivos;
            if (totalObjetivos > 0)
            {
                GUI.skin.label.fontSize = 24;
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                float posy = gameScript.ScreenY / 2 - 150;
                GUI.contentColor = Color.black;
                GUI.Label(new Rect(gameScript.ScreenX - 200, posy, 200, 40), "Goals:");
                GUI.contentColor = Color.white;
                GUI.Label(new Rect(gameScript.ScreenX - 200-3, posy-3, 200, 40), "Goals:");

                string msg;
                int objetivo = 0;
                Rect pos; 
                if (objetivoScript.Objetivos[round].Cleans > 0)
                {
                    msg = objetivoScript.Objetivos[round].CleansDone.ToString();
                    msg += "/" + objetivoScript.Objetivos[round].Cleans.ToString();
                    msg += " clean shots";

                    objetivo++;
                    pos = GetPosObjetivo(objetivo, totalObjetivos);
                    GUI.contentColor = Color.black;
                    GUI.Label(pos, msg);
                    GUI.contentColor = Color.white;
                    GUI.Label(new Rect(pos.position.x-3, pos.position.y - 3, pos.width, pos.height), msg);
                }

                if (objetivoScript.Objetivos[round].Uphighs > 0)
                {
                    msg = objetivoScript.Objetivos[round].UphighsDone.ToString();
                    msg += "/" + objetivoScript.Objetivos[round].Uphighs.ToString();
                    msg += " uphighs shots";

                    objetivo++;
                    pos = GetPosObjetivo(objetivo, totalObjetivos);
                    GUI.contentColor = Color.black;
                    GUI.Label(pos, msg);
                    GUI.contentColor = Color.white;
                    GUI.Label(new Rect(pos.position.x - 3, pos.position.y - 3, pos.width, pos.height), msg);
                }

                if (objetivoScript.Objetivos[round].Banks > 0)
                {
                    msg = objetivoScript.Objetivos[round].BanksDone.ToString();
                    msg += "/" + objetivoScript.Objetivos[round].Banks.ToString();
                    msg += " bank shots";

                    objetivo++;
                    pos = GetPosObjetivo(objetivo, totalObjetivos);
                    GUI.contentColor = Color.black;
                    GUI.Label(pos, msg);
                    GUI.contentColor = Color.white;
                    GUI.Label(new Rect(pos.position.x - 3, pos.position.y - 3, pos.width, pos.height), msg);
                }

                if (objetivoScript.Objetivos[round].Longs > 0)
                {
                    msg = objetivoScript.Objetivos[round].LongsDone.ToString();
                    msg += "/" + objetivoScript.Objetivos[round].Longs.ToString();
                    msg += " long shots";

                    objetivo++;
                    pos = GetPosObjetivo(objetivo, totalObjetivos);
                    GUI.contentColor = Color.black;
                    GUI.Label(pos, msg);
                    GUI.contentColor = Color.white;
                    GUI.Label(new Rect(pos.position.x - 3, pos.position.y - 3, pos.width, pos.height), msg);
                }

                if (objetivoScript.Objetivos[round].Tricks > 0)
                {
                    msg = objetivoScript.Objetivos[round].TricksDone.ToString();
                    msg += "/" + objetivoScript.Objetivos[round].Tricks.ToString();
                    msg += " trick shots";

                    objetivo++;
                    pos = GetPosObjetivo(objetivo, totalObjetivos);
                    GUI.contentColor = Color.black;
                    GUI.Label(pos, msg);
                    GUI.contentColor = Color.white;
                    GUI.Label(new Rect(pos.position.x - 3, pos.position.y - 3, pos.width, pos.height), msg);
                }

                if (objetivoScript.Objetivos[round].Fallos > 0)
                {
                    msg = objetivoScript.Objetivos[round].FallosDone.ToString();
                    msg += "/" + objetivoScript.Objetivos[round].Fallos.ToString();
                    msg += " miss shots";

                    objetivo++;
                    pos = GetPosObjetivo(objetivo, totalObjetivos);
                    GUI.contentColor = Color.black;
                    GUI.Label(pos, msg);
                    GUI.contentColor = Color.white;
                    GUI.Label(new Rect(pos.position.x - 3, pos.position.y - 3, pos.width, pos.height), msg);
                }
            }
        }
    }

    private Rect GetPosObjetivo(int objetivo, int totalObjetivos)
    {
        Rect pos;
        switch(totalObjetivos)
        {
            case 1:
                pos = new Rect(gameScript.ScreenX - 200, gameScript.ScreenY / 2 - 100 , 200, 40);
                break;
            case 2:
                if(objetivo == 1)
                    pos = new Rect(gameScript.ScreenX - 200, gameScript.ScreenY / 2 - 100, 200, 40);
                else
                    pos = new Rect(gameScript.ScreenX - 200, gameScript.ScreenY / 2 - 50, 200, 40);
                break;
            case 3:
                if (objetivo == 1)
                    pos = new Rect(gameScript.ScreenX - 200, gameScript.ScreenY / 2 - 100, 200, 40);
                else if (objetivo == 2)
                    pos = new Rect(gameScript.ScreenX - 200, gameScript.ScreenY / 2 - 50, 200, 40);
                else
                    pos = new Rect(gameScript.ScreenX - 200, gameScript.ScreenY / 2, 200, 40);
                break;
            default:
                pos = new Rect(gameScript.ScreenX - 200, gameScript.ScreenY / 2 - 100, 200, 40);
                break;
        }
        return pos;
    }

    public void ShowSprite(bool clean_shot, bool bank_shot, bool long_shot, bool uphigh_shot, bool trick_shot, int iCombo)
    {
        ShowSprite(clean_shot, bank_shot, long_shot, uphigh_shot,  trick_shot, iCombo, shoot_cooldown);
    }

    public void ShowSprite(bool clean_shot, bool bank_shot, bool long_shot, bool uphigh_shot, bool trick_shot, int iCombo, float time_cooldown)
    {
        if (iCombo > 1)
        {
            combo_count = time_cooldown;
        }
        else
            combo_count = 0;

        int sprite = 0;
        Vector3 pos;

        //primero cuenta cuantas son y luego va poniendo una a una
        int totalSprites = 0;
        if (clean_shot)
            totalSprites++;
        if (bank_shot)
            totalSprites++;
        if (long_shot)
            totalSprites++;
        if (uphigh_shot)
            totalSprites++;
        if (trick_shot)
            totalSprites++;

        if (clean_shot)
        {
            sprite++;
            pos = GetPosicion(sprite, totalSprites);
            clean_sprite.transform.position = pos;
            clean_sprite.GetComponent<Renderer>().enabled = true;
            clean_count = time_cooldown;
        }
        else
            clean_count = 0;

        if (bank_shot)
        {
            sprite++;
            pos = GetPosicion(sprite, totalSprites);
            bank_sprite.transform.position = pos;
            bank_sprite.GetComponent<Renderer>().enabled = true;
            bank_count = time_cooldown;
        }
        else
            bank_count = 0;

        if (long_shot)
        {
            sprite++;
            pos = GetPosicion(sprite, totalSprites);
            long_sprite.transform.position = pos;
            long_sprite.GetComponent<Renderer>().enabled = true;
            long_count = time_cooldown;
        }
        else
            long_count = 0;

        if (uphigh_shot)
        {
            sprite++;
            pos = GetPosicion(sprite, totalSprites);
            uphigh_sprite.transform.position = pos;
            uphigh_sprite.GetComponent<Renderer>().enabled = true;
            uphigh_count = time_cooldown;
        }
        else
            uphigh_count = 0;

        if (trick_shot)
        {
            sprite++;
            pos = GetPosicion(sprite, totalSprites);
            trick_sprite.transform.position = pos;
            if(sprite != 5) //5 no caben
                trick_sprite.GetComponent<Renderer>().enabled = true;
            trick_count = time_cooldown;
        }
        else
            trick_count = 0;
    }

    private Vector3 GetPosicion(int sprite, int totalSprites)
    {
        Vector3 pos;
        switch(totalSprites)
        {
            case 1:
                pos = new Vector3(-3, 3.2f, 10); //junto a canasta
                break;
            case 2:
                if(sprite == 1)
                    pos = new Vector3(-3, 3.2f, 10); //junto a canasta
                else
                    pos = new Vector3(2.5f, 3.2f, 10); //a la derecha del primero
                break;
            case 3:
                //2 en fila y 1 debajo
                if (sprite == 1)
                    pos = new Vector3(-3, 3.2f, 10); //junto a canasta
                else if (sprite == 2)
                    pos = new Vector3(2.5f, 3.2f, 10); //a la derecha del primero
                else 
                    pos = new Vector3(-3, -0.5f, 10); //debajo del primero
                break;
            case 4:
                //2 en fila y 2 debajo
                if (sprite == 1)
                    pos = new Vector3(-3, 3.2f, 10); //junto a canasta
                else if (sprite == 2)
                    pos = new Vector3(2.5f, 3.2f, 10); //a la derecha del primero
                else if (sprite == 3)
                    pos = new Vector3(-3, -0.5f, 10); //debajo del primero
                else
                    pos = new Vector3(2.5f, -0.5f, 10); //a la derecha del tercero
                break;
            default:
                if (sprite == 1)
                    pos = new Vector3(-3, 3.2f, 10); //junto a canasta
                else if (sprite == 2)
                    pos = new Vector3(2.5f, 3.2f, 10); //a la derecha del primero
                else if (sprite == 3)
                    pos = new Vector3(-3, -0.5f, 10); //debajo del primero
                else 
                    pos = new Vector3(2.5f, -0.5f, 10); //a la derecha del tercero
                break;
        }
        return pos;
    }

    void UpdateScore()
    {
		//showScore.Set(balls, ball, gameScript.round, gameScript.points + score, gameScript.record, gameScript.multi, gameScript.player, gameScript.mpoints);
        showScore.Set(balls, ball, limit, gameScript.round, gameScript.points + score, gameScript.record);
    }

}
