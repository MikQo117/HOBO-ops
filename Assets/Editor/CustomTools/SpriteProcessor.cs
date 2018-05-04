using UnityEngine;
using UnityEditor;

// Made by Sipi Raussi 25.4.2018
public class SpriteProcessor : AssetPostprocessor
{
    /// <summary>
    /// Set correct setting for sprites
    /// </summary>
    /// <param name="texture"></param>
    private void OnPostprocessTexture(Texture2D texture)
    {
        // Get the file assets path
        string lowerCaseStringPath = assetPath.ToLower();

        bool isInSpritesDirectory =     lowerCaseStringPath.IndexOf("/sprites/") != -1;
        bool isInTilesDirectory =       lowerCaseStringPath.IndexOf("/sprites/tiles/") != -1;
        bool isInMapsDirectory =        lowerCaseStringPath.IndexOf("/sprites/maps/") != -1;
        bool isInUIDirectory =          lowerCaseStringPath.IndexOf("/sprites/ui/") != -1;

        // All sprites gets these import settings
        if (isInSpritesDirectory)
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spritePixelsPerUnit = AssetDatabase.LoadAssetAtPath<Settings>("Assets/Settings/Settings.asset").PixelsPerUnit;
            textureImporter.filterMode = FilterMode.Point;

            // All tile sprites have pivot point in Bottom Left
            if (isInTilesDirectory)
            {
                TextureImporterSettings settings = new TextureImporterSettings();

                textureImporter.ReadTextureSettings(settings);
                settings.spriteAlignment = (int)SpriteAlignment.BottomLeft;
                textureImporter.SetTextureSettings(settings);
            }

            if (isInMapsDirectory)
            {
                TextureImporterSettings settings = new TextureImporterSettings();

                textureImporter.ReadTextureSettings(settings);
                settings.readable = true;
                textureImporter.SetTextureSettings(settings);
            }

            if(isInUIDirectory)
            {
                textureImporter.filterMode = FilterMode.Bilinear;
            }
        }
    }
}