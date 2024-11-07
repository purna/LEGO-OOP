using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.LEGO.Minifig
{

    public class MinifigFaceAnimationController : MonoBehaviour
    {
        public enum FaceAnimation
        {
            Accept,
            Blink,
            BlinkTwice,
            Complain,
            Cool,
            Dissatisfied,
            Doubtful,
            Frustrated,
            Laugh,
            Mad,
            Sleepy,
            Smile,
            Surprised,
            Wink
        }

        [Serializable]
        class AnimationData
        {
            public Texture2D[] textures;
        }

        
        
         #if UNITY_EDITOR
        [Header("Animation")]
       #endif

       
        [SerializeField]
        [Tooltip("This is the tag name of your face object. Drag the prefab onto the scene to select the face oject and assign a tag 'Face'.")]
        private string faceTag = "Face"; // Tag of the child object you want to find


        [HideInInspector,SerializeField]
        private Transform face;
        [SerializeField]
        [Tooltip("The default texture of the minifigure")]
        Texture2D defaultTexture;
         [SerializeField]
        List<FaceAnimation> animations = new List<FaceAnimation>();
         [SerializeField]
        List<AnimationData> animationData = new List<AnimationData>() ;

        Material faceMaterial;

        bool playing;

        AnimationData currentAnimationData;
         float currentFrame;
         int showingFrame;
   
        float framesPerSecond;

    
         int shaderTextureId;
        public void Init(Transform face, Texture2D defaultTexture)
        {
       
            this.face = face;
            this.defaultTexture = defaultTexture;
        }

        public void AddAnimation(FaceAnimation animation, Texture2D[] textures)
        {
            if (!animations.Contains(animation))
            {
                animations.Add(animation);
                var animationData = new AnimationData();
                animationData.textures = textures;
                this.animationData.Add(animationData);
            }
            else
            {
                Debug.LogErrorFormat("Face animation controller already contains animation {0}", animation);
            }
        }

        public bool HasAnimation(FaceAnimation animation)
        {
            return animations.IndexOf(animation) >= 0;
        }

        public void PlayAnimation(FaceAnimation animation, float framesPerSecond = 24.0f)
        {
            var animationIndex = animations.IndexOf(animation);
            if (animationIndex < 0)
            {
                Debug.LogErrorFormat("Face animation controller does not contatin animation {0}", animation);
                return;
            }

            if (framesPerSecond <= 0.0f)
            {
                Debug.LogError("Frames per second must be positive");
                return;
            }

            playing = true;
            currentAnimationData = animationData[animationIndex];
            currentFrame = 0.0f;
            showingFrame = -1;
            this.framesPerSecond = framesPerSecond;

        }

        void Start()
        {
            
            //FindAndAssignGrandchild();

            FindAndAssignGrandchildByTag();

            // Use the script's own transform as the starting point
            //Transform rootObject = transform;

            // Find the descendant called "Face" under the root object
            //face = rootObject.Find("Face");

            if (face != null)
            {

                Debug.Log("Found object: " + face.gameObject.name);

                Renderer faceRenderer = face.GetComponent<Renderer>();

                // Check if the Renderer component was found
                if (faceRenderer != null)
                {
                    // Get the material of the Renderer component
                    faceMaterial = faceRenderer.material;

                    // Log the name of the found object and the material
                    Debug.Log("Found object: " + face.gameObject.name);
                    Debug.Log("Material: " + faceMaterial.name);
                }
                else
                {
                    Debug.LogError("Renderer component not found on 'Face' object!");
                }
            }
            
            
            //faceMaterial = face.GetComponent<Renderer>().material;
            shaderTextureId = Shader.PropertyToID("_BaseMap");
        }

        // Update is called once per frame
        void Update()
        {
            if (playing)
            {
                currentFrame += Time.deltaTime * framesPerSecond;

                var wholeFrame = Mathf.FloorToInt(currentFrame);
                if (wholeFrame != showingFrame)
                {
                    if (wholeFrame >= currentAnimationData.textures.Length)
                    {
                        faceMaterial.SetTexture(shaderTextureId, defaultTexture);
                        playing = false;
                    }
                    else
                    {
                        faceMaterial.SetTexture(shaderTextureId, currentAnimationData.textures[wholeFrame]);
                        showingFrame = wholeFrame;
                    }
                }

            }
        }


        // Make the 'animations' property accessible from other scripts
        public void PlayAllAnimationsList()
        {
            List<MinifigFaceAnimationController.FaceAnimation> animationsList = animations;

            // Loop through the animationList 
           foreach (var animation in animationsList)
            {                
                if (animation.ToString() != null)
                {
                    //Debug.Log("Animations: " + animation.ToString() );
                    PlayAnimation(animation);
                }
            }
        }

        public void PlayAnimationsFromList(int i)
        {       
                List<MinifigFaceAnimationController.FaceAnimation> animationsList = animations;

                if (animationsList[i].ToString() != null)
                {
                    // Debug.Log("Animations: " + animationsList[i].ToString() );
                    PlayAnimation(animationsList[i]);
                }
        }


        private void FindAndAssignGrandchildByTag()
        {
            // Iterate through all children of the GameObject with the script
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                // Check if the child has the specified tag
                if (child.CompareTag(faceTag))
                {
                    // Assign the transform of the found grandchild to the serialized Transform field
                    face = child;

                    // Log the name of the found grandchild
                    Debug.Log("Found grandchild object with tag '" + faceTag + "': " + child.gameObject.name);

                    // Stop searching as want only want to find the first matching child
                    break;
                }
            }

            if (face == null)
            {
                Debug.LogError("Grandchild object with tag '" + faceTag + "' not found!");
            }
        }
        
    } 

}