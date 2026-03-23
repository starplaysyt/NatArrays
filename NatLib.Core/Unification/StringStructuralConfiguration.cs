using System.Text.Json.Serialization;

namespace NatLib.Core.Unification;

public class StringStructuralConfiguration
{
    [JsonIgnore] public static readonly StringStructuralConfiguration Instance = new();

    [JsonPropertyName("crn_top_left")] public char CornerTopLeft = '╔';
    [JsonPropertyName("crn_top_right")] public char CornerTopRight = '╗';
    [JsonPropertyName("crn_bottom_left")] public char CornerBottomLeft = '╚';
    [JsonPropertyName("crn_bottom_right")] public char CornerBottomRight = '╝';

    [JsonPropertyName("hori_line")] public char HorizontalLine = '═';
    [JsonPropertyName("vert_line")] public char VerticalLine = '║';

    [JsonPropertyName("sect_right")] public char SectionTRight = '╠';
    [JsonPropertyName("sect_left")] public char SectionTLeft = '╣';
    [JsonPropertyName("sect_top")] public char SectionTTop = '╩';
    [JsonPropertyName("sect_bottom")] public char SectionTBottom = '╦';

    public char SectionX = '╬';

    public char EmptyBlock = ' ';

    public int PreferableWidth = 70;

    public (char Left, char Center, char Right, int Width) DeconstructTop()
        => (CornerTopLeft, HorizontalLine, CornerTopRight, PreferableWidth);
    public (char Left, char Center, char Right, int Width) DeconstructSeparator()
        => (SectionTRight, HorizontalLine, SectionTLeft, PreferableWidth);
    public (char Left, char Center, char Right, int Width) DeconstructBottom()
        => (CornerBottomLeft, HorizontalLine, CornerBottomRight, PreferableWidth);

    public (char Side, char Center, int Width) DeconstructMiddle()
        => (VerticalLine, EmptyBlock, PreferableWidth);

    public (char Left, char Center, char Center2, char Right, int Width) DeconstructTableTop()
        => (CornerTopLeft, HorizontalLine, SectionTBottom, CornerTopRight, PreferableWidth);
    public (char Left, char Center, char Center2, char Right, int Width) DeconstructTableSeparator()
        => (SectionTRight, HorizontalLine, SectionX, SectionTLeft, PreferableWidth);
    public (char Left, char Center, char Center2, char Right, int Width) DeconstructTableBottom()
        => (CornerBottomLeft, HorizontalLine, SectionTTop, CornerBottomRight, PreferableWidth);
}