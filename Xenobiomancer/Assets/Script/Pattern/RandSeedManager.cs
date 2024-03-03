using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Patterns
{
    public class RandSeedManager : MonoBehaviour
    {
        private string currentSeed;
        public TMP_Text seedText;

        // Generates a new random seed using the system's current time
        public void GenerateSeed()
        {
            int seed = (int)System.DateTime.Now.Ticks; //using system ticks to get a random number to use as seed
            currentSeed = seed.ToString(); // Convert the seed to a string for display.
            //displaySeed(); // Update the UI to show the new seed
            Random.InitState(seed); // set the seed for the random number generator
        }

        // Set a specific seed value and update the random number generator.
        public void SetSeed(int seed)
        {
            currentSeed = seed.ToString(); // Store the provided seed as a string for display
            Random.InitState(seed); // Set the seed for the random number generator
            displaySeed(); // Update the UI to show the new seed
        }

        // Update the text element to display the current seed
        public void displaySeed()
        {
            seedText.text = "Seed : " + currentSeed;
        }
    }
}