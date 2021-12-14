using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMidnightCommander.EditorFolder.Functions
{
    public static class Selection
    {
        public static List<string> Delete(List<string> linesOfText, int startX, int startY, int endX, int endY)
        {
            int[] positions = GetTrueStart(startX, startY, endX, endY);
            List<string> newLinesOfText = new List<string>();

            return newLinesOfText; 
        }

        public static int[] GetTrueStart(int startX, int startY, int endX, int endY)
        {
            int[] trueValues = new int[3];

            if(startY > endY)
            {
                int something = startY;
                startY = endY;
                endY = something;
            }
            else if (startY == endY)
            {
                if (startX > endX)
                {
                    int something = startX;
                    startX = endX;
                    endX = something;
                }
            }

            trueValues[0] = startX;
            trueValues[1] = startY;
            trueValues[2] = endX;
            trueValues[3] = endY;

            return trueValues;
        }
    }
}
