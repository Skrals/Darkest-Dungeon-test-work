using UnityEngine;
using Spine.Unity;
using System.Threading.Tasks;

public enum AnimationState { Idle, Attack, DoubleAttack, Damaged }

public class Animations : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    [SerializeField] private AnimationReferenceAsset _idle, _attak, _doubleAttack, _damageg;
    [SerializeField] private AnimationState _animationState = 0;

    // Start is called before the first frame update
    private void Start()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
        _animationState = AnimationState.Idle;
        SetCharacterState(_animationState);

    }

    private void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(_animationState))
        {
            return;
        }
        _skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    public async void SetCharacterState(AnimationState state)
    {
        await Task.Delay(500);

        switch (state)
        {
            case AnimationState.Idle:
                SetAnimation(_idle, true, 1f);
                break;

            case AnimationState.Attack:
                SetAnimation(_attak, false, 1f);
                break;

            case AnimationState.Damaged:
                SetAnimation(_damageg, false, 1f);
                break;

            case AnimationState.DoubleAttack:
                SetAnimation(_doubleAttack, false, 1f);
                break;

            default:
                SetAnimation(_idle, true, 1f);
                break;
        }
    }
}
