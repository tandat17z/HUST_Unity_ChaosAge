using System.Collections.Generic;

namespace AILibraryForNPC.core
{
    public interface IGoalSelector
    {
        // Chọn goal tốt nhất dựa trên trạng thái hiện tại
        public BaseGoalSO SelectBestGoal(List<BaseGoalSO> goals, WorldState worldState);

        // Đánh giá tất cả các goal có thể
        public Dictionary<BaseGoalSO, float> EvaluateAllGoals(List<BaseGoalSO> goals, WorldState worldState);

        // Kiểm tra tính khả thi của một goal
        public bool IsGoalFeasible(BaseGoalSO goal, WorldState worldState);

        // Cập nhật trạng thái của goal selector
        public void UpdateGoalSelector(WorldState worldState);

        // Lấy goal hiện tại đang được thực thi
        public BaseGoalSO GetCurrentGoal();

        // Kiểm tra xem goal hiện tại có còn phù hợp không
        public bool IsCurrentGoalValid(WorldState worldState);
    }
}
