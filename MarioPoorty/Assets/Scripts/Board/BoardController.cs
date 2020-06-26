using System;
using System.Collections.Generic;
using Board.Character;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Board {
    public class BoardController : MonoBehaviour {
        public static int NumberOfCells = 26;
        private int _randomNumberForOrder;
        private int _numberOfPlayers;
        private readonly List<int> _optionsForTurn = new List<int>();
        private GameObject[] _board;
        public Transform[] wayPoints;

        [SerializeField] private GameObject rollBtnTxt;
        [SerializeField] private GameObject currentPlayer;
        
        
        //Numeric method
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject btnText;
        [SerializeField] private GameObject btn;
        [SerializeField] private GameObject inputFieldText;
        [SerializeField] private GameObject numericMethodWindow;
        [SerializeField] private GameObject randomNumberFinal;
        [SerializeField] private GameObject currentPlayerDecision;
        [SerializeField] private GameObject[] orderNumbers;
        [SerializeField] private GameObject[] playerOrders;
        
        

        //private readonly List<int> order = new List<int>();
        private List<int> order;

        private GameMaster.GameMaster _gameMaster;
        
        private void Start() {
            _gameMaster = GameObject.Find("GameMasterController").GetComponent<GameMaster.GameMaster>();
            order = _gameMaster.gameOrder;
            try {
                _numberOfPlayers = _gameMaster.numberOfPlayers;
            } catch (Exception) {
                Debug.Log("Default Number of Players loaded (2)");
                _numberOfPlayers = 2;
            }

            _randomNumberForOrder = Random.Range(1, 1001);
            //_randomNumberForOrder = 632;
            
            InitializeUiElements();
            _gameMaster.DisplayBoard();
            _gameMaster.DisplayPlayers();

            if (order.Count>0) {
                numericMethodWindow.SetActive(false);
            }
            
        }

        private void Update() {
            try {
                var player = _gameMaster._players[order[_gameMaster.currentOrderIndex]].GetComponent<Player>();
                currentPlayer.GetComponent<TextMeshProUGUI>().text = "P" +  (order[_gameMaster.currentOrderIndex] + 1);
                if (!player.needUpdateUiOnBoard) return;


                if (player.turnCooldown == 0) {
                    rollBtnTxt.GetComponent<TextMeshProUGUI>().text = "Roll";
                }
                else if (player.turnCooldown == 1) {
                    rollBtnTxt.GetComponent<TextMeshProUGUI>().text = "Try Again";
                }
                else {
                    rollBtnTxt.GetComponent<TextMeshProUGUI>().text = "No Turn";
                }
                    
                player.needUpdateUiOnBoard = false;
            }
            catch (Exception) {
                //Ignored
            }
        }

        private void InitializeUiElements() {
            for (var i = 0; i < 6; i++) {
                orderNumbers[i].SetActive(false);
                playerOrders[i].SetActive(false);
            }

            for (var i = 0; i < _numberOfPlayers; i++) {
                orderNumbers[i].SetActive(true);
                playerOrders[i].SetActive(true);
            }
            
            btn.GetComponent<Button>().onClick.AddListener(GetValueFromInput);
        }

        private void GetValueFromInput() {
            inputFieldText.GetComponent<TextMeshProUGUI>().text = " ";
            
            
            if (_optionsForTurn.Count == _numberOfPlayers-1) {
                var input = inputFieldText.GetComponent<TextMeshProUGUI>().text.Trim((char)8203);
                try {
                    var inputNumeric = Convert.ToInt32(input);
                    _optionsForTurn.Add(inputNumeric);
                }
                catch (Exception) {
                    //Ignored
                }
                var btnListener = btn.GetComponent<Button>();
                btnListener.onClick.RemoveAllListeners();
                btnListener.onClick.AddListener(CalculateOrder);
                inputFieldText.GetComponent<TextMeshProUGUI>().text = " ";
                btnText.GetComponent<TextMeshProUGUI>().text = "Reveal";
            } else if (_optionsForTurn.Count < _numberOfPlayers) {
                var input = inputFieldText.GetComponent<TextMeshProUGUI>().text.Trim((char)8203);
                try {
                    var inputNumeric = Convert.ToInt32(input);
                    _optionsForTurn.Add(inputNumeric);
                    currentPlayerDecision.GetComponent<TextMeshProUGUI>().text = (_optionsForTurn.Count + 1).ToString();
                }
                catch (Exception) {
                    // ignored
                }
            }
            inputFieldText.GetComponent<TextMeshProUGUI>().text = " ";
        }

        private void CalculateOrder() {
            //var order = GameObject.Find("MainMenuController").GetComponent<GameMaster.GameMaster>().gameOrder;
            //var order = new List<int>();
            
            for (var j = 0; j < _optionsForTurn.Count; j++) {
                var nearest = 0;
                var nearestIndex = 0;

                for (var i = 0; i < _optionsForTurn.Count; i++) {
                    if (!order.Contains(i)) {
                        nearest = Math.Abs(_optionsForTurn[i]-_randomNumberForOrder);
                        break;
                    }
                }
                
                for (var i = 0; i < _optionsForTurn.Count; i++) {
                    if (!order.Contains(i)) {
                        var secondNearest = Math.Abs(_optionsForTurn[i]-_randomNumberForOrder);
                        if (secondNearest<=nearest) {
                            nearest = secondNearest;
                            nearestIndex = i;
                        }
                    }
                }
                
                order.Add(nearestIndex);
                
            }
            
            foreach (var i in order) {
                Debug.Log(i);
            }
            DisplayOrder();
            _gameMaster.currentOrderIndex = 0;
            currentPlayer.GetComponent<TextMeshProUGUI>().text = "P" +  (order[_gameMaster.currentOrderIndex] + 1);
        }

        private void DisplayOrder() {
            var btnListener = btn.GetComponent<Button>();
            btnListener.onClick.RemoveAllListeners();
            btnListener.onClick.AddListener(CloseWindow);
            btnText.GetComponent<TextMeshProUGUI>().text = "Close";
            randomNumberFinal.GetComponent<TextMeshProUGUI>().text = _randomNumberForOrder.ToString();
            
            for (var i = 0; i < order.Count; i++) {
                playerOrders[i].SetActive(true);
                playerOrders[i].GetComponent<TextMeshProUGUI>().text = "P" + (order[i]+1);
            }
        }

        private void CloseWindow() {
            numericMethodWindow.SetActive(false);
        }


        private GameObject CreateNewCell(int x, int y) {
            var newCell = Instantiate(cellPrefab, transform);
            var newTransform = newCell.GetComponent<Transform>();
            newTransform.position = new Vector2(x, y);
            var cell = newCell.GetComponent<BoardCell>();
            cell.BoardController = this;
            return newCell;
        }

        public void RollDicesAndPlay() {
            var firstDice = Random.Range(1, 6);
            var secondDice = Random.Range(1, 6);

            var player = _gameMaster._players[order[_gameMaster.currentOrderIndex]].GetComponent<Player>();

            if (player.turnCooldown==1) {
                player.turnCooldown = 0;
                player.CellBehavior();
            }
            else {
                if (firstDice==6 || secondDice == 6) {
                    Debug.Log("Bowser LOSES");
                }
                else {
                
                    Debug.Log(firstDice+secondDice);
                    player._currentIndex += firstDice+secondDice;
                    player._lockMove = false;
                }    
            }
        }

    }
}