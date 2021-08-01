using UnityEngine;

namespace InteractiveObjects
{
    public class BouncyBall : MonoBehaviour
    {
        [SerializeField] private int _bouncesForAchievement = 3;
        
        private int _bounces;

        private void OnCollisionEnter(Collision other)
        {
            // If the ball bounced enough times
            if (++_bounces > _bouncesForAchievement)
                Achievements.Award("BouncesForDays");
        }
    }
}
