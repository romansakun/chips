using UnityEngine;

namespace Gameplay
{
    public class GameplayObjectsHolder : MonoBehaviour 
    {
        [SerializeField] SpriteRenderer _allowedScatterSpriteRenderer;
        [SerializeField] Transform _myPlayerChipDestination;
        [SerializeField] Transform _leftPlayerChipDestination;
        [SerializeField] Transform _rightPlayerChipDestination;

        public Transform RightPlayerChipDestination => _rightPlayerChipDestination;
        public Transform LeftPlayerChipDestination => _leftPlayerChipDestination;
        public Transform MyPlayerChipDestination => _myPlayerChipDestination;
        public SpriteRenderer AllowedScatterCircleSpriteRenderer => _allowedScatterSpriteRenderer;
    }
}