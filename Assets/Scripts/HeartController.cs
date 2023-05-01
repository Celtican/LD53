﻿using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HeartController : MonoBehaviour
{
    public static HeartController instance;

    public UnityEvent onLose;

    public Image hurtTexture;
    
    [SerializeField] private Image[] heartImages;
    private int numHearts;

    private bool damagedThisFrame = false;

    private void Awake()
    {
        instance = this;
        numHearts = heartImages.Length;
        onLose.AddListener(() => PauseController.instance.Pause());
    }

    private void Update()
    {
        damagedThisFrame = false;
    }

    public void LoseHeart()
    {
        if (!IsAlive() || damagedThisFrame) return;

        damagedThisFrame = true;
        numHearts--;
        heartImages[numHearts].color = Color.clear;

        hurtTexture.color = Color.white;
        hurtTexture.DOFade(0, 2).SetUpdate(true);
        
        if (!IsAlive()) onLose.Invoke();
        else
        {
            DestroyAllEnemies();
            WaveController.instance.RestartWave(false);
        }
    }

    private void DestroyAllEnemies()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            Destroy(enemies[i].gameObject);
        }
    }

    public bool IsAlive()
    {
        return numHearts >= 1;
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}