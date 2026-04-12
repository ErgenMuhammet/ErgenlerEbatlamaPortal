using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Application.DTOs;
using Application.Interface;
namespace Infrastructure.Services
{
    public class FileReaderService : IFileReader
    {
        protected string MyPath => @"C:\\Users\\Muhammet\\Desktop\\Siparişler";
        public async Task<OrdersDto> ReadFile(string path , string OrderName)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"İlgili sipariş bulunamadı.");
            }

            var List = new List<SawnPieceForOrders>();
            var datFiles = Directory.GetFiles(path, "*.dat", SearchOption.TopDirectoryOnly); //yalnızca dat dosyalarını klasor içerisinde arar derine inmez

            foreach (var file in datFiles)
            {
                var lines = await File.ReadAllLinesAsync(file, Encoding.GetEncoding(1254)); // split ile bölebilmek için karakteri algılayabilmesi için türkçeye göre kodladık
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var columns = line.Split('¶');

                    if (columns.Length >= 8 )
                    {
                        var Measurements = new SawnPieceForOrders();

                        Measurements.Height = float.TryParse(columns[5], out float height) ? height /10f : 0f;
                        Measurements.Width = float.TryParse(columns[6], out float width) ? width /10f : 0f;
                        Measurements.Count = int.TryParse(columns[7], out int count) ? count : 0;
                        List.Add(Measurements);
                    }
                    else
                    {
                       Console.WriteLine($"Hata: Sütun sayısı {columns.Length}. Satır içeriği: {line}");
                    }

                }
            }
            return new OrdersDto
            {
                SawnPiece = List,
                CustomerName = OrderName
            };         
        }

        public async Task<List<FileDetailDto>> ReadFile()
        {
            
            if (!Directory.Exists(MyPath))
            {
                throw new DirectoryNotFoundException($"İlgili klasor {MyPath} yolunda değil lütfen klasor yolunu kontrol ediniz.");
            }
            var FilePath = Directory.GetFileSystemEntries(MyPath).ToList();

            List<FileDetailDto> FileDetail = FilePath.Select(x => new FileDetailDto
            {
                FileName = Path.GetFileName(x),
                FilePath = Path.GetFullPath(x)
            }).ToList();

            return FileDetail;

        }

       
    }
}

