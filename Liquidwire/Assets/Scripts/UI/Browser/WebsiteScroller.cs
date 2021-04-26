using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WebsiteScroller : MonoBehaviour
{
    [SerializeField] private bool _isBasicVariant = true;
    [SerializeField] private TextMeshProUGUI _referenceLine;
    [SerializeField] private TextMeshProUGUI _bodyText;

    private void Start()
    {
        if (_isBasicVariant && _referenceLine != null && _bodyText!=null)
        {
            _bodyText.fontSize = _referenceLine.fontSize;
        }
    }
}
