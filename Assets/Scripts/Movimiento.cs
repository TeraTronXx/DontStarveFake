using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clase abstracta que solo servira para llamarse desde otros scripts implementando siempre sus metodos abstractos. 
public abstract class Movimiento : MonoBehaviour {

    //tiempo que tarda en moverse entre las casillas ysu velocidad
    public float movingTime = 0.1f;
    public float speed;
    public LayerMask blockingLayer;

    private BoxCollider2D bc;
    private Rigidbody2D rb;

    protected virtual void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    protected virtual void Start () {
        //asignamos la velocidad necesaria para que tarde 0.1s en pasar de una casilla a otra
        speed = 1f / movingTime;
		
	}
	
	protected bool Mover(int xUnit, int yUnit, out RaycastHit2D hit)
    {
        //cogemos las posiciones inicial y final. La posicion final, sera la iniciar mas las unidades de x e y que se tiene que mover el personaje
        Vector2 inicio = transform.position;
        Vector2 fin = inicio + new Vector2(xUnit, yUnit);
        //primero es necesario desactivar el boxcollider para saber si chocamos con algo mediante el raycast
        bc.enabled = false;
        //requiere un punto inicial y un punto final, y muestra siempre que golpea con algun objeto de la capa blockingLayer
        hit = Physics2D.Linecast(inicio, fin, blockingLayer);
        //lo volvemos a activar ya que los enemigos pueden chocar con nosotros y nosotros podemos seguir chocando con cosas
        bc.enabled = true;

        //tras saber si hemos chocado con algo, acudimos a hit. 
        //si no nos hemos chocado, nos movemos. 
        if (hit.transform == null)
        {
            //movemos el personaje
            StartCoroutine(MovimientoPersonaje(fin));
            return true;
        }
        else //en el caso de haber encontrado algun obstaculo, devolvemos false porque el objeto no se pudo mover.
        {
            return false;
        }
    }

    //en el caso de que se haya podido mover o no tras la corrutina, debemos hacer diferentes cosas. 
    protected virtual void IntentaMover(int xUnit, int yUnit)
    {
        RaycastHit2D hit;
        //recoge en un booleano si el personaje se puede mover o no
        bool puedeMover = Mover(xUnit, yUnit, out hit);
        //si se puede mover, sale
        if (puedeMover) return;

        //si no se puede mover, muestra con que se ha chocado para tomar las medidas oportunas.
        NoPuedeMover(hit.transform.gameObject);
    }

    protected abstract void NoPuedeMover(GameObject go);






    //corrutina para mover. se mueve hasta detectar que ha llegado al final
    protected IEnumerator MovimientoPersonaje(Vector2 fin)
    {
        //obtenemos la distancia entre la posicion actual hasta la posicion final. 
        float dist = Vector2.Distance(rb.position, fin);
        //mientras haya distancia, debe moverse. cogemos el valor de epsilon ya que se asemeja mucho a cero ya que es posible que nunca se acerque del todo a cero.
        while (dist > float.Epsilon)
        {
            //movemos el personake
            Vector2 pos = Vector2.MoveTowards(rb.position, fin, (speed * Time.deltaTime));
            rb.MovePosition(pos);
            //recalculamos la distancia hasta el destino para el proximo loop
            dist = Vector2.Distance(rb.position, fin);
            yield return null;
        }
    }



}
