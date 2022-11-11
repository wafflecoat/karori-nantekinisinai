using System.Collections;
using System.Collections.Generic;
using System;
using Unity.Collections;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Start_and_End : MonoBehaviour
{
    public GameObject ob_fadeout;
    
    public static int win = 0;//１：1P勝利、２：2P勝利、３：時間切れ

    [SerializeField] private float FadeSpeed;
    [SerializeField] private float Time_limit;//ゲームの制限時間、単位は秒

    private Image fade_panel;
    private Sprite sprite;
    private GameObject UI_Canvas_texts;
    private Transform TF_Canvas_texts;

    private GameObject[] number_text;
    private GameObject Start_text;
    private GameObject GameSet_text;
    private GameObject TimeUp_text;
    private GameObject Minutes;//残り時間の分
    private GameObject Seconds_10;//残り時間の秒の１０の位
    private GameObject Seconds_1;//残り時間の秒の１の位

    private Coroutine _Time_Count;

    void Awake()
    {
        UI_Canvas_texts = GameObject.Find("Canvas_texts");
        TF_Canvas_texts = UI_Canvas_texts.GetComponent<RectTransform>();
        fade_panel = ob_fadeout.GetComponent<Image>();
        number_text = new GameObject[3];

        //１〜３の絵を取得
        for (int i = 0; i < 3; i++)
        {
            number_text[i] = TF_Canvas_texts.GetChild(i).gameObject;
        }
        Start_text = TF_Canvas_texts.GetChild(3).gameObject;
        GameSet_text = TF_Canvas_texts.GetChild(4).gameObject;
        TimeUp_text = TF_Canvas_texts.GetChild(5).gameObject;
        Minutes = TF_Canvas_texts.GetChild(6).gameObject;
        Seconds_10 = TF_Canvas_texts.GetChild(7).gameObject;
        Seconds_1 = TF_Canvas_texts.GetChild(8).gameObject;

        ob_fadeout.SetActive(true);
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //画面が黒からフェードイン
        yield return StartCoroutine(fadein());

        yield return new WaitForSeconds(0.5f);
        //３カウント後スタートの表示
        yield return StartCoroutine(Start_Count());
        //制限時間のカウントスタート
        _Time_Count = StartCoroutine(Time_Count());

    }

    //落ちた時の勝敗判定
    private void OnTriggerEnter(Collider player)
    {

        if(player.gameObject.CompareTag("Player") && win == 0 )
        {
            win = 2;
            Debug.Log("1");
            StartCoroutine(GameEnd());
        }
        else if (player.gameObject.CompareTag("Player2") && win == 0)
        {
            win = 1;
            Debug.Log("2");
            StartCoroutine(GameEnd());
        }
    } 

   
    IEnumerator fadein()
    {
        var wait = new WaitForEndOfFrame();
        while (true)
        {
            yield return wait;
            Color color = fade_panel.color;
            color.a -= FadeSpeed * Time.deltaTime;
            fade_panel.color = color;
            if (color.a <= 0) break;
        }
    }

    IEnumerator fadeout()
    {
        var wait = new WaitForEndOfFrame();
        while (true)
        {
            yield return wait;
            Color color = fade_panel.color;
            color.a += FadeSpeed * Time.deltaTime;
            fade_panel.color = color;
            if (color.a >= 1) break;
        }
    }

    IEnumerator Start_Count()
    {
        var wait = new WaitForSeconds(1f);
        for(int i = 2; i > -1; i--)
        {
            Debug.Log($"{i}");
            number_text[i].SetActive(true);
            yield return wait;
            number_text[i].SetActive(false);
        }

        Start_text.SetActive(true);
        yield return wait;
        Start_text.SetActive(false);
    }

    IEnumerator Time_Count()
    {
        var wait = new WaitForFixedUpdate();//FixedUpdateはデフォルトで0.02秒ごとに呼ばれる
        float last_time = Time_limit;
        Time_draw();
        while (0 < Time_limit) 
        {
            yield return wait;
            Time_limit -= 0.02f;
            //一秒ごとに時間の表示を‐１
            if(last_time - Time_limit >= 1)
            {
                //制限時間表示の変更
                Time_draw();
                last_time = Time_limit;
            }
        }
        win = 3;
        StartCoroutine(GameEnd());
    }

    private void Time_draw()
    {
        double game_time = Math.Ceiling(Time_limit) - 1;
        int minutes = (int)game_time / 60;
        int seconds = (int)game_time % 60;
        Debug.Log(minutes + ":" + seconds / 10 + seconds % 10);
        //何分か表示
        sprite = Resources.Load<Sprite>($"Images/UI/UI_number_{minutes % 10}_outline");
        Minutes.GetComponent<Image>().sprite = sprite;
        //何秒（１０の位）を表示
        sprite = Resources.Load<Sprite>($"Images/UI/UI_number_{seconds / 10}_outline");
        Seconds_10.GetComponent<Image>().sprite = sprite;
        //何秒（１の位）を表示
        sprite = Resources.Load<Sprite>($"Images/UI/UI_number_{seconds % 10}_outline");
        Seconds_1.GetComponent<Image>().sprite = sprite;
    }

    IEnumerator GameEnd()
    {

        if (win == 3)
        {
            TimeUp_text.SetActive(true);
        }
        else
        {
            StopCoroutine(_Time_Count);
            GameSet_text.SetActive(true);
        }

        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(fadeout());
        SceneManager.LoadScene("Result_scene");
    }
}
