using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticVerticalSize : MonoBehaviour
{

    public float childWidth = 80f;
    public float childHeigth = 40f;

    void Start()
    {
        AdjustSize();
    }

    
    void Update()
    {
        
    }

    public void AdjustSize()
    {
        Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
        size.x = childWidth;
        size.y = this.transform.childCount * childHeigth;
        this.GetComponent<RectTransform>().sizeDelta = size;
    }
}
