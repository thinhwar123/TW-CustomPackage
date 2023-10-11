using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TW.Utility.Tool
{
#if UNITY_EDITOR
    public class TMP_OverrideSpriteAsset : Editor
    {
        [MenuItem("Assets/Override Sprite Asset")]
        public static void DoUpdateSpriteAsset()
        {
            OverrideSpriteAsset(Selection.activeObject as TMP_SpriteAsset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
         
        [MenuItem("Assets/Override Sprite Asset", true)]
        private static bool CanUpdateSpriteAsset()
        {
            return Selection.activeObject is TMP_SpriteAsset;
        }

        private static void OverrideSpriteAsset(TMP_SpriteAsset spriteAsset)
        {
            EditorUtility.SetDirty(spriteAsset);
            
            // Get a list of all the sprites contained in the texture referenced by the sprite asset.
            // This only works if the texture is set to sprite mode.
            string filePath = AssetDatabase.GetAssetPath(spriteAsset.spriteSheet);
            if (string.IsNullOrEmpty(filePath))
                return;
            // Get all the sprites defined in the sprite sheet texture referenced by this sprite asset.
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(filePath).Select(x => x as Sprite)
                .Where(x => x != null).ToArray();
            // Return if sprite sheet texture does not have any sprites defined in it.
            if (sprites.Length == 0)
            {
                Debug.Log(
                    "Sprite Asset <color=#FFFF80>[" + spriteAsset.name +
                    "]</color>'s atlas texture does not appear to have any sprites defined in it. Use the Unity Sprite Editor to define sprites for this texture.",
                    spriteAsset.spriteSheet);
                return;
            }
        
            List<TMP_SpriteGlyph> spriteGlyphTable = spriteAsset.spriteGlyphTable;
            // Find available glpyh indexes
            uint[] existingGlyphIndexes = spriteGlyphTable.Select(x => x.index).ToArray();
            List<uint> availableGlyphIndexes = new List<uint>();
            uint lastGlyphIndex = existingGlyphIndexes.Length > 0 ? existingGlyphIndexes.Last() : 0;
            int elementIndex = 0;
            for (uint i = 0; i < lastGlyphIndex; i++)
            {
                uint existingGlyphIndex = existingGlyphIndexes[elementIndex];
                if (i == existingGlyphIndex)
                    elementIndex += 1;
                else
                    availableGlyphIndexes.Add(i);
            }
        
            // Iterate over sprites contained in the updated sprite sheet to identify new and / or modified sprites.
            for (int i = 0; i < sprites.Length; i++)
            {
                Sprite sprite = sprites[i];
                // Check if current sprites is already contained in the sprite glyph table of the sprite asset.
                TMP_SpriteGlyph spriteGlyph = spriteGlyphTable.FirstOrDefault(x => x.sprite == sprite);

                if (spriteGlyph != null)
                {
                    // update existing sprite glyph
                    if (spriteGlyph.glyphRect.x != sprite.rect.x || spriteGlyph.glyphRect.y != sprite.rect.y ||
                        spriteGlyph.glyphRect.width != sprite.rect.width ||
                        spriteGlyph.glyphRect.height != sprite.rect.height)
                        spriteGlyph.glyphRect = new GlyphRect(sprite.rect);
                }
                else
                {
                    TMP_SpriteCharacter spriteCharacter;
                    // Check if this sprite potentially exists under the same name in the sprite character table.
                    if (spriteAsset.spriteCharacterTable is { Count: > 0 })
                    {
                        spriteCharacter = spriteAsset.spriteCharacterTable.FirstOrDefault(x => x.name == sprite.name);
                        spriteGlyph = spriteCharacter != null
                            ? spriteGlyphTable[(int)spriteCharacter.glyphIndex]
                            : null;
                        if (spriteGlyph != null)
                        {
                            // Update sprite reference and data
                            spriteGlyph.sprite = sprite;
                            if (spriteGlyph.glyphRect.x != sprite.rect.x || spriteGlyph.glyphRect.y != sprite.rect.y ||
                                spriteGlyph.glyphRect.width != sprite.rect.width ||
                                spriteGlyph.glyphRect.height != sprite.rect.height)
                                spriteGlyph.glyphRect = new GlyphRect(sprite.rect);
                        }
                        else
                        {
                            // Add new Sprite Glyph to the table
                            spriteGlyph = new TMP_SpriteGlyph();
                            // Get available glyph index
                            if (availableGlyphIndexes.Count > 0)
                            {
                                spriteGlyph.index = availableGlyphIndexes[0];
                                availableGlyphIndexes.RemoveAt(0);
                            }
                            else
                                spriteGlyph.index = (uint)spriteGlyphTable.Count;
        
                            spriteGlyph.metrics = new GlyphMetrics(sprite.rect.width, sprite.rect.height, -sprite.pivot.x,
                                sprite.rect.height - sprite.pivot.y, sprite.rect.width);
                            spriteGlyph.glyphRect = new GlyphRect(sprite.rect);
                            spriteGlyph.scale = 1.0f;
                            spriteGlyph.sprite = sprite;
                            spriteGlyphTable.Add(spriteGlyph);
                            spriteCharacter = new TMP_SpriteCharacter(0xFFFE, spriteGlyph);
                            spriteCharacter.name = sprite.name;
                            spriteCharacter.scale = 1.0f;
                            spriteAsset.spriteCharacterTable.Add(spriteCharacter);
                        }
                    }
                }
            }
        
            // Update Sprite Character Table to replace unicode 0x0 by 0xFFFE
            for (int i = 0; i < spriteAsset.spriteCharacterTable.Count; i++)
            {
                TMP_SpriteCharacter spriteCharacter = spriteAsset.spriteCharacterTable[i];
                if (spriteCharacter.unicode == 0)
                    spriteCharacter.unicode = 0xFFFE;
            }
        
            // Sort glyph table by glyph index
            spriteAsset.SortGlyphTable();
            spriteAsset.UpdateLookupTables();
            TMPro_EventManager.ON_SPRITE_ASSET_PROPERTY_CHANGED(true, spriteAsset);
            
        }
    }
#endif
}