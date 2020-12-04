using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    private static SoundScript instance = null;
    
    // Start is called before the first frame update
    void Start()
    {
     
    }
  
    public static SoundScript Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if(instance != null && instance !=this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
