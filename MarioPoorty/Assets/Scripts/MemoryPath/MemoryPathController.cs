using UnityEngine;
using Random = UnityEngine.Random;

namespace MemoryPath {
    public class MemoryPathController : MonoBehaviour {
        // Start is called before the first frame update
        private const int Height = 6;
        private const int Length = 3;
        public int currentRow = 5;
        private GameObject[,] _board;

        [SerializeField] private GameObject cellPrefab;
        [SerializeField] public GameObject attemptsText;
        [SerializeField] public GameObject gameOverText;
        [SerializeField] public GameObject winText;

        void Start() {
            _board = new GameObject[Height, Length];
            gameOverText.SetActive(false);
            winText.SetActive(false);
            DrawBoard();
            SetRowInteractable();
        }

        public void SetRowUninteractable() {
            for (var i = 0; i < 3; i++) {
                _board[currentRow, i].GetComponent<MemoryPathCell>().interact = false;
            }
            currentRow--;
        }

        public void SetRowInteractable() {
            for (var i = 0; i < 3; i++) {
                _board[currentRow, i].GetComponent<MemoryPathCell>().interact = true;
            }
        }

        public void GameOver() {
            gameOverText.SetActive(true); //false to hide, true to show
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
                    newCell.GetComponent<MemoryPathCell>().MemoryPathController = this;
                    _board[i, j] = newCell;
                }
                _board[i, Random.Range(0,3)].GetComponent<MemoryPathCell>().IsChoice = true;
                y -= 2;
            }
        }

        public void Win() {
            winText.SetActive(true);
        }
    }
}
