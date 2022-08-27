using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace truYumRestaurant
{
    class Program
    {
        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysuperdupersecret"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                        issuer: "mySystem",
                        audience: "myUsers",
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        

        public async void GetMenuList()
        {
            string token = GenerateJSONWebToken();
            List<MenuItem> menus = new List<MenuItem>();
            string apiUrl = "http://20.237.16.241/";
            using (var client = new HttpClient() )
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage httpResponseMessage = await client.GetAsync("api/menuitem");
                if(httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    menus = JsonConvert.DeserializeObject<List<MenuItem>>(json);
                }
            }
            foreach (var item in menus)
            {
                Console.WriteLine($"Item No is {item.Id}, Item Name is {item.Name}and Item Price is {item.Price}");
            }
        }

        public async void OrderItem(int Id)
        {
            string token = GenerateJSONWebToken();
            Cart order = new Cart();
            string apiUrl = "http://20.241.177.87/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string json = JsonConvert.SerializeObject(Id);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await client.PostAsync($"api/order/{Id}", content);
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    string json1 = await httpResponseMessage.Content.ReadAsStringAsync();
                    order = JsonConvert.DeserializeObject<Cart>(json1);
                }
                else if(httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Incorrect menu item id chosen. Please choose the appropriate Id");
                    return;
                }
            }
            
            Console.WriteLine($"Your Cart of Ordered Item Name is {order.MenuItemName}");

        }


        static void Main(string[] args)
        {
            Console.WriteLine("----Welcome To TruYum Restaurant----");
            Program obj = new Program();
            Console.WriteLine("How Can I Serve You");
            while (true)
            {
                string userInput = Console.ReadLine();
                if(userInput == "menu")
                {
                    Console.WriteLine("Our Menu List Item To Order :");
                    Console.WriteLine("-------------------------------");
                    obj.GetMenuList();
                }
                else if(userInput == "order")
                {

                    string itemNumber = Console.ReadLine();        
                    obj.OrderItem(Convert.ToInt32(itemNumber));
                }
                else
                {
                    Console.WriteLine("Please Enter, Either menu or order");
                }
            }
            

            Console.WriteLine(obj.GenerateJSONWebToken());
            Console.ReadKey();
        }
    }
}
