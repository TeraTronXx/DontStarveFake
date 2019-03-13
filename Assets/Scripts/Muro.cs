using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muro : MonoBehaviour {

    public Sprite dañado;
    public int vida = 3;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    //cogemos el muro y lo cambiamos por el sprite del muro dañado. De esta forma se sabe cuando se esta haciendo daño a un muro
    public void DañoMuro(int daño)
    {
        sr.sprite = dañado;
        vida = vida - daño;

        if(vida <= 0)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }

    }

    
}
