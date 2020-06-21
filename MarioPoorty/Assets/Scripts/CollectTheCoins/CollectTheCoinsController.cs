using System.Globalization;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CollectTheCoins {
    public class CollectTheCoinsController : MonoBehaviour {

        private float _time;
        private const int Size = 25;
        private GameObject[,] _board;
        private TextMeshProUGUI _timer;
        
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject winText;
        [SerializeField] private GameObject gameOverText;
        [SerializeField] private GameObject time;
        
        
        
        void Start() {
            _board = new GameObject[Size, Size];
            _time = GetTime(Random.Range(0, 3));
            _timer = time.GetComponent<TextMeshProUGUI>();
            _timer.text = _time.ToString("0.00");
            
            
            winText.SetActive(false);
            gameOverText.SetActive(false);
            DrawBoard();
        }

        private void Update() {
            _time -= Time.deltaTime;
            if ( _time < 0 ) {
                EndGame();
            } else {
                _timer.text = _time.ToString("0.00");
            }
        }

        private void EndGame() {
            winText.SetActive(true);
        }

        private void DrawBoard() {
            var y = 0;
            for (var i = 0; i < Size; i++) {
                var x = 0;
                for (var j = 0; j < Size; j++) {
                    var newCell = Instantiate(cellPrefab, transform);
                    var newTransform = newCell.GetComponent<Transform>();
                    newCell.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    newTransform.position = new Vector2(x, y);
                    var cell = newCell.GetComponent<CollectTheCoinsCell>();
                    cell.CollectTheCoinsController = this;
                    cell.Value = Random.Range(-10, 11);
                    
                    x += 2;
                    _board[i, j] = newCell;
                }
                y -= 2;
            }
        }

        private float GetTime(int option) {
            switch (option) {
                case 0:
                    return 30;
                case 1:
                    return 45;
                case 2:
                    return 60;
                default:
                    return 30;
            }
        }

    }
}
