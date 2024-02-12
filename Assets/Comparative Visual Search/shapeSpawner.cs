using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shapeSpawner : MonoBehaviour
{
    public List<shape> shapes = new List<shape>();
    public List<shape> copyShapes = new List<shape>();
    public List<ShapeData> shapeDatas = new List<ShapeData>();
    public GameObject prefab;
    public Vector2[] bounds;
    public Canvas canvas;
    public Text statusText;
    public Text scoreText;
    float highScore = 0;
    public bool singular = true;

    List<Vector2> positions = new List<Vector2>();
    [Space]
    public float minimumDistance = 50f;
    public int trialIterations = 4;
    public int reductionPerIter = 10;

    float time;
    bool shown = false;

    [Header("Test Values")]
    public Vector2 sizeRange;
    public Vector2 hueRange;
    public Vector2 satRange;
    public Vector2 valRange;
    public int count;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        statusText.text = "";
        scoreText.text = "";
        highScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !shown)
        {
            SpawnShape(count, sizeRange, satRange, valRange);
            shown = true;
            statusText.text = "";
            scoreText.text = "";
            
        }
        if (shown)
        {
            time += Time.deltaTime;
        }
    }
    public void shapeSelected(shape shape)
    {
        bool gotCorrect = compareShape(shape) ^ singular;
        ClearShapes();
        shown = false;
        if(time < highScore && gotCorrect || highScore == 0)
        {
            highScore = time;
        }
        statusText.text = "Got Correct: " + gotCorrect.ToString() + ", Time Taken: " + roundToHundreths(time).ToString() + " sec, press space to continue.";
        scoreText.text = "Fastest score: " + roundToHundreths(highScore).ToString() + " sec";
        time = 0;
    }
    void SpawnShape(int count, Vector2 sizeRange, Vector2 satRange, Vector2 valRange)
    {
        for (int i = 0; i < count; i++)
        {
            //alpha
            shape s = Instantiate(prefab, canvas.transform).GetComponent<shape>();
            shapes.Add(s);
            float size = Random.Range(sizeRange.x, sizeRange.y);
            Color color = Color.HSVToRGB(Random.Range(hueRange.x, hueRange.y), Random.Range(satRange.x, satRange.y), Random.Range(valRange.x, valRange.y));
            Vector2 position = new Vector2(Random.Range(bounds[0].x, bounds[1].x), Random.Range(bounds[0].y, bounds[1].y));

            bool tooClose = false;
            if (positions != null)
            {
                for (int j = 0; j < trialIterations; j++)
                {
                    foreach (Vector2 pos in positions)
                    {
                        if (Vector2.Distance(pos, position) < minimumDistance - j * reductionPerIter)
                        {
                            tooClose = true;
                        }
                    }
                    if (tooClose)
                    {
                        position = new Vector2(Random.Range(bounds[0].x, bounds[1].x), Random.Range(bounds[0].y, bounds[1].y));
                        tooClose = false;
                    }
                    else
                    {
                        goto LoopEnd;
                    }
                }
            }
            LoopEnd:

            positions.Add(position);
            int spriteID = Random.Range(0, s.sprites.Length);
            s.UpdateShape(size, color, position, spriteID, false);
            s.ID = i;
            shapeDatas.Add(s.shapeData);

            //beta
            int scSpriteID = spriteID;
            if (!singular)
            {
                scSpriteID += Random.Range(1, 3);
                if(scSpriteID >= 3)
                {
                    scSpriteID -= 3;
                }
            }
            shape sc = Instantiate(prefab, canvas.transform).GetComponent<shape>();
            copyShapes.Add(sc);
            Vector2 copyPosition = new Vector2(position.x + 500, position.y);
            sc.ID = i;
            sc.UpdateShape(size, color, copyPosition, scSpriteID, true);
        }

        int changeShapeID = Random.Range(0, copyShapes.Count);
        int changeShapeShapeID = copyShapes[changeShapeID].shapeData.shapeID;
        int randomShapeID;
        if (singular)
        {
            randomShapeID = changeShapeShapeID + Random.Range(1, 3);
            if (randomShapeID >= 3)
            {
                randomShapeID -= 3;
            }
        }
        else
        {
            randomShapeID = shapes[changeShapeID].shapeData.shapeID;
        }
        copyShapes[changeShapeID].UpdateShapeShape(randomShapeID);
    }
    void ClearShapes()
    {
        for (int i = 0; i < shapes.Count; i++)
        {
            Destroy(shapes[i].gameObject);
            Destroy(copyShapes[i].gameObject);
        }
        shapeDatas.Clear();
        shapes.Clear();
        copyShapes.Clear();
        positions.Clear();
    }
    bool compareShape(shape shape)
    {
        return shapes[shape.ID].shapeData.shapeID == copyShapes[shape.ID].shapeData.shapeID;
    }
    float roundToHundreths(float i)
    {
        return Mathf.Round(i * 100) / 100;
    }
    [System.Serializable]
    public class ShapeData
    {

        public float xPos;
        public float yPos;
        public float size;
        public float r;
        public float g;
        public float b;
        public int shapeID;
        public bool isCopy;
        public ShapeData(float _xPos, float _yPos, float _size, float _r, float _g, float _b, int _shapeID, bool _isCopy)
        {
            xPos = _xPos;
            yPos = _yPos;
            size = _size;
            r = _r;
            g = _g;
            b = _b;
            shapeID = _shapeID;
            isCopy = _isCopy;
        }

    }
}
