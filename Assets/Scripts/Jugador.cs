using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Jugador : Movimiento {

    //variables del personaje. todas ellas publicas para que puedan verse tanto en unity como en otras clases.
    public int dañoMuro = 1;
    public int puntosBebida = 15;
    public int puntosComida = 10;
    public float comienzoNivel = 1f;

    private Animator anim;
    private int comida;

    public Text foodText;

    protected override void Awake()
    {
        anim = GetComponent<Animator>();
        base.Awake();
    }

    protected override void Start()
    {
        //antes de comenzar la escena, guardamos los puntos de vida que tiene el personaje de la escena anterior
        comida = GameManager.instance.hambre;
        foodText.text = "Comida: " + comida;
        base.Start();
    }

    //cuando se carga la escena en otro nivel, desactivamos el jugador.
    void DesactivarJugador()
    {
        GameManager.instance.hambre = comida;
    }

    void CheckGameOver()
    {
        if (comida <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    protected override void IntentaMover(int xUnit, int yUnit)
    {
        //cada vez que el jugador se mueva, pierde una unidad de vida.
        comida--;
        foodText.text = "Comida: " + comida;
        base.IntentaMover(xUnit, yUnit);
        CheckGameOver();
        //tras moverse y ver que el juego no ha terminado, termina su turno.
        GameManager.instance.turno = false;
    }

    //en caso de no poder moverse, comprueba que es unmuro y en su caso, el muro pierde durabilidad
    protected override void NoPuedeMover(GameObject go)
    {
        Muro muro = go.GetComponent<Muro>();
        if(muro != null)
        {
            muro.DañoMuro(dañoMuro);
            anim.SetTrigger("playerAttack");
        }
    }

    //reinicia la escena a ella misma
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene("Main2");
    }

    //en caso de perder vida
    public void PierdeHambre(int hunger)
    {
        comida = comida - hunger;
        foodText.text = "-" + hunger + "  Comida: " + comida;
        anim.SetTrigger("playerHit");
        CheckGameOver();

    }

    //comprobamos si choca con algun alimento para poder recuperar vida
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //si el objeto contra el que ha chocado es la salida de exit, debemos pasar de lv con sus segundos de retraso entre nivel
        if (collision.CompareTag("Exit"))
        {
            Invoke("Restart", comienzoNivel);
            enabled = false;
        }
        else if (collision.CompareTag("Food"))
        {//si choca contra la comida, aumentamos la vida
            comida = comida + puntosComida;
            foodText.text = "+"+puntosComida+"  Comida: " + comida;
            Destroy(collision.gameObject);
        }else if (collision.CompareTag("Soda"))//lo mismo con la bebida
        {
            comida = comida + puntosBebida;
            foodText.text = "+" + puntosBebida + "  Comida: " + comida;
            Destroy(collision.gameObject);
        }
    }

    void Update()
    {
        //si no es el turno del jugador, termina
        if (!GameManager.instance.turno || GameManager.instance.cargandoEscena) return;

        //si es el turno del jugador, se mueve.
        int hor;
        int ver;
        hor = (int)Input.GetAxisRaw("Horizontal");
        ver = (int)Input.GetAxisRaw("Vertical");

        if (hor != 0) ver = 0;
        if(hor != 0 || ver != 0)
        {
            IntentaMover(hor, ver);
        }
            
    }
}
