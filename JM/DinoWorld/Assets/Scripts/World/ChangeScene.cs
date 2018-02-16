using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    private Camera WorldCamera;
    private Camera BattleCamera;
    private Canvas BattleCanvas;

    void Start()
    {
        WorldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        BattleCamera = GameObject.Find("BattleCamera").GetComponent<Camera>();
        BattleCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        BattleCamera.enabled = false;
        BattleCanvas.enabled = false;
    }

    public void WorldToBattle()
    {
        WorldCamera.enabled = false;
        BattleCamera.enabled = true;
        BattleCanvas.enabled = true;

        SceneManager.LoadScene("BattleField");
    }

    public void BattleToWorld()
    {
        SceneManager.LoadScene("World");
    }
}
