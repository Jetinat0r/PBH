using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTileSelector : MonoBehaviour
{
    [SerializeField]
    private Tilemap arenaTilemap;
    [SerializeField]
    private SpriteRenderer tileSelectorVisual;
    [SerializeField]
    private Color validTileColor = Color.green;
    [SerializeField]
    private Color invalidTileColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        /*
        Debug.Log(arenaTilemap.cellBounds);
        Debug.Log(arenaTilemap.size);
        Debug.Log(arenaTilemap.origin);
        */
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int _currentCell = arenaTilemap.WorldToCell(_mouseWorldPos);

        if(arenaTilemap.GetTile(_currentCell) != null)
        {
            //tileSelectorVisual.transform.position = arenaTilemap.CellToWorld(_currentCell) + arenaTilemap.cellSize / 2f;
            tileSelectorVisual.transform.position = arenaTilemap.CellToLocal(_currentCell) + arenaTilemap.cellSize / 2f;
            /*
            Vector3 _weirdPos = arenaTilemap.WorldToLocal(_currentCell);
            _weirdPos.Scale(arenaTilemap.cellSize);
            tileSelectorVisual.transform.position = _weirdPos + arenaTilemap.cellSize / 2f;
            */

            tileSelectorVisual.color = _currentCell.x <= 3 ? invalidTileColor : validTileColor;

            tileSelectorVisual.gameObject.SetActive(true);
        }
        else
        {
            tileSelectorVisual.gameObject.SetActive(false);
        }
    }
}
