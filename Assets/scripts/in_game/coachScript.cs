using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class coachScript : MonoBehaviour
{
    static gameScript gameScript;
    static scoreScript scoreScript;
    static showScore showScore;
    static objetivoScript objetivoScript;
    private GameObject coach_bueno, coach_malo, coach_mision, coach_presentacion;
    public Texture2D OK;
    private Graficos graficos = new Graficos();

    public bool esInicio; //inicio pone el objetivo antes ronda, fin muestra los resultados tras la ronda
    public int round;
    private string msg, msg2, msg3;
    private int mensaje; //que mensaje estamos mostrando, el primero, segundo, etc
    private bool conseguido; //fin de ronda, si ha conseguido el objetivo
    private int vidas; //vidas ganadas o perdidas final ronda

    void Start()
    {
        gameScript = GameObject.Find("gameScript").GetComponent<gameScript>();
        scoreScript = gameObject.GetComponent<scoreScript>();
        showScore = gameObject.GetComponent<showScore>();
        objetivoScript = GameObject.Find("objetivoScript").GetComponent<objetivoScript>();

        coach_presentacion = GameObject.Find("coach_presentacion");
        coach_mision = GameObject.Find("coach_mision");
        coach_bueno = GameObject.Find("coach_bueno");
        coach_malo = GameObject.Find("coach_malo");

        esInicio = true;
        round = gameScript.round;
        mensaje = 0;

        if (objetivoScript.Objetivos[round].NumMensajes > 0)
        {
            if (objetivoScript.Objetivos[round].Coach == 0)
            {
                coach_mision.transform.position = new Vector3(7.75f, 0, 0);
                coach_mision.GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (objetivoScript.Objetivos[round].Coach == 1)
            {
                coach_presentacion.transform.position = new Vector3(7.5f, 0, 0);
                coach_presentacion.GetComponent<SpriteRenderer>().enabled = true;
            }

            msg = objetivoScript.Objetivos[round].Mensajes[mensaje].Texto1;
            msg2 = objetivoScript.Objetivos[round].Mensajes[mensaje].Texto2;
            msg3 = objetivoScript.Objetivos[round].Mensajes[mensaje].Texto3;
        }
        else
        {
            //no hay coach, volver al juego
            this.enabled = false;
            scoreScript.StartScript();
        }
    }

    public void FinRonda()
    {
        this.enabled = true;
        esInicio = false;
        mensaje = 0;

        vidas = 0;
        conseguido = objetivoScript.Objetivos[round].Conseguido(ref vidas);

        if(conseguido) //OK
        {
            if(objetivoScript.Objetivos[round].NumMensajesFinalOK > 0)
            {
                coach_bueno.transform.position = new Vector3(7.5f, 0, 0);
                coach_bueno.GetComponent<SpriteRenderer>().enabled = true;

                msg = objetivoScript.Objetivos[round].Mensaje(objetivoScript.Objetivos[round].MensajesFinalOK[mensaje].Texto1);
                msg2 = objetivoScript.Objetivos[round].Mensaje(objetivoScript.Objetivos[round].MensajesFinalOK[mensaje].Texto2);
                msg3 = objetivoScript.Objetivos[round].Mensaje(objetivoScript.Objetivos[round].MensajesFinalOK[mensaje].Texto3);

                //ganas vidas
                if (vidas > 0)
                {
                    scoreScript.CambiarVidas(vidas);
                }

            }
            else
            {
                //volver al juego (round end)
                this.enabled = false;
                scoreScript.FinRonda();
            }
        }
        else //KO
        {
            if (objetivoScript.Objetivos[round].NumMensajesFinalKO > 0)
            {
                coach_malo.transform.position = new Vector3(7.5f, 0, 0);
                coach_malo.GetComponent<SpriteRenderer>().enabled = true;

                msg = objetivoScript.Objetivos[round].Mensaje(objetivoScript.Objetivos[round].MensajesFinalKO[mensaje].Texto1);
                msg2 = objetivoScript.Objetivos[round].Mensaje(objetivoScript.Objetivos[round].MensajesFinalKO[mensaje].Texto2);
                msg3 = objetivoScript.Objetivos[round].Mensaje(objetivoScript.Objetivos[round].MensajesFinalKO[mensaje].Texto3);

                //pierdes vidas
                if (vidas < 0)
                {
                    scoreScript.CambiarVidas(vidas);
                }

            }
            else
            {
                //volver al juego (round end)
                this.enabled = false;
                scoreScript.FinRonda();
            }
        }
    }

    /*
    void Update()
    {
		//if(presentacion_count > 0)
        //  presentacion_count -= Time.deltaTime;
    }
    */

    void Continuar()
    {
        //al pulsar en boton OK

        mensaje++; //pasamos al siguiente msg (si hay)
        if (esInicio)
        {
            if (mensaje >= objetivoScript.Objetivos[round].NumMensajes)
            {
                //volver al juego
                if (objetivoScript.Objetivos[round].Coach == 1)
                    coach_presentacion.GetComponent<SpriteRenderer>().enabled = false;
                else
                    coach_mision.GetComponent<SpriteRenderer>().enabled = false;
                this.enabled = false;
                scoreScript.StartScript();
            }
            else
            {
                //siguiente mensaje
                msg = objetivoScript.Objetivos[round].Mensajes[mensaje].Texto1;
                msg2 = objetivoScript.Objetivos[round].Mensajes[mensaje].Texto2;
                msg3 = objetivoScript.Objetivos[round].Mensajes[mensaje].Texto3;
            }
        }
        else
        {
            //final
            if (conseguido) //OK
            {
                if (mensaje >= objetivoScript.Objetivos[round].NumMensajesFinalOK)
                {
                    //volver al juego (round end)
                    coach_bueno.GetComponent<SpriteRenderer>().enabled = false;
                    this.enabled = false;
                    scoreScript.FinRonda();
                }
                else
                {
                    //siguiente mensaje
                    msg = objetivoScript.Objetivos[round].Mensaje(objetivoScript.Objetivos[round].MensajesFinalOK[mensaje].Texto1);
                    msg2 = objetivoScript.Objetivos[round].Mensaje(objetivoScript.Objetivos[round].MensajesFinalOK[mensaje].Texto2);
                    msg3 = objetivoScript.Objetivos[round].Mensaje(objetivoScript.Objetivos[round].MensajesFinalOK[mensaje].Texto3);
                }
            }
            else //KO
            {
                if (mensaje >= objetivoScript.Objetivos[round].NumMensajesFinalKO)
                {
                    //volver al juego (round end)
                    coach_malo.GetComponent<SpriteRenderer>().enabled = false;
                    this.enabled = false;
                    scoreScript.FinRonda();
                }
                else
                {
                    //siguiente mensaje
                    msg = objetivoScript.Objetivos[round].Mensaje(objetivoScript.Objetivos[round].MensajesFinalKO[mensaje].Texto1);
                    msg2 = objetivoScript.Objetivos[round].Mensaje(objetivoScript.Objetivos[round].MensajesFinalKO[mensaje].Texto2);
                    msg3 = objetivoScript.Objetivos[round].Mensaje(objetivoScript.Objetivos[round].MensajesFinalKO[mensaje].Texto3);
                }
            }
        }
    }

    void OnGUI()
    {
        Pantalla.AutoResize(gameScript.ScreenX, gameScript.ScreenY);
        //AutoResize(800, 470);

        if ((msg != "") || (msg2 != "") || (msg3 != "")) //si hay algun mensaje
        {
            float centerx = gameScript.ScreenX / 2;
            float centery = gameScript.ScreenY / 2;
            graficos.InitStyles(Color.black); //recuadro de color solido
            GUI.Box(new Rect(centerx - 320, centery - 175, 450, 350), "", graficos.currentStyle);

            GUI.skin.label.fontSize = 24; //34
            GUI.color = Color.white;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;

            if ((msg != "") && (msg2 == "") && (msg3 == ""))
            {
                GUI.Label(new Rect(centerx - 300, centery - 150, 410, 240), msg);
            }

            if ((msg != "") && (msg2 != "") && (msg3 == ""))
            {
                GUI.Label(new Rect(centerx - 300, centery - 150, 410, 120), msg);

                GUI.Label(new Rect(centerx - 300, centery - 25, 410, 120), msg2);
            }

            if ((msg != "") && (msg2 != "") && (msg3 != ""))
            {
                GUI.Label(new Rect(centerx - 300, centery - 150, 410, 80), msg);

                GUI.Label(new Rect(centerx - 300, centery - 70, 410, 80), msg2);

                GUI.Label(new Rect(centerx - 300, centery + 10, 410, 80), msg3);
            }

            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            GUIContent btn = new GUIContent("OK", OK);
            if (GUI.Button(new Rect(centerx - 170, centery + 100, 150, 50), btn))
            {
                Continuar();
            }
        }
    }

}
