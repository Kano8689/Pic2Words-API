using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class DataBase : MonoBehaviour
{
    public GameObject cateObj, pzlObj, plyObj, winObj; 
    public GameObject Prefab, puzzleImage, ansBtnPrefab, userAnsBtnPrefab;
    public Transform cateParent, pzlParent, upperHolder, lowerHolder, winnerImg;
    public Sprite lvlImg;
    public GameObject[] Btn = new GameObject[12];
    public GameObject[] cateBtn = new GameObject[12];
    public GameObject[] ansBtn = new GameObject[14];
    public GameObject[] userAnsBtn = new GameObject[14];
    int[] Position = new int[14];
    int cnt = 0;
    int levelNo = 1;
    int maxLevel = 1;

    int[] Pos = new int[14];
    string[] alphabetAry = new string[26];
    //char[] word1 = new char[14];

    // Start is called before the first frame update
    void Start()
    {
        pzlObj.SetActive(false);
        plyObj.SetActive(false);
        cateObj.SetActive(true);
        winObj.SetActive(false);

        levelNo = PlayerPrefs.GetInt("LevelNo", 1);
        maxLevel = PlayerPrefs.GetInt("MaxLevel", 1);

        int j = 0;
        for(int i=65; i<=90; i++)
        {
            char c = Convert.ToChar(i);
            //Debug.Log(c);
            alphabetAry[j] = c.ToString();
            j++;
        }

        StartCoroutine(getData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator getData()
    {
        WWW web = new WWW("http://localhost:3000/View_Category");
        yield return web;

        JSONArray jsonarray = (JSONArray)JSON.Parse(web.text);

        //Debug.Log("jsonarray count = " + jsonarray.Count);
        for(int i=0;i<jsonarray.Count;i++)
        {
            cateBtn[i] = Instantiate(Prefab, cateParent);

            cateBtn[i].transform.GetChild(0).GetComponent<Text>().text = jsonarray[i]["cate_name"];
            string str = jsonarray[i]["_id"];
            WWW cateImg = new WWW("http://localhost:3000/images/" + jsonarray[i]["image"]);
            yield return cateImg;
            Texture2D texture = cateImg.texture;
            cateBtn[i].transform.GetChild(1).GetComponent<RawImage>().texture = texture;
            cateBtn[i].GetComponent<Button>().onClick.AddListener(() => onClick(str));
            cateBtn[i].GetComponent<Button>().onClick.AddListener(() => levelMAnager(str));
        }
    }

    void onClick(string  str)
    {
       // Debug.Log("onClick str = "+str);
        StartCoroutine(getPuzzle(str));
    }

    IEnumerator getPuzzle(string str) //650b26ed1e068a1dea1c9c34
    {
        cateObj.SetActive(false);
        pzlObj.SetActive(true);

        WWW web = new WWW("http://localhost:3000/Get_Puzzle/"+str);
        yield return web;

        JSONArray jsonarray = (JSONArray)JSON.Parse(web.text);
        //Debug.Log(str);


        //Debug.Log("jsonarray count = " + jsonarray.Count);
        for (int i = 0; i < jsonarray.Count; i++)
        {
            Btn[i] = Instantiate(Prefab, pzlParent);
            Btn[i].GetComponent<Button>().interactable = false;

            Btn[i].transform.GetChild(0).GetComponent<Text>().text = jsonarray[i]["cate_name"];
            string pzl_id = jsonarray[i]["_id"];
            WWW cateImg = new WWW("http://localhost:3000/images/" + jsonarray[i]["image"]);
            yield return cateImg;
            Texture2D texture = cateImg.texture;
            Btn[i].transform.GetChild(1).GetComponent<RawImage>().texture = texture;
            Btn[i].GetComponent<Button>().onClick.AddListener(() => singlePuzzle(pzl_id));
        }

        int lvl = PlayerPrefs.GetInt("Level_" + str, 1);

        for (int i = 0; i <lvl; i++)
        {
            //Btn[i].transform.GetChild(2).GetComponent<Image>().sprite = lvlImg;
        }

        Debug.Log("jsonarray text = " + jsonarray.ToString());
        Level(str, jsonarray);
        //levelNo = PlayerPrefs.GetInt("LevelNo", 1);
        //Debug.Log("levelNo = " + levelNo);
        //for (int i = 0; i < levelNo; i++)
        //{
        //    Btn[i].GetComponent<Button>().interactable = true;
        //}
    }

    void singlePuzzle(string pzl_id)
    {
       // Debug.Log("pzl_id = " + pzl_id);
        StartCoroutine(getSinglePzl(pzl_id));
    }

    IEnumerator getSinglePzl(string str) 
    {
        pzlObj.SetActive(false);
        plyObj.SetActive(true);

        WWW Single_puzzle = new WWW("localhost:3000/Single_Puzzle/" + str);
        yield return Single_puzzle;
       // Debug.Log("Single puzzle = " + Single_puzzle.text);

        JSONArray jsonArray = (JSONArray)JSON.Parse(Single_puzzle.text);
        WWW pzlImg = new WWW("http://localhost:3000/images/" + jsonArray[0]["image"]);
        yield return pzlImg;
        Texture2D textureImg = pzlImg.texture;
        puzzleImage.GetComponent<RawImage>().texture = textureImg;

        wordAlternate(jsonArray);

        //  Debug.Log("hello");

        // WWW singlePzl = new WWW("http://localhost:3000/Single_Puzzle/"+str);
        // yield return singlePzl;
        //// Debug.Log("Single PZl = " + singlePzl);

        // JSONArray pzlData = (JSONArray)JSON.Parse(singlePzl.text);

        // string id = pzlData["_id"];
        // Debug.Log("pzlData id = " + id);

        // WWW pzlImg = new WWW("http://localhost:3000/images/" + pzlData["image"]);
        // yield return pzlImg;
        // Texture2D textureImg = pzlImg.texture;
        // Instantiate(pzlImgPrefab, plyParent);

        // plyParent.GetChild(0).GetComponent<RawImage>().texture = textureImg;
        // pzlImgPrefab.transform.GetComponent<RawImage>().texture = textureImg;
    }

    void wordAlternate(JSONArray json)
    {
        string word = (json[0]["pzl_word"]);
        int n = word.Length;

       // Debug.Log("word = " + word);
        //Debug.Log("Word1 = " + word1);

        for (int i = 0; i < word.Length; i++)
        {
            userAnsBtn[i] = Instantiate(userAnsBtnPrefab, upperHolder.transform);
            string chr = userAnsBtn[i].GetComponentInChildren<Text>().text;
            int temp = i;
            userAnsBtn[i].GetComponent<Button>().onClick.AddListener(() => onClickUpBtn(chr, temp, n));
        }
        
        int lessWord = 14 - n;
        //Debug.Log("lessWord = " + lessWord);

        List<string> mixWords = new List<string>();

        for(int i=0;i<word.Length;i++)
        {
            mixWords.Add(word[i].ToString());
        }
        //Debug.Log("mixWord count = " + mixWords.Count);

        for (int i = 0; i < lessWord; i++)
        {
            int rndNum = UnityEngine.Random.Range(0, alphabetAry.Length);
            mixWords.Add(alphabetAry[rndNum]);
        }

        for(int i=0;i<mixWords.Count; i++) 
        {
            int rndNum = UnityEngine.Random.Range(0, mixWords.Count);
            string temp = mixWords[rndNum];
            mixWords[rndNum] = mixWords[i];
            mixWords[i] = temp;
        }

        string Answer = string.Join("", mixWords);
        char[] mixAnswer = Answer.ToCharArray();
        //Debug.Log("mix answer length = " + mixAnswer.Length);

        if(n>7)
        {
            upperHolder.GetComponentInChildren<GridLayoutGroup>().cellSize = new Vector2(110, 110);
            upperHolder.GetComponentInChildren<GridLayoutGroup>().spacing = new Vector2(10, 10);
        }
        if(n>9)
        {
            upperHolder.GetComponentInChildren<GridLayoutGroup>().cellSize = new Vector2(93, 93);
            upperHolder.GetComponentInChildren<GridLayoutGroup>().spacing = new Vector2(5, 5);
        }
        if(n>11)
        {
            upperHolder.GetComponentInChildren<GridLayoutGroup>().cellSize = new Vector2(85, 85);
            upperHolder.GetComponentInChildren<GridLayoutGroup>().spacing = new Vector2(5, 5);
        }
        
        
        for(int i=0; i<mixAnswer.Length; i++)
        {
            ansBtn[i] = Instantiate(ansBtnPrefab, lowerHolder.transform);
            ansBtn[i].transform.GetChild(0).GetComponent<Text>().text = Answer[i].ToString();
            string chr = Answer[i].ToString();
            int temp = i;
            ansBtn[i].GetComponent<Button>().onClick.AddListener(() => onClickDownBtn(chr, temp, n, word, json));
        }
    }

    void onClickDownBtn(string chr, int temp, int n, string trueAns, JSONArray json)
    {
        //Debug.Log("trueAns = " + trueAns);
        for(int i=0; i<n; i++)
        {
            if (userAnsBtn[i].GetComponentInChildren<Text>().text == "")
            {
                Position[i] = temp;
                cnt++;
                userAnsBtn[i].GetComponentInChildren<Text>().text = chr;
                ansBtn[temp].GetComponentInChildren<Text>().text = "";
                ansBtn[temp].GetComponent<Button>().interactable = false;
                break;
            }
        }

        //Debug.Log("userAnsBtn Length = " + userAnsBtn.Length);
        //Debug.Log("cnctAns = " + cnctAns);
        //Debug.Log("hello4");
        //Debug.Log("json text = " + json.ToString());
        Win(json);
        //Debug.Log("hello5");
    }

    void Win(JSONArray json)
    {
        string cnctAns = "";
        string word = (json[0]["pzl_word"]);
        int n = word.Length;
        //Debug.Log("word in win = " + word);

        for (int i = 0; i < cnt; i++)
        {
            if (userAnsBtn[i].GetComponentInChildren<Text>().text != "")
            {
                string s = userAnsBtn[i].GetComponentInChildren<Text>().text;
                cnctAns += s;
               // Debug.Log("S = " + s);
            }
        }
        //Debug.Log("cncntAns = " + cnctAns);

        if (n == cnt)
        {
            //Debug.Log("cnt equal to n");
            if (cnctAns == word)
            {
                //Debug.Log("Win");
                plyObj.SetActive(false);
                winObj.SetActive(true);

                //winnerImage(json);
                levelNo++;
                if(maxLevel <= levelNo)
                {
                    maxLevel = levelNo;
                    PlayerPrefs.SetInt("maxLevel", maxLevel);
                    PlayerPrefs.SetInt("Level_" + json[0]["cate_id"], maxLevel);
                }
                PlayerPrefs.SetInt("LevelNo", levelNo);
                //Level(json[0]["cate_id"]);
                //Debug.Log("hello1");
                StartCoroutine(winnerImage(json));
                //Debug.Log("hello2");
            }
        }
    }

    IEnumerator winnerImage(JSONArray json)
    {
        string winWord = (json[0]["pzl_word"]);
        WWW winImg = new WWW("http://localhost:3000/images/" + json[0]["image"]);
        yield return winImg;
        //Debug.Log("pzl dtl = "+ winImg.text);
        Texture2D textureImg = winImg.texture;
        winnerImg.GetComponent<RawImage>().texture = textureImg;
        winnerImg.GetComponentInChildren<Text>().text = winWord; 
    }

    void Level(string str, JSONArray json)
    {
        Debug.Log(str);
        //Debug.Log("hello3");
        //int lvl = PlayerPrefs.GetInt("Level_" + str);
        //Debug.Log("lvl = " + lvl);
        int lvl = PlayerPrefs.GetInt("Level_" + str, 1);
        Debug.Log("lvl = " + lvl);
        //levelNo = PlayerPrefs.GetInt("LevelNo", 1);
        //Debug.Log("levelNo = " + levelNo);
        for(int i=0; i < lvl; i++)
        {
            Btn[i].GetComponent<Button>().interactable = true;

            if (i < (lvl-1))
            {
                Btn[i].transform.GetChild(0).GetComponent<Text>().text = "" + json[lvl-1]["pzl_word"];
            }
            //Btn[i].transform.GetChild(2).GetComponent<Image>().sprite = ;
        }
        for(int i=lvl; i<json.Count; i++)
        {
            Btn[i].transform.GetChild(3).GetComponent<Text>().text = "" + (i + 1);
            //Btn[i].GetComponent<Image>().sprite
        }
    }
    void levelMAnager(string str)
    {
        if (!PlayerPrefs.HasKey("Level_"+str))
        {
            PlayerPrefs.SetInt("Level_" + str, 1);
        }
        else
        {
            levelNo = PlayerPrefs.GetInt("Level_" + str);
        }
    }

    void onClickUpBtn(string chr, int temp, int n)
    {
        int dwnBtn = Position[temp];
        cnt--;

        ansBtn[dwnBtn].GetComponentInChildren<Text>().text = userAnsBtn[temp].GetComponentInChildren<Text>().text;
        userAnsBtn[temp].GetComponentInChildren<Text>().text = "";
        ansBtn[dwnBtn].GetComponent<Button>().interactable = true;

        //for (int i = 0; i < n; i++)
        //{
        //    if (userAnsBtn[i].GetComponentInChildren<Text>().text == "")
        //    {
        //        userAnsBtn[i].GetComponentInChildren<Text>().text = chr;
        //        ansBtn[temp].GetComponentInChildren<Text>().text = "";
        //        ansBtn[temp].GetComponent<Button>().interactable = false;
        //        break;
        //    }
        //}
    }

    public void onClickContinueBtn()
    {
        string str = "";
        int lvl = PlayerPrefs.GetInt("Level_" + str, 1);
    }

    public void onClickMainMenuBtn()
    {
        winObj.SetActive(false);
        cateObj.SetActive(true);
        //LoadS
        SceneManager.LoadScene("Database");
    }

    //void OnClickDownBtn(string s, int tempNo)
    //{
    //    string cnctAns = "";
    //    //print(s);
    //    GameObject[] UpperBtns = GameObject.FindGameObjectsWithTag("upper-btn");
    //    GameObject[] LowerBtns = GameObject.FindGameObjectsWithTag("lower-btn");

    //    //print(s);
    //    //print(UpperBtns.Length);
    //    //print(tempNo);

    //    for (int i = 0; i < UpperBtns.Length; i++)
    //    {
    //        if (UpperBtns[i].GetComponentInChildren<Text>().text == "")
    //        {
    //            UpperBtns[i].GetComponentInChildren<Text>().text = s;
    //            LowerBtns[tempNo].GetComponentInChildren<Text>().text = "";
    //            LowerBtns[tempNo].GetComponent<Button>().interactable = false;
    //            break;
    //        }
    //    }

    //    for (int i = 0; i < UpperBtns.Length; i++)
    //    {
    //        cnctAns += UpperBtns[i].GetComponentInChildren<Text>().text;
    //    }
    //}

    //void OnClickUpperBtn(int tempNo)
    //{

    //    GameObject[] UpperBtns = GameObject.FindGameObjectsWithTag("upper-btn");
    //    GameObject[] LowerBtns = GameObject.FindGameObjectsWithTag("lower-btn");

    //    int dwnBtn = Pos[tempNo];

    //    LowerBtns[dwnBtn].GetComponentInChildren<Text>().text = UpperBtns[tempNo].GetComponentInChildren<Text>().text;
    //    UpperBtns[tempNo].GetComponentInChildren<Text>().text = "";
    //    LowerBtns[dwnBtn].GetComponent<Button>().interactable = true;
    //}
}
