﻿using UnityEngine;
using UnityEngine.UI;

public class CharacterSkillButton : MonoBehaviour
{
    public Button yourButton;

    private CharacterState CS;

    public GameObject CharacterMenu;

    private void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(OnMouseDown);

        CS = GameObject.Find("Character").GetComponent<CharacterState>();
    }

    private void OnMouseDown()
    {
        CS.attack_or_skill = 2;
        CharacterMenu.SetActive(false);
    }
}
