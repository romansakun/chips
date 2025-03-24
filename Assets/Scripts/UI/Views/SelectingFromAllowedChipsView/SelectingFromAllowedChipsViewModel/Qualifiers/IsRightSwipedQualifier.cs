using UnityEngine;
using Definitions;

namespace UI
{
    public class IsRightSwipedQualifier : BaseSelectingFromAllowedChipsViewModelQualifier
    {
        protected override float Score(SelectingFromAllowedChipsViewModelContext context)
        {
            if (context.Input.Item1 != InputType.OnEndDrag)
                return 0;

            if (context.RightSideChips.Count == 0)
                return 0;

            var endDragPosition = context.Input.Item2.position;
            var swipeDelta = endDragPosition - context.StartSwipePosition;

            // Проверка направления по оси X
            if (swipeDelta.x < -50 && Mathf.Abs(swipeDelta.y) < 50) // Порог для фильтрации случайных движений
            {
                context.Input = (InputType.None, default);
                return 1;
            }

            return 0;
        }
    }
}