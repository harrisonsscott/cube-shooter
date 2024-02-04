public static class Constants {
    #if UNITY_EDITOR

    public static float gravity = 5f; // how fast everything falls per second
    public static float rowTime = 2f; // how quickly a block row spawns after the other
    public static bool debugMode = true; // makes the player overpowered (only in editor)
    #else
    // normal
    public static float gravity = 0.5f;
    public static float rowTime = 10f;
    public static bool debugMode = false;
    #endif
    public static float blockSize = 0.7f;
    
}