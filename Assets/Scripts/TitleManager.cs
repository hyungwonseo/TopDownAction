using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject startButton;      //스타트 버튼
    public GameObject continueButton;   //이어하기 버튼

    Button startBtn;
    Button continueBtn;

    // Start is called before the first frame update
    void Start()
    {
        startBtn = startButton.GetComponent<Button>();
        startBtn.onClick.AddListener(StartButtonClicked);
        continueBtn = continueButton.GetComponent<Button>();
        continueBtn.onClick.AddListener(ContinueButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonClicked()
    {
        SceneManager.LoadScene("WorldMap");
    }

    public void ContinueButtonClicked()
    {

    }
}
