using System;
using Board.Character;
using BomberMario.Bombs;
using LetterSoup;
using TicTacToeGame;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace BomberMario {
    public class BomberMarioController : MonoBehaviour {

        private int _size;
        private const int NumberOfBombs = 7;
        private int _totalBombs;
        private GameMaster.GameMaster _gameMaster;
        
        public int simpleBombAmount;
        public int doubleBombAmount;
        public int crossBombAmount;
        public int lineBombAmount;
        
        
        public int treasureLeft = 4;

        public Bomb Bomb { get; private set; }

        public GameObject[,] Board { get; set; }

        [SerializeField] public ImagesContainer imagesContainer;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject cameraSecond; 
        [SerializeField] public GameObject gameOverText;
        [SerializeField] public GameObject winText;
        [SerializeField] public GameObject continueBtn;
        
        [SerializeField] public GameObject simpleBombCount;
        [SerializeField] public GameObject doubleBombCount;
        [SerializeField] public GameObject crossBombCount;
        [SerializeField] public GameObject lineBombCount;
        
        

        // Start is called before the first frame update
        private void Start() { 
            _gameMaster = GameObject.Find("GameMasterController").GetComponent<GameMaster.GameMaster>();
            _gameMaster.HideBoard();
            _gameMaster.HidePlayers();
            var values = Enum.GetValues(typeof(Size));
            _size = (int) values.GetValue(Random.Range(0, 3));
            Board = new GameObject[_size, _size];
            Bomb = new SimpleBomb(BombType.Simple);
            
            
            gameOverText.SetActive(false);
            winText.SetActive(false);
            continueBtn.SetActive(false);

            DrawBoard();
            AdjustCameraSize();
            AddRandomTreasure();
            GenerateRandomBombsAmount();
        }


        private void EndGame() {
            Debug.Log(treasureLeft);
            if (treasureLeft<=0) {
                winText.SetActive(true);
                _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " has won Bomber Mario.";
                LockAllCells();
                continueBtn.SetActive(true);
            } else if (_totalBombs<=0) {
                gameOverText.SetActive(true);
                var player = _gameMaster._players[_gameMaster.gameOrder[_gameMaster.currentOrderIndex]].GetComponent<Player>(); //Gets active player
                _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " has lost Bomber Mario.";
                player.turnCooldown = 1;
                player.needUpdateUiOnBoard = true;
                continueBtn.SetActive(true);
            }
        }

        private void LockAllCells() {
            for (var i=0;i<_size;i++) {
                for (var j=0;j<_size;j++) {
                    Board[i, j].GetComponent<BomberMarioCell>().Locked = true;
                }
            }
        }


        private void GenerateRandomBombsAmount() {
            simpleBombAmount = Random.Range(1, 3);
            doubleBombAmount = Random.Range(1, 3);
            crossBombAmount = Random.Range(1, 3);
            lineBombAmount = 1;

            _totalBombs = simpleBombAmount + doubleBombAmount + crossBombAmount + lineBombAmount;
            if (_totalBombs<NumberOfBombs) {
                lineBombAmount += (NumberOfBombs - _totalBombs);
            }
            _totalBombs = NumberOfBombs;

            simpleBombCount.GetComponent<TextMeshProUGUI>().text = simpleBombAmount.ToString();
            doubleBombCount.GetComponent<TextMeshProUGUI>().text = doubleBombAmount.ToString();
            crossBombCount.GetComponent<TextMeshProUGUI>().text = crossBombAmount.ToString();
            lineBombCount.GetComponent<TextMeshProUGUI>().text = lineBombAmount.ToString();
        }

        private void AddRandomTreasure() {
            var y = Random.Range(0, _size-1);
            var x = Random.Range(1, _size-1);

            //Board[x, y].GetComponent<SpriteRenderer>().sprite = imagesContainer.xImage;
            Board[x, y].GetComponent<BomberMarioCell>().IsTreasure = true;
            
            //Board[x, y+1].GetComponent<SpriteRenderer>().sprite = imagesContainer.xImage;
            Board[x, y+1].GetComponent<BomberMarioCell>().IsTreasure = true;
            
            //Board[x+1, y].GetComponent<SpriteRenderer>().sprite = imagesContainer.xImage;
            Board[x+1, y].GetComponent<BomberMarioCell>().IsTreasure = true;
            
            //Board[x+1, y+1].GetComponent<SpriteRenderer>().sprite = imagesContainer.xImage;
            Board[x+1, y+1].GetComponent<BomberMarioCell>().IsTreasure = true;
        }

        private void DrawBoard() {
            var y = 0;
            for (var i=0;i<_size;i++) {
                var x = 0;
                for (var j=0;j<_size;j++) {
                    var newCell = Instantiate(cellPrefab, transform);
                    var newTransform = newCell.GetComponent<Transform>();
                    newTransform.position = new Vector2(x,y);
                    x += 2;
                    newCell.GetComponent<BomberMarioCell>().BomberMarioController = this;
                    newCell.GetComponent<BomberMarioCell>().I = i;
                    newCell.GetComponent<BomberMarioCell>().J = j;
                    Board[i, j] = newCell;
                }
                y -= 2;
            }
        }
        
        private void AdjustCameraSize() {
            if (Camera.main == null) return;
            var main = Camera.main;
            var second = cameraSecond.GetComponent<Camera>();
            switch (_size) {
                case 15:
                    main.orthographicSize = 16;
                    main.transform.position = new Vector3( 14, -14, -10);
                    second.orthographicSize = 16;
                    second.transform.position = new Vector3( 14, -14, -10);
                    break;
                case 20:
                    main.orthographicSize = 21;
                    main.transform.position = new Vector3( 19, -19, -10);
                    second.orthographicSize = 21;
                    second.transform.position = new Vector3( 19, -19, -10);
                    break;
            }
        }

        public void SetBombType(int bombType) {
            switch (bombType) {
                case 0:
                    Bomb = new SimpleBomb(BombType.Simple);
                    break;
                case 1:
                    Bomb = new DoubleBomb(BombType.Double);
                    break;
                case 2:
                    Bomb = new CrossBomb(BombType.Cross);
                    break;
                case 3:
                    Bomb = new LineBomb(BombType.Line);
                    break;
            }
        }

        public int GetBombsLeft() {
            switch (Bomb.BombType) {
                case BombType.Simple:
                    return simpleBombAmount;
                case BombType.Double:
                    return doubleBombAmount;
                case BombType.Cross:
                    return crossBombAmount;
                case BombType.Line:
                    return lineBombAmount;
                default:
                    return simpleBombAmount;
            }
        }
        
        public void DecreaseBombCount() {
            switch (Bomb.BombType) {
                case BombType.Simple:
                    simpleBombAmount--;
                    simpleBombCount.GetComponent<TextMeshProUGUI>().text = simpleBombAmount.ToString();
                    break;
                case BombType.Double:
                    doubleBombAmount--;
                    doubleBombCount.GetComponent<TextMeshProUGUI>().text = doubleBombAmount.ToString();
                    break;
                case BombType.Cross:
                    crossBombAmount--;
                    crossBombCount.GetComponent<TextMeshProUGUI>().text = crossBombAmount.ToString();
                    break;
                case BombType.Line:
                    lineBombAmount--;
                    lineBombCount.GetComponent<TextMeshProUGUI>().text = lineBombAmount.ToString();
                    break;
                default:
                    simpleBombAmount--;
                    simpleBombCount.GetComponent<TextMeshProUGUI>().text = simpleBombAmount.ToString();
                    break;
            }
            _totalBombs--;
            EndGame();
        }
        
        public void LoadBoard() {
            _gameMaster.TurnChange();
            SceneManager.LoadScene("MainBoard");
        }
    }
    
}
