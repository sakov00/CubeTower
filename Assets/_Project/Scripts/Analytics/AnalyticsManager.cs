using UnityEngine;

namespace _Project.Scripts.Analytics
{
    public class AnalyticsManager
    {
        public void LogAction(string action)
        {
            Debug.Log($"Action logged: {action}");
        }
    }
}