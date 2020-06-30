using System;
using System.Collections.Generic;
using Board.Character;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace LetterSoup {
    public class LetterSoupController : MonoBehaviour {

        private int _size;
        private readonly string[] _words = new string[5];
        private GameObject[,] _board;
        private const string RandomChar = "abcdefghijklmneopqrstuvwxyz";
        private int _diagonalAttempts;
        private int _secondDiagonalAttempt;
        private GameMaster.GameMaster _gameMaster;
        private float _time = 120;
        private TextMeshProUGUI _timerText; 
        
        private List<LetterSoupCell> _horizontal = new List<LetterSoupCell>();
        private List<LetterSoupCell> _vertical = new List<LetterSoupCell>();
        private List<LetterSoupCell> _diagonal = new List<LetterSoupCell>();
        private List<LetterSoupCell> _secondDiagonal = new List<LetterSoupCell>();

        private bool _isHorizontalComplete;
        private bool _isVerticalComplete;
        private bool _isDiagonalComplete;
        private bool _isSecondDiagonoalComplete;
        
        
        private int _wordsFound;
        

        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject firstWord;
        [SerializeField] private GameObject secondWord;
        [SerializeField] private GameObject thirdWord;
        [SerializeField] private GameObject fourthWord;
        [SerializeField] private GameObject winText;
        [SerializeField] private GameObject gameOverText;
        [SerializeField] private GameObject continueBtn;
        [SerializeField] private GameObject timer;
        private bool gameEnded;


        // Start is called before the first frame update
        void Start() {
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

            var values = Enum.GetValues(typeof(Size));
            _size = (int) values.GetValue(Random.Range(0, 3));
            //_size = 10;
            _board = new GameObject[_size, _size];
            _timerText = timer.GetComponent<TextMeshProUGUI>();
            _timerText.text = _time.ToString("0.00");;
            
            DrawBoard();
            AdjustCameraSize();
            LoadFile();
            InsertHorizontalWord();
            InsertVerticalWord();
            InsertDiagonal();
            InsertSecondDiagonal();
            FillRandom();
        }

        private void Update() {
            if (gameEnded) return;
            
            _time -= Time.deltaTime;

            if (_time<0.00) {
                EndGame();
            }
            else if (_isHorizontalComplete && _isVerticalComplete && _isDiagonalComplete && _isSecondDiagonoalComplete){
                EndGame();
            }
            else {
                _timerText.text = _time.ToString("0.00");
            }
        }

        public void EndGame() {
            gameEnded = true;
            if (_isHorizontalComplete && _isVerticalComplete && _isDiagonalComplete && _isSecondDiagonoalComplete) {
                winText.SetActive(true);
                _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " has won Letter Soup.";
            }
            else {
                winText.SetActive(true);
                var player = _gameMaster._players[_gameMaster.gameOrder[_gameMaster.currentOrderIndex]].GetComponent<Player>(); //Gets active player
                _gameMaster.logOfEvents += "\n" + "P" + (_gameMaster.gameOrder[_gameMaster.currentOrderIndex] + 1) + " has lost Memory Cards.";
                player.turnCooldown = 1;
                player.needUpdateUiOnBoard = true;
            }
            continueBtn.SetActive(true);
            LockBoard();
        }

        private void LockBoard() {
            foreach (var o in _board) {
                o.GetComponent<LetterSoupCell>()._locked = true;
            }
        }

        public void CheckWordCompletion() {

            if (!_isHorizontalComplete) {
                foreach (var cell in _horizontal) {
                    _isHorizontalComplete = cell.IsComplete;
                    if (!_isHorizontalComplete) {
                        break;
                    }
                }

                if (_isHorizontalComplete) {
                    firstWord.GetComponent<TextMeshProUGUI>().color = Color.green;
                }
            }
            
            if (!_isVerticalComplete) {
                foreach (var cell in _vertical) {
                    _isVerticalComplete = cell.IsComplete;
                    if (!_isVerticalComplete) {
                        break;
                    }
                }  
                
                if (_isVerticalComplete) {
                    secondWord.GetComponent<TextMeshProUGUI>().color = Color.green;
                }
            }
            
            if (!_isDiagonalComplete) {
                foreach (var cell in _diagonal) {
                    _isDiagonalComplete = cell.IsComplete;
                    if (!_isDiagonalComplete) {
                        break;
                    }
                }

                if (_isDiagonalComplete) {
                    thirdWord.GetComponent<TextMeshProUGUI>().color = Color.green;
                }
            }
            
            if (!_isSecondDiagonoalComplete) {
                foreach (var cell in _secondDiagonal) {
                    _isSecondDiagonoalComplete = cell.IsComplete;
                    if (!_isSecondDiagonoalComplete) {
                        break;
                    }
                }

                if (_isSecondDiagonoalComplete) {
                    fourthWord.GetComponent<TextMeshProUGUI>().color = Color.green;
                }
            }
        }

        private void FillRandom() {
            for (var i = 0; i < _size; i++) {
                for (var j = 0; j < _size; j++) {
                    var cell = _board[i, j].GetComponent<LetterSoupCell>();
                    if (cell.Letters.Equals(Letters.Null)) {
                        cell.UpdateChar(RandomChar[Random.Range(0, RandomChar.Length)]);
                    }
                }
            }
        }

        private void InsertSecondDiagonal() {
            _secondDiagonalAttempt++;
            
            if (_secondDiagonalAttempt>5) {
                return;
            }
            
            var fitDiagonal = false;
            int x;
            int y;
            var counter = 0;
             
            do {
                counter++;
                x = Random.Range(0, _size/2);
                y = Random.Range(0, _size);
                var yCopy = y;
                var xCopy = x;
                if (y+_words[3].Length <= _size && x+_words[3].Length <= _size) {
                    fitDiagonal = true;
                    for (var i = 0; i < _words[3].Length; i++) {
                        var letter = _board[yCopy, xCopy].GetComponent<LetterSoupCell>().Letters;
                        
                        if (!letter.Equals(Letters.Null)) {
                            if (!letter.Equals(GetLetterFromChar(_words[3][i]))) {
                                fitDiagonal = false;
                            }
                        }
                        yCopy++;
                        xCopy++;
                    }
                }

                if (counter>=500) {
                    var lines = System.IO.File.ReadAllLines(@"Assets\Scripts\LetterSoup\WordsSources\words.txt");
                    var currentLength = _words[3].Length;

                    if (currentLength<=5) {
                        return;
                    }

                    var counter2 = 0;
                    
                    while (counter2<300) {
                        _words[3] = lines[Random.Range(0, 100)];
                        if (_words[3].Length<currentLength) {
                            Debug.Log(_words[3]);
                            break;
                        }
                        counter2++;
                    }
                    InsertSecondDiagonal();
                    return;
                }
            } while (!fitDiagonal);
            Debug.Log(counter);
            
            _words[3] = _words[3].ToLower();
            fourthWord.GetComponent<TextMeshProUGUI>().text = _words[3];
            
            for (var j=0; j < _words[3].Length; j++) {
                var cell = _board[y, x].GetComponent<LetterSoupCell>(); 
                cell.UpdateChar(_words[3][j]);
                cell.IsPartOfSolution = true;
                _secondDiagonal.Add(cell);
                y++;
                x++;
            }
            return;
        }

        private void InsertDiagonal() {
            _diagonalAttempts++;
            
            if (_diagonalAttempts>5) {
                return;
            }

            var fitDiagonal = false;
            int x;
            int y;
            var counter = 0;
             
            do {
                counter++;
                x = Random.Range(0, _size/2);
                y = Random.Range(0, _size);
                var yCopy = y;
                var xCopy = x;
                if (y+1 >= _words[2].Length && x+_words[2].Length <= _size) {
                    fitDiagonal = true;
                    for (var i = 0; i < _words[2].Length; i++) {
                        var letter = _board[yCopy, xCopy].GetComponent<LetterSoupCell>().Letters;
                        
                        if (!letter.Equals(Letters.Null)) {
                            if (!letter.Equals(GetLetterFromChar(_words[2][i]))) {
                                fitDiagonal = false;
                            }
                        }
                        yCopy--;
                        xCopy++;
                    }
                }

                if (counter>=1000) {
                    Debug.Log(counter);
                    var lines = System.IO.File.ReadAllLines(@"Assets\Scripts\LetterSoup\WordsSources\words.txt");
                    var currentLength = _words[2].Length;

                    if (currentLength<=5) {
                        Debug.Log(_words[3]);
                        return;
                    }


                    var counter2 = 0;
                    
                    while (counter2<300) {
                        _words[2] = lines[Random.Range(0, 100)];
                        if (_words[2].Length<currentLength) {
                            break;
                        }
                        counter2++;
                    }
                    InsertDiagonal();
                    return;
                }
            } while (!fitDiagonal);
            Debug.Log(counter);
            
            _words[2] = _words[2].ToLower();
            thirdWord.GetComponent<TextMeshProUGUI>().text = _words[2];
            
            for (var j=0; j < _words[2].Length; j++) {
                var cell = _board[y, x].GetComponent<LetterSoupCell>();
                cell.UpdateChar(_words[2][j]);
                cell.IsPartOfSolution = true;
                _diagonal.Add(cell);
                y--;
                x++;
            }
            return;
        }

        private void InsertVerticalWord() {
            var fitVertical = false;
            int x;
            int y;
             
            do {
                x = Random.Range(0, _size);
                y = Random.Range(0, _size);
                var yCopy = y;
                if (y+_words[1].Length < _size) {
                    fitVertical = true;
                    for (var i = 0; i < _words[1].Length; i++) {
                        var letter = _board[yCopy, x].GetComponent<LetterSoupCell>().Letters;
                        
                        if (!letter.Equals(Letters.Null)) {
                            if (!letter.Equals(GetLetterFromChar(_words[1][i]))) {
                                fitVertical = false;
                            }
                        }
                        yCopy++;
                    }
                }
            } while (!fitVertical);
            
            _words[1] = _words[1].ToLower();
            secondWord.GetComponent<TextMeshProUGUI>().text = _words[1];
            
            for (var j=0; j < _words[1].Length; j++) {
                var cell = _board[y, x].GetComponent<LetterSoupCell>(); 
                cell.UpdateChar(_words[1][j]);
                cell.IsPartOfSolution = true;
                _vertical.Add(cell);
                y++;
            }
        }

        private void InsertHorizontalWord() {
             //Horizontal
             var fitHorizontal = false;
             int x;
             int y;
             
             do {
                 x = Random.Range(0, _size);
                 y = Random.Range(0, _size);
                 if (x+_words[0].Length < _size) {
                     fitHorizontal = true;
                 }
             } while (!fitHorizontal);

             _words[0] = _words[0].ToLower();

             firstWord.GetComponent<TextMeshProUGUI>().text = _words[0]; 
             
             for (var j=0; j < _words[0].Length; j++) {
                 var cell = _board[y, x].GetComponent<LetterSoupCell>();
                 cell.UpdateChar(_words[0][j]);
                 cell.IsPartOfSolution = true;
                 _horizontal.Add(cell);
                 x++;
             }
             
             
        }

        private void LoadFile() {
            var lines = System.IO.File.ReadAllLines(@"Assets\Scripts\LetterSoup\WordsSources\words.txt");
            for (var i=0; i<4; i++) {
                _words[i] = lines[Random.Range(0, 100)];
                Debug.Log(_words[i]);
            }
        }

        private void DrawBoard() {
            var y = 0;
            for (var i=0;i<_size;i++) {
                var x = 0;
                for (var j=0;j<_size;j++) {
                    var newCell = Instantiate(cellPrefab, transform);
                    newCell.GetComponent<SpriteRenderer>().color = Color.black;
                    newCell.GetComponent<LetterSoupCell>().letterSoupController = this;
                    var newTransform = newCell.GetComponent<Transform>();
                    newTransform.position = new Vector2(x,y);
                    x += 2;
                    _board[i, j] = newCell;
                }
                y -= 2;
            }
        }
        
        private void AdjustCameraSize() {
            if (Camera.main == null) return;
            var main = Camera.main;
            
            main.orthographicSize = _size+2f;
            main.transform.position = new Vector3( _size, -_size, -10);
        }

        public Letters GetLetterFromChar(char c) {
            switch (c) {
                case 'a':
                    return Letters.A;
                case 'b':
                    return Letters.B;
                case 'c':
                    return Letters.C;
                case 'd':
                    return Letters.D;
                case 'e':
                    return Letters.E;
                case 'f':
                    return Letters.F;
                case 'g':
                    return Letters.G;
                case 'h':
                    return Letters.H;
                case 'i':
                    return Letters.I;
                case 'j':
                    return Letters.J;
                case 'k':
                    return Letters.K;
                case 'l':
                    return Letters.L;
                case 'm':
                    return Letters.M;
                case 'n':
                    return Letters.N;
                case 'o':
                    return Letters.O;
                case 'p':
                    return Letters.P;
                case 'q':
                    return Letters.Q;
                case 'r':
                    return Letters.R;
                case 's':
                    return Letters.S;
                case 't':
                    return Letters.T;
                case 'u':
                    return Letters.U;
                case 'v':
                    return Letters.V;
                case 'w':
                    return Letters.W;
                case 'x':
                    return Letters.X;
                case 'y':
                    return Letters.Y;
                case 'z':
                    return Letters.Z;
                default:
                    return Letters.Null;
            }
        }
        
        public void LoadBoard() {
            _gameMaster.TurnChange();
            SceneManager.LoadScene("MainBoard");
        }
    }
}
