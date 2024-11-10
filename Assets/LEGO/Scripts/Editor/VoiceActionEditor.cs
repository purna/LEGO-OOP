using UnityEditor;
using UnityEngine;
using Unity.LEGO.Minifig;
using Unity.LEGO.Behaviours.Actions;
using System.Linq;


namespace Unity.LEGO.EditorExt
{
    [CustomEditor(typeof(VoiceAction))]
    public class VoiceActionEditor : Editor
    {
        private VoiceAction voiceAction;
        private SerializedProperty voiceAnimationControllerProp;
        private int selectedAnimationIndex;

        private void OnEnable()
        {
            voiceAction = (VoiceAction)target;
            voiceAnimationControllerProp = serializedObject.FindProperty("voiceAnimationController");

            UpdateAvailableAnimations();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(voiceAnimationControllerProp);

            if (voiceAction.voiceAnimationController != null)
            {
                var availableAnimations = voiceAction.voiceAnimationController.GetAvailableVoiceAnimations();
                
                // Extract animation names for the Popup selection
                string[] animationNames = availableAnimations
                    .Select(a => a.animationClip != null ? a.animationClip.name : "Unnamed Animation")
                    .ToArray();

                if (animationNames.Length > 0)
                {
                    selectedAnimationIndex = EditorGUILayout.Popup("Select Animation", selectedAnimationIndex, animationNames);

                    if (GUILayout.Button("Play Selected Animation"))
                    {
                        voiceAction.SelectAnimationToPlay(selectedAnimationIndex);
                        voiceAction.TriggerVoiceAnimation();
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("No animations available in the controller.", MessageType.Warning);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Assign a Voice Animation Controller to play animations.", MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateAvailableAnimations()
        {
            if (voiceAnimationControllerProp.objectReferenceValue is MinifigVoiceAnimationController controller)
            {
                var availableAnimations = controller.GetAvailableVoiceAnimations();
                if (selectedAnimationIndex >= availableAnimations.Count)
                {
                    selectedAnimationIndex = 0;
                }
            }
        }
    }
}
