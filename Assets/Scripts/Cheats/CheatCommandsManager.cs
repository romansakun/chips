#if WITH_CHEATS
using System;
using System.Collections.Generic;
using Zenject;

namespace Cheats
{
    public class CheatCommandsManager : IInitializable, IDisposable
    {
        [Inject] private DiContainer _diContainer;

        private readonly List<ICommandCheat> _cheats = new();

        public void Initialize()
        {
            AddCheat<GameplayViewCheatCommands>();
        }

        private void AddCheat<T>() where T : ICommandCheat
        {
            var cheat = _diContainer.Instantiate<T>();
            cheat.Initialize();
            _cheats.Add(cheat);
        }

        public void Dispose()
        {
            _cheats.ForEach(c => c.Dispose());
            _cheats.Clear();
        }

    }
}
#endif