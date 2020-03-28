/**
 * 
 * Receives an opened nw file (parseCell) and returns tile positions in pics1_dyl.png array
 * 
 * TODO: Links, Signs, NPCs, Chests and Baddies
 * 
 *  */ 



public partial class NwParser {

    public struct CellTile
    {
        public int x;
        public int y;
        public int frameX;
        public int frameY;
        public int id;
    }
    
    public static List<CellTile> parseCell (string cellFile) {
      const string[,] cellData = NwParser.createCellData(cellFile);
      const List<CellTile> cellTilesList = NwParser.createTiles(cellData);
      return cellTilesList;
    }
  
    private static List<CellTile> createTiles (string[,] cellData) {
      const List<CellTile> cellTilesList = new List<CellTile>();
      // Cells are 64 x 64 tiles in size.
      for (int y = 0; y < 64; y++) {
        for (int x = 0; x < 64; x++) {
            // let tile = []
            const string tileStr = cellData[y][5][x*2] + cellData[y][5][2*x+1];
            const int[] tile = tileSwitch[tileStr];
            // console.log(tile)
            cellTilesList.Add(
                new CellTile{
                    x = x,
                    y = y,
                    frameX = tile[0],
                    frameY = tile[1],
                    id = tile[2]
                }
            );
        }
      }
      return cellTilesList;
    }
  
    private static string[,] createCellData (string cellFile) {
      const string[] cellArray = cellFile.Split("BOARD");
      string[,] cellData = new string[,];
  
      for (int i = 1; i < 65; i++) {
        cellData[i-1] = cellArray[i].Split(' ');
      }
  
      return cellData;
    }
  
}

