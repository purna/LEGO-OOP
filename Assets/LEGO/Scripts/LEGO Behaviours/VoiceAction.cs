using UnityEngine;
using Unity.LEGO.Minifig;
using System.Collections.Generic;

namespace Unity.LEGO.Behaviours.Actions
{
    public class VoiceAction : MonoBehaviour
    {
        [SerializeField]
        public MinifigVoiceAnimationController voiceAnimationController;

        [SerializeField]
        private int selectedAnimationIndex = 0;

        private List<VoiceAnimation> availableVoiceAnimations = new List<VoiceAnimation>();

        private void OnEnable()
        {
            if (voiceAnimationController)
            {
                availableVoiceAnimations = voiceAnimationController.GetAvailableVoiceAnimations();
            }
        }

        public void SelectAnimationToPlay(int index)
        {
            if (index >= 0 && index < availableVoiceAnimations.Count)
            {
                selectedAnimationIndex = index;
                Debug.Log($"Selected voice animation: {availableVoiceAnimations[selectedAnimationIndex].animationClip.name}");
            }
        }

        public void TriggerVoiceAnimation()
        {
            if (voiceAnimationController && selectedAnimationIndex >= 0 && selectedAnimationIndex < availableVoiceAnimations.Count)
            {
                voiceAnimationController.PlayVoiceAnimation(availableVoiceAnimations[selectedAnimationIndex]);
            }
        }
    }
}
