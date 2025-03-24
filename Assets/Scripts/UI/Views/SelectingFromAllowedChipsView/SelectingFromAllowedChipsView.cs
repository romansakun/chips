using UnityEngine.EventSystems;

namespace UI
{
    public class SelectingFromAllowedChipsView : View, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        private SelectingFromAllowedChipsViewModel _viewModel;
        
        public override void Initialize(ViewModel viewModel)
        {
            UpdateViewModel(ref _viewModel, viewModel);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _viewModel.OnBeginDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _viewModel.OnEndDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _viewModel.OnDrag(eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _viewModel.OnPointerClick(eventData);
        }
        
        private void OnDestroy()
        {
            _viewModel.Dispose();
        }
    }
}