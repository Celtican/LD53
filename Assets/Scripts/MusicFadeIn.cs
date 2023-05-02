using System;
using DG.Tweening;
using UnityEngine;

public class MusicFadeIn : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.DOFade(0.3f, 1);
    }
}