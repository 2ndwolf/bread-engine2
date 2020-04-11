namespace Shared.Cells.Tiled {
    public struct Layer {
      public int[] data;
      public int height;
      public int id;
      public string name;
      public int opacity;
      public string tilelayer;
      public bool visible;
      public int width;
      public int x;
      public int y;
    } 

    public struct Tileset {
      public int columns;
      public int firstgid;
      public string image;
      public int imageheight;
      public int imagewidth;
      public int margin;
      public string name;
      public int spacing;
      public int tilecount;
      public int tileheight;
      public int tilewidth;
    }

    public struct Level {
      public int compressionlevel;
      public int height;
      public bool infinite;
      public int nextlayerid;
      public string orientation;
      public string renderorder;
      public string tiledversion;
      public int tileheight;
      public int tilewidth;
      public string type;
      public float version;
      public int width;
      public Layer[] layers;
    }
}