using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Skrypt udostępnia metody potrzebne do kontroli contentem */

public class ContentController
{
    private DBImage _dbImage;
    private Image _image;
    private int activeImage;

    public ContentController(DBImage dBImage, Image content) {
        _dbImage = dBImage;
        _image = content;
        activeImage = 0;
    }

    public void Init() {
        if (_image != null) {
            _image.sprite = _dbImage.imageSet[activeImage];
        }
    }

    public void GoToContent(int contentIndex) {
        _image.sprite = _dbImage.imageSet[contentIndex];
        activeImage = contentIndex;
    }

    public void NextContent() {
        if (activeImage != _dbImage.imageSet.Count-1) {
            _image.sprite = _dbImage.imageSet[activeImage + 1];
            activeImage += 1;
        } else {
            return;
        }
    }

    public void PrevContent() {
        if (activeImage != 0) {
            _image.sprite = _dbImage.imageSet[activeImage - 1];
            activeImage -= 1;
        }   else {
            return;
        }
    }
}
