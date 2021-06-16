using UnityEngine;
using System.Collections;

public class active_buttons : MonoBehaviour
{

	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
			PauseMenu();

        //si pulso en boton pausa tambien !!!
	}

	void PauseMenu()
    {
        //pauseMenu pauseMenu = gameObject.GetComponent<pauseMenu>();
        pauseMenu pauseMenu = GameObject.Find("Controller").GetComponent<pauseMenu>();
        pauseMenu.PonerPausa();
	}
}
