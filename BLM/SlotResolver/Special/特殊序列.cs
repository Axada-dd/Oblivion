namespace Oblivion.BLM.SlotResolver.Special;

public static class 特殊序列
{
    public static ISlotSequence[] Build()
    {
        return
        [
            new 开满转火(),
            new 双星灵墨泉()
        ];
    }
        
}