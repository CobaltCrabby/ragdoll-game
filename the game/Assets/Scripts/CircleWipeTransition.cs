using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CircleWipeTransition : MonoBehaviour {
    
    private Animator animator;
    public string scene;

    void Start() {
        animator = GetComponent<Animator>();    
    }

    public void CircleWipe() {
        StartCoroutine(SceneTransitionString(scene));
    }

    public IEnumerator SceneTransitionString(string scene) {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(scene);
    }

    public IEnumerator SceneTransition() {
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
