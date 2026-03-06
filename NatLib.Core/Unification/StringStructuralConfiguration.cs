namespace NatLib.Core.Unification;

public class StringStructuralConfiguration
{
    public static readonly StringStructuralConfiguration Instance = new();
    
    public char CornerTopLeft = '╔';
    public char CornerTopRight = '╗';
    public char CornerBottomLeft = '╚';
    public char CornerBottomRight = '╝';
    
    public char HorizontalLine = '═';
    public char VerticalLine = '║';
    
    public char SectionTRight = '╠';
    public char SectionTLeft = '╣';
    public char SectionTTop = '╩';
    public char SectionTBottom = '╦';

    public char SectionX = '╬';

    public char EmptyBlock = ' ';

    public int PreferableWidth = 70;

    public (char Left, char Center, char Right, int Width) DeconstructTop()
        => (CornerTopLeft, HorizontalLine, CornerTopRight, PreferableWidth);
    
    public (char Left, char Center, char Right, int Width) DeconstructBottom()
        => (CornerBottomLeft, HorizontalLine, CornerBottomRight, PreferableWidth);
    
    public (char Left, char Center, char Right, int Width) DeconstructMiddle()
        => (VerticalLine, EmptyBlock, VerticalLine, PreferableWidth);
}