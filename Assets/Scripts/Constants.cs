public static class Constants {
    #if UNITY_EDITOR
    public static float gravity = 5f; // how fast everything falls per second
    #else
    public static float gravity = 0.5f; // how fast everything falls per second
    #endif
    public static float blockSize = 0.7f;
    public static bool debugMode = true; // makes the player overpowered (only in editor)
    public static float rowTime = 10f; // how quickly a block row spawns after the other
}