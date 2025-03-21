using System.Collections.Generic;
using System.Threading.Tasks;
using Definitions;
using Gameplay;
using Gameplay.Chips;
using Managers;
using Model;
using UnityEngine;
using Zenject;

namespace UI
{
    public class PrepareChipsStackAction : BaseGameplayViewModelAction
    {
        [Inject] private Chip.Factory _chipFactory;
        [Inject] private CameraController _cameraController;
        [Inject] private GameDefs _gameDefs;
        [Inject] private AddressableManager _addressableManager;
        //[Inject] private PlayerContextRepository _playerContext;

        private readonly Dictionary<ChipDef, int> _chipsCount = new Dictionary<ChipDef, int>();

        public override async Task ExecuteAsync(GameplayViewModelContext context)
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
                foreach (var pair in context.Players)
                {
                    var chips = pair.BetChips;
                    foreach (var chip in chips)
                    {
                        var chipDef = _gameDefs.Chips[chip];
                        if (_chipsCount.TryAdd(chipDef, 1) == false)
                            _chipsCount[chipDef]++;
                    }
                }
            }
            
            var _chipNumber = 0;
            context.HittingChips.ForEach(c => c.Dispose());
            context.HittingChips.Clear();
            context.HittingChipsAndDefs.Clear();
            foreach (var pair in _chipsCount)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    var chipDef = pair.Key;
                    var chip = _chipFactory.Create();
                    chip.gameObject.SetActive(false);
                    chip.MeshFilter.sharedMesh = await _addressableManager.LoadAsync<Mesh>(chipDef.Mesh);
                    chip.MeshRenderer.sharedMaterial = await _addressableManager.LoadAsync<Material>(chipDef.Material);
                    chip.Transform.rotation = Quaternion.identity;
                    chip.Transform.position = new Vector3(0, 6 + _chipNumber * .25f, 0);
                    chip.Rigidbody.automaticCenterOfMass = true;
                    //chip.Rigidbody.automaticCenterOfMass = false;
                    //chip.Rigidbody.centerOfMass = new Vector3(Random.Range(-0.15f, 0.15f), 0, Random.Range(-0.15f, 0.15f));
                    context.HittingChips.Add(chip);
                    context.HittingChipsAndDefs.Add((chip, chipDef));

                    _chipNumber++;
                }
            }

            foreach (var chip in context.HittingChips)
            {
                chip.gameObject.SetActive(true);
            }

            _cameraController.ResetPosition();
        }
        
    }
}