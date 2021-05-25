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
        UpdateFontSize();
    }

    public void UpdateFontSize()
    {
        if (_isBasicVariant && _referenceLine != null && _bodyText!=null)
        {
            _bodyText.fontSize = _referenceLine.fontSize;
        }
    }
}
