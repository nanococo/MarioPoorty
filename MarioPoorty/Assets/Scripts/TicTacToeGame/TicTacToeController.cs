using System;
using Board.Character;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TicTacToeGame {
    public class TicTacToeController : MonoBehaviour {

        public GameObject cellPrefab;
        private GameObject[,] _grid = new GameObject[3,3];
        private bool _firstPlayer = true;
        public int selectedSquares;

        private GameMaster.GameMaster _gameMaster;


        [SerializeField] private GameObject winText;
        [SerializeField] private GameObject gameOverText;
        [SerializeField] private GameObject continueBtn;

        [SerializeField] private GameObject playerText;
        [SerializeField] private GameObject helperText;

        public TicTacToeMarks CurrentMark { private set; get; } = TicTacToeMarks.XMark;

        private void Start() {
            try {
                _gameMaster = GameObject.Find("GameMasterController").GetComponent<GameMaster.GameMaster>();
                _gameMaster.HideBoard();
                _gameMaster.HidePlayers();
            }
            catch (Exception) {
                //ignored
            }

            winText.SetActive(false);
            gameOverText.SetActive(false);
            continueBtn.SetActive(false);
            
            playerText.GetComponent<TextMeshProUGUI>().color = Color.magenta;
            var y = 2;
            for (var i=0;i<3;i++) {
                var x = -2;
                for (var j=0;j<3;j++) {
                    var newCell = Instantiate(cellPrefab, transform);
                    var newTransform = newCell.GetComponent<Transform>();
                    newTransform.position = new Vector2(x,y);
                    x += 2;
                    newCell.GetComponent<Cell>().TicTacToeController = this;
                    _grid[i, j] = newCell;
                }
                y -= 2;
            }
        }

        public void CheckWin() {
            for (var lines=0; lines<3; lines++) {
                if (CheckRow(lines)) {
                    SetWin(true);
                    return;
                }

                if (CheckColumn(lines)) {
                    SetWin(true);
                    return;
                }
            }

            if (CheckDiagonals()) {
                SetWin(true);
            }
            else {
                SetWinFilled();
            }
        }

        private void SetWinFilled() {
            if (selectedSquares>=9) {
                gameOverText.SetActive(true);
                var player = _gameMaster._players[_gameMaster.gameOrder[_gameMaster.currentOrderIndex]].GetComponent<Player>(); //Gets active player
                _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " has lost Tic Tac Toe.";
                player.turnCooldown = 1;
                continueBtn.SetActive(true);
                LockBoard();
            }
        }

        private void SetWin(bool winLose) {
            if (winLose && _firstPlayer) {
                winText.SetActive(true);
                _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " has won Tic Tac Toe.";
            }
            else {
                gameOverText.SetActive(true);
                var player = _gameMaster._players[_gameMaster.gameOrder[_gameMaster.currentOrderIndex]].GetComponent<Player>(); //Gets active player
                _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " has lost Tic Tac Toe.";
                player.turnCooldown = 1;
                player.needUpdateUiOnBoard = true;
            }
            continueBtn.SetActive(true);
            LockBoard();
        }

        private void LockBoard() {
            foreach (var o in _grid) {
                o.GetComponent<Cell>().locked = true;
            }
        }

        private bool CheckColumn(int j) {
            var compare =  _grid[0, j].GetComponent<Cell>().CellMark;
            for (int i = 0; i < 3; i++) {
                var current = _grid[i, j].GetComponent<Cell>().CellMark;
                if (current == TicTacToeMarks.Empty) {
                    return false;
                }

                if (current==compare) {
                    compare = current;
                }
                else {
                    return false;
                }
            }
            return true;
        }

        private bool CheckRow(int i) {
            var compare =  _grid[i, 0].GetComponent<Cell>().CellMark;
            for (var j = 0; j < 3; j++) {
                var current = _grid[i, j].GetComponent<Cell>().CellMark;
                if (current == TicTacToeMarks.Empty) {
                    return false;
                }

                if (current==compare) {
                    compare = current;
                }
                else {
                    return false;
                }
            }
            return true;
        }

        private bool CheckDiagonals() {
            if (_grid[0, 0].GetComponent<Cell>().CellMark == _grid[1, 1].GetComponent<Cell>().CellMark &&
                _grid[1, 1].GetComponent<Cell>().CellMark == _grid[2, 2].GetComponent<Cell>().CellMark) {
                return _grid[0, 0].GetComponent<Cell>().CellMark != TicTacToeMarks.Empty;
            }
            
            return _grid[0, 2].GetComponent<Cell>().CellMark == _grid[1, 1].GetComponent<Cell>().CellMark &&
                   _grid[1, 1].GetComponent<Cell>().CellMark == _grid[2, 0].GetComponent<Cell>().CellMark && 
                   _grid[0, 2].GetComponent<Cell>().CellMark != TicTacToeMarks.Empty;
        }
        
        public void TickTurn() {
            if (CurrentMark == TicTacToeMarks.XMark) {
                CurrentMark = TicTacToeMarks.Circle;
                _firstPlayer = true;
                helperText.GetComponent<TextMeshProUGUI>().color = Color.magenta;
                playerText.GetComponent<TextMeshProUGUI>().color = Color.white;
            }
            else {
                CurrentMark = TicTacToeMarks.XMark;
                _firstPlayer = false;
                helperText.GetComponent<TextMeshProUGUI>().color = Color.white;
                playerText.GetComponent<TextMeshProUGUI>().color = Color.magenta;
            }
        }
        
        public void LoadBoard() {
            _gameMaster.TurnChange();
            SceneManager.LoadScene("MainBoard");
        }
    }
 }