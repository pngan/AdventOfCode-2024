using System.Collections.Immutable;

namespace AdventureOfCode.Utilities.Image;

public class CharImage2 : Image2<char>
{
    public CharImage2(int rows, int cols) : base(rows, cols) { }

    public CharImage2(CharImage2 old) : this(old.ROWS, old.COLS)
    {
        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                this[(r, c)] = old[(r,c)];
            }
        }
    }

    public static CharImage2 Parse(IEnumerable<string> input)
    {
        int rows = input.Count();
        int cols = input.First().Length;
        CharImage2 result = new(rows,cols);

        int r = 0;
        foreach (var row in input)
        {
            int c = 0;
            foreach (var ch in row)
            {
                result[(r, c)] = ch;
                c++;
            }
            r++;
        }
        return (CharImage2) result;
    }

    public void DrawMap()
    {
        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                Console.Write(this[(r, c)]);
            }
            Console.WriteLine();
        }
    }
}
