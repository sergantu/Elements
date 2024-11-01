using System;

namespace CodeBase.GameLevel
{
    [Serializable]
    public class BoardCell
    {
        
        public BoardCellType CellType { get; set; }
        public ElementBlock ElementBlock { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public BoardCell(string id, int x, int y, BoardCellType cellType, ElementBlock elementBlock)
        {
            CellType = cellType;
            ElementBlock = elementBlock;
            X = x;
            Y = y;

            if (ElementBlock != null) {
                ElementBlock.Id = id;
            }

        }

        public bool IsEmpty()
        {
            return CellType == BoardCellType.Empty;
        }
    }
}