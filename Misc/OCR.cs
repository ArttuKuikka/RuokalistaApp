using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

namespace RuokalistaApp.Misc
{
    public class OCR
    {
       public async Task PerformOCR()
        {




            if(!File.Exists(Path.Combine(FileSystem.CacheDirectory, "ruokalista.jpg")))
            {
                throw new Exception("Tiedosto ei ole olemassa");
            }
            byte[] fileContent = File.ReadAllBytes(Path.Combine(FileSystem.CacheDirectory, "ruokalista.jpg"));
            //string base64String = Convert.ToBase64String(fileContent);

            //if(base64String == null || base64String.Length <= 10)
            //{
            //    throw new Exception("Virheellinen Base64");
            //}

            MemoryStream ms = new MemoryStream();
            using (Image image = Image.Load(fileContent))
            {
                int width = image.Width / 2;
                int height = image.Height / 2;
                image.Mutate(x => x.Resize(width, height));

                image.SaveAsJpeg(ms);
            }
            ms.Position = 0;    
            string base64String = Convert.ToBase64String(ms.ToArray());

            using (var client = new HttpClient())
            {
                var url = "http://api.ocr.space/parse/image";

                client.DefaultRequestHeaders.Add("apikey", "K89075064488957");

                var body = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("language", "fin"),
            new KeyValuePair<string, string>("isTable", "true"),
            new KeyValuePair<string, string>("base64Image", "data:image/jpeg;base64," + base64String)
        };

                var response = await client.PostAsync(url, new FormUrlEncodedContent(body));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    File.WriteAllText(Path.Combine(FileSystem.CacheDirectory, "base64.txt"), content);
                }
                else
                {
                    throw new Exception("httpclient error " + response.ReasonPhrase);
                }
            }
            

            

           
        }
    }

    
}
