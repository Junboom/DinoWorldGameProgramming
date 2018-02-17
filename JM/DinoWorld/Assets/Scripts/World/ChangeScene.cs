using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    private Camera WorldCamera;
    private Camera BattleCamera;
    private Canvas BattleCanvas;

    public Scene BattleField;
    public Scene World;

    void Start()
    {
        BattleCamera = GameObject.Find("BattleCamera").GetComponent<Camera>();
        BattleCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        BattleCamera.enabled = false;
        BattleCanvas.enabled = false;
    }

    public void WorldToBattle()
    {
        // WorldCamera.enabled = false;
        BattleCamera.enabled = true;
        BattleCanvas.enabled = true;

        SceneManager.UnloadScene(World);
        SceneManager.LoadScene("BattleField");
    }

    public void BattleToWorld()
    {
        // WorldCamera.enabled = true;
        BattleCamera.enabled = false;
        BattleCanvas.enabled = false;

        SceneManager.UnloadScene(BattleField);
        SceneManager.LoadScene("World");
    }
}
