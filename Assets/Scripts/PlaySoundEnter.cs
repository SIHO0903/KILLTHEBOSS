using UnityEngine;

public class PlaySoundEnter : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;
    [SerializeField,Range(0,1)] private float volume = 1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.instance.PlaySound(sound, volume);
        //Debug.Log("animator.angularVelocity : " + animator.angularVelocity);
        //Debug.Log("GetComponent : " + animator.GetComponent<PlayerController>() == null);
        //Debug.Log("animator.velocity : " + animator.velocity);
        //Debug.Log("stateInfo : " + stateInfo.ToString());
        //Debug.Log("layerIndex : " + layerIndex);
    }
}
