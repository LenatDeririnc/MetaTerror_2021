using System;
using System.Linq;
using UnityEngine;

namespace Minigames
{
    public enum MiniGameType
    {
        Generic,
        Speaker
    }
    
    public class MiniGameManager : MonoBehaviour
    {
        public static MiniGameManager Instance { get; private set; }
        
        [SerializeField]
        private MiniGame[] games;

        private IMiniGame currentMiniGame;

        private void Awake()
        {
            Instance = this;
        }

        public void StartNewGame(MiniGameType type, Action<MiniGameResult> result)
        {
            AbortCurrentGame();
            
            currentMiniGame = games.First(g => g.type == type).game;
            currentMiniGame.StartNewGame(r =>
            {
                currentMiniGame = null;
                result?.Invoke(r);
            });
        }

        public void AbortCurrentGame()
        {
            currentMiniGame?.AbortGame();
        }

        [Serializable]
        private struct MiniGame
        {
            public MiniGameType type;
            public MiniGameBase game;
        }
    }
}