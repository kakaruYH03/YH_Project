using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    private static Dictionary<string, Sprite> cashedSprites
        = new Dictionary<string, Sprite>();

    public static Sprite[] Load()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Photo");

        foreach (Sprite sprite in sprites)
        {
            if (!cashedSprites.ContainsKey(sprite.name))
            {
                cashedSprites.Add(sprite.name, sprite);
            }
        }
        return sprites;
    }

    public static Sprite GetSprite(string name)
    {
        if (!cashedSprites.ContainsKey(name))
        {
            Sprite sprite = Resources.Load<Sprite>("Photo/" + name);
            if (sprite) cashedSprites.Add(sprite.name, sprite);
            
            return sprite;
        }
        else
        {
            return cashedSprites[name];
        }
    }
}