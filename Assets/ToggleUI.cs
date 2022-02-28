using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ToggleUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    bool i;

    void UIToggle(bool i, object CanvasGroup) {
        canvasGroup = GetComponent<CanvasGroup>();
        if (i == true)
        {
            canvasGroup.alpha = 1;
        }
        else canvasGroup.alpha = 0;
    }
void Start()
    {

    }
}