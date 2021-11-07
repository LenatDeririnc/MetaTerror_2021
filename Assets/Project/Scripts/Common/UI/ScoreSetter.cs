using System;
using TMPro;
using UnityEngine;

namespace Common.UI
{
    public class ScoreSetter : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _text;
        [SerializeField] protected string scoreNames = "Score";
        [SerializeField] protected int currentScore = 0;

        public static Action<int> UpdateScoreAction; 

        public int CurrentScore
        {
            get => currentScore;
            set
            {
                currentScore = value;
                UpdateText();
            }
        }

        private void Start()
        {
            UpdateText();
        }

        private void OnEnable()
        {
            UpdateScoreAction += SetScore;
        }

        private void OnDisable()
        {
            UpdateScoreAction -= SetScore;
        }

        private void SetScore(int value)
        {
            CurrentScore = value;
        }

        private void UpdateText()
        {
            _text.text = $"{scoreNames}: {currentScore}";
        }
    }
}