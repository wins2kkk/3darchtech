using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllBallFallingDirection : MonoBehaviour
{
    [SerializeField] private Transform _modelToFollow;
    [SerializeField] private ParticleSystem _waterFallingBall;
    [SerializeField] private float _gravityMagnitude = 9.8f;
    [SerializeField] private float _smoothTime = 0.1f;
    [SerializeField] private float _moveDistance = 2f;

    [SerializeField] private Transform _sphere;

    private Vector3[] _initialPositions;

    private Vector3 _currentGravityDirection;
    private Vector3 _gravityVelocity;
    private float _elapsedTime = 0f;

    private bool _isMoveUp; // kiểm tra đi lên 
    private bool _isDisableParticlesCollision;


    private bool _isSetUp;
    private void Awake()
    {
        //Debug.Log(transform.position);
    }

    private void Start()
    {
        _modelToFollow = GetComponentInParent<MeshRenderer>()?.GetComponent<Transform>();
        InitializeParticlePositions();
    }


    private void Update()
    {

        _modelToFollow = GetComponentInParent<MeshRenderer>()?.GetComponent<Transform>();
        if (_waterFallingBall == null || _modelToFollow == null)
        {
            return;
        }

        this.transform.position = _sphere.transform.position;

      Vector3 modelPosition = _modelToFollow.position;
        Vector3 targetPosition = modelPosition + _modelToFollow.transform.up * 0.2f;
        Debug.Log(_modelToFollow.transform.up);
        // Debug.Log("gege" + targetPosition);
        // Di chuyển các particle từ vị trí của model đến targetPosition
        if (!_isMoveUp && MoveParticlesHigher(targetPosition))
        {
            _isMoveUp = true;
            // tính thời gian chính xác rơi xuống  ==> nảy lên 1 lần r tắt collision
            StartCoroutine(DisableParticleCollision());
        }

        if (!_isMoveUp) return;

        if (_isDisableParticlesCollision) return;

        //SmoothGravity();

        // Cập nhật velocity của particles

        //   SetParticlesVelocity();

          ApplyGravityToEachParticle();
    }

    private IEnumerator DisableParticleCollision()
    {

        yield return new WaitForSeconds(0.58f);
        //ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        //_isDisableParticlesCollision = true;
        //foreach (ParticleSystem ps in particleSystems)
        //{
        //    var collisionModule = ps.collision;
        //    collisionModule.enabled = false; 
        //}
        SetZeroVelocity();
        
    }
    private void InitializeParticlePositions()
    {
        // Khởi tạo mảng lưu vị trí ban đầu của các particle
        _initialPositions = new Vector3[_waterFallingBall.main.maxParticles];
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_waterFallingBall.particleCount];
        _waterFallingBall.GetParticles(particles);

        for (int i = 0; i < particles.Length; i++)
        {
            _initialPositions[i] = _modelToFollow.transform.position; // Lưu vị trí ban đầu
        }
        Vector3 direction = _modelToFollow.transform.up.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

      //  transform.parent.transform.rotation = Quaternion.LookRotation(-direction);

        //transform.rotation= targetRotation;
        Debug.Log("dir " + direction);

        // Thiết lập velocityOverLifetime để di chuyển theo hướng direction
        //var velocityOverLifetime = _waterFallingBall.velocityOverLifetime;
        //velocityOverLifetime.enabled = true;

        //// Đảm bảo rằng mỗi thành phần x, y, z được gán với một MinMaxCurve
        //velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(direction.x);
        //velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(direction.y);
        //velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(direction.z);
        this.transform.position = _modelToFollow.transform.position + direction; 
        _sphere.transform.position = _modelToFollow.transform.position + direction;
    }

    private void SmoothGravity()
    {
        // Làm mượt hướng trọng lực
        Quaternion adjustedRotation = Quaternion.Euler(0, _modelToFollow.rotation.eulerAngles.y, 0);
        Vector3 targetGravityDirection = (adjustedRotation * _modelToFollow.transform.up);
        SmoothParticleGravity(targetGravityDirection);
    }

    private bool MoveParticlesHigher(Vector3 targetPosition)
    {
        _elapsedTime += Time.deltaTime*1.5f;
        float journeyLength = _moveDistance;
        float distanceCovered = Mathf.Min(_elapsedTime, journeyLength);
        float fractionOfJourney = distanceCovered / journeyLength;

        //ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_waterFallingBall.particleCount];
        //_waterFallingBall.GetParticles(particles);

        //Vector3 startPosition = targetPosition - _modelToFollow.transform.up * _moveDistance;

        //for (int i = 0; i < particles.Length; i++)
        //{

        //    //particles[i].position = Vector3.Lerp(_modelToFollow.transform.position, targetPosition, fractionOfJourney);
        //}
        Vector3 direction = _modelToFollow.transform.up.normalized;
        _sphere.transform.position =  Vector3.Lerp( _modelToFollow.transform.position,targetPosition,fractionOfJourney);

      //  _waterFallingBall.SetParticles(particles, particles.Length);
        //if (Vector3.Distance(this.transform.position, _modelToFollow.position + _modelToFollow.transform.up * 2) < 0.1f)
        //    return true;
                

        return fractionOfJourney >= 1f;
    }

    private void SmoothParticleGravity(Vector3 targetGravityDirection)
    {
        _currentGravityDirection = Vector3.SmoothDamp(
            _currentGravityDirection,
            targetGravityDirection,
            ref _gravityVelocity,
            _smoothTime
        );
    }

    private void SetParticlesVelocity()
    {
        var velocityOverLifetime = _waterFallingBall.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(_currentGravityDirection.x * _gravityMagnitude);
        velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(_currentGravityDirection.y * _gravityMagnitude);
        velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(_currentGravityDirection.z * _gravityMagnitude);
    }

    private void ApplyGravityToEachParticle()
    {
        //ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_waterFallingBall.particleCount];
        //_waterFallingBall.GetParticles(particles);

        //for (int i = 0; i < particles.Length; i++)
        //{
        //    // Lấy trọng lực đã làm mượt

        //    Vector3 modelPosition = _modelToFollow.localPosition;
        //    Vector3 targetPosition = modelPosition + _modelToFollow.transform.up * _moveDistance;


        //    Vector3 startPosition = targetPosition - _modelToFollow.transform.up * _moveDistance;

        //    Vector3 gravityForce = modelPosition + _modelToFollow.transform.up * _gravityMagnitude;

        //    // Áp dụng lực trọng lực vào vận tốc của từng hạt
        //    //particles[i].velocity += gravityForce * Time.deltaTime;
        //    particles[i].position = Vector3.Lerp(particles[i].position, startPosition, 3 * Time.deltaTime);
        //}

        //_waterFallingBall.SetParticles(particles, particles.Length);


        _sphere.transform.position = Vector3.Lerp(_sphere.transform.position, _modelToFollow.transform.position, 3 * Time.deltaTime);
    }

    private void SetZeroVelocity()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_waterFallingBall.particleCount];
        _waterFallingBall.GetParticles(particles);

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].velocity = Vector3.zero;
        }
    }
}
