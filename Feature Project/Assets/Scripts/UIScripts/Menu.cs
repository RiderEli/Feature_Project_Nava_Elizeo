using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
/* [Nava, Elizeo]
 * [December 7, 2023]
 * [This is the Script for the Main Menu]
 */
public class Menu : MonoBehaviour
{
    //Starts the game
    public void StartButton()
    {
        SceneManager.LoadScene("Feature");
    }
    //Goes to a tutorial screen
    public void HowToPlayButton()
    {
        SceneManager.LoadScene("HowToPlay");
    }
    //Quits the game
    public void QuitButton()
    {
        Application.Quit();
    }
    //Goes back to the Menu scene
    public void BackButton()
    {
        SceneManager.LoadScene("Menu");
    }

}
