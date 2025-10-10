using System;
using System.Collections.Generic;
using UnityEngine;

namespace mySystem.AI.HFSM
{
    public abstract class State<T> : IState where T : MonoBehaviour
    {
        #region Constructor
        public State(State<T> parent, StateMachine<T> owner)
        {
            Parent = parent;
            Context = owner.Context;
            Owner = owner;
        }
        #endregion

        #region State Proparty
        public State<T> Parent { get; private set; } = null;
        public T Context { get; private set; } = null;
        public State<T> Current { get { return _currentChild; } }
        public StateMachine<T> Owner { get; private set; } = null;
        public bool Enable { get; private set; } = false;
        #endregion

        #region Private Variable
        Dictionary<Type, State<T>> _childStateMap = new();
        Dictionary<int, Type> _transitionMap = new();

        State<T> _currentChild = null;
        State<T> _startChild = null;
        Stack<State<T>> _prevChildStack = new();
        #endregion

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }

        #region Call StateMachine Methots
        public void OnEnable()
        {
            Enable = true;
            if (ChildCount() > 0)
            {
                foreach (var state in _childStateMap)
                {
                    state.Value?.OnEnable();
                }
                _currentChild = _startChild;
            }
        }

        public void SendEnter()
        {
            if (Enable == false)
            {
                return;
            }

            OnEnter();
            _currentChild = _startChild;
            if (_currentChild != null)
            {
                _currentChild.SendEnter();
            }
        }

        public void SendUpdate()
        {
            if (Enable == false)
            {
                return;
            }

            OnUpdate();

            if (_currentChild != null)
            {
                _currentChild.SendUpdate();
            }
        }

        public void SendExit()
        {
            if (Enable == false)
            {
                return;
            }

            OnExit();

            if (_currentChild != null)
            {
                _currentChild.SendExit();
            }
        }

        public void OnDisble()
        {
            if (ChildCount() > 0)
            {
                foreach (var state in _childStateMap)
                {
                    state.Value?.OnDisble();
                }
                _currentChild = null;
                _prevChildStack.Clear();
            }
            Enable = false;
        }
        #endregion

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

        public void AddChildState<U>() where U : State<T>
        {
            Type type = typeof(U);
            if (HasChildState(type) == false)
            {
                // ステートの生成と初期化
                _childStateMap[type] = Activator.CreateInstance(type, new object[] { this, Owner }) as U;
            }
        }

        public void AddChildTransition<From, To>(int eventId) where From : State<T> where To : State<T>
        {
            Type fromStateType = typeof(From);
            Type toStateType = typeof(To);

            // ステートマップに登録ステートが存在するかを確認している
            var hasState = _childStateMap.ContainsKey(fromStateType) && _childStateMap.ContainsKey(toStateType);
            if (hasState == false)
            {
                Debug.LogError("ステートが存在しません");
                return;
            }

            // 遷移イベントを追加
            _childStateMap[fromStateType].AddTransition(eventId, toStateType);
        }

        public void SetStartState<U>() where U : State<T>
        {
            Type type = typeof(U);
            var hasState = _childStateMap.ContainsKey(type);
            if (hasState == false)
            {
                Debug.LogError($"ステートがありません[{type.Name}]");
                return;
            }
            _startChild = _childStateMap[type];
        }

        public void SendTrigger(int eventId)
        {
            var state = this;
            while (state.Parent != null)
            {
                state = state.Parent;
            }

            while (state != null)
            {
                var nextType = state.GetTransitionMap(eventId);
                if (nextType != null)
                {
                    state.Parent.NextState(nextType);
                    break;
                }
                state = state.Current;
            }
        }

        public void RollBack()
        {
            if (_currentChild == null)
            {
                return;
            }

            BackState();
        }

        public bool HasChildState<U>() where U : State<T>
        {
            Type type = typeof(U);
            return _childStateMap.ContainsKey(type);
        }

        public bool HasChildState(Type type) => _childStateMap.ContainsKey(type);
        public int ChildCount() => _childStateMap.Count;

        #region Change State Function
        void NextState(Type stateType)
        {
            _currentChild?.SendExit();

            _prevChildStack.Push(_currentChild);
            _currentChild = _childStateMap[stateType];
            _currentChild?.SendEnter();
        }

        void BackState()
        {
            _currentChild?.SendExit();

            if (_prevChildStack.Count == 0)
            {
                _currentChild = _startChild;
            }
            else
            {
                _currentChild = _prevChildStack.Pop();
            }

            _currentChild?.SendEnter();
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
        State<T> _rootState = null;
        private T _context = null;
        #endregion

        public void SetRootState<U>() where U : State<T>
        {
            // RootStateの生成と初期化
            // OnEnableの前に設定する
            Type type = typeof(U);
            _rootState = Activator.CreateInstance(type, new object[] { null, this }) as U;
        }

        public void OnEnable()
        {
            if (_rootState == null)
            {
                Debug.LogError("RootStateが設定されていません");
                return;
            }

            Enable = true;
            _rootState?.OnEnable();
            _rootState?.SendEnter();
        }

        public void OnUpdate()
        {
            if (Enable == true)
            {
                _rootState?.SendUpdate();
            }
        }

        public void OnDisable()
        {
            _rootState?.SendExit();
            _rootState?.OnDisble();
            Enable = false;
        }
    }
}