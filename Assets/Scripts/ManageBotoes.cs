using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
///<summary>
/// Classe que controla todas as funções referentes
/// aos botões dos menus
///</summary>

public class ManageBotoes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
    }
    
    /*
        Inicia o Jogo
    */
    public void StartMundoGame()
    {
        SceneManager.LoadScene("Lab1");  //Carrega a scene lab1
    }

    /*
        Carrega a "Scene" do menu principal
    */
    public void CarregaMenu()
    {
        SceneManager.LoadScene("Lab1_Start");  //Carrega a scene Menu Principal
    }

    /*
        Carrega a "Scene" dos créditos
    */
    public void CarregaCreditos()
    {
        SceneManager.LoadScene("Lab1_Creditos");  //Carrega a scene Menu Principal
    }
}
