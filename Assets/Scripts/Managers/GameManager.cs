using System.Globalization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private SceneManagerScript sceneManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            sceneManager.LoadScene("Hilo");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sceneManager.LoadScene("Plinko");
        }
    }
}
