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
        [Inject] private Chip.Pool _chipPool;
        [Inject] private AddressableManager _addressableManager;
        [Inject] private UserContextRepository _userContext;

        private int _asyncInProgressCount;
        private SelectingFromAllowedChipsViewModelContext _context;
        
        public override async Task ExecuteAsync(SelectingFromAllowedChipsViewModelContext context)
        {
            _context = context;
            _context.AllPlayersChips.ForEach(c => c.Item1.Dispose());
            _context.AllPlayersChips.Clear();

            _userContext.ForeachChips(ProcessPlayerChip);
            while (_asyncInProgressCount > 0)
            {
                await Task.Yield();
            }

            _context.RightSideChips.Clear();
            _context.RightSideChips.AddRange(_context.AllPlayersChips);
            _context.CurrentWatchingChip = _context.RightSideChips[0];
            _context.BetChipsCount.Value = 0;
            _context.RightSideChips.RemoveAt(0);

            _context.AllPlayersChips.ForEach(c => c.Item1.Facade.GameObject.SetActive(true));
        }

        private async void ProcessPlayerChip(KeyValuePair<string, int> pair)
        {
            _asyncInProgressCount++;
            var chipDef = _gameDefs.Chips[pair.Key];
            var mesh = await _addressableManager.LoadAsync<Mesh>(chipDef.Mesh);
            var material = await _addressableManager.LoadAsync<Material>(chipDef.Material);
            for (int i = 0; i < pair.Value; i++)
            {
                var chip = _chipPool.Spawn(_chipPool);
                var chipFacade = chip.Facade;
                {
                    chipFacade.GameObject.SetActive(false);
                    chipFacade.Rigidbody.isKinematic = true;
                    chipFacade.MeshFilter.sharedMesh = mesh;
                    chipFacade.MeshRenderer.sharedMaterial = material;
                }
                _context.AllPlayersChips.Add((chip, chipDef));
            }
            _asyncInProgressCount--;
        }

    }
}