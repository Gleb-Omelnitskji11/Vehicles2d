using UnityEngine;

namespace Gameplay
{
    public abstract class VehicleMovement : MonoBehaviour
    {
        public abstract void Move(float move, float turn);
    }
}