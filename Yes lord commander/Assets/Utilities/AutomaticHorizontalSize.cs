using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticHorizontalSize : MonoBehaviour
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
        size.x = this.transform.childCount * childWidth;
        size.y = childHeigth;
        this.GetComponent<RectTransform>().sizeDelta = size;
    }
}
