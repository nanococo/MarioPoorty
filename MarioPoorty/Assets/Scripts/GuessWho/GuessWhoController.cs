using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GuessWho {
    public class GuessWhoController : MonoBehaviour {

        private int _imageIndex;
        private const int Size = 10;
        private GameObject[,] _board;
        private bool _win;

        [SerializeField] private GameObject image;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] public GameObject attemptsText;
        [SerializeField] public GameObject gameOverText;
        [SerializeField] public GameObject winText;

        private void Start() {
            attemptsText.GetComponent<TextMeshProUGUI>().text = Random.Range(4, 9).ToString();
            _board = new GameObject[Size, Size];
            gameOverText.SetActive(false);
            winText.SetActive(false);

            RandomImageLoad();
            DrawBoard();
        }

        private void DrawBoard() {
            var y = 0;
            for (var i = 0; i < Size; i++) {
                var x = 0;
                for (var j = 0; j < Size; j++) {
                    var newCell = Instantiate(cellPrefab, transform);
                    var newTransform = newCell.GetComponent<Transform>();
                    newCell.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    newTransform.position = new Vector2(x, y);
                    newCell.GetComponent<GuessWhoCell>().GuessWhoController = this;
                    x += 2;
                    _board[i, j] = newCell;

                }

                y -= 2;
            }
        }

        private void RandomImageLoad() {
            _imageIndex = Random.Range(0, 15);
            //var imageIndex =2;
            switch (_imageIndex) {
                case 0:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Boo");
                    image.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    break;
                case 1:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Bowser");
                    image.GetComponent<Transform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    break;
                case 2:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Daisy");
                    image.GetComponent<Transform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    break;
                case 3:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Dry_Bones");
                    image.GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    break;
                case 4:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Goomba");
                    image.GetComponent<Transform>().localScale = new Vector3(5f, 5f, 5f);
                    break;
                case 5:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Koopa");
                    image.GetComponent<Transform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
                    break;
                case 6:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Luigi");
                    image.GetComponent<Transform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    break;
                case 7:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Mario");
                    image.GetComponent<Transform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    break;
                case 8:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Peach");
                    image.GetComponent<Transform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    break;
                case 9:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Rosalina");
                    image.GetComponent<Transform>().localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    break;
                case 10:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Toad");
                    image.GetComponent<Transform>().localScale = new Vector3(4f, 4f, 4f);
                    break;
                case 11:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Toadette");
                    image.GetComponent<Transform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    break;
                case 12:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Waluigi");
                    image.GetComponent<Transform>().localScale = new Vector3(2.5f, 2.5f, 2.5f);
                    break;
                case 13:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Wario");
                    image.GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    break;
                case 14:
                    image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/GuessWho/Yoshi");
                    image.GetComponent<Transform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    break;
            }
        }

        public void GetOptionOnClick(int id) {
            if (_win) return;
            if (id == _imageIndex) {
                winText.SetActive(true);
                _win = true;
            }
            else {
                _win = true;
                gameOverText.SetActive(true);
            }

            RevealAll();
        }

        public void LockAllCells() {
            for (var i = 0; i < Size; i++) {
                for (var j = 0; j < Size; j++) {
                    _board[i, j].GetComponent<GuessWhoCell>().IsBlocked = true;
                }
            }
        }

        private void RevealAll() {
            for (var i = 0; i < Size; i++) {
                for (var j = 0; j < Size; j++) {
                    var cell =  _board[i, j].GetComponent<GuessWhoCell>(); 
                    cell.GetComponent<SpriteRenderer>().sprite = cell.imagesContainer.emptyImage;
                }
            }
        }
    }
}