using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsExample : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("PlayerScore"))
        {
            // 정수 저장
            PlayerPrefs.SetInt("PlayerScore", 100);
        }
        // 정수 로딩
        int score = PlayerPrefs.GetInt("PlayerScore");

        if (!PlayerPrefs.HasKey("PlayerDefense"))
        {
            // 실수 저장
            PlayerPrefs.SetFloat("PlayerDefense", 75.5f);
        }
        // 실수 로딩
        float defense = PlayerPrefs.GetFloat("PlayerDefense");

        if (!PlayerPrefs.HasKey("PlayerName"))
        {
            // 문자열 저장
            PlayerPrefs.SetString("PlayerName", "Player");
        }
        // 문자열 로딩
        string name = PlayerPrefs.GetString("PlayerName");

        Debug.Log("PlayerScore " + score);
        Debug.Log("PlayerDefense " + defense);
        Debug.Log("PlayerName " + name);

        // 키 삭제
        //PlayerPrefs.DeleteKey("PlayerScore");
        //PlayerPrefs.DeleteKey("PlayerDefense");
        //PlayerPrefs.DeleteKey("PlayerName");

        //Debug.Log("Data Deleted");
    }
}
