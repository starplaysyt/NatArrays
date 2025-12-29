namespace NatLib.UniConsole.Graphics;

public class RenderingStyleConfiguration
{
    public static readonly RenderingStyleConfiguration Instance = new();
    
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

    public int PreferableWidth = 40;

    public RenderingStyleConfiguration()
    {
        
    }
}