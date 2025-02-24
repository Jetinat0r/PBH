using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridTileSelector : MonoBehaviour
{
    public static GridTileSelector instance;

    [SerializeField]
    private Tilemap arenaTilemap;
    [SerializeField]
    private SpriteRenderer tileSelectorVisual;
    [SerializeField]
    private Color validTileColor = Color.green;
    [SerializeField]
    private Color invalidTileColor = Color.red;

    private bool selectorActive = false;

    public delegate void ClickTile(Vector3Int _tilePos, bool _isValidTile);
    public ClickTile onTileClick;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivateSelector()
    {
        selectorActive = true;
    }

    public void DeactivateSelector()
    {
        selectorActive = false;
        tileSelectorVisual.gameObject.SetActive(false);
    }

    public bool TryGetWorldPosFromTilePos(Vector3Int _tilePos, out Vector3 _worldPos)
    {
        //Needs assigned for the false case, so we do it here anyways
        _worldPos = Vector3Int.zero;
        if(_tilePos.x < 0 || _tilePos.x >= arenaTilemap.size.x || _tilePos.y < 0 || _tilePos.y >= arenaTilemap.size.y)
        {
            return false;
        }

        _worldPos = arenaTilemap.CellToWorld(_tilePos) + arenaTilemap.cellSize / 2f;
        return true;
    }

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
        //If we're not trying to select a tile, don't bother!
        if (!selectorActive)
        {
            return;
        }

        Vector3 _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mouseWorldPos.z = 0;
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

            bool _validTile;
            if(_currentCell.x <= 0 || _currentCell.x >= 9 || _currentCell.y <= 0 || _currentCell.y >= 9)
            {
                _validTile = false;
                tileSelectorVisual.color = invalidTileColor;
            }
            else
            {
                _validTile = true;
                tileSelectorVisual.color = validTileColor;
            }

            tileSelectorVisual.gameObject.SetActive(true);

            // Left-Click detector on tile selector
            if (Input.GetMouseButtonDown(0)) 
            {
                OnTileSelectorClicked(_currentCell, _validTile);
            }
        }
        else
        {
            tileSelectorVisual.gameObject.SetActive(false);
        }
    }

    private void OnTileSelectorClicked(Vector3Int _currentTile, bool _onValidTile)
    {
        if (_onValidTile)
        {
            Debug.Log("Valid tile clicked! Using card.");
            onTileClick?.Invoke(_currentTile, _onValidTile);
        }
        else
        {
            Debug.Log("On invalid tile, can't use card!");
        }
    }
}



