using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargaMapa : MonoBehaviour {

    public GameObject gameManager;

    private void Awake()
    {
        //conseguimos general el gameManager en cada escena
        if(GameManager.instance == null)
        {
            Instantiate(gameManager);
        }


    }
}
