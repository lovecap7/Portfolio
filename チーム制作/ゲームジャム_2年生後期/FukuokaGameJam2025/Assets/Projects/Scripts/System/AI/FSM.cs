using System;
using System.Collections.Generic;
using UnityEngine;

namespace mySystem.AI.FSM
{
    public abstract class State<T> : IState where T : MonoBehaviour
    {
        #region Constructor
        public State(StateMachine<T> owner)
        {
            Owner = owner;
            Context = owner.Context;
        }
        #endregion

        public T Context { get; private set; } = null;
        public StateMachine<T> Owner { get; private set; } = null;
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }

        #region Private Variables
        private Dictionary<int, Type> _transitionMap = new Dictionary<int, Type>();
        #endregion

        #region Call StateMachine Functions
        public void AddTransition(int eventId, Type stateType)
        {
            var hasEvent = _transitionMap.ContainsKey(eventId);
            if (hasEvent == true)
            {
                Debug.LogError($"既にイベントが登録されています[{stateType.Name}]");
                return;
            }
            _transitionMap[eventId] = stateType;
        }

        public Type GetTransitionMap(int eventId)
        {
            var hasEvent = _transitionMap.ContainsKey(eventId);
            if (hasEvent == true)
            {
                return _transitionMap[eventId];
            }
            return null;
        }
        #endregion
    }

    public class StateMachine<T> : IStateMachine where T : MonoBehaviour
    {

        #region StateMachine Property
        public bool Enable { get; private set; } = false;
        public T Context { get { return _context; } }
        #endregion

        #region Constructor
        public StateMachine(T context)
        {
            _context = context;
        }
        #endregion

        #region Private Variable
        private Dictionary<Type, State<T>> _stateMap = new();

        private State<T> _start = null;
        private State<T> _current = null;
        private Stack<State<T>> _prevStack = new();

        private T _context = null;
        #endregion

        public void OnEnable()
        {
            if (_start == null)
            {
                Debug.LogError("StartStateが設定されていません");
                return;
            }

            Enable = true;
            _current = _start;
            _current?.OnEnter();
        }

        public void OnUpdate()
        {
            if (Enable == true)
            {
                _current?.OnUpdate();
            }
        }

        public void OnDisable()
        {
            _current = null;
            _prevStack.Clear();
            Enable = false;
        }

        public void SendTrigger(int eventId)
        {
            Type next = _current.GetTransitionMap(eventId);
            if (next != null)
            {
                NextState(next);
            }
        }

        public void RollBack()
        {
            BackState();
        }

        public bool HasState<U>() where U : State<T>
        {
            Type type = typeof(U);
            return _stateMap.ContainsKey(type);
        }

        public bool HasState(Type type) => _stateMap.ContainsKey(type);

        #region Add Methots
        public void AddState<U>() where U : State<T>
        {
            Type type = typeof(U);
            if (HasState(type) == false)
            {
                // ステートの生成と初期化
                _stateMap[type] = Activator.CreateInstance(type, new object[] { this }) as U;
            }
        }

        public void AddTransition<From, To>(int eventId) where From : State<T> where To : State<T>
        {
            Type fromStateType = typeof(From);
            Type toStateType = typeof(To);

            // ステートマップに登録ステートが存在するかを確認している
            var hasState = _stateMap.ContainsKey(fromStateType) && _stateMap.ContainsKey(toStateType);
            if (hasState == false)
            {
                Debug.LogError("ステートが存在しません");
                return;
            }

            // 遷移イベントを追加
            _stateMap[fromStateType].AddTransition(eventId, toStateType);
        }

        public void SetStartState<U>() where U : State<T>
        {
            Type type = typeof(U);
            var hasState = _stateMap.ContainsKey(type);
            if(hasState == false)
            {
                Debug.LogError($"ステートがありません[{type.Name}]");
                return;
            }
            _start = _stateMap[type];
        }
        #endregion
        
        #region Change State Function
        private void NextState(Type stateType)
        {
            _current?.OnExit();

            _prevStack.Push(_current);
            _current = _stateMap[stateType];
            
            _current?.OnEnter();
        }

        private void BackState()
        {
            _current?.OnExit();

            if(_prevStack.Count == 0)
            {
                _current = _start;
            }
            else
            {
                _current = _prevStack.Pop();
            }

            _current?.OnEnter();
        }
        #endregion
    }
}