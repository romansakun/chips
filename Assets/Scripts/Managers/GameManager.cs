using System;
using Factories;
using UI;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : IDisposable
    {
        [Inject] private GuiManager _guiManager;
        [Inject] private ViewModelFactory _viewModelFactory;

        public async void Start()
        {
           //после загрузки показываем главный экран
           //+ возможно еще будут другие штуки
           
           Debug.Log($"Start: Showing main screen");

           var viewModel = _viewModelFactory.Create<GameplayViewModel>();
           var view = await _guiManager.ShowAsync<GameplayView, GameplayViewModel>(viewModel);
        }
        
        public void Dispose()
        {
            
        }
    }
}