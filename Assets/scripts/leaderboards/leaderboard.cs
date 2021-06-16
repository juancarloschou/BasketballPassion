using System;
using UnityEngine;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp;  
using com.shephertz.app42.paas.sdk.csharp.game;  

public class leaderboard : MonoBehaviour
{
    static gameScript gameScript;

    public static string API_KEY = "06166c21b0e2f3f9c4e0988502f8f1393f6d371aceaf340b73a36246e0175dd5";
    public static string SECRET_KEY = "1d0e6f032bd2214552ca94f4dffe0012fad436d270dfb8bdfa25d479240b1be9";
    string gameName = "BasketballPassion";

    ServiceAPI api;
	ScoreBoardService scoreBoardService;
	
	void Start ()
    {
        gameScript = GameObject.Find("gameScript").GetComponent<gameScript>();

        api = new ServiceAPI(API_KEY, SECRET_KEY);  
		scoreBoardService = api.BuildScoreBoardService();    
	}
	
	public void SendRecord()
    {
		string username = gameScript.username;
		double record = gameScript.record; 
		
		if(record > 2000)	
		    scoreBoardService.SaveUserScore(gameName, username, record, new UnityCallBack());  
	}
	
	public class UnityCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)
        {
			gameScript.SentScore();
		
		} 		
		public void OnException(Exception e)
        {
            App42Log.Console("Exception : " + e);
        }  
	}
	
	public void GetLeaderBoard()
    {	
		int max = 10;
		if(PlayerPrefs.GetInt("SentRecord", 0) < gameScript.record)
        {
			SendRecord();
		}
	
		//App42Log.SetDebug(true);        //Print output in your editor console  
		scoreBoardService.GetTopNRankings(gameName, max, new LeaderBoardCallBack());  
	}
	
	public class LeaderBoardCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			string[] score_names = new string[10];
			double[] score_points = new double[10];
			
			Game game = (Game) response;  
			for(int i = 0;i<game.GetScoreList().Count;i++)  
			{  
				score_names[i] = game.GetScoreList()[i].GetUserName();  
				score_points[i] = game.GetScoreList()[i].GetValue();
			}    
			GameObject.Find("gameScript").GetComponent<gameScript>().score_names = score_names;
			GameObject.Find("gameScript").GetComponent<gameScript>().score_points = score_points;
		}  
		
		public void OnException(Exception e)
        {
            App42Log.Console("Exception : " + e);
        }  
	}  
}
