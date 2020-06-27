using System;
using System.Collections.Generic;
using Board;
using Board.Character;
using MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace GameMaster {
    public class GameMaster : MonoBehaviour {
        
        public List<int> gameOrder = new List<int>();
        public int[] boardGeneratorHelper = new int[15];
        public List<GameObject> tubes = new List<GameObject>();
        
        public int currentOrderIndex; //Index from gameOrder
        public string logOfEvents = "";

        public GameObject[] _players;
        public GameObject[] _board;
        public int numberOfPlayers;

        
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject playerPrefab; 
        
        
        private void Start() {
            _board = new GameObject[26];
            DontDestroyOnLoad(gameObject);
            GenerateBoard();
        }

        private void GenerateBoard() {
            var y = 0;
            var x = 0;
            var helper = 0;
            
            for (var j = 0; j < 6; j++) {
                _board[helper] = CreateNewCell(x, y);
                helper++;
                y -= 2;
            }
            y += 2;
            x += 2;
            for (var j = 0; j < 4; j++) {
                _board[helper] = CreateNewCell(x, y);
                helper++;
                x += 2;
            }
            
            for (var j = 0; j < 6; j++) {
                _board[helper] = CreateNewCell(x, y);
                helper++;
                y += 2;
            }
            y -= 2;
            x += 2;
            for (var j = 0; j < 4; j++) {
                _board[helper] = CreateNewCell(x, y);
                helper++;
                x += 2;
            }
            for (var j = 0; j < 6; j++) {
                _board[helper] = CreateNewCell(x, y);
                helper++;
                y -= 2;
            }
            
            for (var i = 0; i < _board.Length; i++) {
                _board[i].SetActive(false);
                _board[i].GetComponent<BoardCell>().index = i;
                Debug.Log(_board[i].GetComponent<BoardCell>().cellType);
            }
        }
        
        private GameObject CreateNewCell(int x, int y) {
            var newCell = Instantiate(cellPrefab, transform);
            var newTransform = newCell.GetComponent<Transform>();
            newTransform.position = new Vector2(x, y);
            var cell = newCell.GetComponent<BoardCell>();
            cell.cellType = GetCellType();
            cell.x = x;
            cell.y = y;
            newCell.GetComponent<SpriteRenderer>().color = GetColorForType(cell.cellType);

            if (cell.cellType.Equals(CellTypes.Tube)) {
                tubes.Add(newCell);
            }

            return newCell;
        }

        private Color GetColorForType(CellTypes cellType) {
            switch (cellType) {
                case CellTypes.TicTacToe:
                    return Color.red;
                case CellTypes.LetterSoup:
                    return Color.white;
                case CellTypes.MemoryPath:
                    return Color.blue;
                case CellTypes.MemoryCards:
                    return Color.magenta;
                case CellTypes.GuessWho:
                    return Color.cyan;
                case CellTypes.CollectTheCoins:
                    return Color.yellow;
                case CellTypes.BomberMario:
                    return Color.black;
                case CellTypes.Prison:
                    return Color.gray;
                case CellTypes.Tube:
                    return Color.green;
                case CellTypes.Star:
                    return Color.gray;
                case CellTypes.FireFlower:
                    return Color.gray;
                case CellTypes.IceFlower:
                    return Color.gray;
                case CellTypes.Tail:
                    return Color.gray;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cellType), cellType, null);
            }
        }

        private CellTypes GetCellType() {
            while (true) {
                var selector = Random.Range(0, 10);
                switch (selector) {
                    case 0:
                        if (boardGeneratorHelper[0]<=1) {
                            boardGeneratorHelper[0]++;
                            return CellTypes.TicTacToe;
                        }
                        break;
                    case 1:
                        if (boardGeneratorHelper[1]<=1) {
                            boardGeneratorHelper[1]++;
                            //return CellTypes.LetterSoup;
                            return CellTypes.TicTacToe;
                        }
                        break;
                    case 2:
                        if (boardGeneratorHelper[2]<=1) {
                            boardGeneratorHelper[2]++;
                            return CellTypes.MemoryPath;
                        }
                        break;
                    case 3:
                        if (boardGeneratorHelper[3]<=1) {
                            boardGeneratorHelper[3]++;
                            return CellTypes.MemoryCards;
                        }
                        break;
                    case 4:
                        if (boardGeneratorHelper[4]<=1) {
                            boardGeneratorHelper[4]++;
                            return CellTypes.GuessWho;
                        }
                        break;
                    case 5:
                        if (boardGeneratorHelper[5]<=1) {
                            boardGeneratorHelper[5]++;
                            return CellTypes.CollectTheCoins;
                        }
                        break;
                    case 6:
                        if (boardGeneratorHelper[6]<=1) {
                            boardGeneratorHelper[6]++;
                            return CellTypes.BomberMario;
                        }
                        break;
                    case 7:
                        if (boardGeneratorHelper[7]<=1) {
                            boardGeneratorHelper[7]++;
                            return CellTypes.MemoryPath;
                        }
                        break;
                    case 8:
                        if (boardGeneratorHelper[8]<=1) {
                            boardGeneratorHelper[8]++;
                            return CellTypes.MemoryCards;
                        }
                        break;
                    case 9:
                        //Special Cells
                        var specialCell = Random.Range(9, 15);

                        switch (specialCell) {
                            case 9:
                                if (boardGeneratorHelper[9]<1) {
                                    boardGeneratorHelper[9]++;
                                    return CellTypes.Prison;
                                }
                                break;
                            case 10:
                                if (boardGeneratorHelper[10]<3) {
                                    boardGeneratorHelper[10]++;
                                    return CellTypes.Tube;
                                }
                                break;
                            case 11:
                                if (boardGeneratorHelper[11]<1) {
                                    boardGeneratorHelper[11]++;
                                    return CellTypes.Star;
                                }
                                break;
                            case 12:
                                if (boardGeneratorHelper[12]<1) {
                                    boardGeneratorHelper[12]++;
                                    return CellTypes.FireFlower;
                                }
                                break;
                            case 13:
                                if (boardGeneratorHelper[13]<1) {
                                    boardGeneratorHelper[13]++;
                                    return CellTypes.IceFlower;
                                }
                                break;
                            case 14:
                                if (boardGeneratorHelper[14]<1) {
                                    boardGeneratorHelper[14]++;
                                    return CellTypes.Tail;
                                }
                                break;
                        }
                        
                        break;
                    default:
                        return CellTypes.GuessWho;
                }
            }
        }

        //MENU SECTION STUFF
        public void CreatePlayers() {
            numberOfPlayers = GameObject.Find("MainMenuController").GetComponent<MainMenuController>().NumberOfPlayers;
            _players = new GameObject[numberOfPlayers];
            var spacing = 0;
            for (var i = 0; i < numberOfPlayers ; i++) {
                var newPlayer = Instantiate(playerPrefab, transform);
                newPlayer.GetComponent<SpriteRenderer>().sortingOrder = 1;
                newPlayer.GetComponent<Transform>().position = new Vector2(spacing, 6);
                
                var newPlayerScript = newPlayer.GetComponent<Player>();
                
                newPlayerScript.wayPoints = new Transform[BoardController.NumberOfCells];
                for (var j=0; j<BoardController.NumberOfCells; j++) {
                    newPlayerScript.wayPoints[j] = _board[j].transform;
                }
                
                _players[i] = newPlayer;
                spacing++;
            }
        }

        public void LoadMainBoard() {
            SceneManager.LoadScene("MainBoard");
        }

        public void DisplayBoard() {
            foreach (var o in _board) {
                o.SetActive(true);
            }
        }

        public void HideBoard() {
            foreach (var o in _board) {
                o.SetActive(false);
            }
        }

        public void HidePlayers() {
            foreach (var o in _players) {
                o.SetActive(false);
            }
        }

        public void DisplayPlayers() {
            foreach (var o in _players) {
                o.SetActive(true);
            }
        }

        public void TurnChange() {
            if (currentOrderIndex+1>_players.Length-1) {
                currentOrderIndex = 0;
            }
            else {
                currentOrderIndex++;
            }
        }
    }
}