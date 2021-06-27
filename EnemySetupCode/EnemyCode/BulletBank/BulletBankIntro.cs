using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Planetside
{

    [RequireComponent(typeof(GenericIntroDoer))]
    public class BulletBankIntro : SpecificIntroDoer
    {

        public bool m_finished;
        public AIActor m_AIActor;
        public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
        {
            GameManager.Instance.StartCoroutine(PlaySound());
        }
        private IEnumerator PlaySound()
        {   
            yield return StartCoroutine(WaitForSecondsInvariant(3.6f));
            //AkSoundEngine.PostEvent("Play_ENV_time_shatter_01", base.aiActor.gameObject);
            //AkSoundEngine.PostEvent("Play_ENM_bombshee_scream_01", base.aiActor.gameObject);
            
            yield break;
        }

        private IEnumerator WaitForSecondsInvariant(float time)
        {
            for (float elapsed = 0f; elapsed < time; elapsed += GameManager.INVARIANT_DELTA_TIME) { yield return null; }
            yield break;
        }
    }
}