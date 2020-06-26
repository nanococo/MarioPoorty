using System;
using TMPro;
using UnityEngine;

namespace MainMenu {
    public class MainMenuController : MonoBehaviour {
        [SerializeField] private GameObject numberOfPlayersText;

        public int NumberOfPlayers { get; private set; } = 2;

        public void IncreaseNumberOfPlayers() {
            if (NumberOfPlayers >= 6) return;
            
            NumberOfPlayers++;
            var numPlayersTextUi = numberOfPlayersText.GetComponent<TextMeshProUGUI>();
            numPlayersTextUi.text = (Convert.ToInt32(numPlayersTextUi.text) + 1).ToString();
        }

        public void DecreaseNumberOfPlayers() {
            if (NumberOfPlayers <= 2) return;

            NumberOfPlayers--;
            var numPlayersTextUi = numberOfPlayersText.GetComponent<TextMeshProUGUI>();
            numPlayersTextUi.text = (Convert.ToInt32(numPlayersTextUi.text) - 1).ToString();
        }
    }
}