using System;
using System.Collections.Generic;
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
                       Chips = new List<ChipData>()
                       {
                          new ChipData(){Id = 1, Name = "Chip1"},
                          new ChipData(){Id = 2, Name = "Chip2"},
                          new ChipData(){Id = 3, Name = "Chip3"},
                          new ChipData(){Id = 4, Name = "Chip4"},
                          new ChipData(){Id = 5, Name = "Chip5"},
                          new ChipData(){Id = 6, Name = "Chip6"},
                          new ChipData(){Id = 7, Name = "Chip7"},
                          new ChipData(){Id = 8, Name = "Chip8"},
                          new ChipData(){Id = 9, Name = "Chip9"},
                          new ChipData(){Id = 10, Name = "Chip10"},
                       }
                   },
                   new PlayerData()
                   {
                       Id = 2,
                       Name = "Player2",
                       Chips = new List<ChipData>()
                       {
                          new ChipData(){Id = 11, Name = "Chip11"},
                          new ChipData(){Id = 12, Name = "Chip12"},
                          new ChipData(){Id = 13, Name = "Chip13"},
                          new ChipData(){Id = 14, Name = "Chip14"},
                          new ChipData(){Id = 15, Name = "Chip15"},
                          new ChipData(){Id = 16, Name = "Chip16"},
                          new ChipData(){Id = 17, Name = "Chip17"},
                          new ChipData(){Id = 18, Name = "Chip18"},
                          new ChipData(){Id = 19, Name = "Chip19"},
                          new ChipData(){Id = 20, Name = "Chip20"},
                       }
                   }
               });
        }

        public void Dispose()
        {
            
        }
    }
}