using System;
using Board.Character;
using TicTacToeGame;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace CollectTheCoins {
    public class CollectTheCoinsController : MonoBehaviour {

        private float _time;
        private const int Size = 25;
        private int _totalCoinsValue;
        private bool _gameEnded;
        private GameObject[,] _board;
        private TextMeshProUGUI _timer;
        private TextMeshProUGUI _scoreText;
        private GameMaster.GameMaster _gameMaster;
        
        [SerializeField] public ImagesContainer imagesContainer;


        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject winText;
        [SerializeField] private GameObject gameOverText;
        [SerializeField] private GameObject time;
        [SerializeField] private GameObject continueBtn;
        [SerializeField] private GameObject score;
        
        
        
        void Start() {

            try {
                _gameMaster = GameObject.Find("GameMasterController").GetComponent<GameMaster.GameMaster>();
                _gameMaster.HideBoard();
                _gameMaster.HidePlayers();
            }
            catch (Exception) {
                //ignored
            }

            _board = new GameObject[Size, Size];
            _time = GetTime(Random.Range(0, 3));
            _timer = time.GetComponent<TextMeshProUGUI>();
            _scoreText = score.GetComponent<TextMeshProUGUI>();
            _timer.text = _time.ToString("0.00");
            
            
            winText.SetActive(false);
            gameOverText.SetActive(false);
            continueBtn.SetActive(false);
            DrawBoard();
        }

        private void Update() {
            if (_gameEnded) return;
            
            _time -= Time.deltaTime;
            if ( _time < 0 ) {
                EndGame();
            } else {
                _timer.text = _time.ToString("0.00");
            }
            _scoreText.text = _totalCoinsValue.ToString();
        }

        private void EndGame() {
            if (_gameEnded) return;
            
            if (_totalCoinsValue<0) {
                gameOverText.SetActive(true);
                var player = _gameMaster._players[_gameMaster.gameOrder[_gameMaster.currentOrderIndex]].GetComponent<Player>(); //Gets active player
                _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " has lost Collect the Coins.";
                player.turnCooldown = 1;
                player.needUpdateUiOnBoard = true;
            }
            else {
                winText.SetActive(true);
                _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " has won Collect the Coins.";
            }
            continueBtn.SetActive(true);
            _gameEnded = true;
            LockBoard();
        }

        private void LockBoard() {
            foreach (var o in _board) {
                o.GetComponent<CollectTheCoinsCell>().Clicked = true;
            }
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

        public void AddCoinsValue(int coinValue) {
            _totalCoinsValue += coinValue;
        }

        public void LoadBoard() {
            _gameMaster.TurnChange();
            SceneManager.LoadScene("MainBoard");
        }
    }
}
