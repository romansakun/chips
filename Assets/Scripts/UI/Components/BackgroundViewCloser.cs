using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class BackgroundViewCloser : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private View _view;

        public void OnPointerClick(PointerEventData eventData)
        {
            _view.Close();
        }
    }
}