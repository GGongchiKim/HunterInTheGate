using System.Collections;
using UnityEngine;

namespace CutsceneSystem
{
    public class CutsceneFxUI : MonoBehaviour
    {
        [Header("����")]
        public PanelTransitionType transitionType = PanelTransitionType.FadeIn;
        public float transitionDuration = 0.5f;
        public float lifeTime = 2.0f; // ���� �� �ڵ� ���� �ð�

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

            // ���� �ð� �� ����
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }
    }
}