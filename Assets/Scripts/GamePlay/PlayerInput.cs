using UnityEngine;

enum PlayerAction
{
    Pause = 0,
    Play,
    UseScrewdrive,
    UseSaw,
}

class PlayerInput : MonoBehaviour
{
    private static readonly float holeRadius = 0.16f;
    private GamePlay gamePlay;
    public PlayerAction action { get; set; }

    public void Start() => gamePlay = GetComponent<GamePlay>();
    public void Update()
    {
//# if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            StartCoroutine(gamePlay.NextStageOrFinished());
//            return;
//        }
//#endif
        if (!Input.GetMouseButtonDown(0)) return;
        switch (action)
        {
            case PlayerAction.Play:
                SelectOrMoveScrew();
                break;
            case PlayerAction.UseScrewdrive:
                RemoveScrew();
                break;
            case PlayerAction.UseSaw:
                RemovePlate();
                break;
        }
    }
    public RaycastHit2D[] GetPlayerInputHit()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        return Physics2D.RaycastAll(worldPosition, Vector2.zero);
    }
    public void SelectOrMoveScrew()
    {
        Vector3? newPos = null;
        foreach (var hit in GetPlayerInputHit())
        {
            if (hit.collider.TryGetComponent(out Screw s))
            {
                gamePlay.SelectScrew(s);
                return;
            }
            if (hit.collider.TryGetComponent(out Hole h) && !h.isPlateHole)
            {
                if (h.isExtra)
                {
                    gamePlay.UnlockExtraHole(h);
                    return;
                }
                newPos = h.transform.position;
            }
        }
        if (newPos == null) return;
        var hits = Physics2D.CircleCastAll(newPos.Value, holeRadius, Vector2.zero);
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out Plate p))
            {
                bool okPlate = false;
                foreach (var hit2 in hits)
                    if (hit2.collider.TryGetComponent(out Hole h) && h.parent == p && Vector2.Distance(h.transform.position, newPos.Value) < 0.02f)
                    {
                        okPlate = true;
                        break;
                    }
                if (!okPlate) return;
            }
        }
        gamePlay.MoveScrew(newPos.Value);
    }
    public void RemoveScrew()
    {
        foreach (var hit in GetPlayerInputHit())
            if (hit.collider.TryGetComponent(out Screw s))
            {
                gamePlay.RemoveScrew(s);
                return;
            }
    }
    public void RemovePlate()
    {
        Plate p = null;
        int maxLayer = -1;
        foreach (var hit in GetPlayerInputHit())
        {
            if (hit.collider.TryGetComponent(out Screw s))
                return;
            if (hit.collider.TryGetComponent(out Plate tempPlate) && maxLayer < tempPlate.layer)
            {
                maxLayer = tempPlate.layer;
                p = tempPlate;
            }
        }
        if (p != null)
            gamePlay.RemovePlate(p);
    }
}