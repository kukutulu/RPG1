using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerExp : MonoBehaviour
{
    public float level = 1;
    public float currentExp = 0;
    public float requiredExp = 100;
    public float expMultiplier = 1.2f;
    public Slider slider;
    public TMP_Text levelText;
    AudioManager audioManager;

    private void OnEnable()
    {
        Monster.OnMonsterDead += GainExp;
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnDisable()
    {
        Monster.OnMonsterDead -= GainExp;
    }


    private void Update()
    {
        slider.value = currentExp / requiredExp;
        levelText.text = "Level: " + level;

        if (Input.GetKeyDown(KeyCode.E))
        {
            GainExp(10);
        }
    }

    public void LevelUp()
    {
        audioManager.PlaySFX(audioManager.powerUp);
        level++;
        requiredExp = (requiredExp * expMultiplier);
        currentExp = 0;
        slider.value = currentExp / requiredExp;
        levelText.text = "Level: " + level;
        PlayerStats.Instance.LevelUp();
    }

    public void GainExp(float exp)
    {
        currentExp += exp;
        if (currentExp >= requiredExp)
        {
            LevelUp();
        }
    }


}
