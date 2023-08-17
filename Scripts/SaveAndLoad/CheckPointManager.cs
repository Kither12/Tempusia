using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance;
    public CinemachineVirtualCamera cineCamera;

    private Image cover;
    private List<CheckPoint> checkpoints;
    private PlayerInput input;
    private GameObject player;
    private bool isInCheckPointSelection;
    private int resultCheckpointIndex;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("DM có 2 checkPointManager");
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        input = KeyboardManager.input;
        input.Enable();
        cover = GameManager.instance.cover;

        checkpoints = new List<CheckPoint>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).GetComponent<CheckPoint>().gameData != null)
            {
                checkpoints.Add(transform.GetChild(i).GetComponent<CheckPoint>());
            }
        }

        SortCheckpoints();

        input.normal.CheckPoint.performed += CheckPointPerformed;
        input.normal.Exit.performed += ExitPerformed;
        input.normal.RightArrow.performed += RightPerform;
        input.normal.LeftArrow.performed += LeftPerform;
        input.normal.Enter.performed += EnterPerform;

        DataManager.instance.getStartData();
    }
    private void OnDisable()
    {
        input.normal.CheckPoint.performed -= CheckPointPerformed;
        input.normal.Exit.performed -= ExitPerformed;
        input.normal.RightArrow.performed -= RightPerform;
        input.normal.LeftArrow.performed -= LeftPerform;
        input.normal.Enter.performed -= EnterPerform;
    }
    private void CheckPointPerformed(InputAction.CallbackContext e)
    {
        if (checkpoints.Count > 0 && !isInCheckPointSelection && !GameManager.instance.isGameOver && !GameManager.instance.isInGameMenu && !GameManager.instance.isShowingInteractCanvas && cover.color.a == 0)
        {
            ActiveSelectionCheckPoint();
        }
        else if (isInCheckPointSelection && !GameManager.instance.isInGameMenu)
        {
            DeactiveSelectionCheckPoint();
        }
    }
    private void ExitPerformed(InputAction.CallbackContext e)
    {
        if (isInCheckPointSelection && !GameManager.instance.isInGameMenu)
        {
            DeactiveSelectionCheckPoint();
        }
    }
    private void RightPerform(InputAction.CallbackContext e)
    {
        nextCheckPoint(1);
    }
    private void LeftPerform(InputAction.CallbackContext e)
    {
        nextCheckPoint(-1);
    }
    private void EnterPerform(InputAction.CallbackContext e)
    {
        if (isInCheckPointSelection && !GameManager.instance.isInGameMenu)
        {
            StartCoroutine(selectCurrentCheckPointFade());
        }
    }
    public int checkPointCount()
    {
        return checkpoints.Count;
    }
    private void SortCheckpoints()
    {
        checkpoints.Sort((x, y) => { return x.gameObject.transform.position.x.CompareTo(y.gameObject.transform.position.x); });
    }
    public void AddCheckPoint(CheckPoint checkpoint)
    {
        checkpoints.Add(checkpoint);
        SortCheckpoints();
    }
    public void ActiveSelectionCheckPoint()
    {
        isInCheckPointSelection = true;

        //Binary Search for the closest checkpoint to the player
        int start = 0;
        int end = checkpoints.Count - 1;
        float minDistance = 1e9f;
        resultCheckpointIndex = 0;
        while (start <= end)
        {
            int mid = (start + end) >> 1;
            if (Vector2.Distance(checkpoints[mid].transform.position, player.transform.position) < minDistance)
            {
                minDistance = Mathf.Abs(checkpoints[mid].transform.position.x - player.transform.position.x);
                resultCheckpointIndex = mid;
            }
            if (checkpoints[mid].transform.position.x >= player.transform.position.x)
            {
                end = mid - 1;
            }
            else start = mid + 1;
        }

        cineCamera.Follow = checkpoints[resultCheckpointIndex].transform;
        cineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 0f;
        cineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0f;
        Time.timeScale = 0;
    }
    public void DeactiveSelectionCheckPoint()
    {
        cineCamera.Follow = player.transform;
        cineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 0.2f;
        cineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0.1f;
        isInCheckPointSelection = false;
        Time.timeScale = 1;
    }
    public void nextCheckPoint(int index)
    {
        if (isInCheckPointSelection && !GameManager.instance.isInGameMenu)
        {
            resultCheckpointIndex += index;
            if (resultCheckpointIndex >= 0 && resultCheckpointIndex < checkpoints.Count)
            {
                cineCamera.Follow = checkpoints[resultCheckpointIndex].transform;
                cineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 0f;
                cineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0f;
            }
            resultCheckpointIndex = Mathf.Clamp(resultCheckpointIndex, 0, checkpoints.Count - 1);
        }
    }
    public void selectCurrentCheckPoint()
    {
        DataManager.instance.LoadDataToWorld(checkpoints[resultCheckpointIndex].gameData);
        player.transform.position = checkpoints[resultCheckpointIndex].gameObject.transform.position;
        player.GetComponent<RewindRecorder>().endRewind();
        player.GetComponent<PlayerCloneManager>().canCreateClone = true;
        GameObject[] clones = GameObject.FindGameObjectsWithTag("PlayerClone");
        foreach (GameObject clone in clones)
        {
            Destroy(clone);
        }
        player.transform.parent = null;
        DeactiveSelectionCheckPoint();
    }
    IEnumerator selectCurrentCheckPointFade()
    {
        isInCheckPointSelection = false;
        while (cover.color.a < 1)
        {
            cover.color = new Color(cover.color.r, cover.color.g, cover.color.b, Mathf.Min(cover.color.a + Time.unscaledDeltaTime, 1));
            yield return null;
        }
        selectCurrentCheckPoint();
        while (cover.color.a > 0)
        {
            cover.color = new Color(cover.color.r, cover.color.g, cover.color.b, Mathf.Max(cover.color.a - Time.unscaledDeltaTime, 0));
            yield return null;
        }
    }
}
