using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    int LvlNo = 0;
    public GameObject winAns, Prefab;
    public Text WishBox;
    string[] Wish = { "AMAZING!", "SENSATIONAL!", "WONDERFULL!", "WEL DONE!", "UNBELIEVABLE!", "SPLENDID!", "EXCELLENT!" };

    // Start is called before the first frame update
    void Start()
    {
        string Ans = PlayerPrefs.GetString("CnctAns");
        char[] ans = Ans.ToCharArray();

        for (int i = 0; i < Ans.Length; i++)
        {
            GameObject g = Instantiate(Prefab, winAns.transform);
            g.GetComponent<Image>().color = Color.white;
            g.tag = "win-ans-btn";

            g.GetComponentInChildren<Text>().text = ""+ans[i];
        }

        int j = Random.Range(0, 8);
        WishBox.text = Wish[j];

        print("ans = "+Ans);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void onClickOnwards()
    {
        LvlNo = PlayerPrefs.GetInt("LevelNo", 1);

        SceneManager.LoadScene("Play");
    }
}
