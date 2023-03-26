using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StardropTools;

public class UIManager : BaseManager
{
    [SerializeField] UIMenu[] menus;

    public override void Initialize()
    {
        base.Initialize();

        Utilities.InitializeBaseComponents(menus);
        ChangeMenu(0);
    }

    protected override void EventFlow()
    {
        GameManager.OnMainMenu.AddListener(()   => ChangeMenu(0));
        GameManager.OnPlayStart.AddListener(()  => ChangeMenu(1));
        GameManager.OnPlayEnd.AddListener(()    => ChangeMenu(2));
    }

    /// <summary>
    /// 0-Main, 1-Play, 2-End
    /// </summary>
    public void ChangeMenu(int menuID)
    {
        for (int i = 0; i < menus.Length; i++)
            menus[i].Close();

        menus[menuID].Open();
    }
}
