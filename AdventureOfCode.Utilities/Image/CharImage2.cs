using System.Collections.Immutable;

namespace AdventureOfCode.Utilities.Image;

public class CharImage2 : Image2<char>
{
    public CharImage2(int rows, int cols) : base(rows, cols) { }

    public static CharImage2 Parse(ImmutableArray<string> input)
    {
        int rows = input.Length;
        int cols = input[0].Length;
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
}
