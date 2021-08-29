using UnityEngine.SceneManagement;
using UnityEngine;

public class PrescribedNumberLoadScene : MonoBehaviour
{
    public int sceneNum;
    void Update()
    {
        LoadNext();
    }

    void LoadNext()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneNum);
        }
    }
}
