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
    private GameObject UI_Canvas;
    private Transform TF_Canvas;

    private GameObject[] number_text;
    private GameObject Start_text;
    private GameObject GameSet_text;
    private GameObject TimeUp_text;

    private Coroutine _Time_Count;

    void Awake()
    {
        UI_Canvas = GameObject.Find("Canvas_texts");
        TF_Canvas = UI_Canvas.GetComponent<RectTransform>();
        fade_panel = ob_fadeout.GetComponent<Image>();
        number_text = new GameObject[3];

        //�P�`�R�̊G���擾
        for (int i = 0; i < 3; i++)
        {
            number_text[i] = TF_Canvas.GetChild(i).gameObject;
        }
        Start_text = TF_Canvas.GetChild(3).gameObject;
        GameSet_text = TF_Canvas.GetChild(4).gameObject;
        TimeUp_text = TF_Canvas.GetChild(5).gameObject;
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //��ʂ�������t�F�[�h�C��
        yield return StartCoroutine(fadein());

        yield return new WaitForSeconds(0.5f);
        //�R�J�E���g��X�^�[�g�̕\��
        yield return StartCoroutine(Start_Count());
        //�������Ԃ̃J�E���g�X�^�[�g
        _Time_Count = StartCoroutine(Time_Count());

    }

    //���������̏��s����
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
        var wait = new WaitForFixedUpdate();//FixedUpdate�̓f�t�H���g��0.02�b���ƂɌĂ΂��
        float last_time = Time_limit;
        Time_draw();
        while (0 < Time_limit) 
        {
            yield return wait;
            Time_limit -= 0.02f;
            //��b���ƂɎ��Ԃ̕\�����]�P
            if(last_time - Time_limit >= 1)
            {
                //�������ԕ\���̕ύX
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
