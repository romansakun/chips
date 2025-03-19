using System;
using System.Collections.Generic;
using Definitions;
using Factories;
using Gameplay.Battle;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : IDisposable
    {
        [Inject] private BattleController _battleController;
        [Inject] private GuiManager _guiManager;
        [Inject] private ViewModelFactory _viewModelFactory;
        
        public async void Start()
        {
            Application.targetFrameRate = 60;

           //после загрузки показываем главный экран
           //+ возможно еще будут другие штуки

           Debug.Log($"Start: Showing main screen");

           // var viewModel = _viewModelFactory.Create<GameplayViewModel>();
           // var view = await _guiManager.ShowAsync<GameplayView, GameplayViewModel>(viewModel);
           
           await 
               _battleController.ExecuteBattle(new List<PlayerData>()
               {
                   new PlayerData()
                   {
                       Id = 1,
                       Name = "Player1",
                       // Chips = new List<ChipDef>()
                       // {
                       //    new ChipDef(){Id = 1, LocalizationKey = "Chip1"},
                       //    new ChipDef(){Id = 2, LocalizationKey = "Chip2"},
                       //    new ChipDef(){Id = 3, LocalizationKey = "Chip3"},
                       //    new ChipDef(){Id = 4, LocalizationKey = "Chip4"},
                       //    new ChipDef(){Id = 5, LocalizationKey = "Chip5"},
                       //    new ChipDef(){Id = 6, LocalizationKey = "Chip6"},
                       //    new ChipDef(){Id = 7, LocalizationKey = "Chip7"},
                       //    new ChipDef(){Id = 8, LocalizationKey = "Chip8"},
                       //    new ChipDef(){Id = 9, LocalizationKey = "Chip9"},
                       //    new ChipDef(){Id = 10,LocalizationKey = "Chip10"},
                       // }
                   },
                   new PlayerData()
                   {
                       Id = 2,
                       Name = "Player2",
                       // Chips = new List<ChipDef>()
                       // {
                       //    new ChipDef(){Id = 11, LocalizationKey = "Chip11"},
                       //    new ChipDef(){Id = 12, LocalizationKey = "Chip12"},
                       //    new ChipDef(){Id = 13, LocalizationKey = "Chip13"},
                       //    new ChipDef(){Id = 14, LocalizationKey = "Chip14"},
                       //    new ChipDef(){Id = 15, LocalizationKey = "Chip15"},
                       //    new ChipDef(){Id = 16, LocalizationKey = "Chip16"},
                       //    new ChipDef(){Id = 17, LocalizationKey = "Chip17"},
                       //    new ChipDef(){Id = 18, LocalizationKey = "Chip18"},
                       //    new ChipDef(){Id = 19, LocalizationKey = "Chip19"},
                       //    new ChipDef(){Id = 20, LocalizationKey = "Chip20"},
                       // }
                   }
               });
        }

        public void Dispose()
        {
            
        }
    }
}