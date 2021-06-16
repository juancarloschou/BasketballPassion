using System;
using UnityEngine;

public class Objetivo
{
    //mensajes
    public int Coach = 0; //1 presentacion, 0 mision
    public int NumMensajes; //numero de mensajes que hay (iniciales)
    public Mensaje[] Mensajes = new Mensaje[5]; //maximo 5 mensajes (iniciales)

    public int NumMensajesFinalOK; //numero de mensajes que hay (finales)
    public Mensaje[] MensajesFinalOK = new Mensaje[5]; //maximo 5 mensajes (finales)

    public int NumMensajesFinalKO; //numero de mensajes que hay (finales)
    public Mensaje[] MensajesFinalKO = new Mensaje[5]; //maximo 5 mensajes (finales)

    public int Bolas = 10; //bolas de la ronda

    //objetivos
    public int Objetivos = 0; //numero de objetivos diferentes en la ronda
    public int Cleans; //clean shots que hay que hacer
    public int Uphighs;
    public int Banks;
    public int Longs;
    public int Tricks;

    public int Fallos; //numero de tiros fallados permitido
    public int Tiempo; //tiempo para hacer la ronda

    public int Obstaculos;

    //hecho
    public int CleansDone; //clean shots hechos
    public int UphighsDone;
    public int BanksDone;
    public int LongsDone;
    public int TricksDone;

    public int FallosDone; //numero de tiros fallados
    public int TiempoDone; //tiempo ha pasado

    //vidas
    public bool VidasSiGanasSobreObjetivo; //si hay que sumar las canastas de mas sobre el objetivo
    public int VidasSiGanas; //cantidad fija, puede ser cero, si el anterior es true se suman
    public bool VidasSiPierdesFaltanObjetivo; //si hay que restar tantas canastas como te falten para objetivo
    public int VidasSiPierdes; //cantidad fija, puede ser cero, si el anterior es true se suman (restan)

    public int Vidas; //vidas ganadas o perdidas en la ronda


    public Objetivo()
    {
        for(int iMensaje = 0; iMensaje < 5; iMensaje++)
        {
            Mensajes[iMensaje] = new Mensaje();
            MensajesFinalOK[iMensaje] = new Mensaje();
            MensajesFinalKO[iMensaje] = new Mensaje();
        }
    }

    public bool Conseguido(ref int vidas)
    {
        //en fin de ronda se comprueba si ha conseguido objetivo
        bool bConseguido = true;
        vidas = 0;
        int targetFaltan = 0; //vidas que faltan para el objetivo
        int targetSobran = 0; //vidas que haces por encima objetivo

        if (Cleans > 0)
        {
            if (Cleans > CleansDone)
            {
                bConseguido = false;
                targetFaltan += Cleans - CleansDone;
            }
            else
                targetSobran += CleansDone - Cleans;
        }

        if (Uphighs > 0)
        {
            if (Uphighs > UphighsDone)
            {
                bConseguido = false;
                targetFaltan += Uphighs - UphighsDone;
            }
            else
                targetSobran += UphighsDone - Uphighs;
        }

        if (Banks > 0)
        {
            if (Banks > BanksDone)
            {
                bConseguido = false;
                targetFaltan += Banks - BanksDone;
            }
            else
                targetSobran += BanksDone - Banks;
        }

        if (Longs > 0)
        {
            if (Longs > LongsDone)
            {
                bConseguido = false;
                targetFaltan += Longs - LongsDone;
            }
            else
                targetSobran += LongsDone - Longs;
        }

        if (Tricks > 0)
        {
            if (Tricks > TricksDone)
            {
                bConseguido = false;
                targetFaltan += Tricks - TricksDone;
            }
            else
                targetSobran += TricksDone - Tricks;
        }

        if(Fallos > 0)
        {
            //va a la inversa
            if(Fallos < FallosDone)
            {
                bConseguido = false;
                targetFaltan += FallosDone - Fallos;
            }
            else
                targetSobran += Fallos - FallosDone;
        }

        if (bConseguido)
        {
            vidas = VidasSiGanas;
            if (targetSobran - targetFaltan > 0)
                if (VidasSiGanasSobreObjetivo)
                    vidas += targetSobran - targetFaltan;
        }
        else
        {
            vidas = VidasSiPierdes;
            if (targetFaltan - targetSobran > 0)
                if (VidasSiPierdesFaltanObjetivo)
                    vidas += targetFaltan - targetSobran;
            vidas = -vidas;
        }

        Vidas = vidas;
        return bConseguido;
    }

    public void Score(bool clean_shot, bool bank_shot, bool long_shot, bool uphigh_shot, bool trick_shot)
    {
        if (clean_shot)
        {
            CleansDone++;
        }
        if (bank_shot)
        {
            BanksDone++;
        }
        if (long_shot)
        {
            LongsDone++;
        }
        if (uphigh_shot)
        {
            UphighsDone++;
        }
        if (trick_shot)
        {
            TricksDone++;
        }
    }

    public void Falla()
    {
        FallosDone++;
    }

    public string Mensaje(string msg)
    {
        msg = msg.Replace("#VIDAS#", Vidas.ToString());
        msg = msg.Replace("#CLEANS#", CleansDone.ToString());
        msg = msg.Replace("#UPHIGHS#", UphighsDone.ToString());
        msg = msg.Replace("#BANKS#", BanksDone.ToString());
        msg = msg.Replace("#LONGS#", LongsDone.ToString());
        msg = msg.Replace("#TRICKS#", TricksDone.ToString());
        msg = msg.Replace("#FALLOS#", FallosDone.ToString());
        return msg;
    }

}

public class Mensaje
{
    public string Texto1;
    public string Texto2;
    public string Texto3;

    public Mensaje()
    {
        Texto1 = "";
        Texto2 = "";
        Texto3 = "";
    }
}
