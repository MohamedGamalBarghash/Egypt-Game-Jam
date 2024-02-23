using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 

    public TMP_Text timerText;

    [SerializeField] private GameObject[] scenes = new GameObject[5];
    [SerializeField] private GameObject pressToContinue;
    int currentSceneIndex = 0;

    public float timeLimitInSeconds = 1f; 
    private float elapsedTime = 0f;
    private bool levelCompleted = false;

    // PlayerController events
    public event Action<PlayerController> PlayerControllerSpawned;
    public event Action<PlayerController> PlayerControllerSmacked;
    public event Action<PlayerController> PlayerControllerTookPowerUp;
    public event Action<PlayerController> PlayerControllerTookMoneyFromBank;
    public event Action<PlayerController> PlayerControllerTookPrisoner;
    public event Action<PlayerController> PlayerControllerReturnedMoney;
    public event Action<PlayerController> PlayerControllerReturnedPrisoner;

    // Enemy events
    public event Action<Enemy> EnemySpawned;
    public event Action<Enemy> EnemyHit;
    public event Action<Enemy> EnemyStole;
    public event Action<Enemy> EnemyDeposited;
    public event Action<Enemy> EnemyDied;

    // Game events
    public event Action GamePaused;
    public event Action GameStarted;
    public event Action GameEnded;
    public event Action GameWon;
    public event Action GameLost;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GameLost += LevelLost;
        Time.timeScale = 0;
        elapsedTime = timeLimitInSeconds;
        currentSceneIndex = 0;
        foreach (GameObject scene in scenes)
        {
            scene.SetActive(false);
        }
        scenes[currentSceneIndex].SetActive(true);
        pressToContinue.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentSceneIndex ++;
            if (currentSceneIndex >= 5) {
                foreach (GameObject scene in scenes)
                {
                    scene.SetActive(false);
                }
                pressToContinue.SetActive(false);
                Time.timeScale = 1;
            }
            else {
                foreach (GameObject scene in scenes)
                {
                    scene.SetActive(false);
                }
                scenes[currentSceneIndex].SetActive(true);
            }
            // RaiseGameStarted();
        }

        if (!levelCompleted)
        {
            elapsedTime -= Time.deltaTime;

            if (elapsedTime <= 0)
            {
                LevelCompleted();
            }
        }

        timerText.text = "Time: " + Mathf.Floor(elapsedTime).ToString();
    }

    private void LevelCompleted()
    {
        Debug.Log("Congratulations! You won the level!");
        levelCompleted = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        RaiseGameWon(); 
    }

    private void LevelLost () {
        Debug.Log("You lost the level!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // 3ashan nngz
    // Methods to raise PlayerController events
    public void RaisePlayerControllerSpawned(PlayerController PlayerController) => PlayerControllerSpawned?.Invoke(PlayerController);
    public void RaisePlayerControllerSmacked(PlayerController PlayerController) => PlayerControllerSmacked?.Invoke(PlayerController);
    public void RaisePlayerControllerTookPowerUp(PlayerController PlayerController) => PlayerControllerTookPowerUp?.Invoke(PlayerController);
    public void RaisePlayerControllerTookMoneyFromBank(PlayerController PlayerController) => PlayerControllerTookMoneyFromBank?.Invoke(PlayerController);
    public void RaisePlayerControllerTookPrisoner(PlayerController PlayerController) => PlayerControllerTookPrisoner?.Invoke(PlayerController);
    public void RaisePlayerControllerReturnedMoney(PlayerController PlayerController) => PlayerControllerReturnedMoney?.Invoke(PlayerController);
    public void RaisePlayerControllerReturnedPrisoner(PlayerController PlayerController) => PlayerControllerReturnedPrisoner?.Invoke(PlayerController);

    // Methods to raise enemy events
    public void RaiseEnemySpawned(Enemy enemy) => EnemySpawned?.Invoke(enemy);
    public void RaiseEnemyHit(Enemy enemy) => EnemyHit?.Invoke(enemy);
    public void RaiseEnemyStole(Enemy enemy) => EnemyStole?.Invoke(enemy);
    public void RaiseEnemyDeposited(Enemy enemy) => EnemyDeposited?.Invoke(enemy);
    public void RaiseEnemyDied(Enemy enemy) => EnemyDied?.Invoke(enemy);

    // Methods to raise game events
    public void RaiseGameWon() => GameWon?.Invoke();
    public void RaiseGamePaused() => GamePaused?.Invoke();
    public void RaiseGameStarted() => GameStarted?.Invoke();
    public void RaiseGameEnded() => GameEnded?.Invoke();
    public void RaiseGameLost() => GameLost?.Invoke();
}
