using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionState : BaseAbilityMenuState
{
    public static int category;
    string[] whiteMagicOptions = new string[] { "Cure", "Raise", "Holy" };
    string[] blackMagicOptions = new string[] { "Fire", "Ice", "Lightning" };

    protected override void LoadMenu()
    {
        if (menuOptions == null)
            menuOptions = new List<string>(3);

        if (category == 0)
        {
            menuTitle = "White Magic";
            SetOptions(whiteMagicOptions);
        }
        else
        {
            menuTitle = "Black Magic";
            SetOptions(blackMagicOptions);
        }

        abilityMenuPanelController.Show(menuTitle, menuOptions);
    }

    void SetOptions(string[] options)
    {
        menuOptions.Clear();
        for (int i = 0; i < options.Length; ++i)
            menuOptions.Add(options[i]);
    }

    protected override void Confirm()
    {
        // For now, just transition to ability target state
        // In a full implementation, this would set up the specific ability
        owner.ChangeState<AbilityTargetState>();
    }

    protected override void Cancel()
    {
        owner.ChangeState<CategorySelectionState>();
    }
}