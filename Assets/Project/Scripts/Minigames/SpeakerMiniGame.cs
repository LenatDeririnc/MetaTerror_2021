using DG.Tweening;
using Minigames;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpeakerMiniGame : MiniGameBase
{
    public RectTransform jackTransform;
    public RectTransform speakerTransform;
    public Image greenZone;
        
    public float successHitDistance = 0.125f;
    public float jackMoveTimeInSeconds = 1f;

    public InputActionReference plugAction;

    public AudioSource clipAudioSource;
    public AudioClip failAudioClip;
    public AudioClip successAudioClip;

    private float speakerPosition;
    private bool isFailAnimationPlaying;
    private float jackTimer;
    
    private float JackPosition => DOVirtual.EasedValue(0f, 1f, 
        Mathf.PingPong(jackTimer / jackMoveTimeInSeconds, 1f), Ease.InOutSine);

    protected override void OnGameFinished()
    {
        // noop
    }

    protected override void OnGameStarted()
    {
        DOTween.Kill(this);
        
        speakerPosition = Random.value;
        jackTimer = 0f;
        
        var pivot = speakerTransform.pivot;
        pivot.y = speakerPosition;
        speakerTransform.pivot = pivot;
    }

    private void Update()
    {
        if (!IsGameRunning || isFailAnimationPlaying)
            return;

        jackTimer += Time.deltaTime;
        
        var pivot = jackTransform.pivot;
        pivot.x = 0.5f;
        pivot.y = JackPosition;
        jackTransform.pivot = pivot;

        var greenZoneColor = greenZone.color;
        greenZoneColor.a = IsHoveringAboveSpeakers() ? 0.5f : 0.25f;
        greenZone.color = greenZoneColor;

        if (plugAction.action.WasPressedThisFrame())
        {
            TryPlug();
        }
    }

    private bool IsHoveringAboveSpeakers()
    {
        return Mathf.Abs(JackPosition - speakerPosition) < successHitDistance;
    }

    public void TryPlug()
    {
        if (IsHoveringAboveSpeakers())
        {
            clipAudioSource.PlayOneShot(successAudioClip);
            FinishGame(MiniGameResult.Success, 0.5f);
            jackTransform.DOPivot(new Vector2(-0.5f, speakerPosition), 0.25f)
                .SetEase(Ease.InOutExpo)
                .SetTarget(this);
        }
        else
        {
            isFailAnimationPlaying = true;

            clipAudioSource.PlayOneShot(failAudioClip);
            DOTween.Sequence()
                .Append(jackTransform.DOPivotX(-2f, 0.25f).SetEase(Ease.OutCubic))
                .Append(jackTransform.DOPivotX(0.5f, 0.25f).SetEase(Ease.InOutCubic))
                .OnComplete(() => isFailAnimationPlaying = false)
                .SetTarget(this);
        }
    }
}
