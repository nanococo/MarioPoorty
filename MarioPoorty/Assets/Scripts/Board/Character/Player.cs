using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Board.Character {
    public class Player : MonoBehaviour {
        
        [SerializeField] private float moveSpeed = 100.0f;

        public Transform[] wayPoints;
        public int _wayPointIndex { get; set; }
        public int _currentIndex;
        public bool _lockMove = true;
        public int turnCooldown;
        public bool needUpdateUiOnBoard;
        public int retrogressionValue;

        private bool justUsedTube;

        public int numberOfTurnsToLose;
        
        private GameMaster.GameMaster _gameMaster;
        public bool NoAction { get; set; }

        private void OnEnable() {
            //Debug.Log("OnEnable called");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            //Debug.Log("OnSceneLoaded: " + scene.name);
            //Debug.Log(mode);
        }

        private void Start() {
            _gameMaster = GameObject.Find("GameMasterController").GetComponent<GameMaster.GameMaster>();
        }

        private void Update() {
            Move();
        }

        public void Move() {
            if (_lockMove) return;

            if (_wayPointIndex <= _currentIndex) {
                transform.position = Vector2.MoveTowards(transform.position,
                    wayPoints[_wayPointIndex].transform.position, moveSpeed * Time.deltaTime*2);
                if (transform.position == wayPoints[_wayPointIndex].transform.position) {
                    if (_wayPointIndex==_currentIndex) {
                        _lockMove = true;

                        if (retrogressionValue!=0) {
                            _currentIndex = retrogressionValue;
                            retrogressionValue = 0;
                            _lockMove = false;
                            _wayPointIndex--;
                        }
                        else {
                            if (NoAction) {
                                NoAction = false;
                                return;
                            }
                            CellBehavior();
                        }
                    }
                    else {
                        _wayPointIndex++;
                    }
                }
            } 
            else if (_wayPointIndex > _currentIndex) {
                transform.position = Vector2.MoveTowards(transform.position,
                    wayPoints[_wayPointIndex].transform.position, moveSpeed * Time.deltaTime*2);
                if (transform.position == wayPoints[_wayPointIndex].transform.position) {
                    if (_wayPointIndex==_currentIndex) {
                        _lockMove = true;
            
                        if (retrogressionValue!=0) {
                            _currentIndex = retrogressionValue;
                            retrogressionValue = 0;
                            _lockMove = false;
                        }
                        else {
                            if (NoAction) {
                                NoAction = false;
                                return;
                            }
                            CellBehavior();    
                        }
                    }
                    else {
                        _wayPointIndex--;
                    }
                }
            }
        }
        
        public void CellBehavior() {
            var cellType = _gameMaster._board[_currentIndex].GetComponent<BoardCell>().cellType;

            int randomIndex;
            switch (cellType) {
                case CellTypes.TicTacToe:
                    SceneManager.LoadScene("TicTacToe");
                    break;
                case CellTypes.LetterSoup:
                    SceneManager.LoadScene("LettersSoup");
                    break;
                case CellTypes.MemoryPath:
                    SceneManager.LoadScene("MemoryPath");
                    break;
                case CellTypes.MemoryCards:
                    SceneManager.LoadScene("MemoryCards");
                    break;
                case CellTypes.GuessWho:
                    SceneManager.LoadScene("GuessWho");
                    break;
                case CellTypes.CollectTheCoins:
                    SceneManager.LoadScene("CollectTheCoins");
                    break;
                case CellTypes.BomberMario:
                    SceneManager.LoadScene("BomberMario");
                    break;
                case CellTypes.Prison:
                    numberOfTurnsToLose = 2;
                    _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " Went to prison. Loses 2 turns";
                    _gameMaster.ForceUpdateLog();
                    needUpdateUiOnBoard = true;
                    _gameMaster.TurnChange();
                    break;
                case CellTypes.Tube:

                    if (justUsedTube) {
                        justUsedTube = false;
                        return;
                    }

                    justUsedTube = true;
                    var currentTube = _gameMaster._board[_currentIndex].GetComponent<BoardCell>();
                    var tubeIndex = 0;
                    
                    for (var i = 0; i < _gameMaster.tubes.Count; i++) {
                        if (_gameMaster.tubes[i].GetComponent<BoardCell>().index==currentTube.index) {
                            tubeIndex = i;
                            break;
                        }
                    }
                    switch (tubeIndex) {
                        case 0:
                            _currentIndex = _gameMaster.tubes[1].GetComponent<BoardCell>().index;
                            _lockMove = false;
                            _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " Used tube 1";
                            _gameMaster.ForceUpdateLog();
                            break;
                        case 1:
                            _currentIndex = _gameMaster.tubes[2].GetComponent<BoardCell>().index;
                            _lockMove = false;
                            _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " Used tube 2";
                            _gameMaster.ForceUpdateLog();
                            break;
                        case 2:
                            _currentIndex = _gameMaster.tubes[0].GetComponent<BoardCell>().index;
                            _lockMove = false;
                            _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " Used tube 3";
                            _gameMaster.ForceUpdateLog();
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    _gameMaster.TurnChange();
                    break;
                case CellTypes.Star:
                    _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " Used STAR. Roll again!";
                    _gameMaster.ForceUpdateLog();
                    break;
                case CellTypes.FireFlower:
                    randomIndex = 0;
                    while (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1 == _gameMaster.gameOrder[randomIndex] + 1) {
                        randomIndex = Random.Range(0, _gameMaster.numberOfPlayers - 1);
                    }

                    _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " Used Fire Flower." + " P" + (_gameMaster.gameOrder[randomIndex] + 1) + " was forced to start again!";
                    _gameMaster._players[_gameMaster.gameOrder[randomIndex]].GetComponent<Player>()._currentIndex = 0;
                    _gameMaster._players[_gameMaster.gameOrder[randomIndex]].GetComponent<Player>()._lockMove = false;
                    _gameMaster._players[_gameMaster.gameOrder[randomIndex]].GetComponent<Player>().NoAction = true;
                    _gameMaster.ForceUpdateLog();
                    _gameMaster.TurnChange();
                    break;
                case CellTypes.IceFlower:
                    randomIndex = 0;
                    while (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1 == _gameMaster.gameOrder[randomIndex] + 1) {
                        randomIndex = Random.Range(0, _gameMaster.numberOfPlayers - 1);
                    }
                    
                    _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " Used Ice Flower." + " P" + (_gameMaster.gameOrder[randomIndex] + 1) + " was delayed two turns!";
                    _gameMaster._players[_gameMaster.gameOrder[randomIndex]].GetComponent<Player>().numberOfTurnsToLose = 0;
                    _gameMaster._players[_gameMaster.gameOrder[randomIndex]].GetComponent<Player>().needUpdateUiOnBoard = true;;
                    
                    _gameMaster.ForceUpdateLog();
                    _gameMaster.TurnChange();
                    break;
                case CellTypes.Tail:
                    if (justUsedTube) {
                        justUsedTube = false;
                        return;
                    }

                    justUsedTube = true;
                    
                    var spacesToMove = Random.Range(-3, 4);

                    if (_currentIndex+spacesToMove<0) {
                        spacesToMove = math.abs(spacesToMove); //Towards the positive
                    }

                    if (_currentIndex+spacesToMove>_gameMaster._board.Length-1) {
                        retrogressionValue = (_gameMaster._board.Length-1) - (spacesToMove-1);
                        _currentIndex = _gameMaster._board.Length-1;
                        _lockMove = false;
                    } else if (_currentIndex+spacesToMove==_gameMaster._board.Length-1) {
                        _currentIndex = _gameMaster._board.Length-1;
                        _lockMove = false;
                        _gameMaster.BoardController.EndGame();
                    }else {
                        _currentIndex += spacesToMove;
                        _lockMove = false;
                    }
                    
                    _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " Used tail. Moved: "+spacesToMove;
                    _gameMaster.ForceUpdateLog();
                    _gameMaster.TurnChange();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}