using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Copyright (c) 2020 Alen Smajic

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

/// <summary>
/// Changes the language settings of both scenes.
/// </summary>
public class LanguageSettings : MonoBehaviour
{
    [SerializeField] List<GameObject> UI_Eng;
    [SerializeField] List<GameObject> UI_Ger;

    // Standard Language is English.
    public static string Language = "English";

    private void Start()
    {
        if (Language == "English")
        {
            EnglishUI();
        }
        else
        {
            GermanUI();
        }
    }

    /// <summary>
    /// Switches all UI-Elements to English.
    /// </summary>
    public void EnglishUI()
    {
        foreach (var obj in UI_Ger)
           obj.SetActive(false);

        foreach (var obj in UI_Eng)
           obj.SetActive(true);

        Language = "English";
    }

    /// <summary>
    /// Switches all UI-Elements to German.
    /// </summary>
    public void GermanUI()
    {
        foreach (var obj in UI_Ger)
            obj.SetActive(true);

        foreach (var obj in UI_Eng)
            obj.SetActive(false);

        Language = "German";
    }
}
