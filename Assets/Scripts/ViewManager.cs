using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class ViewManager : MonoBehaviour
{
    [SerializeField] protected GameObject buttonPrefab;
    [HideInInspector] public string title;                        // Navigation View에 표시할 타이틀
    [HideInInspector] public SCButton leftNavgationViewButton;      // Navigation View에 표시할 타이틀
    [HideInInspector] public SCButton rightNavgationViewButton;     // Navigation View에 표시할 타이틀
    [HideInInspector] public MainManager mainManager;

    Animator animator;

    public void Close()
    {
        GetComponent<Animator>().SetTrigger(Constant.kViewManagerClose);
    }

    public void Open(bool isAnimated = false)
    {
        if (isAnimated)
        {
            GetComponent<Animator>().SetTrigger(Constant.kViewManagerOpen);

        }
    }

    public void DestroyViewManager()
    {
        Destroy(gameObject);
    }
}
