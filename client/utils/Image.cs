using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace LegendOfWorlds.Image {

    public class Image{

        public int width;
        public int height;
        public int bitDepth;
        public int colorType;
        public int compressionMethod;
        public int filterMethod;
        public int interlaceMethod;
        public byte[] palette;
        public byte[] byteImage;
        private byte[] completeByteImage;
        public byte[] transparency;
        public string fileType;

        //* Signatures
        private byte[] pngSignature = {137, 80, 78, 71, 13, 10, 26, 10}; 

        //*

        public Image(byte[] imgByteArray, string type){
            this.fileType = type.ToLower();

            switch (fileType)
            {
                case "png":
                    if(CheckSignature(imgByteArray))
                        LoadPng(imgByteArray);
                    break;
                default:
                    Console.WriteLine("Unsupported file format");
                    break;
            }
            

        }

        private void LoadPng(byte[] byteArray){
            
            //this.width = (byteArray[16] * 256 * 256 * 256) + (byteArray[17] * 256 * 256)  + byteArray[18] * 256 + byteArray[19];
            this.width =  Read4Bytes(byteArray, 16);
            //this.height = (byteArray[20] * 256 * 256 * 256) + (byteArray[21] * 256 * 256)  + byteArray[22] * 256 + byteArray[23];
            this.height = Read4Bytes(byteArray, 20);

            this.bitDepth = byteArray[23];
            this.colorType = byteArray[24];
            if(byteArray[25] == 0){
                this.compressionMethod = byteArray[25];
            } else {
                //Throw error
            }
            this.filterMethod = byteArray[26];
            this.interlaceMethod = byteArray[27];

            int currentAddress = 33;

            Console.WriteLine(byteArray.Length);

            while(currentAddress < byteArray.Length){
                int length = Read4Bytes(byteArray, currentAddress);
                Console.WriteLine(length);
                string chunkType = GetChunkType(byteArray, currentAddress + 4);
                Console.WriteLine(chunkType);
                if(chunkType == "invalidChunk"){
                    Console.WriteLine("Invalid chunk type");
                    return;
                } else if(chunkType != "notCrit"){
                    byte[] chunkData = ReadChunk(byteArray, currentAddress + 8, length);
                    if (chunkType == "PLTE"){
                        if(length % 3 != 0){
                            Console.WriteLine("RETURNSSFASFAS");
                            //ERROR
                            return;
                        }
                        palette = chunkData;
                    } else if (chunkType == "IDAT"){
                        //Does the data need to be interpreted with the palette in mind? Probably
                        if(completeByteImage.Length > 0){
                            // If there are more than one IDAT
                            var temp = new byte[completeByteImage.Length + chunkData.Length];
                            completeByteImage.CopyTo(temp,0);
                            chunkData.CopyTo(temp, completeByteImage.Length);
                            completeByteImage = new byte[temp.Length];
                            completeByteImage = temp;
                        } else {
                            completeByteImage = chunkData;
                        }
                    } else if (chunkType == "tRNS"){
                        transparency = chunkData;
                    } else if (chunkType == "IEND"){
                        //break;
                    }
                }
                currentAddress += length + 12;
                Console.WriteLine(currentAddress);
            }

            Console.WriteLine("ADSKFJADSKJADSGKHADFGJDSFHGSDGSD");
            //uncompress
            byteImage = Decompress(completeByteImage);

            Console.WriteLine(byteImage);

            //unfilter

            //

            //if(chunkType == )


            Console.WriteLine(width.ToString());
            Console.WriteLine(height.ToString());
        }



        private bool CheckSignature(byte[] byteArray){
            switch (fileType){
                case "png":
                    int signatureLength = 8;
                    for(var i = 0; i < signatureLength; i++){
                        if(byteArray[i] != pngSignature[i]){
                            Console.WriteLine("Invalid png signature");
                            return false;
                        }
                    }
                    return true;
                default:
                    return false;
            }
        }

        private int Read4Bytes(byte[] byteArray, int start){
            int value = 0;
            for(var i = start; i < start + 4; i++){
                value = value << 8;
                Console.WriteLine(value);
                value += byteArray[i];
            }

            return value;
        }

        private string GetChunkType(byte[] byteArray, int origin){
            string chunkName = "";
            for(var i = origin; i < origin + 4; i ++){
                if(!((byteArray[i] >= 65 && byteArray[i] <= 90) || (byteArray[i] >= 97 && byteArray[i] <= 122))){
                    return "invalidChunk";
                }

                chunkName += Convert.ToChar(byteArray[i]).ToString();

            }

            if((Char.IsUpper(chunkName[0]) || chunkName[0] == 't') && Char.IsUpper(chunkName[2])){
                return chunkName;
            } else {
                return "notCrit";
            }

        }

        private byte[] ReadChunk(byte[] byteArray, int start, int length){
            byte[] chunkData = new byte[length];
            for(var i = start; i < start + length; i++){
                chunkData[i-start] = byteArray[i];
            }
            return chunkData;
        }

        public static byte[] Decompress(byte[] data)
        {
            var outputStream = new MemoryStream();
            using (var compressedStream = new MemoryStream(data))
            using (var inputStream = new InflaterInputStream(compressedStream))
            {
                inputStream.CopyTo(outputStream);
                outputStream.Position = 0;

                return outputStream.ToArray();
            }


        }
    }

}
