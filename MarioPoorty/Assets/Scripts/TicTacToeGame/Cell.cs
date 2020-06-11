using UnityEngine;

namespace TicTacToeGame {
    public class Cell : MonoBehaviour {

        public ImagesContainer imagesContainer;
        public TicTacToeMarks CellMark { set; get; } = TicTacToeMarks.Empty;
        public TicTacToeController TicTacToeController { get; set; }
        
        void OnMouseEnter() {
            if (CellMark.Equals(TicTacToeMarks.Empty)) {
                switch (TicTacToeController.CurrentMark) {
                    case TicTacToeMarks.XMark:
                        gameObject.GetComponent<SpriteRenderer>().sprite = imagesContainer.xImage;
                        break;
                    case TicTacToeMarks.Circle:
                        gameObject.GetComponent<SpriteRenderer>().sprite = imagesContainer.circleImage;
                        break;
                }
            }
        }

        private void OnMouseExit() {
            if (CellMark.Equals(TicTacToeMarks.Empty)) {
                gameObject.GetComponent<SpriteRenderer>().sprite = imagesContainer.emptyImage;    
            }
        }

        private void OnMouseDown() {
            switch (TicTacToeController.CurrentMark) {
                case TicTacToeMarks.XMark:
                    CellMark = TicTacToeController.CurrentMark; 
                    gameObject.GetComponent<SpriteRenderer>().sprite = imagesContainer.xImage;
                    break;
                case TicTacToeMarks.Circle:
                    CellMark = TicTacToeController.CurrentMark; 
                    gameObject.GetComponent<SpriteRenderer>().sprite = imagesContainer.circleImage;
                    break;
            }
            TicTacToeController.TickTurn();
            Debug.Log(TicTacToeController.CheckWin());
        }
    }
}