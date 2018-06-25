using UnityEngine.UI;
using UnityEngine;

public enum GameStatus
{
    menu, play, gameover
}

public class GameManager : MonoBehaviour
{

    private GameStatus currentState = GameStatus.menu;
    public GameStatus CurrentState { get { return currentState; } }

    private bool canDo = false;
    public bool CanDo
    {
        get { return canDo; }
        set { canDo = value; }
    }
    
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private Button PauseBtn;

    private Vector3 menuPos;
    private Vector3 btnPos;

    public static GameManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }
    // Use this for initialization
    void Start()
    {
        menuPos = Menu.transform.position;
        btnPos = PauseBtn.transform.position;
        PauseMenu.transform.position = menuPos + new Vector3(0, -2000, 0);
        PauseBtn.transform.position = menuPos + new Vector3(0, -2000, 0);
        GameOverMenu.transform.position = menuPos + new Vector3(0, -2000, 0);
    }


    public void Pause()
    {
        if (currentState == GameStatus.play)
        {
            if (Time.timeScale == 0) //if pressed to continue the game
            {
                Time.timeScale = 1;
                PauseMenu.transform.position = menuPos + new Vector3(0, -2000, 0);
                PauseBtn.transform.position = btnPos; //show pause button
            }
            else //if pressed to pause
            {
                Time.timeScale = 0;
                PauseMenu.transform.position = menuPos; //show pause menu
                PauseBtn.transform.position = btnPos + new Vector3(0, -2000, 0);
            }
        }

    }

    public void NewGame()
    {
        Menu.transform.position = menuPos + new Vector3(0, -2000, 0);
        PauseMenu.transform.position = menuPos + new Vector3(0, -2000, 0);
        GameOverMenu.transform.position = menuPos + new Vector3(0, -2000, 0);
        PauseBtn.transform.position = btnPos;
        currentState = GameStatus.play;
    }

    public void GoToMenu()
    {
        Menu.transform.position = menuPos;
        currentState = GameStatus.menu;
        PauseMenu.transform.position = menuPos + new Vector3(0, -2000, 0);
        PauseBtn.transform.position = menuPos + new Vector3(0, -2000, 0);
        GameOverMenu.transform.position = menuPos + new Vector3(0, -2000, 0);
    }


    public void GameOver()
    {
        currentState = GameStatus.gameover;
        GameOverMenu.transform.position = menuPos;
        PauseBtn.transform.position = btnPos + new Vector3(0, -2000, 0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            Pause();
        }
    }
}
