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
           
           _battleController.StartBattle(new List<string>() {"Kuno1", "Kuno2"});
        }

        public void Dispose()
        {
            
        }
    }
}