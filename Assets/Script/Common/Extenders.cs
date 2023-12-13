using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extenders 
{
    public static string ToString(this string text, TypeGame typeGame)
    {
        
        switch (typeGame)
        {
            case TypeGame.Difficult:
                return "Expert";
            case TypeGame.Medium:
                return "Advance";
            case TypeGame.Easy:
                return "Begginer";

        }
        // other ones, just use the base method
        return typeGame.ToString();
    }
}
