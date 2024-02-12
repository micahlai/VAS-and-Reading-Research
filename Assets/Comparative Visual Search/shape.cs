using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shape : MonoBehaviour
{
    public Sprite[] sprites;
    public Image image;
    public shapeSpawner.ShapeData shapeData;
    public Button button;
    public int ID;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void attempt()
    {
        FindObjectOfType<shapeSpawner>().shapeSelected(this);
    }
    public void UpdateShape(float size, Color color, Vector2 pos, int shapeID, bool isCopy)
    {
        shapeData = new shapeSpawner.ShapeData(pos.x, pos.y, size, color.r, color.b, color.g, shapeID, isCopy);
        image.sprite = sprites[shapeData.shapeID];
        image.color = new Color(shapeData.r, shapeData.g, shapeData.b, 1);
        image.rectTransform.localScale = new Vector2(shapeData.size, shapeData.size);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(shapeData.xPos, shapeData.yPos);
    }
    public void UpdateShapeShape(int shapeID)
    {
        shapeData.shapeID = shapeID;
        print(shapeID);
        image.sprite = sprites[shapeID];

    }

}


