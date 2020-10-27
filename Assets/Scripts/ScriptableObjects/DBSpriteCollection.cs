using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SO/DB/Collection/SpriteSet", fileName = "NewSpriteSetCollection")]
public class DBSpriteCollection : ScriptableObject
{
    [SerializeField]
    public List<DBImage> spriteSetCollection;
}
