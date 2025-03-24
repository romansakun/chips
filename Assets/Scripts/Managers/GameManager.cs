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
           //после загрузки показываем главный экран
           //+ возможно еще будут другие штуки

           Debug.Log($"Start: Showing main screen");

           // var viewModel = _viewModelFactory.Create<GameplayViewModel>();
           // var view = await _guiManager.ShowAsync<GameplayView, GameplayViewModel>(viewModel);
           
           await _battleController.ExecuteBattle(new List<PlayerData>()
           {
               new PlayerData()
               {
                   PlayerType = PlayerType.User,
                   BetChips = new List<string>()
                   {
                      "Chip1",
                      "Chip2",
                      // "Chip3",
                      // "Chip4",
                      // "Chip5",
                      // "Chip6"
                   },
                   MovementOrder = 1
               },
               new PlayerData()
               {
                   Id = "Npc1",
                   PlayerType = PlayerType.LeftNpc,
                   BetChips = new List<string>()
                   {
                       "Chip1",
                       "Chip1",
                       // "Chip1",
                       // "Chip1",
                       // "Chip1"
                   },
                   MovementOrder = 2
               }
           });
        }

        public void Dispose()
        {
            
        }
    }
}