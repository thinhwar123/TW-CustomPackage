using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.U2D.Sprites;
#endif

namespace TW.Utility.Tool
{
    [CreateAssetMenu(fileName = "TMPSpriteAssetGenerator", menuName = "TW/Utility/TMPSpriteAssetGenerator")]

    public class TMPSpriteAssetGenerator : ScriptableObject
    {
        [field: SerializeField] public TMP_SpriteAsset TMPSpriteAsset { get; private set; }
        [field: SerializeField] public Vector3Int Offset { get; private set; }
        [field: SerializeField] public float Scale { get; private set; } = 1.0f;
        [field: SerializeField, ReadOnly] public Texture2D MergeTexture2D { get; private set; }

        [field: SerializeField, ReadOnly]
        public List<Sprite> SpriteInCurrentPath { get; private set; } = new List<Sprite>();

        [field: SerializeField, ReadOnly]
        public List<Texture2D> Texture2DList { get; private set; } = new List<Texture2D>();

        [field: SerializeField, ReadOnly] public string ObjectName { get; private set; }
        [field: SerializeField, ReadOnly] public string ObjectPath { get; private set; }
        [field: SerializeField, ReadOnly] public string FolderPath { get; private set; }
        [field: SerializeField, ReadOnly] public string TexturePath { get; private set; }
        [field: SerializeField, ReadOnly] public string TMPSpriteAssetPath { get; private set; }
        [field: SerializeField, ReadOnly] public Vector2Int Size { get; private set; }
        [field: SerializeField, ReadOnly] public int Row { get; private set; }
        [field: SerializeField, ReadOnly] public int Column { get; private set; }
#if UNITY_EDITOR
        [Button]
        public void GenerateSpriteAsset()
        {
            EditorUtility.SetDirty(this);


            SetupTexture2D();
            FindTheBestSize();
            FindTheBestRowAndColumn();
            MergerSelectedTexture();
            SliceTexture();
            UpdateTMPSpriteAsset();


            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void SetupTexture2D()
        {
            ObjectName = name;
            ObjectPath = AssetDatabase.GetAssetPath(this);
            FolderPath = ObjectPath[..ObjectPath.LastIndexOf('/')];

            SpriteInCurrentPath.Clear();
            Texture2DList.Clear();

            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new string[] { FolderPath });
            foreach (string guid in guids)
            {
                string spritePath = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter importer = AssetImporter.GetAtPath(spritePath) as TextureImporter;
                if (importer == null) continue;
                if (importer.spriteImportMode != SpriteImportMode.Single) continue;
                importer.isReadable = true;
                importer.SaveAndReimport();

                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                SpriteInCurrentPath.Add(sprite);
                Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(spritePath);
                Texture2DList.Add(texture2D);
            }
        }

        private void FindTheBestSize()
        {
            int width = Texture2DList.Max(t => t.width);
            int height = Texture2DList.Max(t => t.height);
            Size = new Vector2Int(width, height);
        }

        private void FindTheBestRowAndColumn()
        {
            int total = Texture2DList.Count;
            int sqrt = Mathf.CeilToInt(Mathf.Sqrt(total));
            Row = sqrt;
            Column = sqrt;
            while (Row * Column < total)
            {
                if (Row < Column) Row++;
                else Column++;
            }
        }

        private void MergerSelectedTexture()
        {
            int width = Size.x * Column;
            int height = Size.y * Row;

            Texture2D output = new Texture2D(width, height);
            Color[] colors = new Color[width * height];
            for (int i = 0; i < Texture2DList.Count; i++)
            {
                Color[] pixels = Texture2DList[i].GetPixels();
                int x = i % Column;
                int y = Row - 1 - i / Column;
                for (int j = 0; j < pixels.Length; j++)
                {
                    int index = (y * Size.y + j / Size.x) * width + x * Size.x + j % Size.x;
                    colors[index] = pixels[j];
                }
            }

            output.SetPixels(colors);
            output.Apply();

            TexturePath = $"{FolderPath}/{ObjectName}.png";
            File.WriteAllBytes(TexturePath, output.EncodeToPNG());
            AssetDatabase.ImportAsset(TexturePath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();

            MergeTexture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(TexturePath);
        }

        private void SliceTexture()
        {
            TextureImporter importer = AssetImporter.GetAtPath(TexturePath) as TextureImporter;
            if (importer == null) return;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;

            SpriteDataProviderFactories factory = new SpriteDataProviderFactories();
            factory.Init();
            ISpriteEditorDataProvider dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
            dataProvider.InitSpriteEditorDataProvider();
            SpriteRect[] spriteRects = SpriteInCurrentPath.Select((s, i) =>
            {
                Rect rect = new Rect(i % Column * Size.x, (int)(Row - 1 - i / Column) * Size.y, Size.x, Size.y);
                return new SpriteRect
                {
                    alignment = SpriteAlignment.Center,
                    border = Vector4.zero,
                    name = s.name,
                    pivot = new Vector2(0.5f, 0.5f),
                    rect = rect
                };
            }).ToArray();
            dataProvider.SetSpriteRects(spriteRects);
            dataProvider.Apply();
            AssetImporter assetImporter = dataProvider.targetObject as AssetImporter;
            if (assetImporter != null) assetImporter.SaveAndReimport();
        }

        private void UpdateTMPSpriteAsset()
        {
            if (TMPSpriteAsset == null)
            {
                TMPSpriteAsset = CreateInstance<TMP_SpriteAsset>();
                TMPSpriteAsset.name = ObjectName;
                TMPSpriteAssetPath = $"{FolderPath}/{ObjectName}_TMPSpriteAsset.asset";

                AssetDatabase.CreateAsset(TMPSpriteAsset, TMPSpriteAssetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();


                TMPSpriteAsset.material = GetDefaultSpriteMaterial(TMPSpriteAsset);
                TMPSpriteAsset.spriteInfoList = new List<TMP_Sprite>();
                TMPro_EventManager.ON_SPRITE_ASSET_PROPERTY_CHANGED(true, TMPSpriteAsset);
            }

            EditorUtility.SetDirty(TMPSpriteAsset);
            TMPSpriteAsset.spriteSheet = MergeTexture2D;
            TMPSpriteAsset.spriteGlyphTable.Clear();
            TMPSpriteAsset.spriteCharacterTable.Clear();

            Object[] allAssets = AssetDatabase.LoadAllAssetsAtPath(TexturePath);
            Sprite[] allSprites = allAssets.OfType<Sprite>()
                .OrderBy(t => SpriteInCurrentPath.FindIndex(s => s.name == t.name)).ToArray();
            for (int i = 0; i < allSprites.Length; i++)
            {
                Sprite sprite = allSprites[i];
                TMPSpriteAsset.spriteGlyphTable.Add(new TMP_SpriteGlyph()
                {
                    index = (uint)i,
                    metrics = new GlyphMetrics(sprite.rect.width, sprite.rect.height, -sprite.pivot.x,
                        sprite.rect.height - sprite.pivot.y, sprite.rect.width),
                    glyphRect = new GlyphRect(sprite.rect),
                    scale = 1.0f,
                    sprite = sprite,
                });
                TMPSpriteAsset.spriteCharacterTable.Add(
                    new TMP_SpriteCharacter(0xFFFE, TMPSpriteAsset.spriteGlyphTable[i])
                    {
                        name = sprite.name,
                        scale = 1.0f
                    });
            }

            foreach (TMP_SpriteGlyph tmpSpriteGlyph in TMPSpriteAsset.spriteGlyphTable)
            {
                tmpSpriteGlyph.metrics = new GlyphMetrics()
                {
                    width = Size.x,
                    height = Size.y,
                    horizontalBearingX = Offset.x,
                    horizontalBearingY = Offset.y,
                    horizontalAdvance = Offset.z,
                };
                tmpSpriteGlyph.scale = Scale;
            }

            TMPSpriteAsset.SortGlyphTable();
            TMPSpriteAsset.UpdateLookupTables();
            TMPro_EventManager.ON_SPRITE_ASSET_PROPERTY_CHANGED(true, TMPSpriteAsset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private Material GetDefaultSpriteMaterial(TMP_SpriteAsset spriteAsset)
        {
            ShaderUtilities.GetShaderPropertyIDs();

            // Add a new material
            Shader shader = Shader.Find("TextMeshPro/Sprite");
            Material tempMaterial = new Material(shader);
            tempMaterial.SetTexture(ShaderUtilities.ID_MainTex, MergeTexture2D);
            tempMaterial.hideFlags = HideFlags.HideInHierarchy;

            AssetDatabase.AddObjectToAsset(tempMaterial, spriteAsset);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(spriteAsset));

            return tempMaterial;
        }
#endif
    }
}