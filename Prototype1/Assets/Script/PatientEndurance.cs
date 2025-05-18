using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PatientEndurance : MonoBehaviour
{
    public Image EnduranceProgressBar;
    public UnityEvent OnEnduranceProgressStart;
    public UnityEvent OnEnduranceProgressOut;
    public float startEnduranceProgress;
    public float EnduranceProgressCount;
    public bool isStartEnduranceProgress = false;
    [SerializeField] private Canvas EnduranceProgressUI; 

    public void Start()
    {
        StartEnduranceProgress();
        EnduranceProgressUI.gameObject.SetActive(false);
    }

    public void StartEnduranceProgress()
    {
        OnEnduranceProgressStart.Invoke();

        isStartEnduranceProgress = true;
        EnduranceProgressCount = startEnduranceProgress;
    }

    private void FixedUpdate()
    {
        if (EnduranceProgressCount > 0 && isStartEnduranceProgress)
        {
            EnduranceProgressUI.gameObject.SetActive(true);
            EnduranceProgressCount -= Time.deltaTime;
            EnduranceProgressBar.fillAmount = EnduranceProgressCount * (1 / startEnduranceProgress);

            if (EnduranceProgressCount <= 0)
            {
                OnEnduranceProgressOut.Invoke();
                EnduranceProgressUI.gameObject.SetActive(false);
            }
        }
    }

    public void ProgressStart()
    {
        Debug.Log("Starting");
    }

    public void ProgressEnd()
    {
        Debug.Log("Died");
    }
}
