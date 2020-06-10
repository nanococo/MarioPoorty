using System;
using UnityEngine;

namespace TicTacToeGame {
    public class Cell : MonoBehaviour {

        public ImagesContainer imagesContainer;
        //private TicTacToeMarks _ticTacToeMarks = TicTacToeMarks.XMark;
        
        void OnMouseEnter() {
            gameObject.GetComponent<SpriteRenderer>().sprite = imagesContainer.xImage;
        }

        private void OnMouseExit() {
            gameObject.GetComponent<SpriteRenderer>().sprite = imagesContainer.emptyImage;
        }
    }
}