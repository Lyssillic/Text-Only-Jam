using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Selection : MonoBehaviour
{
    public Button playBtn;
    public Button controlBtn;
    public Button quitBtn;
    public Button restartBtn;
    public GameObject Controls;

    Button[] buttons;
    int btnSelectedIndex;
    Button btnSelected;
    int previousBtnSelectedIndex;
    Button previousBtnSelected;
    bool toggleControls = true;
    string sceneName;

    AudioSource audioSource;
    AudioClip button;
    AudioClip switchS;

    private void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        btnSelectedIndex = 0;
        previousBtnSelectedIndex = -1;

        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        button = (AudioClip)Resources.Load("SFX/Button", typeof(AudioClip));
        switchS = (AudioClip)Resources.Load("SFX/Switch", typeof(AudioClip));

        if (sceneName == "Start")
        {
            quitBtn.onClick.AddListener(ExitGame);
            playBtn.onClick.AddListener(StartBtn);
            controlBtn.onClick.AddListener(ControlBtn);
            buttons = new Button[] { playBtn, controlBtn, quitBtn };

            btnSelected = buttons[btnSelectedIndex];
            updateBtn();
        }
        else if (sceneName == "End")
        {
            quitBtn.onClick.AddListener(ExitGame);
            restartBtn.onClick.AddListener(RestartGame);
            buttons = new Button[] { restartBtn, quitBtn };

            btnSelected = buttons[btnSelectedIndex];
            updateBtn();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }

        if (sceneName != "Snake")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                audioSource.PlayOneShot(button, .5f);
                btnSelected.onClick.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                audioSource.PlayOneShot(switchS, .5f);
                previousBtnSelectedIndex = btnSelectedIndex;
                if (btnSelectedIndex == 0)
                {
                    btnSelectedIndex = buttons.Length - 1;
                }
                else
                {
                    btnSelectedIndex--;
                }

                updateBtn();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                audioSource.PlayOneShot(switchS, .5f);
                previousBtnSelectedIndex = btnSelectedIndex;
                if (btnSelectedIndex == buttons.Length - 1)
                {
                    btnSelectedIndex = 0;
                }
                else
                {
                    btnSelectedIndex++;
                }

                updateBtn();
            }
        }
    }

    public void updateBtn()
    {
        btnSelected = buttons[btnSelectedIndex];
        btnSelected.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Sprites/text/glowing/" + btnSelected.name, typeof(Sprite));
        if (previousBtnSelectedIndex != -1)
        {
            previousBtnSelected = buttons[previousBtnSelectedIndex];
            previousBtnSelected.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Sprites/text/" + previousBtnSelected.name, typeof(Sprite));
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartBtn()
    {
        SceneManager.LoadScene("Snake");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Start");
    }

    public void ControlBtn()
    {
        if (toggleControls)
        {
            Controls.SetActive(true);
            toggleControls = false;
        }
        else
        {
            Controls.SetActive(false);
            toggleControls = true;
        }
    }
}
