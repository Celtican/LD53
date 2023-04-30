using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float finalSize = 3;
    public float damage = 1;
    public float timeGrow = 0.2f;
    public float timeStay = 0.2f;
    public float timeShrink = 0.6f;

    private List<EnemyController> affectedEnemies = new List<EnemyController>();
    private bool isAbleToDamage = true;

    private void Start()
    {
        float currentSize = transform.localScale.x;
        transform.DOScale(finalSize, timeGrow);
        Timer.Register(timeGrow + timeStay, () =>
        {
            isAbleToDamage = false;
            Tween tween = transform.DOScale(currentSize, timeShrink);
            Timer.Register(timeShrink, () =>
            {
                Destroy(gameObject);
                tween.Kill();
            });
        });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isAbleToDamage &&
            other.transform.parent != null &&
            other.transform.parent.gameObject.TryGetComponent(out EnemyController enemy) &&
            !affectedEnemies.Contains(enemy))
        {
            enemy.TakeDamage(damage);
            affectedEnemies.Add(enemy);
        }
    }
}