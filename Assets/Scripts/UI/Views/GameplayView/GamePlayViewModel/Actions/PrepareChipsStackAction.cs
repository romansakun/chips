using System.Collections.Generic;
using System.Threading.Tasks;
using Definitions;
using Gameplay.Chips;
using Managers;
using Model;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace UI.Gameplay
{
    //todo: magic numbers and many calculations should be refactored
    public class PrepareChipsStackAction : BaseGameplayViewModelAction
    {
        [Inject] private Chip.Pool _chipPool;
        [Inject] private AddressableManager _addressableManager;
        [Inject] private GameDefs _gameDefs;
        [Inject] private UserContextRepository _userContext;

        private readonly Dictionary<ChipDef, int> _chipsCount = new();

        private PlayerType _playerType;
        private float _deviation;

        public override async Task ExecuteAsync(GameplayViewModelContext context)
        {
            PreparingForNewChipsStack(context);

            _playerType = context.HittingPlayer.Type; 
            _deviation = _gameDefs.GameplaySettings.Deviation;

            var direction = GetDirection(context);
            var chipsRotation = GetChipsRotation(context);
            var firstChipPosition = GetFirstChipPosition(context, direction);

            context.PlayerHitForce = GetPlayerHitForce(context, direction);
            context.PlayerHitTorque = GetPlayerHitTorque(context);

            var chipNumber = 0;
            foreach (var pair in _chipsCount)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    var chipDef = pair.Key;
                    var mesh = await _addressableManager.LoadAsync<Mesh>(chipDef.Mesh);
                    var material = await _addressableManager.LoadAsync<Material>(chipDef.Material);
                    var heightOffset = chipNumber * _gameDefs.GameplaySettings.ChipSpacing + Random.Range(0, _deviation);

                    var chip = _chipPool.Spawn(_chipPool);
                    var chipFacade = chip.Facade;
                    chipFacade.GameObject.SetActive(false);
                    chipFacade.Transform.rotation = chipsRotation;
                    chipFacade.Transform.position = firstChipPosition - direction * heightOffset;
                    chipFacade.Rigidbody.isKinematic = true;
                    chipFacade.Rigidbody.automaticCenterOfMass = true;
                    chipFacade.MeshFilter.sharedMesh = mesh;
                    chipFacade.MeshRenderer.sharedMaterial = material;

                    context.HittingChips.Add(chip);
                    context.HittingChipsAndDefs.Add((chip, chipDef));

                    chipNumber++;
                }
            }
            foreach (var chip in context.HittingChips)
            {
                chip.gameObject.SetActive(true);
            }
        }

        private Vector3 GetPlayerHitForce(GameplayViewModelContext context, Vector3 direction)
        {
            var result = Vector3.zero;
            switch (_playerType)
            {
                case PlayerType.MyPlayer:
                    result = direction * _userContext.GetPreparingForce();
                    break;
                default:
                    var forceRange = _gameDefs.PreparingHitSettings.PrepareForceRange;
                    result = direction * Random.Range(forceRange[0], forceRange[1]/2); //todo: add bot logic and replace it there
                    break;
            }
            result.x += Random.Range(-_deviation, _deviation);
            result.y += Random.Range(-_deviation, _deviation);
            result.z += Random.Range(-_deviation, _deviation);
            return result;
        }

        private Vector3 GetPlayerHitTorque(GameplayViewModelContext context)
        {
            var result = Vector3.zero;
            switch (_playerType)
            {
                case PlayerType.MyPlayer:
                    result.y = _userContext.GetPreparingTorque();
                    break;
                default:
                    var range = _gameDefs.PreparingHitSettings.PrepareTorqueRange;
                    result.y = Random.Range(range[0], range[1]/2);
                    break;
            }
            result.x += Random.Range(-_deviation, _deviation);
            result.y += Random.Range(-_deviation, _deviation);
            result.z += Random.Range(-_deviation, _deviation);
            return result;
        }

        private Vector3 GetFirstChipPosition(GameplayViewModelContext context, Vector3 direction)
        {
            switch (_playerType)
            {
                case PlayerType.MyPlayer:
                    return direction * (-1 * _userContext.GetPreparingHeight());
                default:
                    var heightRange = _gameDefs.PreparingHitSettings.PrepareHeightRange;
                    return direction * (-1 * Random.Range(heightRange[0], heightRange[1]));
            }
        }

        private Vector3 GetDirection(GameplayViewModelContext context)
        {
            var range = _gameDefs.PreparingHitSettings.PrepareAngleRange;
            var needAngle = range[1] - _userContext.GetPreparingAngle() + range[0];
            var radAngle = -1 * needAngle * Mathf.Deg2Rad;
            switch (_playerType)
            {
                case PlayerType.MyPlayer:
                    return new Vector3(0, Mathf.Sin(radAngle),Mathf.Cos(radAngle));
                case PlayerType.RightPlayer:
                    radAngle = -1 * Random.Range(range[0], range[1]) * Mathf.Deg2Rad;
                    return new Vector3(-Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0);
                default:
                    radAngle = -1 * Random.Range(range[0], range[1]) * Mathf.Deg2Rad;
                    return new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0);
            }
        }

        private Quaternion GetChipsRotation(GameplayViewModelContext context)
        {
            var range = _gameDefs.PreparingHitSettings.PrepareAngleRange;
            var minusMax = -1 * range[1];
            switch (_playerType)
            {
                case PlayerType.MyPlayer:
                    var needAngle = range[1] - _userContext.GetPreparingAngle() + range[0];
                    return Quaternion.Euler(minusMax + needAngle,0, 0);
                case PlayerType.RightPlayer:
                    return Quaternion.Euler(0,0, minusMax + Random.Range(range[0], range[1]));
                default:
                    return Quaternion.Euler(0,0, -(minusMax + Random.Range(range[0], range[1])));
            }
        }

        private void PreparingForNewChipsStack(GameplayViewModelContext context)
        {
            _chipsCount.Clear();
            if (context.HittingChipsAndDefs.Count > 0)
            {
                foreach (var chipAndDef in context.HittingChipsAndDefs)
                {
                    var chipDef = chipAndDef.Item2;
                    if (_chipsCount.TryAdd(chipDef, 1) == false)
                        _chipsCount[chipDef]++;
                }
            }
            else
            {
                foreach (var pair in context.Shared.Players)
                {
                    var chips = pair.BetChips;
                    foreach (var chipDef in chips)
                    {
                        if (_chipsCount.TryAdd(chipDef, 1) == false)
                            _chipsCount[chipDef]++;
                    }
                }
            }
            context.HittingChips.ForEach(c => c.Dispose());
            context.HittingChips.Clear();
            context.HittingChipsAndDefs.Clear();
        }
    }
}