using System.Collections;
using UnityEngine;

namespace CutsceneSystem
{
    public class CutsceneFxUI : MonoBehaviour
    {
        [Header("설정")]
        public PanelTransitionType transitionType = PanelTransitionType.FadeIn;
        public float transitionDuration = 0.5f;
        public float lifeTime = 2.0f; // 연출 후 자동 제거 시간

        private TransitionEffect transition;

        private void Awake()
        {
            transition = GetComponent<TransitionEffect>();
        }

        private void Start()
        {
            StartCoroutine(PlayAndDestroy());
        }

        private IEnumerator PlayAndDestroy()
        {
            if (transition != null)
                yield return transition.Play(transitionType, transitionDuration);

            // 지정 시간 후 제거
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }
    }
}