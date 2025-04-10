using UnityEngine;
using UnityEngine.SceneManagement;

public class titleScreenLogic : MonoBehaviour
{
    public GameObject canvas;
    void Start()
    {
        canvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame(){
        SceneManager.LoadScene("SampleScene");
    }
}
