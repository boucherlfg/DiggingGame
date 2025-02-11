using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private GameObject textIndicator;
    [SerializeField] private ParticleSystem jetpackParticle;
    [SerializeField] private ParticleSystem destroyParticle;
    [SerializeField] public float moveSpeed = 3;
    [SerializeField] public float attackInterval = 1f;
    [SerializeField] public float attackDistance = 2;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private Foot foot;
    [SerializeField] private float jetpackForce = 5;
    [SerializeField] private float jetpackTime = 3;
    private float _jetpackTimer = 0;
    [SerializeField] private GameObject selectorFrame;
    private Rigidbody2D _rigidbody;
    private Camera _mainCamera;
    private bool _canJump = true;
    private float _attackTimer;
    private List<string> _inventory = new();
    private GameObject _targetted;

    private GameObject targetted
    {
        get => _targetted;
        set
        {
            if (_targetted == value) return;
            if (_targetted && _targetted.transform.childCount > 0)
            {
                _targetted.transform.GetChild(0).gameObject.SetActive(false);
            }
            
            _targetted = value;
            
            if (!_targetted) return;
            
            if (_targetted.transform.childCount > 0)
            {
                _targetted.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
    private void Start()
    {
        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
        foot.onFloorDetected.AddListener(HandleFloorDetected);
    }

    private void HandleFloorDetected()
    {
        _canJump = true;
    }

    private void Update()
    {
        if (_canJump)
        {
            _jetpackTimer = Mathf.Min(jetpackTime, _jetpackTimer + Time.deltaTime * 5);
            Events.OnEnergyChanged.Invoke(_jetpackTimer / jetpackTime);
        }
        if (Input.GetButton("Jump") && _jetpackTimer > 0)
        {
            var emission = jetpackParticle.emission;
            emission.rateOverTime = 30;
            
            _canJump = false;
            _rigidbody.linearVelocity += Vector2.up * jetpackForce * Time.deltaTime;
            _rigidbody.linearVelocity = Vector2.ClampMagnitude(_rigidbody.linearVelocity, moveSpeed);
            _jetpackTimer -= Time.deltaTime;
            Events.OnEnergyChanged.Invoke(_jetpackTimer / jetpackTime);
        }
        else if (!Input.GetButton("Jump"))
        {
            var emission = jetpackParticle.emission;
            emission.rateOverTime = 0;

        }

        HandleInteract();
    }

    void HandleInteract()
    {
        var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var direction = mousePos - transform.position;
        var walls = new RaycastHit2D[5];
        var size = Physics2D.Raycast(transform.position, 
            direction.normalized, 
            new ContactFilter2D().NoFilter(), 
            walls, 
            direction.magnitude);
        
        if (size <= 0) return;
        
        var closestWall = walls.Where(x => x && x.transform.CompareTag("Wall"))
            .OrderBy(x => Vector2.Distance(transform.position, x.transform.position))
            .FirstOrDefault();
        
        if (closestWall)
        {
            targetted = closestWall.transform.gameObject;
        }
        if (!Input.GetButton("Fire1")) return;
        
        _attackTimer += Time.deltaTime;
        if (_attackTimer <= attackInterval) return;
        _attackTimer = 0;
        
        var textPos = targetted.transform.position;
        textPos.z = -1;
        var text = Instantiate(textIndicator, textPos, Quaternion.identity);
        var res = targetted.GetComponent<ResourceScript>().resourceName;
        text.GetComponent<TMPro.TMP_Text>().text = res;
        _inventory.Add(res);
        Instantiate(destroyParticle, targetted.transform.position, Quaternion.identity);
        Destroy(targetted);
    }

    private void FixedUpdate()
    {
        var move = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, _rigidbody.linearVelocity.y);
        _rigidbody.linearVelocity = move;
    }
}