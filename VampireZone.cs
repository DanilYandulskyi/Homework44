using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VampireZone : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private int _stealingHealth;
    [SerializeField] private float _radius;
    [SerializeField] private Color _changingColor;
    [SerializeField] private float _waitTime;
    
    private Color _defaultColor;
    private Coroutine _coroutine;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightControl))
        {
            _image.color = _changingColor;

            HandleColliders();
        }
    }

    private IEnumerator StealHealth(Enemy enemy)
    {
        yield return new WaitForSeconds(_waitTime);

        enemy.TakeDamage(_stealingHealth);
        _player.Heal(_stealingHealth);

        _image.color = _defaultColor;
        _coroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    private void HandleColliders()
    {
        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(StealHealth(GetEnemy()));
        }
    }

    private Enemy GetEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out Enemy enemy))
            {
                return enemy;
            }
        }

        return null;
    }
}