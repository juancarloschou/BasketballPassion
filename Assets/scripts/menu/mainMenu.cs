using UnityEngine;
using System.Collections;

public class mainMenu : MonoBehaviour
{
    static gameScript gameScript;

    float width = 168f, height = 48f, Yoffset = 38f;
    float xcenter = 316f, ycenter = 216f;
    //float cooldown = 3f; //publicidad €€€

    string username, tempname = "Insert name";
	bool options = false, saves = false, name_select = false, top10 = false;
	
	string[] score_names = new string[10];	
	double[] score_points = new double[10];	
	public GUISkin newSkin;
	
	void Start()
    {
        gameScript = GameObject.Find("gameScript").GetComponent<gameScript>();
        //xcenter = gameScript.ScreenX / 2;
        //ycenter = gameScript.ScreenY / 2;


        //publicidad inicial a pantalla completa €€€

        //https://support.google.com/admob/answer/6201362?hl=es&ref_topic=2745287
        //abrir aplicacion, pantalla cargando (aju soft), anuncio pantalla completa, menu ppal

        //Intersticial antes de menu principal
        GameObject.Find("AdsManager").GetComponent<AdsManager>().VerAnuncioImagenMenuPrincipal();


        username = GameObject.Find("gameScript").GetComponent<gameScript>().username;
		if(username == "player")
        {
			name_select = true;
		}
		GameObject.Find("gameScript").GetComponent<leaderboard>().GetLeaderBoard();
	}

	void Update()
    {
        //publicidad inicial a pantalla completa €€€
        
		//if(cooldown > 0)
        //  cooldown -= Time.deltaTime;
		//else if(!GameObject.Find("ChartBoostManager").GetComponent<CBads>().shown)			
		//	GameObject.Find("ChartBoostManager").GetComponent<CBads>().ShowAd();
		//else
		//	cooldown = 3f;
    }

    // Use this for initialization
    void OnGUI()
    {
        Pantalla.AutoResize(gameScript.ScreenX, gameScript.ScreenY);
        //AutoResize(800, 470);
		
		GUI.skin = newSkin;
		GUI.skin.button.fontSize = 24;
		
        //diferentes pantallas, StartMenu, Options, Saves y NewName
		if(name_select)
			NewName();
		else if(saves)
			Saves();
		else if(options)
			Options();
		else if(top10)
			Top10();
		else
			StartMenu();
	}

	void NewName()
    {
        GUI.skin.textField.fontSize = 28;
		tempname = GUI.TextField(new Rect(xcenter - 41, ycenter, 250, 50), tempname, 20); //maxlength 15
		
		if(GUI.Button(new Rect(xcenter, ycenter+1*(height/2+Yoffset), width, height), "Done"))
        {
			if(tempname != "Insert name" && tempname != "player")
            {
				PlayerPrefs.SetString("name", tempname);
				PlayerPrefs.Save();
				username = GameObject.Find("gameScript").GetComponent<gameScript>().username = tempname;
				name_select = false;
			}			
		
		}
	}	
	
	void StartMenu()
    {
        /*
		if(GUI.Button(new Rect(xcenter-width/1.5f, ycenter, width/2, height), "Multi")){
			GameObject.Find("gameScript").GetComponent<gameScript>().MultiRestart();
			AutoFade.LoadLevel(2,0.5f,0.5f,Color.black);
		}
        */
		
		if(GUI.Button(new Rect(xcenter+width*1.2f, ycenter, width/2, height), "Top10"))
        {
			top10 = true;
		}
		
		if(GUI.Button(new Rect(xcenter, ycenter, width, height), "Play"))
        {
			GameObject.Find("gameScript").GetComponent<gameScript>().Restart();
			//AutoFade.LoadLevel(2, 0.5f,0.5f); //level 2 play, el Restart ya lo hace
        }
		
		if(GUI.Button(new Rect(xcenter, ycenter+1*(height/2+Yoffset), 168, height), "Options"))
        {
			options = true;
		}	
		
		if(GUI.Button(new Rect(xcenter, ycenter+2*(height/2+Yoffset), width, height), "Exit") || Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
		
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.skin.label.fontSize = 28;
        //GUI.Label(new Rect(10, 430, 300, 40), "Game by Aju Soft");
        GUI.Label(new Rect(20, gameScript.ScreenY - 50, 300, 40), "Aju Soft");
    }

    void Options()
    {
		string sfx = GameObject.Find("gameScript").GetComponent<gameScript>().sfx ? "On" : "Off";
        //string power = GameObject.Find("gameScript").GetComponent<gameScript>().power ? "x1.5" : "x1";

        //if (GUI.Button(new Rect(xcenter - width / 1.5f, ycenter, width / 2, height), "Back"))
        if (GUI.Button(new Rect(xcenter+width*1.2f, ycenter, width/2, height), "Back"))
        {
			if(GameObject.Find("gameScript").GetComponent<gameScript>().sfx) PlayerPrefs.SetInt("sfx", 1);
			else PlayerPrefs.SetInt("sfx", 0);
			
			/*
			if(GameObject.Find("gameScript").GetComponent<gameScript>().power) PlayerPrefs.SetInt("power", 1);
			else  PlayerPrefs.SetInt("power", 0);
            */
			
			PlayerPrefs.Save();
			
			options = false;
		}

        if (GUI.Button(new Rect(xcenter, ycenter, width, height), "Tutorial"))
        {
            GameObject.Find("gameScript").GetComponent<gameScript>().ponerTutorial = true;
            GameObject.Find("AutoFade").GetComponent<AutoFade>().LoadLevel(2, 0.5f, 0.5f); //level 2 play
            //GameObject.Find("Controller").GetComponent<tutorialScript>().PonerTutorial(true);
        }

        if (GUI.Button(new Rect(xcenter, ycenter + 1 * (height / 2 + Yoffset), width, height), "Sound: "+sfx))
        {
			if(GameObject.Find("gameScript").GetComponent<gameScript>().sfx)
				GameObject.Find("gameScript").GetComponent<gameScript>().sfx = false;
			else
				GameObject.Find("gameScript").GetComponent<gameScript>().sfx = true;
		}

        /*
		if(GUI.Button(new Rect(xcenter, ycenter+1*(height/2+Yoffset), width, height), "Power: "+power)){
			if(GameObject.Find("gameScript").GetComponent<gameScript>().power)
				GameObject.Find("gameScript").GetComponent<gameScript>().power = false;
			else
				GameObject.Find("gameScript").GetComponent<gameScript>().power = true;		
		}
        */

        if (GUI.Button(new Rect(xcenter, ycenter+2*(height/2+Yoffset), width, height), "Manage saves"))
		//if(GUI.Button(new Rect(xcenter, ycenter+(height/2+Yoffset), width, height), "Manage saves"))
        {
            saves = true;
        }

        //if (GUI.Button(new Rect(xcenter, ycenter + 3 * (height / 2 + Yoffset), width, height), "TEST"))
        //{
        //    this.enabled = false;
        //    GameObject.Find("demoAds").GetComponent<demoAds>().PonerDemo();
        //}

    }

    void Saves()
    {		
		float lwidth = 200f;		
		GUI.skin.button.padding.left = 10;
		//int achv = GameObject.Find("gameScript").GetComponent<aManager>().GetDone();
		int record = GameObject.Find("gameScript").GetComponent<gameScript>().record;

        //if (GUI.Button(new Rect(xcenter-width/1.5f, ycenter, width/2, height), "Back"))
        if (GUI.Button(new Rect(xcenter + width * 1.2f + 20, ycenter, width / 2, height), "Back"))
        {
			saves = false;
		}
		
		GUI.skin.button.alignment = TextAnchor.MiddleLeft;
		//GUI.Button(new Rect(xcenter, ycenter, lwidth, height), "Achiments: "+achv+"/21");

		//GUI.Button(new Rect(xcenter, ycenter+1*(height/2+Yoffset), lwidth, height), "Record: "+record);
        GUI.Button(new Rect(xcenter, ycenter, lwidth, height), "Record: " + record);

        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
        //if (GUI.Button(new Rect(xcenter, ycenter + 2 * (height / 2 + Yoffset), lwidth, height), "Reset"))
        if (GUI.Button(new Rect(xcenter, ycenter+1*(height/2+Yoffset), lwidth, height), "Reset"))
        {
			GameObject.Find("gameScript").GetComponent<gameScript>().ResetSave();
			name_select = true;
		}
		
	}
	
	void Top10()
    {	
		if(score_points[0] == 0)
        {
            score_names = GameObject.Find("gameScript").GetComponent<gameScript>().score_names;
			score_points = GameObject.Find("gameScript").GetComponent<gameScript>().score_points;
		}
		
		GUI.skin.button.padding.left = 10;
		
		//if(GUI.Button(new Rect(150, ycenter, width/2, height), "Back"))
        if (GUI.Button(new Rect(xcenter - 120+420+40, ycenter - 10, width / 2, height), "Back"))
            {
                top10 = false;
		}
		
		GUI.skin.box.alignment = TextAnchor.MiddleCenter;
		//GUI.Box(new Rect(xcenter-80, ycenter-15, 400, 270), "");
        GUI.Box(new Rect(xcenter - 120, ycenter - 40, 420, 270), "");
        GUI.skin.label.fontSize = 24;

		//GUI.Label(new Rect(320, ycenter-15, 150, 40), "Name");
		//GUI.Label(new Rect(460, ycenter-15, 200, 40), "Points");
        GUI.Label(new Rect(275, ycenter - 30, 150, 40), "Name");
        GUI.Label(new Rect(470, ycenter - 30, 200, 40), "Points");

        GUI.skin.label.fontSize = 18;
		
		for(int i=1; i<=10; i++)
        {
			if (score_points[i-1] == 0)
                break;
			GUI.Label(new Rect(230, ycenter+22*i, 60, 30), "#"+i);
			GUI.Label(new Rect(275, ycenter+22*i, 170, 30), score_names[i-1]);
			GUI.Label(new Rect(470, ycenter+22*i, 200, 30), ""+score_points[i-1]);
		}
		
	}

}
