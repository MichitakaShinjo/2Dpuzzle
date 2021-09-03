using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    Text textObj;

    public static int score = 0;

    public static int getscore()
    {
        return score;
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int point)
    {
        score += point;
        textObj.text = "" + score;
    }
}
