using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stockfish : MonoBehaviour
{
    public Vector2Int startPos;
    public Vector2Int resultPos;

    public void GetBestMove(string forsythEdwardsNotationString)
    {
        var p = new System.Diagnostics.Process();
        p.StartInfo.FileName = "F:/Portfolio/Unity/Projects/Prototypes/Chess-2/Assets/Scripts/stockfish.exe";
        p.StartInfo.CreateNoWindow = true;
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

        p.StandardInput.WriteLine("setoption name Thread value 2");
        p.StandardInput.WriteLine("setoption name hash value 1024");
        p.StandardInput.WriteLine(processString);

        string bestMoveInAlgebraicNotation = null;
        while (true)
        {
            bestMoveInAlgebraicNotation = p.StandardOutput.ReadLine();

            if (bestMoveInAlgebraicNotation.Contains("bestmove"))
            {
                startPos = ConvertAlgNotationToCoordinates(bestMoveInAlgebraicNotation.Substring(9,2));
                resultPos = ConvertAlgNotationToCoordinates(bestMoveInAlgebraicNotation.Substring(11,2));
                break;
            }
        }

        p.Close();
    }

    public Vector2Int ConvertAlgNotationToCoordinates(string not)
    {
        // Convert letter to x coordinate
        int letterNumber = not[0] - 97;

        // Convert number - 1 to y coordinate 
        int numberCoor = not[1] - '0' - 1;

        return new Vector2Int(letterNumber, numberCoor);
    }
}
