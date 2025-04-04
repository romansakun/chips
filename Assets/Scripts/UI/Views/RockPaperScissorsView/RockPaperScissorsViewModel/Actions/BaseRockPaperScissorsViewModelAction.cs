using System.Threading.Tasks;
using Definitions;
using LogicUtility;
using Managers;
using UnityEngine;
using Zenject;

namespace UI.RockPaperScissors
{
    public abstract class BaseRockPaperScissorsViewModelAction : IAction<RockPaperScissorsViewModelContext> 
    {
        [Inject] protected GameDefs _gameDefs;
        [Inject] protected AddressableManager _addressableManager;

        public INode<RockPaperScissorsViewModelContext> Next { get; set; }
        public string GetLog() =>  GetType().Name;

        public virtual Task ExecuteAsync(RockPaperScissorsViewModelContext context)
        {
            Execute(context);
            return Task.CompletedTask;
        }

        protected virtual void Execute(RockPaperScissorsViewModelContext context)
        {

        }

        protected async Task<Sprite> GetHandSprite(RockPaperScissorsHand hand)
        {
            var handSpriteId = _gameDefs.RockPaperScissorsSettings.HandSprites[hand];
            var handSprite = await _addressableManager.LoadAsync<Sprite>(handSpriteId);
            return handSprite;
        }

        //todo: replace it to other helper class
        protected async Task<Sprite> GetAvatarSprite(string npcDefId)
        {
            var avatarSpriteId = _gameDefs.Npc[npcDefId].AvatarSprite;
            var avatarTexture = await _addressableManager.LoadAsync<Texture2D>(avatarSpriteId);
            var avatarSprite = Sprite.Create(avatarTexture, 
                new Rect(0, 0, avatarTexture.width, avatarTexture.height), 
                new Vector2(.5f, .5f));
            return avatarSprite;
        }

    }
}