using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    public Sprite[] GamePic1;
    public Sprite[] GamePic2;
    public Image LvlPic1, LvlPic2;
    public Text lvlno;
    public Text Score;
    int coin;
    int LvlNo = 0, TtlLvl = 0;
    int[] Pos = new int[14];
    public GameObject upperHolder, lowerHolder, Prefab;

    string[] Answer = { "START", "FOOTBALL", "SANDWICH", "HOTDOG", "EARRING", "HORSESHOE", "KEYBOARD", "CUPCAKE", "HANDSHAKE", "ICEBOX", "JUMPROPE", "JELLY", "SUITCASE", "LIPSTICK", "CHECKBOOK", "WEBMAIL", "LADYBUG", "MOUTHWASH", "BEABBAG", "LAYOFF", "MOUSETRAP", "CANDYCANE", "MANHOLE", "JIGSAW" };

    string[] MixAnswer = { "SATDAURGTHRTKI", "FQODOYTPFASLBL", "WRCTSYNUIIDBHA", "DEGFHBOZPQRPOT", "RANVBIRENVHFSG", "OHESKHMSARHOEW", "BCAGKTOYBESRTD", "LPKEJCNAGKRUCO", "EGKDGANJASLHAH", "MPBICZRXEAOACE", "PNOHMTPREDZJCU", "YSEEAISXHLMLJF", "ICWURTVEHSASVE", "SUPITZMNIHLCPK", "OEKUORVPHOBCKC", "LEATAINAGPWEMB", "GFLUBYAHSDXJMT", "WSTFGHUJMOHAHV", "GRBTAYEUAINHBG", "AFQOBWYELRATFY", "PGREHSVMOXUZTA", "CANDMYCGANEJEK", "IMAHNVHAROLMUE", "AJBICGDSEAEGWH" };

    string CurAns = "", CurMix = "";


    // Start is called before the first frame update
    void Start()
    {
        LvlNo = PlayerPrefs.GetInt("LevelNo", 1);
        lvlno.text = "" + LvlNo;
        
        coin = PlayerPrefs.GetInt("Coin", 0);
        Score.text = "" + coin;

        LvlPic1.sprite = GamePic1[LvlNo - 1];
        LvlPic2.sprite = GamePic2[LvlNo - 1];

        CurAns = Answer[LvlNo - 1];
        CurMix = MixAnswer[LvlNo - 1];
       // print(CurAns);
        //print(CurMix);

        for (int i = 0; i < CurAns.Length; i++)
        {
            int tempNo = i;
            GameObject g = Instantiate(Prefab, upperHolder.transform);
            g.GetComponent<Image>().color = Color.white;
            g.tag = "upper-btn";

            g.GetComponent<Button>().onClick.AddListener(() => OnClickUpperBtn(tempNo));
        }

        for (int i = 0; i < 14; i++)
        {
            string tempStr = CurMix[i].ToString();
            int tempNo = i;
            GameObject g = Instantiate(Prefab, lowerHolder.transform);
            g.GetComponent<Image>().color = Color.yellow;
            g.tag = "lower-btn";

            g.GetComponentInChildren<Text>().text = tempStr;
            g.GetComponent<Button>().onClick.AddListener(() => OnClickDownBtn(tempStr, tempNo));
        }
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
    void OnClickDownBtn(string s, int tempNo)
    {
        string cnctAns = "";
        //print(s);
        GameObject[] UpperBtns = GameObject.FindGameObjectsWithTag("upper-btn");
        GameObject[] LowerBtns = GameObject.FindGameObjectsWithTag("lower-btn");

        //print(s);
        //print(UpperBtns.Length);
        //print(tempNo);

        for (int i = 0; i < UpperBtns.Length; i++)
        {
            if (UpperBtns[i].GetComponentInChildren<Text>().text == "")
            {
                Pos[i] = tempNo;
                UpperBtns[i].GetComponentInChildren<Text>().text = s;
                LowerBtns[tempNo].GetComponentInChildren<Text>().text = "";
                LowerBtns[tempNo].GetComponent<Button>().interactable = false;
                break;
            }
        }

        LvlNo = PlayerPrefs.GetInt("LevelNo", 1);
        for (int i = 0; i < UpperBtns.Length; i++)
        {
            cnctAns += UpperBtns[i].GetComponentInChildren<Text>().text;
        }
       // print(cnctAns);

        if (cnctAns == Answer[LvlNo-1])
        {
            Debug.Log("Ans is True");

            if (TtlLvl <= LvlNo)
            {
                PlayerPrefs.SetInt("TotalLevel", LvlNo);
            }

            LvlNo++;
            PlayerPrefs.SetInt("LevelNo", LvlNo);

            coin = PlayerPrefs.GetInt("Coin", 0);
            if(TtlLvl<=LvlNo)
            {
                if (!PlayerPrefs.HasKey("hint_" + (LvlNo)))
                {
                    coin += 20;
                    PlayerPrefs.SetInt("Coin", coin);
                }
            }

            PlayerPrefs.SetString("CnctAns", cnctAns);

            SceneManager.LoadScene("Win");
        }
        else
        {
            Debug.Log("Try Again");
        }
    }

    void OnClickUpperBtn(int tempNo)
    {

        GameObject[] UpperBtns = GameObject.FindGameObjectsWithTag("upper-btn");
        GameObject[] LowerBtns = GameObject.FindGameObjectsWithTag("lower-btn");

        int dwnBtn = Pos[tempNo];

        LowerBtns[dwnBtn].GetComponentInChildren<Text>().text = UpperBtns[tempNo].GetComponentInChildren<Text>().text;
        UpperBtns[tempNo].GetComponentInChildren<Text>().text = "";
        LowerBtns[dwnBtn].GetComponent<Button>().interactable = true;
    }

    public void onClickHint()
    {
        coin = PlayerPrefs.GetInt("Score", 0);

        //if (coin >= 40)
        //{
        //    coin -= 40;
        //    PlayerPrefs.SetInt("Score", coin);

        //    PlayerPrefs.SetInt("hint_" + LvlNo, LvlNo);
        //}
    }

    public void OnClickLvlBtn()
    {
        SceneManager.LoadScene("Level");
    }
}
