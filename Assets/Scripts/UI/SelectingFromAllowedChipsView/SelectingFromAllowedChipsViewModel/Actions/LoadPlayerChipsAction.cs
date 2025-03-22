using System.Collections.Generic;
using System.Threading.Tasks;
using Definitions;
using Gameplay.Chips;
using Managers;
using Model;
using UnityEngine;
using Zenject;

namespace UI
{
    public class LoadPlayerChipsAction : BaseSelectingFromAllowedChipsViewModelAction
    {
        [Inject] private GameDefs _gameDefs;
        [Inject] private Chip.Factory _chipFactory;
        [Inject] private AddressableManager _addressableManager;
        [Inject] private PlayerContextRepository _playerContext;

        private readonly List<Chip> _chips = new List<Chip>();

        public override async Task ExecuteAsync(SelectingFromAllowedChipsViewModelContext context)
        {
            _chips.ForEach(c => c.Dispose());
            _chips.Clear();

            var processingChipsCount = _playerContext.GetAllChipsCount();
            _playerContext.ForeachChips(ProcessPlayerChip);
            while (_chips.Count != processingChipsCount)
            {
                await Task.Yield();
            }
            foreach (var chip in _chips)
            {
                chip.Configure(c => c.GameObject.SetActive(true));
            }

            context.AllPlayersChips.ForEach(c => c.Dispose());
            context.AllPlayersChips.Clear();
            context.AllPlayersChips.AddRange(_chips);

            context.RightSideChips.Clear();
            context.RightSideChips.AddRange(_chips);
        }

        private async void ProcessPlayerChip(KeyValuePair<string, int> pair)
        {
            var chipDef = _gameDefs.Chips[pair.Key];
            var mesh = await _addressableManager.LoadAsync<Mesh>(chipDef.Mesh);
            var material = await _addressableManager.LoadAsync<Material>(chipDef.Material);
            for (int i = 0; i < pair.Value; i++)
            {
                var offset = _chips.Count * .025f;
                var chip = _chipFactory.Create();
                chip.Configure(c =>
                {
                    c.GameObject.SetActive(false);
                    c.Transform.rotation = Quaternion.Euler(315, 180, 150);
                    c.Transform.position = new Vector3(offset, 3, -3);
                    c.Rigidbody.isKinematic = true;
                    c.MeshFilter.sharedMesh = mesh;
                    c.MeshRenderer.sharedMaterial = material;
                });
                _chips.Add(chip);
            }
        }
    }
}