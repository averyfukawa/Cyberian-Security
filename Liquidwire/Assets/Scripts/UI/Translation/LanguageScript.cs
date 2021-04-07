using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageScript : MonoBehaviour
{
    public enum Languages
    {
        Nederlands,
        English
    }

    public Languages languages;

    public int LanguageNumber()
    {
        return System.Enum.GetValues(typeof(Languages)).Length;
    }

}
