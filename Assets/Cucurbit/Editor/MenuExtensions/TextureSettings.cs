using System.IO;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Cucurbit.Editor.MenuExtensions
{
    public class TextureSettings
    {
        [MenuItem("Assets/Texture Settings/Set Default", false, 10002)]
        static void ChangeToDefault() { ChangeTextureType(TextureImporterType.Default); }
        [MenuItem("Assets/Texture Settings/Set Sprite", false, 10002)]
        static void ChangeToSprite() { ChangeTextureType(TextureImporterType.Sprite); }
        [MenuItem("Assets/Texture Settings/Set App Icon", false, 10002)]
        static void ChangeToAppIcon() { ChangeTextureType(TextureImporterType.GUI); }

        static void ChangeTextureType(TextureImporterType textureType)
        {
            //選んだファイル全てに対して実行
            foreach (var o in Selection.objects) {
                var path = AssetDatabase.GetAssetPath(o);
                var isDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory);

                if (isDirectory) {
                    //ディレクトリ対象のときにはサブフォルダも含めて全てに設定を行う
                    var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                    foreach (var item in files) {
                        SetTextureType(item, textureType);
                    }
                }
                else {
                    SetTextureType(path, textureType);
                }
            }
        }
        static void SetTextureType(string path, TextureImporterType textureType)
        {
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (textureImporter != null) {
                if (textureImporter.textureType != textureType) {
                    textureImporter.textureType = textureType;
                    textureImporter.SaveAndReimport();
                }
            }
        }
    }
}
