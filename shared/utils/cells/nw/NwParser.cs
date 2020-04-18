/**
 * 
 * Receives an opened nw file (parseCell) and returns tile positions in pics1_dyl.png array
 * 
 * TODO: Links, Signs, NPCs, Chests and Baddies
 * 
 *  */ 
using System;
using System.Collections.Generic;



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
      string[,] cellData = NwParser.createCellData(cellFile);
      List<CellTile> cellTilesList = NwParser.createTiles(cellData);
      return cellTilesList;
    }
  
    private static List<CellTile> createTiles (string[,] cellData) {
      List<CellTile> cellTilesList = new List<CellTile>();
      // Cells are 64 x 64 tiles in size.
      for (int y = 0; y < 64; y++) {
        for (int x = 0; x < 64; x++) {
            // let tile = []
            string tileStr = cellData[y,5][x*2].ToString() + cellData[y,5][2*x+1].ToString();
            int[] tile = tileSwitch[tileStr];
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
      string[] cellArray = cellFile.Split("BOARD");
      string[,] cellData = new string[64,5];
  
      for (int i = 1; i < 65; i++) {
        string[] splitCellArray = cellArray[i].Split(' ');
        for(int j = 0; j < splitCellArray.Length; j++){
          cellData[i-1,j] = splitCellArray[j];
        }
      }
  
      return cellData;
    }
  
}

