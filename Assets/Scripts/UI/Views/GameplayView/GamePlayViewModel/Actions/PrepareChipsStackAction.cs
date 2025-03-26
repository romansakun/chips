using System.Collections.Generic;
using System.Threading.Tasks;
using Definitions;
using Gameplay;
using Gameplay.Chips;
using Managers;
using UnityEngine;
using Zenject;

namespace UI
{
    public class PrepareChipsStackAction : BaseGameplayViewModelAction
    {
        [Inject] private Chip.Pool _chipPool;
        [Inject] private CameraController _cameraController;
        [Inject] private GameDefs _gameDefs;
        [Inject] private AddressableManager _addressableManager;

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

            var chipNumber = 0;
            context.HittingChips.ForEach(c => c.Dispose());
            context.HittingChips.Clear();
            context.HittingChipsAndDefs.Clear();
            foreach (var pair in _chipsCount)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    var chipDef = pair.Key;
                    var mesh = await _addressableManager.LoadAsync<Mesh>(chipDef.Mesh);
                    var material = await _addressableManager.LoadAsync<Material>(chipDef.Material);

                    var heightOffset = chipNumber * .25f;
                    var chip = _chipPool.Spawn(_chipPool);
                    var chipFacade = chip.Facade;
                    chipFacade.GameObject.SetActive(false);
                    chipFacade.Transform.rotation = Quaternion.identity;
                    chipFacade.Transform.position = new Vector3(0, 6 + heightOffset, 0);
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

            _cameraController.ResetPosition();
        }

    }
}