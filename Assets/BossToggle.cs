using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossToggle : MonoBehaviour
{
    private Toggle toggle;
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnToggleValueChanged(bool isOn)
    {
        ColorBlock cb = toggle.colors;
        if (isOn)
        {
            cb.normalColor = Color.white;
            cb.selectedColor = Color.white;
        }
        else
        {
            cb.normalColor = Color.black;
            cb.selectedColor = Color.black;
        }
        toggle.colors = cb;
    }
}
