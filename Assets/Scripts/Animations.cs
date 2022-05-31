using UnityEngine;
using Spine.Unity;
using System.Threading.Tasks;

public enum AnimationState { Idle, Attack, DoubleAttack, Damaged }

public class Animations : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    [SerializeField] private AnimationReferenceAsset _idle, _attak, _doubleAttack, _damageg;

    private void Start()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
        SetCharacterState(AnimationState.Idle);
    }

    private void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        _skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    public async void AttacksAnimations(Unit attacker, Unit target)
    {
        var attackerAnimation = attacker.GetComponent<Animations>();
        var targetAnimation = target.GetComponent<Animations>();

        attackerAnimation.SetCharacterState(AnimationState.Attack);
        await Task.Delay(1200);

        targetAnimation.SetCharacterState(AnimationState.Damaged);
        attackerAnimation.SetCharacterState(AnimationState.Idle);

        await Task.Delay(1500);
        targetAnimation.SetCharacterState(AnimationState.Idle);
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
