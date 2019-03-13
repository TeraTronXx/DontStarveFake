using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour {

    public int filas = 10;
    public int columnas = 10;
    public int minMurosInternos = 5;
    public int maxMurosInternos = 10;
    public int minComida = 2;
    public int maxComida = 5;
    
    //almacena todos los prefabs que tenemos
    public GameObject[] suelos, muros, murosInternos, comida, enemigos;
    public GameObject exit;

    //lista que recoge todas las posiciones del mapa
    private List<Vector2> mapPositions = new List<Vector2>();

    private Transform mapTransform;

    //llama a todos los metodos que se encargan de crear el mapa
    public void InicioEscena(int level)
    {
        InicioMapa();
        RellenaLista();
        ObjetoAleatorio(murosInternos, minMurosInternos, maxMurosInternos);
        ObjetoAleatorio(comida, minComida, maxComida);
        ObjetoAleatorio(enemigos, level, level);
        Instantiate(exit, new Vector2(columnas - 1, filas - 1), Quaternion.identity);

    }

    void RellenaLista()
    {
        mapPositions.Clear();
        for (int i = 1; i < columnas - 1; i++)
        {
            for (int j = 1; j < filas - 1; j++)
            {
                //rellenamos el array con las posiciones del mapa
                mapPositions.Add(new Vector2(i, j));
            }
        }
    }

    //cojo una posicion aleatoria del mapa de posibles posiciones para poder general objetos. Con esto podemos saber que posicion está ocupada y cual no
    Vector2 RandomMapPosition()
    {
        int randomPos = Random.Range(0, mapPositions.Count);
        Vector2 randomMapPosition = mapPositions[randomPos];
        mapPositions.RemoveAt(randomPos);
        return randomMapPosition;
    }

    //con la posicion obtenida, se genera un objeto aleatorio en esa posicion. Lista de los objetos, objetos minimos que queremos y objetos maximos que queremos
    void ObjetoAleatorio(GameObject[] lista, int min, int max)
    {
        int objeto = Random.Range(min, max + 1);
        for (int i = 0; i<objeto; i++)
        {
            //seleccionamos posiciones en las que introducir el objeto que tambien sera generado aleatoriamente.
            Vector2 posicionObjeto = RandomMapPosition();
            GameObject objetoAGenerar = lista[Random.Range(0, lista.Length)];
            Instantiate(objetoAGenerar, posicionObjeto, Quaternion.identity);
        }
    }

	public void InicioMapa()
    {
        //creamos un gameobject en el que englobaremos todos los elementos que crean el mapa
        mapTransform =  new GameObject ("Mapa").transform;
        //recorremos todos los lugares impuestos para el tamaño del mapa
        for (int i = -1; i < columnas + 1; i++)
        {
            for (int j = -1; j < filas + 1; j++)
            {
                //cogemos un suelo aleatorio de todos los suelos de los que disponemos
                GameObject crear = suelos[Random.Range(0, suelos.Length)];

                if(i==-1 || j == -1 || i==columnas || j == filas)
                {
                    //en caso de ser un extremo, cogemos un muro
                    crear = muros[Random.Range(0, muros.Length)];
                }

                //creamos una instancia de dichos objetos
                GameObject instancia = Instantiate(crear, new Vector2(i, j), Quaternion.identity);
                instancia.transform.SetParent(mapTransform);

            }
        }
    }
}
