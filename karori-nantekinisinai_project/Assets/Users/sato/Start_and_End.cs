using System.Collections;
using System.Collections.Generic;
using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class Start_and_End : MonoBehaviour
{
    public GameObject ob_fadeout;
    
    public static int win;//１：1P勝利、２：2P勝利、３：時間切れ

    [SerializeField] private float FadeSpeed;
    [SerializeField] private float Time_limit;//ゲームの制限時間、単位は秒
    [SerializeField] private AudioSource SE_evolution;
    [SerializeField] private AudioSource SE_fall;
    [SerializeField] private AudioSource SE_count;
    [SerializeField] private AudioSource SE_whistle;

    private int Player1_size;// 0:S、1:M、2:L
    private int Player2_size;// 0:S、1:M、2:L
    private GameObject Player1;
    private GameObject Player2;
    private GameObject Hit1;
    private GameObject Hit2;
    private player player1;//プレイヤー１のスクリプト
    private player2 player2;//プレイヤー２のスクリプト
    private Hit hit1;//１P食べるスクリプト
    private Hit2 hit2;//２P食べるスクリプト
    private Image fade_panel;
    private Sprite sprite;//制限時間表示用
    private Sprite size_sprite;//プレイヤーのサイズ表示用
    private GameObject UI_Canvas_texts;
    private Transform TF_Canvas_texts;
    private AudioSource AudioSource;

    private GameObject[] number_text;
    private GameObject gal_size;
    private GameObject seiso_size;
    private GameObject Start_text;
    private GameObject GameSet_text;
    private GameObject TimeUp_text;
    private GameObject Minutes;//残り時間の分
    private GameObject Seconds_10;//残り時間の秒の１０の位
    private GameObject Seconds_1;//残り時間の秒の１の位

    private Coroutine _Time_Count;

    void Awake()
    {
        win = 0;
        //プレイヤーのスクリプト取得
        Player1 = GameObject.FindWithTag("Player1");
        player1 = Player1.GetComponent<player>();
        Player2 = GameObject.FindWithTag("Player2");
        player2 = Player2.GetComponent<player2>();
        //食べるスクリプト取得
        Hit1 = GameObject.Find("Hit_ground");
        hit1 = Hit1.GetComponent<Hit>();
        Hit2 = GameObject.Find("Hit_ground_2");
        hit2 = Hit2.GetComponent<Hit2>();

        UI_Canvas_texts = GameObject.Find("Canvas_UI");
        TF_Canvas_texts = UI_Canvas_texts.GetComponent<RectTransform>();
        fade_panel = ob_fadeout.GetComponent<Image>();
        AudioSource = GetComponent<AudioSource>();

        number_text = new GameObject[3];

        //１～３の絵を取得
        for (int i = 0; i < 3; i++)
        {
            number_text[i] = TF_Canvas_texts.GetChild(i).gameObject;
        }
        gal_size = TF_Canvas_texts.GetChild(4).gameObject;
        seiso_size = TF_Canvas_texts.GetChild(5).gameObject;
        Start_text = TF_Canvas_texts.GetChild(6).gameObject;
        GameSet_text = TF_Canvas_texts.GetChild(7).gameObject;
        TimeUp_text = TF_Canvas_texts.GetChild(8).gameObject;
        Minutes = TF_Canvas_texts.GetChild(9).gameObject;
        Seconds_10 = TF_Canvas_texts.GetChild(10).gameObject;
        Seconds_1 = TF_Canvas_texts.GetChild(11).gameObject;

        ob_fadeout.SetActive(true);
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //制限時間の表示
        Time_draw();
        //画面が黒からフェードイン
        yield return StartCoroutine(fadein());

        yield return new WaitForSeconds(0.5f);
        //３カウント後スタートの表示
        yield return StartCoroutine(Start_Count());
        //プレイヤーが操作出来るように
        player1.enabled = true;
        player2.enabled = true;
        hit1.enabled = true;
        hit2.enabled = true;

        //BGMスタート
        AudioSource.Play();
        //制限時間のカウントスタート
        Time_limit--;
        _Time_Count = StartCoroutine(Time_Count());

    }

    private void Update()
    {
        //プレイヤーのサイズ表示変更
        Player_size_draw();
    }

    //落ちた時の勝敗判定
    private void OnTriggerEnter(Collider player)
    {
        SE_fall.Play();
        if(player.gameObject.CompareTag("Player1") && win == 0 )
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
        SE_count.Play();
        for (int i = 2; i > -1; i--)
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
        while (-1 < Time_limit) 
        {
            yield return wait;
            Time_limit -= 0.02f;
            //一秒ごとに時間の表示を‐１
            if(last_time - Time_limit >= 1)
            {
                //制限時間表示の変更
                Time_draw();
                last_time = Time_limit;
                if(Time_limit <= 60)
                {
                    AudioSource.pitch = 1.2f;
                }
            }
        }
        win = 3;
        StartCoroutine(GameEnd());
    }

    private void Time_draw()
    {
        double game_time = Math.Ceiling(Time_limit);
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

    private void Player_size_draw()
    {
        //ギャルのサイズ表示変更
        if(player1.force_variable >= 100 && 150 > player1.force_variable && Player1_size != 1)
        {
            Player1_size = 1;
            SE_evolution.Play();
            size_sprite = Resources.Load<Sprite>("Images/UI/UI_sizeM_left");
            gal_size.GetComponent<Image>().sprite = size_sprite;
        }
        else if(player1.force_variable >= 150 && Player1_size != 2)
        {
            Player1_size = 2;
            SE_evolution.Play();
            size_sprite = Resources.Load<Sprite>("Images/UI/UI_sizeL_left");
            gal_size.GetComponent<Image>().sprite = size_sprite;
        }
        //清楚のサイズ表示変更
        if (player2.force_variable >= 100 && 150 > player2.force_variable && Player2_size != 1)
        {
            Player2_size = 1;
            SE_evolution.Play();
            size_sprite = Resources.Load<Sprite>("Images/UI/UI_sizeM_right");
            seiso_size.GetComponent<Image>().sprite = size_sprite;
        }
        else if (player2.force_variable >= 150 && Player2_size != 2)
        {
            Player2_size = 2;
            SE_evolution.Play();
            size_sprite = Resources.Load<Sprite>("Images/UI/UI_sizeL_right");
            seiso_size.GetComponent<Image>().sprite = size_sprite;
        }
    }

    IEnumerator GameEnd()
    {
        hit1.enabled = false;
        hit2.enabled = false;
        SE_whistle.Play();
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
