using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class objetivoScript : MonoBehaviour
{
    public Objetivo[] Objetivos = new Objetivo[100]; //100 fases

    void Awake()
    {
        DontDestroyOnLoad(gameObject); //el Script se mantiene en todas las escenas
    }

    public void Start()
    {
        int iRonda, iMensaje;

        for (iRonda = 0; iRonda < 100; iRonda++)
        {
            Objetivos[iRonda] = new Objetivo();
        }

        iRonda = 0;
        Objetivos[iRonda].Coach = 1; //presentacion
        Objetivos[iRonda].NumMensajes = 2;
        iMensaje = 0;
        Objetivos[iRonda].Mensajes[iMensaje].Texto1 = "Hello #PLAYER#, I am Miyagi, the coach of the team. I am glad that you joined us.";
        Objetivos[iRonda].Mensajes[iMensaje].Texto2 = "I will train you until you are ready to be the star player of our team.";
        iMensaje = 1;
        Objetivos[iRonda].Mensajes[iMensaje].Texto1 = "Shoot 5 times this round. If you fail a shoot, will lose a life.";
        Objetivos[iRonda].Mensajes[iMensaje].Texto2 = "Try to do clean shots, bank shots, up high shots and other special shots to get the maximun score. Lets go!";
        Objetivos[iRonda].Bolas = 5;
        //no hay objetivos en ronda 1
        //no hay mensajes final

        iRonda = 1;
        Objetivos[iRonda].Coach = 0; //mision
        Objetivos[iRonda].NumMensajes = 1;
        iMensaje = 0;
        Objetivos[iRonda].Mensajes[iMensaje].Texto1 = "You need to train the clean shots. Throw the ball to the center of the basket and do 3 CLEAN shots.";
        Objetivos[iRonda].Mensajes[iMensaje].Texto2 = "If you hit the target will get 1 life. If you fail will lose as many lives as missing the target.";
        Objetivos[iRonda].Bolas = 5;
        //objetivos
        Objetivos[iRonda].Objetivos = 1;
        Objetivos[iRonda].Cleans = 3;
        Objetivos[iRonda].VidasSiGanasSobreObjetivo = false;
        Objetivos[iRonda].VidasSiGanas = 1;
        Objetivos[iRonda].VidasSiPierdesFaltanObjetivo = true;
        //mensajes final
        Objetivos[iRonda].NumMensajesFinalOK = 1;
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto1 = "You did it well. You made #CLEANS# clean shots.";
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto2 = "You get the target and have gained #VIDAS# lives.";
        Objetivos[iRonda].NumMensajesFinalKO = 1;
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto1 = "You did it bad. You only made #CLEANS# clean shots.";
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto2 = "You missed the target and have lost #VIDAS# lives.";

        iRonda = 2;
        Objetivos[iRonda].NumMensajes = 1;
        iMensaje = 0;
        Objetivos[iRonda].Mensajes[iMensaje].Texto1 = "Now train the up high shots. Throw the ball very high and do 3 UP HIGH shots.";
        Objetivos[iRonda].Mensajes[iMensaje].Texto2 = "If you hit the target will get 1 life. If you fail will lose as many lives as missing the target.";
        Objetivos[iRonda].Bolas = 5;
        //objetivos
        Objetivos[iRonda].Objetivos = 1;
        Objetivos[iRonda].Uphighs = 3;
        Objetivos[iRonda].VidasSiGanasSobreObjetivo = false;
        Objetivos[iRonda].VidasSiGanas = 1;
        Objetivos[iRonda].VidasSiPierdesFaltanObjetivo = true;
        //mensajes final
        Objetivos[iRonda].NumMensajesFinalOK = 1;
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto1 = "You did it well. You made #UPHIGHS# up high shots.";
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto2 = "You get the target and have gained #VIDAS# lives.";
        Objetivos[iRonda].NumMensajesFinalKO = 1;
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto1 = "You did it bad. You only made #UPHIGHS# up high shots.";
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto2 = "You missed the target and have lost #VIDAS# lives.";

        iRonda = 3;
        Objetivos[iRonda].NumMensajes = 1;
        iMensaje = 0;
        Objetivos[iRonda].Mensajes[iMensaje].Texto1 = "You have to master the bank shots. Throw the ball against the board and do 3 BANK shots.";
        Objetivos[iRonda].Mensajes[iMensaje].Texto2 = "If you hit the target will get 1 life. If you fail will lose as many lives as missing the target.";
        Objetivos[iRonda].Bolas = 5;
        //objetivos
        Objetivos[iRonda].Objetivos = 1;
        Objetivos[iRonda].Banks = 3;
        Objetivos[iRonda].VidasSiGanasSobreObjetivo = false;
        Objetivos[iRonda].VidasSiGanas = 1;
        Objetivos[iRonda].VidasSiPierdesFaltanObjetivo = true;
        //mensajes final
        Objetivos[iRonda].NumMensajesFinalOK = 1;
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto1 = "You did it well. You made #BANKS# bank shots.";
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto2 = "You get the target and have gained #VIDAS# lives.";
        Objetivos[iRonda].NumMensajesFinalKO = 1;
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto1 = "You did it bad. You only made #BANKS# bank shots.";
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto2 = "You missed the target and have lost #VIDAS# lives.";

        iRonda = 4; //ronda 5
        Objetivos[iRonda].NumMensajes = 1;
        iMensaje = 0;
        Objetivos[iRonda].Mensajes[iMensaje].Texto1 = "In this practice you have to avoid an oponent. You can only have 3 MISS shots.";
        Objetivos[iRonda].Mensajes[iMensaje].Texto2 = "If you hit the target will get 2 lifes. If you fail will lose as many lives as missing the target.";
        Objetivos[iRonda].Bolas = 7;
        //objetivos
        Objetivos[iRonda].Objetivos = 1;
        Objetivos[iRonda].Fallos = 3;
        Objetivos[iRonda].Obstaculos = 1; //definir posicion, tamaño, movimiento, velocidad
        Objetivos[iRonda].VidasSiGanasSobreObjetivo = false;
        Objetivos[iRonda].VidasSiGanas = 2;
        Objetivos[iRonda].VidasSiPierdesFaltanObjetivo = true;
        //mensajes final
        Objetivos[iRonda].NumMensajesFinalOK = 1;
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto1 = "You did it well. You only have #FALLOS# miss shots.";
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto2 = "You get the target and have gained #VIDAS# lives.";
        Objetivos[iRonda].NumMensajesFinalKO = 1;
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto1 = "You did it bad. You have #FALLOS# miss shots.";
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto2 = "You missed the target and have lost #VIDAS# lives.";

        iRonda = 5;
        Objetivos[iRonda].NumMensajes = 1;
        iMensaje = 0;
        Objetivos[iRonda].Mensajes[iMensaje].Texto1 = "Now train some types of shots. You have to do 4 UP HIGH shots and 4 CLEAN shots.";
        Objetivos[iRonda].Mensajes[iMensaje].Texto2 = "If win will get as many lifes as over the target. If fail will lose as many lives as missing the target.";
        Objetivos[iRonda].Bolas = 7;
        //objetivos
        Objetivos[iRonda].Objetivos = 2;
        Objetivos[iRonda].Uphighs = 4;
        Objetivos[iRonda].Cleans = 4;
        Objetivos[iRonda].VidasSiGanasSobreObjetivo = true;
        Objetivos[iRonda].VidasSiGanas = 0;
        Objetivos[iRonda].VidasSiPierdesFaltanObjetivo = true;
        //mensajes final
        Objetivos[iRonda].NumMensajesFinalOK = 1;
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto1 = "You did it well. You made #UPHIGHS# up high shots and #CLEANS# clean shots.";
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto2 = "You get the target and have gained #VIDAS# lives.";
        Objetivos[iRonda].NumMensajesFinalKO = 1;
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto1 = "You did it bad. You only made #UPHIGHS# up high shots and #CLEANS# clean shots.";
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto2 = "You missed the target and have lost #VIDAS# lives.";

        iRonda = 6;
        Objetivos[iRonda].NumMensajes = 1;
        iMensaje = 0;
        Objetivos[iRonda].Mensajes[iMensaje].Texto1 = "Now train some types of shots. You have to do 4 BANK shots and 4 CLEAN shots.";
        Objetivos[iRonda].Mensajes[iMensaje].Texto2 = "If win will get as many lifes as over the target. If fail will lose as many lives as missing the target.";
        Objetivos[iRonda].Bolas = 7;
        //objetivos
        Objetivos[iRonda].Objetivos = 2;
        Objetivos[iRonda].Banks = 4;
        Objetivos[iRonda].Cleans = 4;
        Objetivos[iRonda].VidasSiGanasSobreObjetivo = true;
        Objetivos[iRonda].VidasSiGanas = 0;
        Objetivos[iRonda].VidasSiPierdesFaltanObjetivo = true;
        //mensajes final
        Objetivos[iRonda].NumMensajesFinalOK = 1;
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto1 = "You did it well. You made #BANKS# bank shots and #CLEANS# clean shots.";
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto2 = "You get the target and have gained #VIDAS# lives.";
        Objetivos[iRonda].NumMensajesFinalKO = 1;
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto1 = "You did it bad. You only made #BANKS# bank shots and #CLEANS# clean shots.";
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto2 = "You missed the target and have lost #VIDAS# lives.";

        iRonda = 7;
        Objetivos[iRonda].NumMensajes = 1;
        iMensaje = 0;
        Objetivos[iRonda].Mensajes[iMensaje].Texto1 = "You need to train the long shots. Throw the ball with power and do 3 LONG shots.";
        Objetivos[iRonda].Mensajes[iMensaje].Texto2 = "If you hit the target will get 2 lifes. If you fail will lose as many lives as missing the target.";
        Objetivos[iRonda].Bolas = 7;
        //objetivos
        Objetivos[iRonda].Objetivos = 1;
        Objetivos[iRonda].Longs = 3;
        Objetivos[iRonda].VidasSiGanasSobreObjetivo = false;
        Objetivos[iRonda].VidasSiGanas = 2;
        Objetivos[iRonda].VidasSiPierdesFaltanObjetivo = true;
        //mensajes final
        Objetivos[iRonda].NumMensajesFinalOK = 1;
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto1 = "You did it well. You made #LONGS# long shots.";
        Objetivos[iRonda].MensajesFinalOK[iMensaje].Texto2 = "You get the target and have gained #VIDAS# lives.";
        Objetivos[iRonda].NumMensajesFinalKO = 1;
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto1 = "You did it bad. You only made #LONGS# long shots.";
        Objetivos[iRonda].MensajesFinalKO[iMensaje].Texto2 = "You missed the target and have lost #VIDAS# lives.";

    }

    public void Restart()
    {
        for(int iRonda = 0; iRonda < 100; iRonda++)
        {
            Objetivos[iRonda].CleansDone = 0;
            Objetivos[iRonda].UphighsDone = 0;
            Objetivos[iRonda].BanksDone = 0;
            Objetivos[iRonda].LongsDone = 0;
            Objetivos[iRonda].TricksDone = 0;
            Objetivos[iRonda].FallosDone = 0;
            Objetivos[iRonda].TiempoDone = 0;
        }
    }
}
