﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MigForwardingLibrary
{
    class MaywoodsHttpClient
    {
        static readonly HttpClient client = new HttpClient();

        public MaywoodsHttpClient()
        {

        }

        public async Task SendToMaywoods(string xmlMessage)
        { 
            try
            {
                var httpContent = new StringContent(xmlMessage, Encoding.UTF8, "application/xml");
               // var httpContent = new StringContent(xmlMessage, Encoding.UTF8, "text/xml");
                HttpResponseMessage response = await client.PostAsync("MaywoodsUrl", httpContent);
                if(response.IsSuccessStatusCode)
                {
                    String responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                }
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException caught");
                Console.WriteLine("Message: {0} ", e.Message);
            }
        }
    }
}