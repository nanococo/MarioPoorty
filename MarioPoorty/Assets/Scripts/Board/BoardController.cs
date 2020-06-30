using System;
using System.Collections.Generic;
using Board.Character;
using TMPro;
using UnityEngine;
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
        public TextMeshProUGUI actualLogText;

        [SerializeField] private GameObject rollBtnTxt;
        [SerializeField] private GameObject currentPlayer;
        [SerializeField] private GameObject currentPlayerTextLeft;
        [SerializeField] private GameObject rollBtn;
        [SerializeField] private GameObject dice1;
        [SerializeField] private GameObject dice2;
        [SerializeField] private GameObject finishText;
        [SerializeField] private GameObject gameEventsText;
        
        
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
        
        //Dices method
        [SerializeField] private GameObject dicesMethodWindow;
        [SerializeField] private GameObject dicesMethodRollBtn;
        [SerializeField] private GameObject dicesMethodRollBtnText;
        [SerializeField] private GameObject dicesMethodDice1;
        [SerializeField] private GameObject dicesMethodDice2;
        [SerializeField] private GameObject[] dicesMethodOrderNumbers;
        [SerializeField] private GameObject[] dicesMethodPlayerOrders;
        [SerializeField] private GameObject dicesMethodCurrentPlayerDecision;


        //private readonly List<int> order = new List<int>();
        private List<int> order;

        private GameMaster.GameMaster _gameMaster;
        
        private void Start() {
            _gameMaster = GameObject.Find("GameMasterController").GetComponent<GameMaster.GameMaster>();
            _gameMaster.BoardController = this;
            actualLogText = gameEventsText.GetComponent<TextMeshProUGUI>();
            actualLogText.text = _gameMaster.logOfEvents;
            
            order = _gameMaster.gameOrder;
            try {
                _numberOfPlayers = _gameMaster.numberOfPlayers;
            } catch (Exception) {
                Debug.Log("Default Number of Players loaded (2)");
                _numberOfPlayers = 2;
            }

            _randomNumberForOrder = Random.Range(1, 1001);
            //_randomNumberForOrder = 632;
            
            
            _gameMaster.DisplayBoard();
            _gameMaster.DisplayPlayers();
            
            currentPlayer.SetActive(false);
            currentPlayerTextLeft.SetActive(false);
            rollBtn.SetActive(false);
            dice1.SetActive(false);
            dice2.SetActive(false);
            finishText.SetActive(false);
            
            numericMethodWindow.SetActive(false);
            dicesMethodWindow.SetActive(false);

            if (order.Count>0) {
                currentPlayer.SetActive(true);
                currentPlayerTextLeft.SetActive(true);
                rollBtn.SetActive(true);
                dice1.SetActive(true);
                dice2.SetActive(true);
            }
            else {
                var rand = Random.Range(0, 2);

                if (rand==1) {
                    dicesMethodWindow.SetActive(true);
                    InitializeUiElementsDicesMethod();
                }
                else {
                    numericMethodWindow.SetActive(true);
                    InitializeUiElementsNumericMethod();
                }
            }
        }

        

        private void Update() {
            try {
                var player = _gameMaster._players[order[_gameMaster.currentOrderIndex]].GetComponent<Player>();
                currentPlayer.GetComponent<TextMeshProUGUI>().text = "P" +  (order[_gameMaster.currentOrderIndex] + 1);
                if (!player.needUpdateUiOnBoard) return;



                if (player.numberOfTurnsToLose>0) {
                    rollBtnTxt.GetComponent<TextMeshProUGUI>().text = "No Turn";
                }
                else {
                    if (player.turnCooldown == 0) {
                        rollBtnTxt.GetComponent<TextMeshProUGUI>().text = "Roll";
                    }
                    else if (player.turnCooldown == 1) {
                        rollBtnTxt.GetComponent<TextMeshProUGUI>().text = "Try Again";
                    }
                    else {
                        rollBtnTxt.GetComponent<TextMeshProUGUI>().text = "No Turn";
                    }
                }
                
                player.needUpdateUiOnBoard = false;

                
            }
            catch (Exception) {
                //Ignored
            }
        }

        private void InitializeUiElementsNumericMethod() {
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
        
        private void InitializeUiElementsDicesMethod() {
            for (var i = 0; i < 6; i++) {
                dicesMethodPlayerOrders[i].SetActive(false);
                dicesMethodOrderNumbers[i].SetActive(false);
            }

            for (var i = 0; i < _numberOfPlayers; i++) {
                dicesMethodOrderNumbers[i].SetActive(true);
                dicesMethodPlayerOrders[i].SetActive(true);
            }
            dicesMethodRollBtn.GetComponent<Button>().onClick.AddListener(RollDiceMethod);
        }

        private void RollDiceMethod() {
            var firstDice = Random.Range(1, 6);
            var secondDice = Random.Range(1, 6);

            dicesMethodDice1.GetComponent<TextMeshProUGUI>().text = firstDice.ToString();
            dicesMethodDice2.GetComponent<TextMeshProUGUI>().text = secondDice.ToString();
            
            
            
            if (_optionsForTurn.Count == _numberOfPlayers-1) {
                _optionsForTurn.Add(firstDice+secondDice);
                dicesMethodRollBtnText.GetComponent<TextMeshProUGUI>().text = "Reveal";
                var btnListener = dicesMethodRollBtn.GetComponent<Button>();
                btnListener.onClick.RemoveAllListeners();
                btnListener.onClick.AddListener(CalculateOrderDiceMethod);
            } else if (_optionsForTurn.Count < _numberOfPlayers) {
                _optionsForTurn.Add(firstDice+secondDice);
                dicesMethodCurrentPlayerDecision.GetComponent<TextMeshProUGUI>().text = (_optionsForTurn.Count + 1).ToString();
            }
        }

        private void CalculateOrderDiceMethod() {
            for (var i = 0; i < _numberOfPlayers; i++) {
                var compare = 0;
                var highIndex = 0;
                for (var j = 0; j < _optionsForTurn.Count; j++) {
                    if (_optionsForTurn[j]>compare) {
                        compare = _optionsForTurn[j];
                        highIndex = j;
                    }
                }
                
                order.Add(highIndex);
                _optionsForTurn[highIndex] = 0;
            }
            DisplayOrderDiceMethod();
        }

        private void GetValueFromInput() {
            if (string.IsNullOrEmpty(inputFieldText.GetComponent<TextMeshProUGUI>().text)) return;
            
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

        private void DisplayOrderDiceMethod() {
            var btnListener = dicesMethodRollBtn.GetComponent<Button>();
            btnListener.onClick.RemoveAllListeners();
            btnListener.onClick.AddListener(CloseWindow);
            dicesMethodRollBtnText.GetComponent<TextMeshProUGUI>().text = "Close";

            for (var i = 0; i < order.Count; i++) {
                dicesMethodPlayerOrders[i].SetActive(true);
                dicesMethodPlayerOrders[i].GetComponent<TextMeshProUGUI>().text = "P" + (order[i]+1);
            }
        }

        private void CloseWindow() {
            numericMethodWindow.SetActive(false);
            dicesMethodWindow.SetActive(false);
            currentPlayer.SetActive(true);
            currentPlayerTextLeft.SetActive(true);
            rollBtn.SetActive(true);
            dice1.SetActive(true);
            dice2.SetActive(true);
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
            
            dice1.GetComponent<TextMeshProUGUI>().text = firstDice.ToString();
            dice2.GetComponent<TextMeshProUGUI>().text = secondDice.ToString();
            
            
            var player = _gameMaster._players[order[_gameMaster.currentOrderIndex]].GetComponent<Player>();
            
            
            if (player.numberOfTurnsToLose>0) {
                dice1.GetComponent<TextMeshProUGUI>().text = "-";
                dice2.GetComponent<TextMeshProUGUI>().text = "-";
                player.numberOfTurnsToLose--;
                _gameMaster.logOfEvents += "\n" + "P" + (order[_gameMaster.currentOrderIndex] + 1) + " lost 1 turn.";
                actualLogText.text = _gameMaster.logOfEvents;
                _gameMaster.TurnChange();
                if (player.numberOfTurnsToLose<=0) {
                    player.needUpdateUiOnBoard = true;
                }
            }
            else {
                _gameMaster.logOfEvents += "\n" + "P" + (order[_gameMaster.currentOrderIndex] + 1) + " moved " + (firstDice+secondDice) + " spaces.";
                actualLogText.text = _gameMaster.logOfEvents;
                
                if (player.turnCooldown==1) {
                    player.turnCooldown = 0;
                    player.CellBehavior();
                }
                else {
                    if (firstDice==6 || secondDice == 6) {
                        Debug.Log("Bowser LOSES");
                    }
                    else {
                    
                        if (player._currentIndex+firstDice+secondDice>_gameMaster._board.Length-1) {
                            player.retrogressionValue = (_gameMaster._board.Length-1) - (firstDice + secondDice-1);
                            player._currentIndex = _gameMaster._board.Length-1;
                            player._lockMove = false;
                        } else if (player._currentIndex+firstDice+secondDice==_gameMaster._board.Length-1) {
                            player._currentIndex = _gameMaster._board.Length-1;
                            player._lockMove = false;
                            EndGame();
                        }
                        else {
                            player._currentIndex += firstDice+secondDice-1;
                            player._lockMove = false;    
                        }
                    
                    }    
                }    
            }
        }

        public void EndGame() {
            finishText.SetActive(true);
            rollBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}