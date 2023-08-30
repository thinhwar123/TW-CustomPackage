using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TW.Utility.Tool
{
    public class TextureToPng : MonoBehaviour
    {
        [field: SerializeField] private Sprite MainSprite { get; set; }
        public void SaveImage()
        {
            byte[] bytes = DeCompress(MainSprite.texture).EncodeToPNG();
            if (DeCompress(MainSprite.texture).EncodeToPNG() == null)
            {
                Debug.Log("Null");
            }
            File.WriteAllBytes(Application.dataPath + "/Test.png", bytes);
        }

        public Texture2D DeCompress(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TextureToPng))]
    [CanEditMultipleObjects]
    public class TextureToPngEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUIContent resetPath = new GUIContent("Save Image");
            if (GUILayout.Button("Save Image"))
            {
                ((TextureToPng)target).SaveImage();
            }
        }
    }

#endif 
}