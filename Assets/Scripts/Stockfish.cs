using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stockfish : MonoBehaviour
{
    public string GetBestMove(string forsythEdwardsNotationString)
    {
        var p = new System.Diagnostics.Process();
        p.StartInfo.FileName = "F:/Portfolio/Unity/Projects/Prototypes/Chess-2/Assets/Scripts/stockfish.exe";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.Start();  
        string setupString = "position fen "+forsythEdwardsNotationString;
        p.StandardInput.WriteLine(setupString);

        // Process for 5 seconds
        string processString = "go depth 1";

        // Process 20 deep
        // String processString = "go depth 20";

        p.StandardInput.WriteLine(processString);

        string bestMoveInAlgebraicNotation = null;
        while (true)
        {
            bestMoveInAlgebraicNotation = p.StandardOutput.ReadLine();

            if (bestMoveInAlgebraicNotation.Contains("bestmove"))
            {
                break;
            }
        }


        p.Close();
        Debug.Log(bestMoveInAlgebraicNotation);
        return bestMoveInAlgebraicNotation;
    }
}
