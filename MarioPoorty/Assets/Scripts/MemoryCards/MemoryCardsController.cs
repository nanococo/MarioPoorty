using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Board.Character;
using MemoryCards.ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace MemoryCards {
    public class MemoryCardsController : MonoBehaviour {

        [SerializeField] private MemoryCardsIconsContainer memoryCardsIconsContainer;
        
        
        private const int Height = 3;
        private const int Length = 6;
        private GameObject[,] _board;
        private int[] _cardsIds;
        private bool _firstPlayer = true;
        private GameMaster.GameMaster _gameMaster;
        
        
        public List<MemoryCardsCell> cardsClicked = new List<MemoryCardsCell>();
        public List<int> matchedCards = new List<int>();

        [SerializeField] private GameObject cellPrefab;
        [SerializeField] public GameObject gameOverText;
        [SerializeField] public GameObject winText;
        [SerializeField] public GameObject continueBtn;

        [SerializeField] private GameObject playerText;
        [SerializeField] private GameObject playerPoints;
        [SerializeField] private GameObject helperText;
        [SerializeField] private GameObject helperPoints;

        // Start is called before the first frame update
        void Start() {
            _gameMaster = GameObject.Find("GameMasterController").GetComponent<GameMaster.GameMaster>();
            _gameMaster.HideBoard();
            _gameMaster.HidePlayers();
            _board = new GameObject[Height, Length];
            gameOverText.SetActive(false);
            winText.SetActive(false);
            continueBtn.SetActive(false);
            DrawBoard();
            SetRandomCards();
            
            playerText.GetComponent<TextMeshProUGUI>().color = Color.magenta;
        }

        private void SetRandomCards() {
            var helper = 0;
            _cardsIds = new int[Height*Length];
            
            for (var i =0; i<Height*Length; i++) {
                if (helper>8) {
                    helper = 0;
                }
                _cardsIds[i] = helper;
                helper++;
            }
            _cardsIds = Shuffle(_cardsIds);

            helper = 0;
            for (var i =0; i<Height; i++) {
                for (var j = 0; j < Length; j++) {
                    _board[i, j].GetComponent<MemoryCardsCell>().Id = _cardsIds[helper];
                    helper++;
                }
            }
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

        private static int[] Shuffle(int[] cards) {
            // Knuth shuffle algorithm :: courtesy of Wikipedia :)
            for (var t = 0; t < cards.Length; t++ ) {
                var tmp = cards[t];
                var r = Random.Range(t, cards.Length);
                cards[t] = cards[r];
                cards[r] = tmp;
            }
            return cards;
        }

        public Sprite GetImageById(int id) {
            var values = Enum.GetValues(typeof(Cards));
            switch ((Cards)id) {
                case Cards.Coin:
                    return memoryCardsIconsContainer.coin;
                case Cards.Banana:
                    return memoryCardsIconsContainer.banana; 
                case Cards.RedShell:
                    return memoryCardsIconsContainer.redShell; 
                case Cards.GreenShell:
                    return memoryCardsIconsContainer.greenShell; 
                case Cards.BlueShell:
                    return memoryCardsIconsContainer.blueShell; 
                case Cards.Mushroom:
                    return memoryCardsIconsContainer.mushroom; 
                case Cards.Flower:
                    return memoryCardsIconsContainer.flower; 
                case Cards.Star:
                    return memoryCardsIconsContainer.star; 
                case Cards.Blooper:
                    return memoryCardsIconsContainer.blooper; 
                default:
                    return memoryCardsIconsContainer.mysteryBox;
            }
        }

        public void CheckPair(MemoryCardsCell cell) {
            cardsClicked.Add(cell);
            if (cardsClicked.Count==2) {
                if (cardsClicked[0].Id==cardsClicked[1].Id) {
                    matchedCards.Add(cardsClicked[0].Id);
                    cardsClicked.Clear();
                    AddScoreToPlayer();
                    CheckWin();
                } 
            } else if (cardsClicked.Count==3) {
                foreach (var memoryCardsCell in cardsClicked) {
                    memoryCardsCell.GetComponent<SpriteRenderer>().sprite = memoryCardsIconsContainer.mysteryBox;
                }
                cardsClicked.Clear();
                FlipPlayer();
            }
        }

        private void CheckWin() {
            if (matchedCards.Count < Height * Length / 2) return;
            
            var textMesh = playerPoints.GetComponent<TextMeshProUGUI>();
            var textMesh2 = helperPoints.GetComponent<TextMeshProUGUI>();
            if (Convert.ToInt32(textMesh.text)>Convert.ToInt32(textMesh2.text)) {
                winText.SetActive(true);
            }
            else {
                gameOverText.SetActive(true);
                var player = _gameMaster._players[_gameMaster.gameOrder[_gameMaster.currentOrderIndex]].GetComponent<Player>(); //Gets active player
                player.turnCooldown = 1;
                player.needUpdateUiOnBoard = true;
            }
            continueBtn.SetActive(true);
        }

        private void AddScoreToPlayer() {
            if (_firstPlayer) {
                var textMesh = playerPoints.GetComponent<TextMeshProUGUI>();
                textMesh.text = (Convert.ToInt32(textMesh.text) + 1).ToString();
            }
            else {
                var textMesh = helperPoints.GetComponent<TextMeshProUGUI>();
                textMesh.text = (Convert.ToInt32(textMesh.text) + 1).ToString();
            }
        }

        private void FlipPlayer() {
            if (_firstPlayer) {
                _firstPlayer = false;
                helperText.GetComponent<TextMeshProUGUI>().color = Color.magenta;
                playerText.GetComponent<TextMeshProUGUI>().color = Color.white;
            }
            else {
                _firstPlayer = true;
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
