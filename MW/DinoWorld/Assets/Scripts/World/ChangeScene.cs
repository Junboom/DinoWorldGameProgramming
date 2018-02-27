using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {



    public void WorldToBattle()
    {
      
        SceneManager.LoadScene("BattleField");
    }

    public void BattleToWorld()
    {
       
        SceneManager.LoadScene("World");
    }
}
