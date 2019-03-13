using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : Movimiento {

    public int dañoJugador = 2;

    private Animator anim;
    private Transform target;
    private bool turn;//turno de los enemigos

    

    protected override void Awake()
    {
        anim = GetComponent<Animator>();
        base.Awake();
    }

    // Use this for initialization
    protected override void Start () {
        GameManager.instance.NuevoEnemigo(this);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}

    protected override void IntentaMover(int xUnit, int yUnit)
    {
        //en caso de que no sea su movimiento
        if (turn)
        {
            turn = false;
            return;
        }
        base.IntentaMover(xUnit, yUnit);
        turn = true; //el proximo movimiento se lo saltará
    }

    //metodo que usara el gamemanager para mover a los enemigos
    public void MovimientoEnemigo()
    {
        int xUnit = 0;
        int yUnit = 0;
        if (Math.Abs(target.position.x - transform.position.x) < 1) //comprovamos si estan en la misma linea vertical
        {
            yUnit = target.position.y > transform.position.y ? 1 : -1; //si el jugador esta debajo, se mueve hacia abajo, si esta arriba, se mueve hacia arriba
        }else
        {
            xUnit = target.position.x > transform.position.x ? 1 : -1; //si el jugador esta a la derecha, va a la derecha. y lo mismo con la izquierda
        }
        //intenta realizar el movimiento del enemigo
        IntentaMover(xUnit, yUnit);

    }

    protected override void NoPuedeMover(GameObject go)
    {
        Jugador AtacaAJugador = go.GetComponent<Jugador>();
        if(AtacaAJugador != null) //significa que tiene al jugador delante
        {
            AtacaAJugador.PierdeHambre(dañoJugador);
            anim.SetTrigger("AtaqueEnemigo");
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
