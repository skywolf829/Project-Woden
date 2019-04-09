using UnityEngine;
using UnityEditor;
using System.Collections;

public class RenameSpritesheet: MonoBehaviour
{
    public Texture2D texture2D;
    public string newName;

    private string path;
    private TextureImporter textureImporter;

    void Start()
    {
        path = AssetDatabase.GetAssetPath(texture2D);
        textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        SpriteMetaData[] sliceMetaData = textureImporter.spritesheet;

        int index = 0;
        foreach (SpriteMetaData individualSliceData in sliceMetaData)
        {
            sliceMetaData[index].name = string.Format(newName + " {0}", index);
            print(sliceMetaData[index].name);

            index++;
        }

        textureImporter.spritesheet = sliceMetaData;
        EditorUtility.SetDirty(textureImporter);
        textureImporter.SaveAndReimport();

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }

    void Update()
    {

    }
}