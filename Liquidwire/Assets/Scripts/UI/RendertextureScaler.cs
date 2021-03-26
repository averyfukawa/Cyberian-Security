using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendertextureScaler : MonoBehaviour
{
    [SerializeField] private RenderTexture _target;
    void Start()
    {
        _target.height = Screen.height;
        _target.width = Screen.width;
    }
}
