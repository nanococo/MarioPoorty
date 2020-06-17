using System;
using TicTacToeGame;
using TMPro;
using UnityEngine;

namespace GuessWho {
    
    public class GuessWhoCell : MonoBehaviour {

        public GuessWhoController GuessWhoController { set; get; }
        public bool IsBlocked { get; set; }

        [SerializeField] public ImagesContainer imagesContainer;
        

        private void OnMouseDown() {
            if(IsBlocked) return;
            
            gameObject.GetComponent<SpriteRenderer>().sprite = imagesContainer.emptyImage;
            var textMesh = GuessWhoController.attemptsText.GetComponent<TextMeshProUGUI>();
            var newValue = Convert.ToInt32(textMesh.text) - 1;
            if (newValue<=0) {
                GuessWhoController.LockAllCells();
            }
            else {
                IsBlocked = true;
            }
            textMesh.text = newValue.ToString();
        }
    }
}