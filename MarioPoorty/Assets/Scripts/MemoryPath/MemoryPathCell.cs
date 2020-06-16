using System;
using TicTacToeGame;
using TMPro;
using UnityEngine;

namespace MemoryPath {
    public class MemoryPathCell : MonoBehaviour {

        public bool interact = false;
        
        [SerializeField]
        private ImagesContainer imagesContainer;

        public MemoryPathController MemoryPathController { get; set; }

        public bool IsChoice { get; set; }
        
        private void OnMouseDown() {
            if (!interact) return;
            if (MemoryPathController.gameOverText.activeSelf) return;
            
            if (IsChoice) {
                gameObject.GetComponent<SpriteRenderer>().sprite = imagesContainer.circleImage;
                MemoryPathController.SetRowUninteractable();
                if (MemoryPathController.currentRow==-1) {
                    MemoryPathController.Win();
                }
                else {
                    MemoryPathController.SetRowInteractable();
                }
            }
            else {
                gameObject.GetComponent<SpriteRenderer>().sprite = imagesContainer.xImage;
                var textMesh = MemoryPathController.attemptsText.GetComponent<TextMeshProUGUI>();
                var newValue = Convert.ToInt32(textMesh.text) - 1;
                if (newValue<=0) {
                    MemoryPathController.GameOver();
                }
                else {
                    textMesh.text = newValue.ToString();
                }
            }
        }
    }
}