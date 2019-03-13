using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public float tiempoTurno = 0.01f; //es lo mismo que tardara un objeto en moverse

    public float delayNiveles = 2f;

    private List<Enemigo> enemigos = new List<Enemigo>();
    private bool enemigosMoviendo;



    public MapCreator mp;

    //indica el nivel de hambre del personaje (su vida)
    public int hambre = 100;
    public bool turno = true;



    private int nivel = 0;
    private GameObject levelImage;
    private Text levelText;
    public bool cargandoEscena;


    void Awake()
    {
        //singleton
        if(GameManager.instance == null)
        {
            GameManager.instance = this;
        }else if(GameManager.instance != this)
        {
            Destroy(gameObject);
        }

        //si al recargar otra o la misma escena, no queremos que este objeto se destruya
        DontDestroyOnLoad(gameObject);

        mp = GetComponent<MapCreator>();
    }

    void Start()
    {
        //pone a true cargando escena para que se sepa que se esta cargando.
        cargandoEscena = true;
        levelImage = GameObject.Find("LevelImage"); //muestra info acerca del nivel en el que esta
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Día " + nivel;
        
        levelImage.SetActive(true);
        
        enemigos.Clear();
        mp.InicioEscena(nivel);

        Invoke("esconderInicioNivel", delayNiveles);
    }
    //se le llamada cuando termina la muestra del nivel
    private void esconderInicioNivel()
    {
        levelImage.SetActive(false);
        cargandoEscena = false;
    }

    public void GameOver()
    {
        levelText.text = "Has muerto tras " + nivel + " días. Bien jugado.";
        levelImage.SetActive(true);
        enabled = false;
        PlayerPrefs.SetInt("HIGHSCORE", Mathf.Max(PlayerPrefs.GetInt("HIGHSCORE", 0), nivel));
    }

    //corrutina para mover a los enemigos
    IEnumerator MoverEnemigos()
    {
        
        enemigosMoviendo = true;

        yield return new WaitForSeconds(tiempoTurno);
        if(enemigos.Count == 0)
        {
            yield return new WaitForSeconds(tiempoTurno);
        }
        //recorre todos los enemigos que se tengan que mover
        for (int i = 0; i < enemigos.Count; i++)
        {
            enemigos[i].MovimientoEnemigo();
            yield return new WaitForSeconds(enemigos[i].movingTime);
        }
        //cuando han terminado todos, es el turno del jugador
        turno = true;
        enemigosMoviendo = false;
    }

    private void Update()
    {
        if (turno || enemigosMoviendo || cargandoEscena) return;

        StartCoroutine(MoverEnemigos());
    }

    public void NuevoEnemigo(Enemigo enemigo)
    {
        enemigos.Add(enemigo);
    }

    //cuando el componente gamemanager se activa, lo notificamos a la escena para que ejecute automaticamente que se ha cargado una pantalla. se usa cada vez que se recarga
    private void OnEnable()
    {
        SceneManager.sceneLoaded += PantallaCargaFinaliza;
    }

    //al contrario, cuando no esta activo, tiene que quitarse
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= PantallaCargaFinaliza;
    }

    

    private void PantallaCargaFinaliza(Scene scene, LoadSceneMode mode)
    {
        nivel++;
        Start();
    }
}
