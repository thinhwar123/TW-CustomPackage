using System.IO;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TW.Utility.Tool
{
#if UNITY_EDITOR
    public class MergeTexture2D : Editor
    {
        private static Texture2D[] Texture2Ds { get; set; }
        private static int Row { get; set; }
        private static int Column { get; set; }
        private static Vector2Int Size { get; set; }

        [MenuItem("Assets/Merge Texture2D", true)]
        private static bool CanMergeTexture2D()
        {
            return Selection.activeObject is Texture2D;
        }

        [MenuItem("Assets/Merge Texture2D")]
        private static void DoMergeTexture2D()
        {
            SetupTexture2D();
            FindTheBestSize();
            FindTheBestRowAndColumn();
            MergerSelectedTexture();
        }
        [MenuItem("Assets/Merge Texture2D 1 Line", true)]
        private static bool CanMergeTexture2D1Line()
        {
            return Selection.activeObject is Texture2D;
        }
        [MenuItem("Assets/Merge Texture2D 1 Line")]
        private static void DoMergeTexture2D1Line()
        {
            SetupTexture2D();
            FindTheBestSize();
            Find1LineColumn();
            MergerSelectedTexture();
        }

        private static void SetupTexture2D()
        {
            // get all selected texture2d to sprite
            Texture2Ds = Selection.objects.Select(o => (o as Texture2D)).ToArray();
        }

        private static void FindTheBestSize()
        {
            int width = Texture2Ds.Max(t => t.width);
            int height = Texture2Ds.Max(t => t.height);
            Size = new Vector2Int(width, height);
        }

        private static void FindTheBestRowAndColumn()
        {
            int total = Texture2Ds.Length;
            int sqrt = Mathf.CeilToInt(Mathf.Sqrt(total));
            Row = sqrt;
            Column = sqrt;
            while (Row * Column < total)
            {
                if (Row < Column) Row++;
                else Column++;
            }
        }

        public static void MergerSelectedTexture()
        {
            int width = Size.x * Column;
            int height = Size.y * Row;

            Texture2D output = new Texture2D(width, height);
            Color[] colors = new Color[width * height];

            Texture2Ds.ForEach((t, i) =>
            {
                Color[] pixels = t.GetPixels();
                int x = i % Column;
                int y = Row - 1 - i / Column;
                pixels.ForEach((px, j) =>
                {
                    int index = (y * Size.y + j / Size.x) * width + x * Size.x + j % Size.x;
                    colors[index] = px;
                });
            });

            output.SetPixels(colors);
            output.Apply();
            SaveTexture2DAsPNG(output);
        }

        private static void SaveTexture2DAsPNG(Texture2D tex)
        {
            string defaultFileName = tex.name + ".png";
            string path = EditorUtility.SaveFilePanel("Save Texture2D As PNG", "", defaultFileName, "png");

            if (!string.IsNullOrEmpty(path))
            {
                File.WriteAllBytes(path, tex.EncodeToPNG());
            }
        }
        
        private static void Find1LineColumn()
        {
            Row = 1;
            Column = Texture2Ds.Length;
        }
    }
#endif

}