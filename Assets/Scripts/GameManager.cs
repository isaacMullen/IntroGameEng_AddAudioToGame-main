using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;
using System.IO;
public class GameManager : MonoBehaviour
{
    public LeaderboardHandler leaderboard;

    public TMP_InputField nameInputField;
    public NameInput nameInput;
    string playerName;
    bool checkingForEnterAfterName;

    Quaternion initialRotation;
    Vector3 initialScale;
    public float playingVolume;
    
    public GameObject player;
    public SFXManager sfxManager;
    public PlayerController playerController;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;
    public int score  = 0;

    public TextMeshProUGUI shieldText;
    public int shield = 3;   

    [Header ("UI_Panels")]
    public GameObject MainMenuUI;
    public GameObject GameplayUI;
    public GameObject PausedMenuUI;
    public GameObject GameOverUI;
    public GameObject EnteringNameUI;
    public GameObject LeaderBoardUI;

    private GameObject asteroidSpawner;

    private bool gameOver;
    public enum GameState { MainMenu, Gameplay, GameOver, Paused, EnteringName, LeaderBoard }

    public GameState gameState;
    //private GameState LastgameState;


    // Specifies this script and all its children as a singleton, Awake function below deletes any extra copies of this onbject so that there exists only a "Single" instance of itself
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);  // will not destroy this object when changing scenes           
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {        
        sfxManager.BGMusic();
        //Setting initial volume of the audioSource
        sfxManager.BgMusicAudioSource.volume = .05f;

        initialRotation = player.transform.rotation;
        initialScale = player.transform.localScale;
    }


    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        shieldText.text = shield.ToString();
        //Debug.Log(score);

        if(checkingForEnterAfterName)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                // Get the text from the input field and store it
                playerName = nameInputField.text;
                Debug.Log("Player name entered: " + playerName);

                // Do something with playerName, like starting the game
                StartGame();
            }
        }

        if (shield < 0)
        {  
            gameOver = true;            
        }        

        switch (gameState)
        {
            case GameState.LeaderBoard:

                MainMenuUI.SetActive(false);
                GameplayUI.SetActive(false);
                PausedMenuUI.SetActive(false);
                GameOverUI.SetActive(false);
                EnteringNameUI.SetActive(false);
                LeaderBoardUI.SetActive(true);
                break;

            case GameState.MainMenu:                

                MainMenuUI.SetActive(true);
                GameplayUI.SetActive(false);
                PausedMenuUI.SetActive(false);
                GameOverUI.SetActive(false);
                EnteringNameUI.SetActive(false);
                LeaderBoardUI.SetActive(false);
                break;

            case GameState.EnteringName:
                MainMenuUI.SetActive(false);
                PausedMenuUI.SetActive(false);
                GameOverUI.SetActive(false);
                GameplayUI.SetActive(false);
                EnteringNameUI.SetActive(true);
                LeaderBoardUI.SetActive(false);
                break;
                

            case GameState.Gameplay:                
                MainMenuUI.SetActive(false);
                GameplayUI.SetActive(true);
                PausedMenuUI.SetActive(false);
                GameOverUI.SetActive(false);
                EnteringNameUI.SetActive(false);
                LeaderBoardUI.SetActive(false);

                if (Input.GetKeyDown(KeyCode.Escape))
                {                    
                    gameState = GameState.Paused;
                }
                //HEREHEREHEREHEREHERE
                if (gameOver == true)
                {
                    using (StreamReader reader = new StreamReader(leaderboard.LBfile))
                    {
                        string line;

                        while((line = reader.ReadLine()) != null)
                        {
                            string[] fields = reader.ReadLine().Split(',');

                            string player = fields[0];
                            int LBscore;
                            if(int.TryParse(fields[1], out LBscore))
                            {
                                Debug.Log($"Player: {player} | Score {LBscore}");
                            }
                        }
                        
                    }
                    leaderboard.WriteFile(playerName, score, leaderboard.LBfile);

                    StartCoroutine(playerController.PlayAnimThenDie());
                    gameState = GameState.GameOver;
                    gameOverScoreText.text = score.ToString();
                    asteroidSpawner = GameObject.Find("AsteroidSpawner");
                    asteroidSpawner.SetActive(false);                    
                }
                break;

            case GameState.GameOver:               

                //this check will wait unitl the Coroutine of the animation is finished
                if(playerController.animationFinished)
                {
                    MainMenuUI.SetActive(false);
                    GameplayUI.SetActive(false);
                    PausedMenuUI.SetActive(false);
                    GameOverUI.SetActive(true);
                    LeaderBoardUI.SetActive(false);
                    //ayerDestroy();

                }
                break;


            case GameState.Paused:
                Time.timeScale = 0f;
                MainMenuUI.SetActive(false);
                GameplayUI.SetActive(false);
                PausedMenuUI.SetActive(true);
                GameOverUI.SetActive(false);
                LeaderBoardUI.SetActive(false);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    gameState = GameState.Gameplay;
                    Time.timeScale = 1f;
                }
                break;
        }
    }

    
    public void LeaderBoard()
    {
        gameState = GameState.LeaderBoard;
    }

    public void EnteringName()
    {
        checkingForEnterAfterName = true;
        nameInput.nameInputField.text = "";
        playerName = nameInput.SubmitName(nameInputField.text);
        gameState = GameState.EnteringName;
    }

    public void StartGame()
    {
        Debug.Log(playerName);

        gameState = GameState.Gameplay;

        

        playerController.animationFinished = false;
        shield = 3;
        score = 0;

        player.transform.localScale = initialScale;
        player.transform.rotation = initialRotation;
        playerController.canMove = true;

        //Starts Fading in Music (freaking awesome)
        StartCoroutine(FadeMusic(5f, playingVolume, sfxManager.BgMusicAudioSource));
        
        SceneManager.LoadScene("Gameplay");
        player.SetActive(true);        
        gameOver = false;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");        
        sfxManager.BGMusic();            
        gameState = GameState.MainMenu;
        gameOver = false;
       
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public IEnumerator FadeMusic(float fadeDuration, float targetVolume, AudioSource sound)
    {
        float startVolume = sound.volume;        
        float timeElapsed = 0f;

        while(timeElapsed < fadeDuration)
        {
            sound.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / fadeDuration);

            timeElapsed += Time.deltaTime;

            yield return null;

        }
        sound.volume = targetVolume;
    }

    
}
