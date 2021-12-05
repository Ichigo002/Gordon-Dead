using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PointsModule : MonoBehaviour
{
    [SerializeField] private Animator gemAnim;
    [SerializeField] private Text gemText;

    [SerializeField] private Animator coinAnim;
    [SerializeField] private Text coinText;

    [SerializeField] private Animator heartAnim;
    [SerializeField] private Image heartImage;

    private void OnEnable()
    {
        gemText.text = 0 + "";
        coinText.text = 0 + "";
        heartImage.fillAmount = 1;
    }
    public void AddGem(int points)
    {
        gemAnim.SetTrigger("pickUp");
        gemText.text = points.ToString();
    }
    public void AddCoin(int points)
    {
        coinAnim.SetTrigger("pickUp");
        coinText.text = points.ToString();
    }
    public void LoseHeart(float health, float maxHp)
    {
        heartAnim.SetTrigger("Lose");
        health /= maxHp;
        heartImage.fillAmount = health;
    }
    public void RegenerateHeart(float health, float maxHp)
    {
        health /= maxHp;
        heartImage.fillAmount = health;
    }
}
