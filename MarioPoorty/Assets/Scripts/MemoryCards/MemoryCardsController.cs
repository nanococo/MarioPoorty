using UnityEngine;

namespace MemoryCards {
    public class MemoryCardsController : MonoBehaviour {
        
        private const int Height = 3;
        private const int Length = 6;
        private GameObject[,] _board;
        
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] public GameObject gameOverText;
        [SerializeField] public GameObject winText;
        
        // Start is called before the first frame update
        void Start() {
            _board = new GameObject[Height, Length];
            gameOverText.SetActive(false);
            winText.SetActive(false);
            DrawBoard();
        }

        private void DrawBoard() {
            var y = 0;
            for (var i=0;i<Height;i++) {
                var x = 0;
                for (var j=0;j<Length;j++) {
                    var newCell = Instantiate(cellPrefab, transform);
                    var newTransform = newCell.GetComponent<Transform>();
                    newTransform.position = new Vector2(x,y);
                    x += 2;
                    newCell.GetComponent<MemoryCardsCell>().MemoryCardsController = this;
                    _board[i, j] = newCell;
                }
                y -= 2;
            }
        }
    }
}
