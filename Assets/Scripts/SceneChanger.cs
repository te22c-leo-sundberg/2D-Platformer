using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string n)
    {
        SceneManager.LoadScene(n);
    }
}
