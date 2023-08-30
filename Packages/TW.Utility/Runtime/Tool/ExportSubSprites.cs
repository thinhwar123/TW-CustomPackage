using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TW.Utility.Tool
{

    #if UNITY_EDITOR
    public class ExportSubSprites : Editor
    {
        [MenuItem("Assets/Export Sub-Sprites")]
        public static void DoExportSubSprites()
        {
            string folder = EditorUtility.OpenFolderPanel("Export sub sprites into what folder?", "", "");
            foreach (Object obj in Selection.objects)
            {
                Sprite sprite = obj as Sprite;
                if (sprite == null) continue;
                // Cache isReadable
                bool isReadable = sprite.texture.isReadable;
                // Change isReadable = true 
                string path = AssetDatabase.GetAssetPath(sprite.texture);
                TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(path);
                textureImporter.isReadable = true;
                textureImporter.SaveAndReimport();


                Texture2D extracted = ExtractAndName(sprite);
                SaveSubSprite(extracted, folder);

                // Return isReadable
                textureImporter.isReadable = isReadable;
                textureImporter.SaveAndReimport();
            }

        }
        [MenuItem("Assets/Export Sub-Sprites", true)]
        private static bool CanExportSubSprites()
        {
            return Selection.activeObject is Sprite;
        }
        // Since a sprite may exist anywhere on a tex2d, this will crop out the sprite's claimed region and return a new, cropped, tex2d.
        private static Texture2D ExtractAndName(Sprite sprite)
        {
            // TODO: Sprite with, height;
            //var output = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height);
            //Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height);
            //Color[] colors = new Color[(int)sprite.textureRect.width * (int)sprite.textureRect.height];
            // TODO: Sprite rect
            Texture2D output = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height);
            Color[] colors = new Color[(int)sprite.rect.width * (int)sprite.rect.height];

            for (int i = 0; i < pixels.Length; i++)
            {
                colors[i] = pixels[i];
            }
            //Debug.Log((int)sprite.textureRect.width + "==" + (int)sprite.textureRect.width);

            output.SetPixels(colors);
            output.Apply();
            output.name = sprite.texture.name + " " + sprite.name;
            return output;
        }
        private static void SaveSubSprite(Texture2D tex, string saveToDirectory)
        {
            if (!System.IO.Directory.Exists(saveToDirectory)) System.IO.Directory.CreateDirectory(saveToDirectory);
            System.IO.File.WriteAllBytes(System.IO.Path.Combine(saveToDirectory, tex.name + ".png"), tex.EncodeToPNG());
        }
    }
    #endif 
}