using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace claemoconsoletest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("文字を入力してください。");

            for (; ; )
            {
                string talk = Console.ReadLine();
                if (talk == "end") break;

                string json = Task.Run(() => HttpPost(talk)).Result;

                string result = JsonConvert.DeserializeObject<string>(json);
                string[] splres = result.Split(';');

                foreach (string msg in splres)
                {
                    string[] reslist = msg.Split(',');

                    

                    if (reslist.Length == 3)
                    {
                        Console.WriteLine(reslist[0] + "." + reslist[1] + "." + reslist[2]);
                    }
                }

                string[] analyze = result.Split(',', ';');

                int e1 = 0, e2 = 0, e3 = 0,
                    e5 = 0, e6 = 0, e8 = 0, e10 = 0;
                int peak = 0;

                for (int i=1; i < analyze.Length; i=i+3)
                {
                    if (analyze[i] != "0")
                    {
                        switch (int.Parse(analyze[i]))
                        {
                            case 1:
                                e1++;
                                break;
                            case 2:
                                e2++;
                                break;
                            case 3:
                                e3++;
                                break;
                            case 5:
                                e5++;
                                break;
                            case 6:
                                e6++;
                                break;
                            case 8:
                                e8++;
                                break;
                            case 10:
                                e10++;
                                break;
                        }
                        peak += int.Parse(analyze[i + 1]);
                    }
                }
                int emo = Maxindex(e1, e2, e3, e5, e6, e8, e10);

                Console.WriteLine("EMO:" + emo.ToString() + 
                    " PEAK:" + peak.ToString());
            }


        }


        public static async Task<string> HttpPost(string talk)
        {
            string url = "https://liplis.mine.nu/Clalis/v41/Json/ClalisEmotional.aspx";

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"sentence",talk }
            });

            var hc = new HttpClient();

            var response = await hc.PostAsync(url, content);

            return await response.Content.ReadAsStringAsync();
        }

        public static int Maxindex(params int[] nums)
        {
            // 引数が渡されない場合
            if (nums.Length == 0) return 0;

            int count = 0;
            int max = nums[0];
            for (int i = 0; i < nums.Length; i++)
            {
                if (max < nums[i])
                {
                    count++;
                    max = nums[i];
                }
            }
            return count;
        }
    }
}
