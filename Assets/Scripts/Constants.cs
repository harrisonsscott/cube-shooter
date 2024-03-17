public static class Constants {
    #if UNITY_EDITOR
    // debug mode
    public static float gravity = 2f; // how fast everything falls per second
    public static float rowTime = 6f; // how quickly a block row spawns after the other
    public static bool debugMode = true; // makes the player overpowered (only in editor)
    
    #else
    // release mode
    public static float gravity = 3f;
    public static float rowTime = 6f;
    public static bool debugMode = false;
    #endif

    public static int blocksPerRow = 5;
    
}