using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(AudioSource))]

public class gameScript : MonoBehaviour
{
	public int round, points, record; //player
    public int balls, max_balls, start_balls, combo;
    public bool new_record, power; //multi
    public int canastasHechas = 0; //estadistica
    public int canastasIntentos = 0;

    public AudioClip net, rim_hit, rim_hit2, ground, ground2;
    public bool sfx; //, music;
    public float sfxV = 1f; //, musicV = 1.0f;
	
	public string username;
    public string[] score_names = new string[10];
	public double[] score_points = new double[10];

    public int MaxEscenarios = 5;
    public bool ponerTutorial = false;
    public bool primerVidasExtra = true; //puedes conseguir 3 vidas extra

    public int ScreenX = 800;
    public int ScreenY = 470;
    //LT26i(JCPC) 360x615->0,585, woxter zielo (CPRM)320x544->0, 588, nexus 5(PT) 360x567->0, 635, Sony Xperia Z2(EB) 360x567->0, 635
    //pantalla completa: sony experia LT26i (JCPC) 360x640->0,562, Samsung galaxi S3 (CPRM) 360x640->0,562, Meizu MX4 Pro (PT) 384x640->0,6, imagino que los otros seran altura 567+25 = 360x592->0,608
    //escala media 0,562 - 0,608 = 640x1094->0,585
    //AutoResize(800, 480); //NO, 0,6
    //AutoResize(800, 470); //470x800->0,587

    void Awake()
    {
		DontDestroyOnLoad(gameObject); //el Script se mantiene en todas las escenas
		Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
		Time.timeScale = 1.0f;
	}
	
	void Start ()
    {
		Reset();
		Load();
		GameObject.Find("AutoFade").GetComponent<AutoFade>().LoadLevel(1, 0.5f,0.5f); //level 1 main menu
    }
	
	public void FinishRound(int lpoints, int lcombo, int lballs)
    {
		points += lpoints;
			
		combo = lcombo;
		balls = lballs;

        if (balls > 0)
        {
            //next round
            GameObject.Find("Controller").GetComponent<roundEnd>().Set(lpoints, points, record, 100*canastasHechas/canastasIntentos, false, round);
        }
        else
        {
            //game over
            GameObject.Find("Controller").GetComponent<roundEnd>().Set(lpoints, points, record, 100*canastasHechas / canastasIntentos, true, round);
        }

        GameObject.Find("Controller").GetComponent<roundEnd>().enabled = true;
		
	}

    public void FinalPartida()
    {
        //agotadas las vidas extra termina juego o cancelada partida, poner records
        if (points > record)
        {
            new_record = true;
            record = points;
            Save();
        }

        //if (balls <= 0 && new_record)
        if (new_record)
        {
            //record
            new_record = false;
            gameObject.GetComponent<leaderboard>().SendRecord();
        }

    }

    public void NextRound()
    {
		round++;
		Save();
		Load();
        int level = Level(round);
        GameObject.Find("AutoFade").GetComponent<AutoFade>().LoadLevel(level, 0.5f,0.5f); //level 2 play
    }
	
    private int Level(int round)
    {
        //Cambia de nivel, pone los escenarios
        return (round % MaxEscenarios) + 2;
    }

	public void Restart()
    {
        primerVidasExtra = true; //al empezar tienes la posibilidad de ganar 3 vidas extra

        Save();
		Reset();
        ResetObjetivos();
		Load();

        int level = Level(round);
        GameObject.Find("AutoFade").GetComponent<AutoFade>().LoadLevel(level, 0.5f,0.5f); //level 2 play
    }
	
	public void Sound(int id)
    {
		if(sfx)
			switch(id)
            {
				case 1:
                    GetComponent<AudioSource>().PlayOneShot(net, sfxV*4/3);
                    break;
				case 2:
                    if (Random.Range(0,2) > 1)
                        GetComponent<AudioSource>().PlayOneShot(rim_hit, sfxV);
					else
                        GetComponent<AudioSource>().PlayOneShot(rim_hit2, sfxV);
                    break;
				case 3:
                    if (Random.Range(0,2) > 1)
                        GetComponent<AudioSource>().PlayOneShot(ground, sfxV);
					else
                        GetComponent<AudioSource>().PlayOneShot(ground2, sfxV);
                    break;
				case 4:
                    if (Random.Range(0,2) > 1)
                        GetComponent<AudioSource>().PlayOneShot(ground, sfxV);
					else
                        GetComponent<AudioSource>().PlayOneShot(ground2, sfxV);
                    break;
				case 5:
                    GetComponent<AudioSource>().PlayOneShot(ground, sfxV);
                    break;
	        }
	}

	void Load()
    {
		record = PlayerPrefs.GetInt("totalRecord", 0);
		
		sfx = PlayerPrefs.GetInt("sfx", 1) == 1 ? true : false; 
		//music = PlayerPrefs.GetInt("music", 1) == 1 ? true : false; 
		username = PlayerPrefs.GetString("name", "player");
	}
	
	void Save()
    {
		PlayerPrefs.SetInt("totalRecord", record);
		
		if(sfx) PlayerPrefs.SetInt("sfx", 1);
		else PlayerPrefs.SetInt("sfx", 0);
		
		PlayerPrefs.SetString("name", username);
		
		PlayerPrefs.Save();
	}
	
	public void SentScore()
    {
		PlayerPrefs.SetInt("SentRecord", record);
		PlayerPrefs.Save();	
	}
	
	public void ResetSave()
    {
        //borra los datos, cuidado, poner alerta y pedir confirmacion!!!
		PlayerPrefs.SetInt("totalRecord", 0);
		PlayerPrefs.SetString("name", "player");
		PlayerPrefs.Save();
		
		Load();
	
	}

    void Reset()
    {
        //nueva partida
        new_record = false;
        points = round = combo = 0;
        balls = start_balls = 10;
        max_balls = 99; //cuando tenga retos y se pierdan vidas como churros poner maximo 99 o 20 vidas para compensar !!!
    }

    void ResetObjetivos()
    { 
        GameObject.Find("objetivoScript").GetComponent<objetivoScript>().Restart(); //limpia los objetivos
    }
	
}
