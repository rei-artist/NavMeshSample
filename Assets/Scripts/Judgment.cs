using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityChan;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.CinemachineBlendDefinition;

public class Judgment : MonoBehaviour
{
    private static float startTime;
    public static float time {  get { return Time.time - startTime; } }
    public static bool isClear { get; private set; }

    [SerializeField] private UnityChanControlScriptWithRgidBody player;
    [SerializeField] private TextMeshProUGUI remainEnemyText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject gameClear;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int remainEnemy = 0;
    private GUIStyle style;

    private void Awake()
    {
        startTime = Time.time;
        isClear = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        style = new GUIStyle();
        style.fontSize = 30;
        style.normal = new GUIStyleState();
        style.normal.textColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (isClear) return;

        float lastTime = time;
        if (lastTime < 0.5f) return;
        remainEnemy = GameObject.FindGameObjectsWithTag("Enemy").Count();
        if (remainEnemy == 0)
        {
            isClear = true;
            gameClear.SetActive(true);
            int scoreTime = (int)lastTime;
            float lastScore = 1000f + player.hp * 100 - scoreTime;
            scoreText.text = string.Format("Score:{0:F0} (1000 + {1:F0}[HP] - {2:F0}[Time])", lastScore, player.hp * 100, scoreTime);
        }
        remainEnemyText.text = "�c�G:" + remainEnemy;
        timeText.text = string.Format("{0:N1}", lastTime);
    }
}
