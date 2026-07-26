using Core.ServiceLocator;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ProgressVisualizer : MonoBehaviour, IService
    {
        [SerializeField] private TMP_Text _text;
        private const string Format = "Enemy Left: {0}";

        public void UpdateInfo(int enemyLeft)
        {
            _text.text = string.Format(Format, enemyLeft);
        }
    }
}