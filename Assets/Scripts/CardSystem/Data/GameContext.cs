namespace CardSystem.Data
{
    public static class GameContext
    {
        public static readonly PawnAttributes Attributes = new();
        
        // 没做 getter setter，危险的半开放，但能够解耦
        public static LoopManager.LoopPhase currentPhase = new();
        public static LoopManager.LoopPhase lastPhase = new();
        
        // 抽牌需要的全局参数
        public static float TotalNumOfCard;
    }
}
