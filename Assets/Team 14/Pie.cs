using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Team14
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Pie : MonoBehaviour
    {
        public enum State { Held, Fire }

        public Action OnFire = () => { };
        public Action OnReset = () => { };

        [SerializeField] private float _moveSpeed = 5;
        [SerializeField] private float _fireSpeed = 5;

        private SpriteRenderer _sr;
        private Collider2D _collider;

        private float _minX;
        private float _maxX;
        private float _maxY;

        private Vector2 _startPos;

        private State _pieState;

        private Coroutine _routine;
        public State PieState 
        { 
            get { return _pieState; }
            set 
            {
                if (_routine != null) StopCoroutine(_routine);
                _pieState = value;
                switch (_pieState)
                {
                    case State.Fire:
                        OnFire();
                        break;
                }
            } 
        }

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();

            Vector2 bl = Camera.main.ViewportToWorldPoint(Vector2.zero);
            Vector2 ur = Camera.main.ViewportToWorldPoint(Vector2.one);

            _minX = bl.x;
            _maxX = ur.x;
            _maxY = ur.y;

            _startPos = transform.position;
        }

        private void Update()
        {
            HandleControls();
        }

        private void HandleControls()
        {
            if (PieState != State.Held) return;

            if (Input.GetButtonDown("Space"))
            {
                Fire();
            }
            
            float x = Input.GetAxis("Horizontal");
            if (InBounds())
                transform.Translate(new Vector3(x * _moveSpeed * Time.deltaTime, 0, 0));
        }

        private void Fire()
        {
            PieState = State.Fire;
            MinigameManager.Instance.PlaySound("Pie Throw");

            if (_routine != null) StopCoroutine(_routine);
            _routine = StartCoroutine(FireRoutine());
        }

        private IEnumerator FireRoutine()
        {
            Debug.Log("fire");
            while (InBounds())
            {
                transform.Translate(new Vector3(0, _fireSpeed * Time.deltaTime, 0));
                yield return null;
            }
            Reset();
        }

        private bool InBounds()
        {
            return transform.position.x <= _maxX 
                && transform.position.x >= _minX 
                && transform.position.y <= _maxY;
        }

        public void Reset()
        {
            transform.position = _startPos;
            PieState = State.Held;
            OnReset();
        }

        private void OnDestroy()
        {
            OnFire = null;
            OnReset = null;
        }
    }
}
