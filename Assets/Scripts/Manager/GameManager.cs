using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // GAME WIN
    public GameObject gameWinScreen;
    bool gameWon = false;
    public string Map;
    public void GameWin()
    {
        if(gameWon == false)
        {
            gameWon = true;
            print("Game Win!");
            winScreen();
        }
    }
    void winScreen()
    {
        gameWinScreen.SetActive(true);
    }

    // GAME OVER
    public GameObject gameOverScreen;
    bool gameEnded = false;
    public void EndGame()
    {
        if(gameEnded == false)
        {
            gameEnded = true;
            print("Game Over");
            endScreen();
        }
    }
    void endScreen()
    {
        gameOverScreen.SetActive(true);
    }

    // Button Control
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void sendToMap()
    {
        SceneManager.LoadScene(Map);
    }
}
