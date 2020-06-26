using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Board.Character {
    public class Player : MonoBehaviour {
        
        [SerializeField] private float moveSpeed = 100.0f;

        public Transform[] wayPoints;
        public int _wayPointIndex { get; set; }
        public int _currentIndex;
        public bool _lockMove = true;
        public int turnCooldown;
        public bool needUpdateUiOnBoard;
        
        private GameMaster.GameMaster _gameMaster;

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
                    _wayPointIndex++;
                    if (_wayPointIndex==_currentIndex) {
                        _lockMove = true;
                       CellBehavior();
                    }
                }
            }
        }
        
        public void CellBehavior() {
            var cellType = _gameMaster._board[_currentIndex-1].GetComponent<BoardCell>().cellType;

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
                    _gameMaster.TurnChange();
                    break;
                case CellTypes.Tube:
                    _gameMaster.TurnChange();
                    break;
                case CellTypes.Star:
                    _gameMaster.TurnChange();
                    break;
                case CellTypes.FireFlower:
                    _gameMaster.TurnChange();
                    break;
                case CellTypes.IceFlower:
                    _gameMaster.TurnChange();
                    break;
                case CellTypes.Tail:
                    _gameMaster.TurnChange();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}