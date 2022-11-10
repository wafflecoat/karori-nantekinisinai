using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Go_main : MonoBehaviour
{
    public GameObject ob_fadeout;
    [SerializeField] private float fadeSpeed;

    private Image fade_panel;
    private bool Get_submit = false;

    // Start is called before the first frame update
    void Start()
    {
        fade_panel = ob_fadeout.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") && Get_submit == false)
        {
            Get_submit = true;
            Debug.Log("Œˆ’è‚ª‰Ÿ‚³‚ê‚½");
            StartCoroutine(Game_start());
        }
    }

    IEnumerator Game_start()
    {
        yield return StartCoroutine(fadeout());
        SceneManager.LoadScene("main");
    }

    IEnumerator fadeout()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            Color color = fade_panel.color;
            color.a += fadeSpeed * Time.deltaTime;
            fade_panel.color = color;
            if (color.a >= 1) break;
        }
    }

}
