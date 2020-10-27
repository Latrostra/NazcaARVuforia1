using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SO/DB/SpriteSet", fileName = "NewSpriteSet")]
public class DBImage : ScriptableObject
{
    [SerializeField]
    public List<Sprite> imageSet;
}
