using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool gameStarted = false;
    public bool crawlersStarted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if (gameStarted)
        {
            crawlersStarted = true;
        }
    }

    public void GameOver()
    {
        ResetGameState();


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetGameState()
    {
        gameStarted = false;
        crawlersStarted = false;
    }
}


