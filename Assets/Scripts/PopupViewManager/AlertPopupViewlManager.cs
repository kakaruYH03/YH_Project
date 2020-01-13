using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertPopupViewlManager : PopupViewManager
{
    public void OnClickOk()
    {
        NavigationManager manager = GameObject.Find("Canvas").GetComponent<NavigationManager>();

        Close();
    }
}
