using System.Collections.Generic;
using UnityEngine;

namespace mySystem
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        #region Config Datas
        [SerializeField]
        GameInstanceBase _gameInstance = null;
        #endregion
        
        public GameInstanceBase GameInstance => _gameInstance;
    }
}
