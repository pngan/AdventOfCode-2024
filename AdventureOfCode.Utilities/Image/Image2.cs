using System.ComponentModel;

namespace AdventureOfCode.Utilities.Image;

public class Image2<T>
{
    private readonly Dictionary<Point2, T> _image = [];
    public int ROWS { get; init; } 
    public int COLS { get; init; }

    public Image2(int rows, int cols)
    {
        ROWS = rows; 
        COLS = cols;
    }

    public T this[Point2 p]
    {
        get {
            if (!InBounds(p))
                throw new IndexOutOfRangeException();
            return _image[p]; }
        set {
            if (!InBounds(p))
                throw new IndexOutOfRangeException();
            
            _image[p] = value;
        }
    }

    public bool InBounds(Point2 p) => p.r >= 0 && p.c >= 0 && p.r < ROWS && p.c < COLS;

    // Returns only neighbours in bouds
    public IEnumerable<Point2> Neighbours8(Point2 p) => p.Neighbours8().Where(InBounds);
    public IEnumerable<Point2> Neighbours4(Point2 p) => p.Neighbours4().Where(InBounds);
    public IEnumerable<Point2> EveryPoint() => _image.Keys;

    public bool Exists(Point2 p) => _image.ContainsKey(p);

    public bool TryGetValue(Point2 p, out T value)
    {
        value = default;
        if (!Exists(p)) return false;
        value = _image[p];
        return true;
    }
}
