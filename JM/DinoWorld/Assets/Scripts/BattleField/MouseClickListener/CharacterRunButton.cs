using UnityEngine;
using UnityEngine.UI;

public class CharacterRunButton : MonoBehaviour
{
    private ChangeScene SceneChange;

    public Button yourButton;
    
    private void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(OnMouseDown);
    }

    private void OnMouseDown()
    {
        SceneChange = GameObject.Find("ChangeSceneManager").GetComponent<ChangeScene>();
        SceneChange.BattleToWorld();
    }
}
