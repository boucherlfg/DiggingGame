using UnityEngine;

public class PlayerVertical : MonoBehaviour
{
    [SerializeField] private bool isJetpack; 
    [SerializeField] private ParticleSystem jetpackParticle;
    [SerializeField] public float moveSpeed = 8;
    [SerializeField] private float jetpackForce = 5;
    [SerializeField] private float jetpackTime = 3;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private Foot foot;
    
    private float _jetpackTimer = 0;    
    private bool _canJump = true;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        foot.onFloorDetected.AddListener(HandleFloorDetected);
    }
    
    private void HandleFloorDetected()
    {
        _canJump = true;
    }

    private void Update()
    {
        if (isJetpack)
        {
            HandleJetpack();
        }
        else
        {
            HandleJump();
        }
    }

    void HandleJump()
    {
        var emission = jetpackParticle.emission;
        emission.rateOverTime = 0;
        if (!Input.GetButtonDown("Jump") || !_canJump) return;
        
        _canJump = false;
        _rigidbody.linearVelocity += Vector2.up * jumpForce;
    }

    void HandleJetpack()
    {
        if (_canJump)
        {
            _jetpackTimer = Mathf.Min(jetpackTime, _jetpackTimer + Time.deltaTime * 5);
            Events.OnEnergyChanged.Invoke(_jetpackTimer / jetpackTime);
        }
        if (Input.GetButton("Jump") && _jetpackTimer > 0)
        {
            _canJump = false;
            var emission = jetpackParticle.emission;
            emission.rateOverTime = 30;
            
            _rigidbody.linearVelocity += Vector2.up * (jetpackForce * Time.deltaTime);
            _rigidbody.linearVelocity = Vector2.ClampMagnitude(_rigidbody.linearVelocity, moveSpeed);
            _jetpackTimer -= Time.deltaTime;
            Events.OnEnergyChanged.Invoke(_jetpackTimer / jetpackTime);
        }
        else if (!Input.GetButton("Jump"))
        {
            var emission = jetpackParticle.emission;
            emission.rateOverTime = 0;

        }
    }

}