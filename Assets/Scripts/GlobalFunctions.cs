using System;

// functions that can be access by all the scripts

public static class GlobalFunctions {
    private static string[] abbreviations = { "", "k", "m", "b", "t", "q", "qu", "s", "o", "n" };
    public static string abbreviate(long number){ // abbreviates a number, ex 1,000 -> 1K
        int index = 0;
        double num = number;

        while (num >= 1000){
            num = Math.Floor(num)/1000;
            index += 1;
        }
        
        return num + abbreviations[index];  
    }

    public static string abbreviate(double number){
        return abbreviate((long)number);
    }
}