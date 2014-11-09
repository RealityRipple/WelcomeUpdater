Imports System.IO
NotInheritable Class GrayBMP_File
  Private Sub New()
  End Sub
  Shared BMP_File_Header As Byte() = New Byte(13) {}
  Shared DIB_header As Byte() = New Byte(39) {}
  Shared Color_palette As Byte() = New Byte(1023) {}
  Shared Bitmap_Data As Byte() = Nothing
  Private Shared Function create_palette() As Byte()
    Dim color_palette As Byte() = New Byte(1023) {}
    For i As Integer = 0 To 255
      color_palette(i * 4 + 0) = CByte(i)
      color_palette(i * 4 + 1) = CByte(i)
      color_palette(i * 4 + 2) = CByte(i)
      color_palette(i * 4 + 3) = CByte(0)
    Next
    Return color_palette
  End Function
  Private Shared Sub create_parts(img As Image)
    Bitmap_Data = ConvertToGrayscale(img)
    Copy_to_Index(BMP_File_Header, New Byte() {CByte(AscW("B"c)), CByte(AscW("M"c))}, 0)
    Copy_to_Index(BMP_File_Header, BitConverter.GetBytes(BMP_File_Header.Length + DIB_header.Length + Color_palette.Length + Bitmap_Data.Length), 2)
    Copy_to_Index(BMP_File_Header, New Byte() {CByte(AscW("M"c)), CByte(AscW("C"c)), CByte(AscW("A"c)), CByte(AscW("T"c))}, 6)
    Copy_to_Index(BMP_File_Header, BitConverter.GetBytes(BMP_File_Header.Length + DIB_header.Length + Color_palette.Length), 10)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(DIB_header.Length), 0)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(DirectCast(img, Bitmap).Width), 4)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(DirectCast(img, Bitmap).Height), 8)
    Copy_to_Index(DIB_header, New Byte() {CByte(1), CByte(0)}, 12)
    Copy_to_Index(DIB_header, New Byte() {CByte(8), CByte(0)}, 14)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(0), 16)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(Bitmap_Data.Length), 20)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(2835), 24)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(2835), 28)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(256), 32)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(0), 36)
    Color_palette = create_palette()
  End Sub
  Private Shared Function ConvertToGrayscale(inImg As Image) As Byte()
    Dim inBMP As Bitmap = DirectCast(inImg, Bitmap)
    Dim padding As Integer = If((inBMP.Width Mod 4) <> 0, 4 - (inBMP.Width Mod 4), 0)
    Dim bytes As Byte() = New Byte(inBMP.Width * inBMP.Height + (padding * inBMP.Height - 1)) {}
    For y As Integer = 0 To inBMP.Height - 1
      For x As Integer = 0 To inBMP.Width - 1
        Dim c As Color = inBMP.GetPixel(x, y)
        Dim g As Integer = Convert.ToInt32(0.3 * c.R + 0.59 * c.G + 0.11 * c.B)
        bytes((inBMP.Height - 1 - y) * inBMP.Width + (inBMP.Height - 1 - y) * padding + x) = CByte(g)
      Next
      For i As Integer = 0 To padding - 1
        bytes((inBMP.Height - y) * inBMP.Width + (inBMP.Height - 1 - y) * padding + i) = CByte(0)
      Next
    Next
    Return bytes
  End Function
  Public Shared Function CreateGrayBitmapFile(Image As Image, Path As String) As Boolean
    Try
      create_parts(Image)
      Dim oFileStream As FileStream
      oFileStream = New FileStream(Path, FileMode.OpenOrCreate)
      oFileStream.Write(BMP_File_Header, 0, BMP_File_Header.Length)
      oFileStream.Write(DIB_header, 0, DIB_header.Length)
      oFileStream.Write(Color_palette, 0, Color_palette.Length)
      oFileStream.Write(Bitmap_Data, 0, Bitmap_Data.Length)
      oFileStream.Close()
      Return True
    Catch
      Return False
    End Try
  End Function
  Private Shared Function Copy_to_Index(destination As Byte(), source As Byte(), index As Integer) As Boolean
    Try
      For i As Integer = 0 To source.Length - 1
        destination(i + index) = source(i)
      Next
      Return True
    Catch
      Return False
    End Try
  End Function
End Class
