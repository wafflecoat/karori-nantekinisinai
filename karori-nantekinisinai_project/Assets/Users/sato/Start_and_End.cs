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
    
    public static int win = 0;//�P�F1P�����A�Q�F2P�����A�R�F���Ԑ؂�

    [SerializeField] private float FadeSpeed;
    [SerializeField] private float Time_limit;//�Q�[���̐������ԁA�P�ʂ͕b

    private Image fade_panel;
    private Sprite sprite;
    private GameObject UI_Canvas_texts;
    private Transform TF_Canvas_texts;
    private AudioSource AudioSource;

    private GameObject[] number_text;
    private GameObject Start_text;
    private GameObject GameSet_text;
    private GameObject TimeUp_text;
    private GameObject Minutes;//�c�莞�Ԃ̕�
    private GameObject Seconds_10;//�c�莞�Ԃ̕b�̂P�O�̈�
    private GameObject Seconds_1;//�c�莞�Ԃ̕b�̂P�̈�

    private Coroutine _Time_Count;

    void Awake()
    {
        UI_Canvas_texts = GameObject.Find("Canvas_UI");
        TF_Canvas_texts = UI_Canvas_texts.GetComponent<RectTransform>();
        fade_panel = ob_fadeout.GetComponent<Image>();
        AudioSource = GetComponent<AudioSource>();

        number_text = new GameObject[3];

        //�P�`�R�̊G���擾
        for (int i = 0; i < 3; i++)
        {
            number_text[i] = TF_Canvas_texts.GetChild(i).gameObject;
        }
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
        //�������Ԃ̕\��
        Time_draw();
        //��ʂ�������t�F�[�h�C��
        yield return StartCoroutine(fadein());

        yield return new WaitForSeconds(0.5f);
        //�R�J�E���g��X�^�[�g�̕\��
        yield return StartCoroutine(Start_Count());
        //BGM�X�^�[�g
        AudioSource.Play();
        //�������Ԃ̃J�E���g�X�^�[�g
        Time_limit--;
        _Time_Count = StartCoroutine(Time_Count());

    }

    //���������̏��s����
    private void OnTriggerEnter(Collider player)
    {

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
        var wait = new WaitForFixedUpdate();//FixedUpdate�̓f�t�H���g��0.02�b���ƂɌĂ΂��
        float last_time = Time_limit;
        Time_draw();
        while (-1 < Time_limit) 
        {
            yield return wait;
            Time_limit -= 0.02f;
            //��b���ƂɎ��Ԃ̕\�����]�P
            if(last_time - Time_limit >= 1)
            {
                //�������ԕ\���̕ύX
                Time_draw();
                last_time = Time_limit;
                if(Time_limit <= 30)
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
        //�������\��
        sprite = Resources.Load<Sprite>($"Images/UI/UI_number_{minutes % 10}_outline");
        Minutes.GetComponent<Image>().sprite = sprite;
        //���b�i�P�O�̈ʁj��\��
        sprite = Resources.Load<Sprite>($"Images/UI/UI_number_{seconds / 10}_outline");
        Seconds_10.GetComponent<Image>().sprite = sprite;
        //���b�i�P�̈ʁj��\��
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
