using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
///<summary>
/// Classe que controla todas as funções referentes a scene "lab1"
/// todas as mecânicas referentes ao jogo da forca em si estão aqui
///</summary>

public class GameManager : MonoBehaviour
{
    private int numTentativas;  // Armazena as tentativas válidas da rodada
    private int maxNumTentativas;  // Número máximo de tentativas para Forca ou Salvação
    int score = 0;

    public GameObject letra;  // Prefab da letra no Game
    public GameObject centro;  // Objeto de texto que indica o centro da tela

    private string palavraOculta = "";  // Palavra oculta a ser descoberta
    // private string[] palavrasOcultas = new string[] {"carro", "elefante", "futebol"}; // Array de palavras ocultas
    private int tamanhoPalavraOculta;  // Tamanho da palavra oculta
    char[] letrasOcultas; // Letras da palavra oculta
    bool[] letrasDescobertas; // Indicador de quais letras foram descobertas
    public AudioSource audioSource;
    public AudioClip acerto;
    public AudioClip erro;

    // Start is called before the first frame update
    void Start()
    {
        centro = GameObject.Find("centroDaTela");

        InitGame();
        InitLetras();
        numTentativas = 0;
        maxNumTentativas = 10;
        UpdateNumTentativas();
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        checkTeclado();
    }

    /*
        Função que cria os objetos que serão as letras da palavra selecionada
    */
    void InitLetras()
    {
        int numLetras = tamanhoPalavraOculta;
        for (int i=0; i<numLetras; i++){
            Vector3 novaPosicao;
            novaPosicao = new Vector3(centro.transform.position.x + ((i-numLetras/2.0f)*80), centro.transform.position.y, centro.transform.position.z);
            GameObject l = (GameObject)Instantiate(letra, novaPosicao, Quaternion.identity);
            l.name = "letra" + (i + 1);  // Nomeia na hierarquia a GameObject com letra-(iésima+1), i = 1...numLetras
            l.transform.SetParent(GameObject.Find("Canvas").transform);  // Posiciona-se como filho do GameObject Canvas

        }
    }

    /*
        Método que inicializa o game, selecionando a palavra oculta
        e definindo os parametros de acordo com a palavra selecionada
    */
    void InitGame()
    {
        //palavraOculta = "Elefante"; // Definição da palavra a ser descoberta (usado no Lab1 - Parte A)
        //int numeroAleatorio = Random.Range(0, palavrasOcultas.Length); // Sorteamos um número dentro do número de palavras do array
        //palavraOculta = palavrasOcultas[numeroAleatorio]; // Selecionamos uma palavra sorteada

        palavraOculta = PegaUmaPalavraDoArquivo();
        tamanhoPalavraOculta = palavraOculta.Length; // Determina-se o número de letras da palavra oculta
        palavraOculta = palavraOculta.ToUpper(); // Transforma-se a palavra em maiúscula
        letrasOcultas = new char[tamanhoPalavraOculta]; // Instancia-se o array char das letras da palavra
        letrasDescobertas = new bool[tamanhoPalavraOculta]; // Instancia-se o array bool do indicador de letras certas
        letrasOcultas = palavraOculta.ToCharArray(); // Copia-se a palavra no array de letras
    }

    /*
        Método que verifica se houve uma entrada no teclado e
        verifica se a letra teclada corresponde a alguma da
        palavra oculta, caso o numero maximo de tentativas seja
        ultrapassado o metodo carrega a scene "forca"
    */
    void checkTeclado()
    {
        if(Input.anyKeyDown){
            char letraTeclada = Input.inputString.ToCharArray()[0];
            int letraTecladaComoInt = System.Convert.ToInt32(letraTeclada);

            if (letraTecladaComoInt >= 97 && letraTecladaComoInt <= 122)
            {
                numTentativas++;
                UpdateNumTentativas();
                if (numTentativas > maxNumTentativas)
                {
                    SceneManager.LoadScene("Lab1_forca");
                }
                bool trueSeAcertou = false;
                for (int i = 0; i<tamanhoPalavraOculta; i++)
                {
                    if (!letrasDescobertas[i]){
                        letraTeclada = System.Char.ToUpper(letraTeclada);
                        if (letrasOcultas[i] == letraTeclada)
                        {
                            letrasDescobertas[i] = true;
                            GameObject.Find("letra" + (i+1)).GetComponent<Text>().text = letraTeclada.ToString();
                            score = PlayerPrefs.GetInt("score");
                            score++;
                            PlayerPrefs.SetInt("score", score);
                            UpdateScore();
                            trueSeAcertou = true;
                            audioSource.clip = acerto;
                            audioSource.Play();
                            VerificaSePalavraDescoberta();
                        }
                    }
                }
                if(trueSeAcertou == false){
                    audioSource.clip = erro;
                    audioSource.Play();
                }
            }
        }
    }

    /*
        Função que atualiza o valor do número de tentativas
    */
    void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = numTentativas + " | " + maxNumTentativas;
    }

    /*
        Função que atualiza a pontuação no placar "score"
    */
    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Score " + score;
    }

    /*
        Verifica se a palavra foi descoberta, caso sim,
        carrega a scene de "salvo"
    */
    void VerificaSePalavraDescoberta()
    {
        bool condicao = true;
        for (int i = 0; i< tamanhoPalavraOculta; i++)
        {
            condicao = condicao && letrasDescobertas[i];
        }
        if (condicao)
        {
            PlayerPrefs.SetString("ultimaPalavraOculta", palavraOculta);
            SceneManager.LoadScene("Lab1_salvo");
        }
    }

    /*
        Metodo que seleciona uma palavra aleatória do arquivo de palavras
    */
    string PegaUmaPalavraDoArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavras", typeof(TextAsset)); // Carrega o arquivo palavras.txt
        string s = t1.text; // Coloca o texto na string s
        string[] palavras = s.Split(' '); // Separa as palavras do texto em um vetor de string
        int palavraAleatoria = Random.Range(0, palavras.Length); // Sorteia um valor aleatorio para selecionar a palavra aleatória dentro do vetor
        return (palavras[palavraAleatoria]); // Retorna a palavra oculta aleatória
    }
}
