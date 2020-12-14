using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSettings : MonoBehaviour
{
    [SerializeField] List<GameObject> UI_Eng;
    [SerializeField] List<GameObject> UI_Ger;

    public void EnglishUI()
    {
        foreach (var obj in UI_Ger)
           obj.SetActive(false);

        foreach (var obj in UI_Eng)
           obj.SetActive(true);
    }

    public void GermanUI()
    {
        foreach (var obj in UI_Ger)
            obj.SetActive(true);

        foreach (var obj in UI_Eng)
            obj.SetActive(false);
    }
}
