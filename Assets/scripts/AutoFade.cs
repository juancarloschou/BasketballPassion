// AutoFade.cs
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AutoFade : MonoBehaviour
{
    public Texture2D fadeOutTexture;    // the texture that will overlay the screen. This can be a black image or a loading graphic
    //public float fadeSpeed = 0.5f;      // the fading speed

    private int drawDepth = -1000;      // the texture's order in the draw hierarchy: a low number means it renders on top
    private float alpha = 1.0f;         // the texture's alpha value between 0 and 1
    //private int fadeDir = -1;           // the direction to fade: in = -1 or out = 1

    string m_LevelName = "";
    private int m_LevelIndex = 0;
    private bool m_Fading = false;      //si esta haciendo el fade in/out
                                        //private static AutoFade m_Instance = null;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        //m_Instance = this;

        //m_Material = new Material("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
        //m_Material = new Material(Shader.Find("Diffuse"));
    }

    void OnGUI()
    {
        if (m_Fading)
        {
            // fade out/in the alpha value using a direction, a speed and Time.deltaTime to convert the operation to seconds
            //alpha += fadeDir * fadeSpeed * Time.deltaTime;
            // force (clamp) the number to be between 0 and 1 because GUI.color uses Alpha values between 0 and 1
            //alpha = Mathf.Clamp01(alpha);

            // set color of our GUI (in this case our texture). All color values remain the same & the Alpha is set to the alpha variable
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
            GUI.depth = drawDepth;                                                              // make the black texture render on top (drawn last)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);       // draw the texture to fit the entire screen area
        }
    }

    // sets fadeDir to the direction parameter making the scene fade in if -1 and out if 1
    /*
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    // OnLevelWasLoaded is called when a level is loaded. It takes loaded level index (int) as a parameter so you can limit the fade in to certain scenes.
    void OnLevelWasLoaded()
    {
        // alpha = 1;		// use this if the alpha is not set to 1 by default
        BeginFade(-1);      // call the fade in function
    }
    */

    /*
    private static AutoFade Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = (new GameObject("AutoFade")).AddComponent<AutoFade>();
			}
			return m_Instance;
		}
	}

	public static bool Fading
	{
		get
        {
            return Instance.m_Fading;
        }
	}
    */

    /*
	private void DrawQuad(Color aColor, float aAlpha)
	{
		aColor.a = aAlpha;
		m_Material.SetPass(0);
		GL.Color(aColor);
		GL.PushMatrix();
		GL.LoadOrtho();
		GL.Begin(GL.QUADS);
		GL.Vertex3(0, 0, -1);
		GL.Vertex3(0, 1, -1);
		GL.Vertex3(1, 1, -1);
		GL.Vertex3(1, 0, -1);
		GL.End();
		GL.PopMatrix();
	}
	*/

    private IEnumerator Fade(float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		float t = 0.0f; //alpha
		while (t<1.0f)
		{
			yield return new WaitForEndOfFrame();
			t = Mathf.Clamp01(t + Time.deltaTime / aFadeOutTime);
            alpha = t;
            //DrawQuad(aColor,t);
		}

        //abre el nivel indicado en la funcion LoadLevel, por nombre o index
        if (m_LevelName != "")
        {
            //Application.LoadLevel(m_LevelName);
            SceneManager.LoadScene(m_LevelName);
        }
        else
        {
            //Application.LoadLevel(m_LevelIndex);
            SceneManager.LoadScene(m_LevelIndex);
        }

        while (t>0.0f)
		{
			yield return new WaitForEndOfFrame();
			t = Mathf.Clamp01(t - Time.deltaTime / aFadeInTime);
            alpha = t;
			//DrawQuad(aColor,t);
		}
		m_Fading = false;
	}

	private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		m_Fading = true;
		StartCoroutine(Fade(aFadeOutTime, aFadeInTime, aColor));
	}
	
	public void LoadLevel(string aLevelName,float aFadeOutTime, float aFadeInTime) //, Color aColor)
	{
		if (m_Fading)
            return;
        m_LevelName = aLevelName;
        Color aColor = Color.black;
        StartFade(aFadeOutTime, aFadeInTime, aColor);
	}

	public void LoadLevel(int aLevelIndex,float aFadeOutTime, float aFadeInTime) //, Color aColor)
	{
		if (m_Fading)
            return;
        m_LevelName = "";
        m_LevelIndex = aLevelIndex;
        Color aColor = Color.black;
        StartFade(aFadeOutTime, aFadeInTime, aColor);
	}
    

}