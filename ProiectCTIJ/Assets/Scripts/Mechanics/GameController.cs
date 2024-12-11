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
        private int enemiesKilled = 0;

        // UI Elements
        public TextMeshProUGUI coinText;

        // Shop
        public GameObject shopPanel;
        public Button openShopButton;
        public Button redButton;
        public Button blueButton;
        public Button greenButton;
        public Button closeShopButton; // Button to close the shop
        public GameObject player;

        // Quests
        public GameObject questPanel;
        public Button quest1Button; // Button for "Kill 3 Enemies"
        public Button quest2Button; // Button for "Collect 10 Coins"
        public TextMeshProUGUI quest1Status; // Status text for quest 1
        public TextMeshProUGUI quest2Status; // Status text for quest 2

        private bool quest1Completed = false;
        private bool quest2Completed = false;

        void OnEnable()
        {
            Instance = this;

            // Shop Button Listeners
            if (openShopButton != null) openShopButton.onClick.AddListener(OpenShop);
            if (redButton != null) redButton.onClick.AddListener(() => TryChangePlayerColor(Color.red));
            if (blueButton != null) blueButton.onClick.AddListener(() => TryChangePlayerColor(Color.blue));
            if (greenButton != null) greenButton.onClick.AddListener(() => TryChangePlayerColor(Color.green));
            if (closeShopButton != null) closeShopButton.onClick.AddListener(CloseShop);

            // Quest Button Listeners
            if (quest1Button != null) quest1Button.onClick.AddListener(CompleteQuest1);
            if (quest2Button != null) quest2Button.onClick.AddListener(CompleteQuest2);

            // Initialize UI
            shopPanel.SetActive(false); // Ensure shop starts closed
            DeselectButton(); // Deselect UI buttons

            // Initialize quest buttons
            quest1Button.interactable = false;
            quest2Button.interactable = false;

            UpdateQuestStatus();
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();

            // Prevent spacebar from triggering buttons unintentionally
            if (Input.GetKeyDown(KeyCode.Space)) DeselectButton();

            // Check quest progress and enable buttons if not completed
            if (enemiesKilled >= 3 && !quest1Completed)
                quest1Button.interactable = true;

            if (coinsCollected >= 10 && !quest2Completed)
                quest2Button.interactable = true;

            UpdateQuestStatus();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                questPanel.SetActive(!questPanel.activeSelf); // Toggle panel visibility.
            }
        }

        public void CollectCoin()
        {
            coinsCollected++;
            UpdateCoinUI();
        }

        public void KillEnemy()
        {
            enemiesKilled++;
        }

        private void UpdateCoinUI()
        {
            if (coinText != null)
            {
                coinText.text = coinsCollected.ToString();
            }
        }

        private void UpdateQuestStatus()
        {
            quest1Status.text = $"Kill 3 Enemies: {enemiesKilled}/3";
            quest2Status.text = $"Collect 10 Coins: {coinsCollected}/10";
        }

        private void CompleteQuest1()
        {
            if (enemiesKilled >= 3 && !quest1Completed)
            {
                coinsCollected += 10; // Reward 10 coins
                UpdateCoinUI();

                quest1Button.interactable = false; // Disable the button
                quest1Completed = true; // Mark quest as completed
                Debug.Log("Quest 1 Completed!");
            }
        }

        private void CompleteQuest2()
        {
            if (coinsCollected >= 10 && !quest2Completed)
            {
                coinsCollected += 10; // Reward 10 coins
                UpdateCoinUI();

                quest2Button.interactable = false; // Disable the button
                quest2Completed = true; // Mark quest as completed
                Debug.Log("Quest 2 Completed!");
            }
        }

        public void OpenShop()
        {
            shopPanel.SetActive(true);
            DeselectButton(); // Ensure no button is preselected
        }

        private void TryChangePlayerColor(Color newColor)
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

        private void ChangePlayerColor(Color newColor)
        {
            SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
            if (playerSpriteRenderer != null)
            {
                playerSpriteRenderer.color = newColor;
            }
        }

        private bool DeductCoins(int amount)
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

        private void CloseShop()
        {
            shopPanel.SetActive(false); // Hide the shop panel
        }

        private void DeselectButton()
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
