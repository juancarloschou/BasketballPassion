using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ballShot : MonoBehaviour
{
    public float life = 1.5f; //tiempo de destruccion del balon
    public float dead_sens = 25f; //rango de movimiento para tirar al soltar el touch
    public float power = 2.5f; //fuerza inicial

    private Vector3 startPos;
    public bool shot = false, aiming = false, hit_ground = false;

    public GameObject shadow;
    static scoreScript scoreScript;

    public bool clean = true, first_clean = false;
    public bool first_back_wall = true, back_wall = false;
    public bool first_trick_board = true, trick = false;
    public bool first_long = false;
    public bool uphigh = false;
    public bool score = false, canScore = true, canFail = true;
    public bool first_check = false, second_check = false, tercer_check = false;
    public bool first_hit_ground = true;

    public int ball; //nº de balon de 1 a 10 en la ronda

    public bool permitirTiro = true;

    // Use this for initialization
    void Start()
    {
        shadow = (GameObject)Instantiate(shadow);
        float sombraY = -6.5f; //altura suelo en la fase 1 (se puede sacar del elemento floor)
        shadow.transform.position = new Vector3(gameObject.transform.position.x, sombraY, 0);

        scoreScript = GameObject.Find("Controller").GetComponent<scoreScript>();

        transform.GetComponent<Rigidbody2D>().isKinematic = true; //flota, no esta afectada por gravedad
        transform.GetComponent<Collider2D>().enabled = false;
        startPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //posiciona shadow
        shadow.transform.position = new Vector3(gameObject.transform.position.x, shadow.transform.position.y, 0);

        //gira la pelota
        transform.Rotate(0, 0, -GetComponent<Rigidbody2D>().velocity.x);

        //comprueba up high shot
        if (!uphigh && (gameObject.transform.position.y >= 6.75f))
            uphigh = true;

        if (permitirTiro)
            Aim(); //lanzamiento mientras haces touch

        if (hit_ground)
        {
            if (life <= 0)
            {
                Destroy(gameObject);
                Destroy(shadow);
                scoreScript.DestroyedBall(gameObject);
            }
            else if (life > 0)
            {
                /*
                //aviso a scoreScript para que mire si has fallado el tiro
                if (first_hit_ground)
                {
                    first_hit_ground = false;
                    scoreScript.BalonLanzadoSuelo(score);
                }
                */

                Color startColor;

                life -= Time.deltaTime;
                //alpha del balon, empieza en 2 sg, cuando baja de 1 a 0 se hace transparente
                startColor = GetComponent<Renderer>().material.GetColor("_Color");
                GetComponent<Renderer>().material.SetColor("_Color", new Color(startColor.r, startColor.g, startColor.b, life));

                //la sombra tenia un error, al cambiar alpha no se veia
                //startColor = shadow.GetComponent<Renderer>().material.GetColor("_Color");
                //shadow.GetComponent<Renderer>().material.SetColor("_Color", new Color(startColor.r, startColor.g, startColor.b, life));
                startColor = shadow.GetComponent<SpriteRenderer>().color;
                startColor.a = life;
                shadow.GetComponent<SpriteRenderer>().color = startColor;

            }
        }
    }


    //tiro a canasta
    void Aim()
    {
        //si estamos disparando o estamos en pausa
        if (shot || GameObject.Find("Controller").GetComponent<pauseMenu>().enabled)
        {
            aiming = false;
            return;
        }

        if (Input.GetAxis("Fire1") == 1)
        {
            if (!aiming && !inPressZone(Input.mousePosition))
            {
                aiming = true;
                startPos = Input.mousePosition;
                scoreScript.CalculatePath();
                scoreScript.ShowPath();
            }
            else
            {
                scoreScript.CalculatePath();
            }
        }
        else if (aiming && !shot)
        {
            if (inDeadZone(Input.mousePosition) || inReleaseZone(Input.mousePosition))
            {
                aiming = false;
                scoreScript.HidePath();
                return;
            }
            
            //comprueba long shot
            if (transform.position.x >= 7.5f)
                first_long = true;

            //lanzar el balon
            transform.GetComponent<Rigidbody2D>().isKinematic = false; //deja de flotar y es afectada por la gravedad
            transform.GetComponent<Collider2D>().enabled = true;
            transform.GetComponent<Rigidbody2D>().AddForce(GetForce(Input.mousePosition));
            shot = true;
            aiming = false;

            first_check = false; second_check = false; tercer_check = false;

            scoreScript.Shot();
            scoreScript.HidePath();
        }
    }

    //lanzamiento del balon
    public Vector2 GetForce(Vector3 toPos)
    {
        Vector2 force = new Vector2(startPos.x, startPos.y) - new Vector2(toPos.x, toPos.y);

        return force * power;
    }

    bool inDeadZone(Vector2 mouse)
    {
        //si sueltas muy cerca del punto de partida
        if (Mathf.Abs(startPos.x - mouse.x) <= dead_sens && Mathf.Abs(startPos.y - mouse.y) <= dead_sens)
            return true;
        return false;
    }

    bool inPressZone(Vector2 mouse)
    {
        //si pulsas en la izquierda o arriba no empieza a cargar el disparo
        //if ((mouse.x <= 70) || (mouse.y >= Screen.height - 50))
        if ((mouse.x <= Screen.width / 10f) || (mouse.y >= Screen.height - 50))
            return true;

        return false;
    }

    bool inReleaseZone(Vector2 mouse)
    {
        //si sueltas en la izquierda no dispara, necesito icono que lo represente para que el usuario se entere.
        //poner lo mismo en la derecha. Explicarlo en tutorial
        //if ((mouse.x <= 70) || (mouse.y >= Screen.height - 50))
        if ((mouse.x <= Screen.width / 10f) || (mouse.y >= Screen.height - 50))
            return true;

        return false;
    }


    //colisiones de score
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Floor") //suelo
        {
            hit_ground = true;
        }

        if (other.gameObject.tag == "BackWall") //tablero
        {
            GameObject.Find("gameScript").GetComponent<gameScript>().Sound(5);
            if (first_back_wall)
                back_wall = true; //si toca primero el tablero antes que el aro
            //first_trick_board = false;
        }

        if (other.gameObject.tag == "Rim") //aro (izq y der)
        {
            GameObject.Find("gameScript").GetComponent<gameScript>().Sound(2);
            clean = false;
            first_back_wall = false; //el tablero no es lo primero que toca
            first_trick_board = false; //tablero encima no es lo primero que toca
        }

        if (other.gameObject.tag == "Floor")
        {
            if (canFail) //por que este cambio de sonido? !!!
                GameObject.Find("gameScript").GetComponent<gameScript>().Sound(3);
            else
                GameObject.Find("gameScript").GetComponent<gameScript>().Sound(4);

            if (!score && canFail) //si falla
            //if (!score) //si falla
            {
                scoreScript.BreakCombo();
                canFail = false; //ya no vuelve a mandar un -1
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "BackWall2") //tablero encima
        {
            GameObject.Find("gameScript").GetComponent<gameScript>().Sound(5);
            if (first_trick_board)
                trick = true; //si toca primero el tablero encima antes que el aro
            first_back_wall = false;
        }

        //Net1 esta en AroArriba, se corresponde con un trigger en la parte superior del aro
        if (other.gameObject.tag == "Net1" && !score)
            first_check = true;

        //Net 2 esta debajo de la canasta, es para el clean shot
        if (other.gameObject.tag == "Net2")
        {
            //if (!first_check)
            //{
            //    canScore = false; //si pasa primero por debajo del aro ya no puede encestar
            //    first_check = false;
            //    second_check = false;
            //}
            //else
            //    second_check = true;
            if (clean)
                first_clean = true;
        }

        //Net3 esta dentro de la red, frena el balon y anima la red (y marca el punto)
        if (other.gameObject.tag == "Net3")
        {
            //aunque no sea valido que suene y anime
            GameObject.Find("Red").GetComponent<Animator>().SetTrigger("Score");
            GameObject.Find("gameScript").GetComponent<gameScript>().Sound(1);

            //reducir velocidad por caer en la red, NO FUNCIONA EL GetComponent<Rigidbody2D>().velocity
            //no funciona porque en el depurador (Watch) se pone gameObject.GetComponent(Rigidbody2D).velocity
            //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(gameObject.GetComponent<Rigidbody2D>().velocity.x / 2, gameObject.GetComponent<Rigidbody2D>().velocity.y / 2, 0);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0); //la red frena en seco

            if (first_check)
                tercer_check = true;
            else
            {
                canScore = false; //si pasa primero por debajo del aro ya no puede encestar
                first_check = false;
                second_check = false;
                tercer_check = false;
            }

            if (tercer_check && canScore && !score) //si ha pasado por arriba, si puede y si no ha encestado ya
            {
                score = true;
                scoreScript.Score(first_clean, back_wall, first_long, uphigh, trick);
            }

        }
    }
}

