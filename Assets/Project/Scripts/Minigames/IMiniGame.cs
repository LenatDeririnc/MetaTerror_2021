using System;

namespace Minigames
{
    public enum MiniGameResult
    {
        Success,
        Abort
    }
    
    public interface IMiniGame
    {
        void StartNewGame(Action<MiniGameResult> onCompleteListener);
        void AbortGame();
    }
}