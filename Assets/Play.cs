using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("Main");
    }
}
