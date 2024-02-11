using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Level : MonoBehaviour
{
    public Sprite tick;
    public Button[] BtnAry;
    int LvlNo = 0;
    int TtlLvl = 0;
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void OnEnable()
    {
        LvlNo = PlayerPrefs.GetInt("LevelNo", 1);
        TtlLvl = PlayerPrefs.GetInt("TotalLevel", 0);

        for (int i=0;i<=TtlLvl;i++)
        {
            BtnAry[i].interactable = true;
            BtnAry[i].GetComponentInChildren<Text>().text = (i + 1).ToString();

            if(i<TtlLvl)
            {
                BtnAry[i].GetComponent<Image>().sprite = tick;
            }
            else
            {
                BtnAry[i].GetComponent<Image>().sprite = null;
            }
        }

    }
    public void OnLevelNoClick(int n)
    {
        Debug.Log(n);
        PlayerPrefs.SetInt("LevelNo", n);

        SceneManager.LoadScene("Play");
    }
}
