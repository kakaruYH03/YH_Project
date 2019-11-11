using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovePopup : PopupViewManager
{
    public delegate void RemovePopupViewManagerDelegate();
    public RemovePopupViewManagerDelegate removePopupViewManagerDelegate;

    public void OnClickOk()
    {
        removePopupViewManagerDelegate?.Invoke();
        Close();
    }

    public void OnClickNO()
    {
        Close();
    }
}
