using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Extensions;
using TMPro;

public class IntroScreen : MonoBehaviour
{
    [SerializeField] private float flt_waitTime = 1.5f;
    public TextMeshProUGUI statusText;
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase is ready!");
                statusText.text = "Firebase is ready!";
                StartCoroutine(LoadNextSceneAfterDelay()); // Chỉ load khi Firebase ok
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {dependencyStatus}");
                statusText.text = $"Firebase error: {dependencyStatus}";
            }
        });
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(flt_waitTime);
        GameManager.instance.LoadScene("MainScene");
    }
}
