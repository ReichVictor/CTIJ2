using Platformer.Core;
using Platformer.Model;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Platformer.Mechanics
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private int coinsCollected = 0;
        public TextMeshProUGUI coinText;

        public GameObject shopPanel;
        public Button openShopButton;
        public Button redButton;
        public Button blueButton;
        public Button greenButton;
        public Button closeShopButton; // New button to close the shop
        public GameObject player;

        void OnEnable()
        {
            Instance = this;

            openShopButton.onClick.AddListener(OpenShop);
            redButton.onClick.AddListener(() => TryChangePlayerColor(Color.red));
            blueButton.onClick.AddListener(() => TryChangePlayerColor(Color.blue));
            greenButton.onClick.AddListener(() => TryChangePlayerColor(Color.green));

            // Add listener for closing the shop panel
            closeShopButton.onClick.AddListener(CloseShop);

            shopPanel.SetActive(false);

            // Ensure no button is selected at the start
            DeselectButton();
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();

            // Prevent spacebar from triggering button actions
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Ensure that spacebar doesn't trigger any button press if a button is selected
                DeselectButton();
            }
        }

        public void CollectCoin()
        {
            coinsCollected++;
            UpdateCoinUI();
        }

        private void UpdateCoinUI()
        {
            if (coinText != null)
            {
                coinText.text = coinsCollected.ToString();
            }
        }

        public int GetCoinsCollected()
        {
            return coinsCollected;
        }

        public void OpenShop()
        {
            shopPanel.SetActive(true);
            // Ensure that no button is selected when the shop opens
            DeselectButton();
        }

        // Try to deduct coins and change color
        void TryChangePlayerColor(Color newColor)
        {
            if (DeductCoins(10)) // Deduct 10 coins if possible
            {
                ChangePlayerColor(newColor); // Change the player's color
            }
            else
            {
                Debug.Log("Not enough coins to unlock color.");
            }
        }

        void ChangePlayerColor(Color newColor)
        {
            SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
            if (playerSpriteRenderer != null)
            {
                playerSpriteRenderer.color = newColor;
            }
        }

        // Deduct coins if enough are available
        public bool DeductCoins(int amount)
        {
            if (coinsCollected >= amount)
            {
                coinsCollected -= amount;
                UpdateCoinUI();
                return true;
            }
            else
            {
                Debug.Log("Not enough coins!");
                return false;
            }
        }

        // Manually close the shop
        void CloseShop()
        {
            shopPanel.SetActive(false); // Hide the shop panel
        }

        // Deselect the currently selected button (prevents spacebar interaction)
        private void DeselectButton()
        {
            // Deselect the current selected UI element if any
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
