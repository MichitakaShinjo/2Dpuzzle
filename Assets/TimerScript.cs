using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TimerScript : MonoBehaviour
{

    public Text timeText;
    float totalTime = 30;
    int retime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        totalTime -= Time.deltaTime;
        retime = (int)totalTime;
        timeText.text = retime.ToString();
        if(retime == 0)
        {
            SceneManager.LoadScene("Result");
        }
    }
}
