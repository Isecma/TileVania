using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] AudioClip nextLevelSFX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(nextLevelSFX, transform.position);
        StartCoroutine(NextLevel());       
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);

    }
}
