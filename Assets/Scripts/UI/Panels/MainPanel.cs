﻿using Peque;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainPanel : MonoBehaviour
{
    public VideoPlayer video;
    public RawImage backgroundImage;

    public VideoClip[] videos;

    private void Start() {
        StartCoroutine(playBackgroundVideo());
    }

    public void play() {
        Menu.Instance.showPanel("LoadingPanel");
        SceneManager.LoadScene("DemoScene");
    }
    public void playTest() {
        Menu.Instance.showPanel("LoadingPanel");
        SceneManager.LoadScene("Test");
    }

    public void openGithub() {
        Application.OpenURL("https://github.com/tetreum/carroamer");
    }

    public void exit() {
        Application.Quit();
    }

    IEnumerator playBackgroundVideo() {
        video.clip = videos[Random.Range(0, videos.Length)];
        video.Play();

        yield return new WaitForSeconds(2);

        backgroundImage.gameObject.SetActive(false);
    }
}
