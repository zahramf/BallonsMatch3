using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BootStrapManager : MonoBehaviour
{
    //Dot dot;
    Button _dotBtn;

    
        
    // Start is called before the first frame update
    void Start()
    {
        _dotBtn = GetComponent<Button>();
        //dot = FindObjectOfType<Dot>();
    }

    public void Clock()
    {
        _dotBtn.onClick.AddListener(deb);

    }
    void deb()
    {
        Debug.Log("blue");
    }
    // Update is called once per frame
    void Update()
    {
       
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        //    int posX = dot.GetComponent<Dot>().targetX;
        //    Debug.Log("posX" + posX);

        //    int posY = dot.GetComponent<Dot>().targetY;
        //    Debug.Log("posY" + posY);
            
        //}
    }
}
