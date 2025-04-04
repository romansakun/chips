using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NpcViewPart : MonoBehaviour
    {
        public GameObject Holder;
        public Image AvatarImage;
        public Image CommunicationImage;
        public RectTransform CommunicationImageRectTransform;
        public TextMeshProUGUI NickText;
        public TextMeshProUGUI InfoText;


        public void Subscribes(NpcViewPartModel viewModel)
        {
            viewModel.AvatarSprite.Subscribe(OnAvatarSpriteChanged);
            viewModel.CommunicationSprite.Subscribe(OnCommunicationSpriteChanged);
            viewModel.CommunicationImageScale.Subscribe(OnCommunicationImageScaleChanged);
            viewModel.NickText.Subscribe(OnNickTextChanged);
            viewModel.InfoText.Subscribe(OnInfoTextChanged);
            viewModel.Visible.Subscribe(OnVisibleChanged);
            viewModel.VisibleCommunicationSprite.Subscribe(OnVisibleCommunicationSpriteChanged);
            viewModel.VisibleInfoText.Subscribe(OnVisibleInfoTextChanged);
        }

        public void Unsubscribes(NpcViewPartModel viewModel)
        {
            viewModel.AvatarSprite.Unsubscribe(OnAvatarSpriteChanged);
            viewModel.CommunicationSprite.Unsubscribe(OnCommunicationSpriteChanged);
            viewModel.CommunicationImageScale.Unsubscribe(OnCommunicationImageScaleChanged);
            viewModel.NickText.Unsubscribe(OnNickTextChanged);
            viewModel.InfoText.Unsubscribe(OnInfoTextChanged);
            viewModel.Visible.Unsubscribe(OnVisibleChanged);
            viewModel.VisibleCommunicationSprite.Unsubscribe(OnVisibleCommunicationSpriteChanged);
            viewModel.VisibleInfoText.Unsubscribe(OnVisibleInfoTextChanged);
        }

        private void OnCommunicationImageScaleChanged(Vector3 scale)
        {
            CommunicationImageRectTransform.localScale = scale;
        }

        private void OnVisibleCommunicationSpriteChanged(bool state)
        {
            CommunicationImage.gameObject.SetActive(state);
        }

        private void OnVisibleInfoTextChanged(bool state)
        {
            InfoText.gameObject.SetActive(state);
        }

        private void OnAvatarSpriteChanged(Sprite sprite)
        {
            AvatarImage.sprite = sprite;
        }

        private void OnCommunicationSpriteChanged(Sprite sprite)
        {
            CommunicationImage.sprite = sprite;
        }

        private void OnNickTextChanged(string nick)
        {
            NickText.text = nick;
        }

        private void OnInfoTextChanged(string info)
        {
            InfoText.text = info;
        }

        private void OnVisibleChanged(bool state)
        {
            Holder.SetActive(state);
        }
    }

    public class NpcViewPartModel
    {
        public IReactiveProperty<bool> Visible => _context.Visible;
        public IReactiveProperty<bool> VisibleCommunicationSprite => _context.VisibleCommunicationSprite;
        public IReactiveProperty<bool> VisibleInfoText => _context.VisibleInfoText;
        public IReactiveProperty<Sprite> AvatarSprite => _context.AvatarSprite;
        public IReactiveProperty<Sprite> CommunicationSprite => _context.CommunicationSprite;
        public IReactiveProperty<Vector3> CommunicationImageScale => _context.CommunicationImageScale;
        public IReactiveProperty<string> NickText => _context.NickText;
        public IReactiveProperty<string> InfoText => _context.InfoText;

        private NpcViewPartModelContext _context;

        public void SetContext(NpcViewPartModelContext context)
        {
            _context = context;
        }
    }

    public class NpcViewPartModelContext: IDisposable
    {
        public ReactiveProperty<bool> Visible { get; } = new();
        public ReactiveProperty<bool> VisibleCommunicationSprite { get; } = new();
        public ReactiveProperty<bool> VisibleInfoText { get; } = new();
        public ReactiveProperty<Sprite> AvatarSprite { get; } = new();
        public ReactiveProperty<Sprite> CommunicationSprite { get; } = new();
        public ReactiveProperty<Vector3> CommunicationImageScale { get; } = new();
        public ReactiveProperty<string> NickText { get; } = new();
        public ReactiveProperty<string> InfoText { get; } = new();

        public void Dispose()
        {
            VisibleCommunicationSprite.Dispose();
            VisibleInfoText.Dispose();
            AvatarSprite.Dispose();
            CommunicationSprite.Dispose();
            CommunicationImageScale.Dispose();
            NickText.Dispose();
            InfoText.Dispose();
            Visible.Dispose();
        }
    }
}

