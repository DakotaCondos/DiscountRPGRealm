using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        GameManager.Instance.Players.Clear();
        SceneManager.LoadScene("Main Menu");
    }

}
