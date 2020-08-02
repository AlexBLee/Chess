using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stockfish : MonoBehaviour
{
    public Vector2Int startPos;
    public Vector2Int resultPos;

    public System.Diagnostics.Process process;

    private void Start() 
    {
        process = new System.Diagnostics.Process();
        process.StartInfo.FileName = "F:/Portfolio/Unity/Projects/Prototypes/Chess-2/Assets/Scripts/stockfish.exe";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
    }

    public void GetBestMove(string forsythEdwardsNotationString)
    {
        process.Start();

        process.StandardInput.WriteLine("position fen "+forsythEdwardsNotationString);
        process.StandardInput.WriteLine("go depth 1");

        string bestMoveInAlgebraicNotation = null;
        while (true)
        {
            bestMoveInAlgebraicNotation = process.StandardOutput.ReadLine();

            if (bestMoveInAlgebraicNotation.Substring(0,4) == "best")
            {
                startPos = ConvertAlgNotationToCoordinates(bestMoveInAlgebraicNotation.Substring(9,2));
                resultPos = ConvertAlgNotationToCoordinates(bestMoveInAlgebraicNotation.Substring(11,2));
                break;
            }
        }

        process.Close();
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
