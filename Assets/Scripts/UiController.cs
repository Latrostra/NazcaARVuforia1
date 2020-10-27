using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
Skrypt kontroluje zachowanie UI na podstawie dostarczonych
Kolekcji SO z SO posiadającym liste spritów (new item => SO => DB => SpriteSet/Collection => SpriteSet)
Wrzuć na prefab znajdujący się na scenie, niezniszczalny)
*/    

public class UiController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Kolekcja scriptable object z wypełnioną listą spritów")]
    public DBSpriteCollection dBSpriteCollection;
    [SerializeField]
    public List<Image> contentFields;
    private Image content;
    private DBImage dBImage;

    private ContentController contentController;

    private void Awake() {
    }

    public void GoTo(int contentIndex) {
        contentController.GoToContent(contentIndex);
    }
    public void Prev() {
        contentController.PrevContent();
    }

    public void Next() {
        contentController.NextContent();
    }

    public void ChangeContentSet(int contentSetIndex) {
        dBImage = dBSpriteCollection.spriteSetCollection[contentSetIndex];
        content = contentFields[contentSetIndex];
        ContentControllerFactory();
    }

    private void ContentControllerFactory() {
        contentController = new ContentController(dBImage, content);
        contentController.Init();
    }

    public void ChangeState(GameObject gameObject) {
        if (gameObject.active == true) {
            gameObject.SetActive(false);
        } else {
            gameObject.SetActive(true);
        }
    }
    // I will refactor it later xD
    public void ScaleTween(GameObject gameObject) {
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 2f);
    }

}
