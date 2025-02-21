using System.Linq;
using UnityEngine;

public class Targetter : Passive
{
    private Camera _mainCamera;
    [SerializeField] private float attackDistance = 5;
    private Targettable _targetted;
    
    public Targettable Targetted
    {
        get => _targetted;
        set
        {
            if (_targetted == value) return;
            
            if (_targetted && _targetted.Selectable)
            {
                _targetted.transform.GetChild(0).gameObject.SetActive(false);
            }
            
            _targetted = value;
            
            if (_targetted && _targetted.Selectable)
            {
                _targetted.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public override void Effect(PlayerInteraction playerInteraction)
    {
        HandleTargettedSelection(playerInteraction);
    }

    private void HandleTargettedSelection(PlayerInteraction player)
    {
        if (!_mainCamera) return;
        
        Targettable target = null;
            
        var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var direction = mousePos - player.transform.position;
        // find if there is a collider on point
        var onPoint = Physics2D.OverlapPoint(mousePos);
            
        // find if collider is targettable
        if (onPoint)
        {
            onPoint.TryGetComponent(out target);
        }
            
        // find if there is a collider in the way
        var walls = new RaycastHit2D[5];
        var size = Physics2D.Raycast(player.transform.position, 
            direction.normalized, 
            new ContactFilter2D().NoFilter(), 
            walls, 
            attackDistance);

        if (size > 0)
        {
            // if collider is a wall
            var closestWall = walls.Where(x => x && x.transform.gameObject.CompareTag("Wall"))
                .OrderBy(x => Vector2.Distance(player.transform.position, x.transform.position))
                .FirstOrDefault();
                
            // if wall is targettable
            if (closestWall && !closestWall.transform.TryGetComponent(out target))
            {
                var point = closestWall.point;
                var closest = float.MaxValue;
                    
                // get closest
                foreach (Transform child in closestWall.transform)
                {
                    if (!child.TryGetComponent(out Targettable temp)) continue;
                    var challenger = Vector2.Distance(point, temp.transform.position);
                    if (challenger > closest) continue;
                        
                    closest = challenger;
                    target = temp;
                }
            }
        }

        Targetted = target;
    }
}