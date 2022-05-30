using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Animations : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    [SerializeField] private AnimationReferenceAsset _idle;
    [SerializeField] private string _currentState;

    // Start is called before the first frame update
    private void Start()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();

        _currentState = "Idle";
        SetCharacterState(_currentState);

    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        _skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    public void SetCharacterState(string state)
    {
        if (state.Equals("Idle"))
        {
            SetAnimation(_idle, true, 1f);
        }
    }
}
