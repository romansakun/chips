using UnityEngine;

namespace Gameplay.Chips
{
    public class ChipDestinationsHolder : MonoBehaviour 
    {
        [SerializeField] Transform _myPlayerChipDestination;
        [SerializeField] Transform _leftPlayerChipDestination;
        [SerializeField] Transform _rightPlayerChipDestination;

        public Transform RightPlayerChipDestination => _rightPlayerChipDestination;
        public Transform LeftPlayerChipDestination => _leftPlayerChipDestination;
        public Transform MyPlayerChipDestination => _myPlayerChipDestination;
    }
}