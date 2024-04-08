using BuyBikeShop.Data;
using BuyBikeShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace BuyBikeShop
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BuyBikeShopContext(serviceProvider.GetRequiredService<DbContextOptions<BuyBikeShopContext>>()))
            {
                //checks if there are all the tables in the database in the latest version, if no it updates the database
                context.Database.Migrate();

                
                if (!context.Products.Any())
                {
                    // Seed Products table
                    context.Products.AddRange(
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "MTB",
                            Title = "CrossCPro Cream",
                            Description = "Cross-country has never been so much fun! Thanks to a completely new geo concept and the latest parts, you are PRETTY DAMN QUICK – on every trail, in every competition and training. Creme Color,Weight 8.4kg.",
                            Price = 8999,
                            Quantity = 30,
                            Sale_Perc = 0,
                            Photo = "/assets/Bikes_MTB_CrossCPro_Creme.png",
                            Color = "Creme",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 3.9
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "MTB",
                            Title = "CrossCPro Black",
                            Description = "Cross-country has never been so much fun! Thanks to a completely new geo concept and the latest parts, you are PRETTY DAMN QUICK – on every trail, in every competition and training. Black Color, Weight 8.4kg.",
                            Price = 8999,
                            Quantity = 30,
                            Sale_Perc = 0,
                            Photo = "/assets/Bikes_MTB_CrossCPro_Black.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2023-10-15"),
                            Age_Limit = 16,
                            Rating = 4.2
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "MTB",
                            Title = "DownHill Silver",
                            Description = "Take on any challenge with the DownHill. Whether you ride: With plenty of travel and a playful mullet concept, you can really push your limits. Silver Color, Weight 9.4kg.",
                            Price = 9999,
                            Quantity = 30,
                            Sale_Perc = 20,
                            Photo = "/assets/Bikes_MTB_DownHill_Silver.png",
                            Color = "Silver",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 4.9
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "MTB",
                            Title = "DownHill Clay",
                            Description = "Take on any challenge with the DownHill. Whether you ride: With plenty of travel and a playful mullet concept, you can really push your limits. Clay Color, Weight 9.4kg.",
                            Price = 9999,
                            Quantity = 30,
                            Sale_Perc = 0,
                            Photo = "/assets/Bikes_MTB_DownHill_Clay.png",
                            Color = "Clay",
                            ReleaseDate = DateTime.Parse("2023-07-28"),
                            Age_Limit = 16,
                            Rating = 4.2
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "MTB",
                            Title = "TrailRock Green",
                            Description = "Whether you are a newcomer to singletrack or an experienced biker who is hungry for new challenges and wants to fuel their adventurous spirit, the Trail Rock offers endless possibilities for your MTB experience. Color Green, Weight 10.3kg",
                            Price = 7999,
                            Quantity = 30,
                            Sale_Perc = 25,
                            Photo = "/assets/Bikes_MTB_TrailRock_Green.png",
                            Color = "Green",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 4.3
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "MTB",
                            Title = "TrailRoot Creme",
                            Description = "The Trail Root is made for fast trails in technical terrain. Its aggressive geometry and powerful chassis make it fast, agile and forgiving. Color Creme, Weight 9.2kg",
                            Price = 7499,
                            Quantity = 30,
                            Sale_Perc = 0,
                            Photo = "/assets/Bikes_MTB_TrailRoot_Creme.png",
                            Color = "Creme",
                            ReleaseDate = DateTime.Parse("2023-11-15"),
                            Age_Limit = 16,
                            Rating = 4.1
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "Road",
                            Title = "RaceXLite Black",
                            Description = "Our fastest road bike ever, so you can be even faster. Whether alone or in a group, in a sprint or on the mountain: The RaceXLITE stands out in every situation. Black Color, Weight 6.9kg",
                            Price = 8999,
                            Quantity = 30,
                            Sale_Perc = 0,
                            Photo = "/assets/Bikes_Road_RaceXLite_Black.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 4.5
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "Road",
                            Title = "RaceXLite Grey",
                            Description = "Our fastest road bike ever, so you can be even faster. Whether alone or in a group, in a sprint or on the mountain: The RaceXLITE stands out in every situation. Grey Color, Weight 6.9kg",
                            Price = 8999,
                            Quantity = 30,
                            Sale_Perc = 20,
                            Photo = "/assets/Bikes_Road_RaceXLite_Grey.png",
                            Color = "Grey",
                            ReleaseDate = DateTime.Parse("2023-08-27"),
                            Age_Limit = 16,
                            Rating = 4.1
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "Road",
                            Title = "RaceXLite Yellow",
                            Description = "Our fastest road bike ever, so you can be even faster. Whether alone or in a group, in a sprint or on the mountain: The RaceXLITE stands out in every situation. Yellow Color, Weight 6.9kg",
                            Price = 9999,
                            Quantity = 30,
                            Sale_Perc = 0,
                            Photo = "/assets/Bikes_Road_RaceXLite_Yellow.png",
                            Color = "Yellow",
                            ReleaseDate = DateTime.Parse("2023-09-27"),
                            Age_Limit = 16,
                            Rating = 4.2
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "Road",
                            Title = "Reveal Black",
                            Description = "Endless fun on long distances. The REVEAL embodies uncompromising design for your longest days in the saddle. Powerful and comfortable at the same time. Black Color, Weight 7.8kg",
                            Price = 7999,
                            Quantity = 30,
                            Sale_Perc = 25,
                            Photo = "/assets/Bikes_Road_Reveal_Black.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2024-01-20"),
                            Age_Limit = 16,
                            Rating = 5
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "Road",
                            Title = "Reveal Orange",
                            Description = "Endless fun on long distances. The REVEAL embodies uncompromising design for your longest days in the saddle. Powerful and comfortable at the same time. Orange Color, Weight 7.8kg",
                            Price = 7999,
                            Quantity = 30,
                            Sale_Perc = 0,
                            Photo = "/assets/Bikes_Road_Reveal_Orange.png",
                            Color = "Orange",
                            ReleaseDate = DateTime.Parse("2023-08-22"),
                            Age_Limit = 16,
                            Rating = 3.9
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "Road",
                            Title = "Reveal White",
                            Description = "Endless fun on long distances. The REVEAL embodies uncompromising design for your longest days in the saddle. Powerful and comfortable at the same time. White Color, Weight 7.8kg",
                            Price = 7999,
                            Quantity = 30,
                            Sale_Perc = 0,
                            Photo = "/assets/Bikes_Road_Reveal_White.png",
                            Color = "White",
                            ReleaseDate = DateTime.Parse("2024-03-22"),
                            Age_Limit = 16,
                            Rating = 4.6
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "Urban",
                            Title = "Sneak Mint",
                            Description = "The SNEAK is made for your city life. Ride on the roads you know – to the places you love. Make a statement for new mobility. Mint Color, Weight 9.7kg",
                            Price = 8999,
                            Quantity = 30,
                            Sale_Perc = 20,
                            Photo = "/assets/Bikes_Urban_Sneak_Mint.png",
                            Color = "Mint",
                            ReleaseDate = DateTime.Parse("2023-01-22"),
                            Age_Limit = 16,
                            Rating = 4.6
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "Urban",
                            Title = "Sneak LightBlue",
                            Description = "The SNEAK is made for your city life. Ride on the roads you know – to the places you love. Make a statement for new mobility. LightBlue Color, Weight 9.7kg",
                            Price = 8999,
                            Quantity = 30,
                            Sale_Perc = 0,
                            Photo = "/assets/Bikes_Urban_Sneak_LightBlue.png",
                            Color = "LightBlue",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 4
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "Urban",
                            Title = "Lava Black",
                            Description = "Whether it’s the route to the bakery, commuting to work, or a tour on the weekend – the LAVA is our hot tip for the things that matter most in life. Black Color, Weight 10.5kg",
                            Price = 9999,
                            Quantity = 30,
                            Sale_Perc = 25,
                            Photo = "/assets/Bikes_Urban_Lava_Black.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2023-12-12"),
                            Age_Limit = 16,
                            Rating = 4.8
                        },
                        new Product
                        {
                            Class_Name = "Bike",
                            Sub_Class = "Urban",
                            Title = "Lava Turquoise",
                            Description = "Whether it’s the route to the bakery, commuting to work, or a tour on the weekend – the LAVA is our hot tip for the things that matter most in life. Turquoise Color, Weight 10.5kg",
                            Price = 9999,
                            Quantity = 30,
                            Sale_Perc = 0,
                            Photo = "/assets/Bikes_Urban_Lava_Turquoise.png",
                            Color = "Turquoise",
                            ReleaseDate = DateTime.Parse("2024-04-01"),
                            Age_Limit = 16,
                            Rating = 4.5
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Helmet",
                            Title = "Abus Black",
                            Description = "Urban unisex bicycle helmet for young and old, Built-in LED light at the back, Black.",
                            Price = 279,
                            Quantity = 200,
                            Sale_Perc = 0,
                            Photo = "/assets/Accessories_Helmet_Abus_Black.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2023-02-22"),
                            Age_Limit = 16,
                            Rating = 4.2
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Helmet",
                            Title = "Abus Yellow",
                            Description = "Urban unisex bicycle helmet for young and old, Built-in LED light at the back, Yellow.",
                            Price = 279,
                            Quantity = 200,
                            Sale_Perc = 25,
                            Photo = "/assets/Accessories_Helmet_Abus_Yellow.png",
                            Color = "Yellow",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 4.8
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Helmet",
                            Title = "Alpina DarkGrey",
                            Description = "Unisex helmet for MTB, Urban and Trekking, Made with INMOULD technology, DarkGrey.",
                            Price = 299,
                            Quantity = 200,
                            Sale_Perc = 0,
                            Photo = "/assets/Accessories_Helmet_Alpina_DarkGrey.png",
                            Color = "DarkGrey",
                            ReleaseDate = DateTime.Parse("2023-11-30"),
                            Age_Limit = 16,
                            Rating = 3.7
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Helmet",
                            Title = "Alpina Yellow",
                            Description = "Unisex helmet for MTB, Urban and Trekking, Made with INMOULD technology, Yellow.",
                            Price = 299,
                            Quantity = 200,
                            Sale_Perc = 0,
                            Photo = "/assets/Accessories_Helmet_Alpina_Yellow.png",
                            Color = "Yellow",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 4.4
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Helmet",
                            Title = "Quatro Black",
                            Description = "Unisex MTB helmet with robust polycarbonate shell, Made with DOUBLE INMOULD technology, Black.",
                            Price = 349,
                            Quantity = 200,
                            Sale_Perc = 20,
                            Photo = "/assets/Accessories_Helmet_Quatro_Black.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2023-02-24"),
                            Age_Limit = 16,
                            Rating = 4.4
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Helmet",
                            Title = "Quatro White",
                            Description = "Unisex MTB helmet with robust polycarbonate shell, Made with DOUBLE INMOULD technology, White.",
                            Price = 349,
                            Quantity = 200,
                            Sale_Perc = 0,
                            Photo = "/assets/Accessories_Helmet_Quatro_White.png",
                            Color = "White",
                            ReleaseDate = DateTime.Parse("2023-05-21"),
                            Age_Limit = 16,
                            Rating = 4.1
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Helmet",
                            Title = "Uvex Blue",
                            Description = "Versatile, unisex bike helmet, Made with INMOULD technology, Blue.",
                            Price = 239,
                            Quantity = 200,
                            Sale_Perc = 25,
                            Photo = "/assets/Accessories_Helmet_Uvex_Blue.png",
                            Color = "Blue",
                            ReleaseDate = DateTime.Parse("2023-04-24"),
                            Age_Limit = 16,
                            Rating = 4
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Helmet",
                            Title = "Uvex Olive",
                            Description = "Versatile, unisex bike helmet, Made with INMOULD technology, Olive.",
                            Price = 239,
                            Quantity = 200,
                            Sale_Perc = 0,
                            Photo = "/assets/Accessories_Helmet_Uvex_Olive.png",
                            Color = "Olive",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 4.3
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Bottle",
                            Title = "Superloli White",
                            Description = "Large plastic drinks bottle, Volume: 800 ml, White.",
                            Price = 49,
                            Quantity = 350,
                            Sale_Perc = 0,
                            Photo = "/assets/Accessories_Bottle_Superloli_White.png",
                            Color = "White",
                            ReleaseDate = DateTime.Parse("2023-05-23"),
                            Age_Limit = 16,
                            Rating = 4.5
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Bottle",
                            Title = "Superloli Black",
                            Description = "Large plastic drinks bottle, Volume: 800 ml, Black.",
                            Price = 49,
                            Quantity = 350,
                            Sale_Perc = 20,
                            Photo = "/assets/Accessories_Bottle_Superloli_Black.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2023-05-24"),
                            Age_Limit = 16,
                            Rating = 3.7
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Bottle",
                            Title = "Rose White",
                            Description = "ROSE Road & Mountain Drinking Bottle Softtouch, Volume 750 ml, White.",
                            Price = 69,
                            Quantity = 350,
                            Sale_Perc = 0,
                            Photo = "/assets/Accessories_Bottle_Rose_White.png",
                            Color = "White",
                            ReleaseDate = DateTime.Parse("2023-08-27"),
                            Age_Limit = 16,
                            Rating = 4.1
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Bottle",
                            Title = "Rose Black",
                            Description = "ROSE Road & Mountain Drinking Bottle Softtouch, Volume 750 ml, Black.",
                            Price = 69,
                            Quantity = 350,
                            Sale_Perc = 25,
                            Photo = "/assets/Accessories_Bottle_Rose_Black.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 4.1
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Bottle",
                            Title = "Fly20 Yellow",
                            Description = "Very lightweight plastic bottle for cycling and leisure, Soft, wear-resistant bottle, Volume: 750 ml, Yellow.",
                            Price = 59,
                            Quantity = 350,
                            Sale_Perc = 0,
                            Photo = "/assets/Accessories_Bottle_Fly20_Yellow.png",
                            Color = "Yellow",
                            ReleaseDate = DateTime.Parse("2024-01-01"),
                            Age_Limit = 16,
                            Rating = 3.7
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Bottle",
                            Title = "Fly20 Blue",
                            Description = "Very lightweight plastic bottle for cycling and leisure, Soft, wear-resistant bottle, Volume: 750 ml, Blue.",
                            Price = 59,
                            Quantity = 350,
                            Sale_Perc = 0,
                            Photo = "/assets/Accessories_Bottle_Fly20_Blue.png",
                            Color = "Blue",
                            ReleaseDate = DateTime.Parse("2024-02-01"),
                            Age_Limit = 16,
                            Rating = 3.9
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Bag",
                            Title = "Deuter Black",
                            Description = "The BIKE I 20 by deuter is ideal for mountain bikers who are looking for a backpack that is easy to use and has a clever division. This one is equally suitable for riding on trails, in the mountains or for a trip to the ice-cream parlour. Black.",
                            Price = 449,
                            Quantity = 150,
                            Sale_Perc = 20,
                            Photo = "/assets/Accessories_Bags_deuter_Black.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2024-02-25"),
                            Age_Limit = 16,
                            Rating = 4.4
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Bag",
                            Title = "Deuter Orange",
                            Description = "The BIKE I 20 by deuter is ideal for mountain bikers who are looking for a backpack that is easy to use and has a clever division. This one is equally suitable for riding on trails, in the mountains or for a trip to the ice-cream parlour. Orange.",
                            Price = 449,
                            Quantity = 150,
                            Sale_Perc = 0,
                            Photo = "/assets/Accessories_Bags_deuter_Orange.png",
                            Color = "Orange",
                            ReleaseDate = DateTime.Parse("2024-01-11"),
                            Age_Limit = 16,
                            Rating = 4.1
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Bag",
                            Title = "Deuter Turquoise",
                            Description = "The BIKE I 20 by deuter is ideal for mountain bikers who are looking for a backpack that is easy to use and has a clever division. This one is equally suitable for riding on trails, in the mountains or for a trip to the ice-cream parlour. Turquoise.",
                            Price = 479,
                            Quantity = 150,
                            Sale_Perc = 25,
                            Photo = "/assets/Accessories_Bags_deuter_Turquoise.png",
                            Color = "Turquoise",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 3.8
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Bag",
                            Title = "Vaude Beige",
                            Description = "Environmentally conscious, nature backpack for short and longer bike tours into the countryside: With the UPHILL 12 backpack, VAUDE is setting standards in the field of sustainable production and at the same time creating a multifunctional bike backpack for your next day tour. Beige.",
                            Price = 499,
                            Quantity = 150,
                            Sale_Perc = 0,
                            Photo = "/assets/Accessories_Bags_Vaude_Beige.png",
                            Color = "Beige",
                            ReleaseDate = DateTime.Parse("2023-03-27"),
                            Age_Limit = 16,
                            Rating = 3.8
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Bag",
                            Title = "Vaude Black",
                            Description = "Environmentally conscious, nature backpack for short and longer bike tours into the countryside: With the UPHILL 12 backpack, VAUDE is setting standards in the field of sustainable production and at the same time creating a multifunctional bike backpack for your next day tour. Black.",
                            Price = 499,
                            Quantity = 150,
                            Sale_Perc = 0,
                            Photo = "/assets/Accessories_Bags_Vaude_Black.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2023-01-27"),
                            Age_Limit = 16,
                            Rating = 4.6
                        },
                        new Product
                        {
                            Class_Name = "Accessories",
                            Sub_Class = "Bag",
                            Title = "Vaude LightGreen",
                            Description = "Environmentally conscious, nature backpack for short and longer bike tours into the countryside: With the UPHILL 12 backpack, VAUDE is setting standards in the field of sustainable production and at the same time creating a multifunctional bike backpack for your next day tour. LightGreen.",
                            Price = 499,
                            Quantity = 150,
                            Sale_Perc = 20,
                            Photo = "/assets/Accessories_Bags_Vaude_LightGreen.png",
                            Color = "LightGreen",
                            ReleaseDate = DateTime.Parse("2023-09-22"),
                            Age_Limit = 16,
                            Rating = 4.3
                        },
                        new Product
                        {
                            Class_Name = "Parts",
                            Sub_Class = "Light",
                            Title = "Sigma AURA 35",
                            Description = "Sigma AURA 35 USB LED Front Light/NUGGET II Rear Light Set",
                            Price = 99,
                            Quantity = 1050,
                            Sale_Perc = 0,
                            Photo = "/assets/Parts_Light_Sigma_AURA35.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 3.8
                        },
                        new Product
                        {
                            Class_Name = "Parts",
                            Sub_Class = "Light",
                            Title = "Sigma AURA 45",
                            Description = "Sigma AURA 45 USB LED Front Light/NUGGET II Rear Light Set",
                            Price = 129,
                            Quantity = 1050,
                            Sale_Perc = 25,
                            Photo = "/assets/Parts_Light_Sigma_AURA45.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2023-08-27"),
                            Age_Limit = 16,
                            Rating = 4
                        },
                        new Product
                        {
                            Class_Name = "Parts",
                            Sub_Class = "Light",
                            Title = "Sigma AURA 60",
                            Description = "Sigma Aura 60 Front Light/Nugget II Rear Light Set",
                            Price = 149,
                            Quantity = 1050,
                            Sale_Perc = 0,
                            Photo = "/assets/Parts_Light_Sigma_AURA60.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2023-03-31"),
                            Age_Limit = 16,
                            Rating = 4.1
                        },
                        new Product
                        {
                            Class_Name = "Parts",
                            Sub_Class = "Light",
                            Title = "Sigma AURA 80",
                            Description = "Sigma AURA 80 USB LED Front Light/BLAZE USB Battery-Powered Rear Light with Brake Light Kit",
                            Price = 189,
                            Quantity = 1050,
                            Sale_Perc = 20,
                            Photo = "/assets/Parts_Light_Sigma_AURA80.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2023-08-02"),
                            Age_Limit = 16,
                            Rating = 4.5
                        },
                        new Product
                        {
                            Class_Name = "Parts",
                            Sub_Class = "Pedals",
                            Title = "Crank Stamp 7",
                            Description = "The ten pins on the wide contact area of the Stamp 7 pedals by CrankBrothers literally screw into your shoe sole to securely keep you on the pedals on dangerous trail rides across the country. For best grip, they are evenly distributed over the concave platform of the extremely flat aluminium pedal.",
                            Price = 359,
                            Quantity = 550,
                            Sale_Perc = 25,
                            Photo = "/assets/Parts_Pedal_Crank_Stamp7.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 5
                        },
                        new Product
                        {
                            Class_Name = "Parts",
                            Sub_Class = "Pedals",
                            Title = "Rose Grip 259",
                            Description = "For the best grip. Whether fast downhill tours, adventurous freeride tours or relaxed discover tours – the ROSE Tour Grip 259 pedals are as solid as a rock.",
                            Price = 199,
                            Quantity = 550,
                            Sale_Perc = 20,
                            Photo = "/assets/Parts_Pedal_Rose_Grip259.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2024-03-11"),
                            Age_Limit = 16,
                            Rating = 4
                        },
                        new Product
                        {
                            Class_Name = "Parts",
                            Sub_Class = "Pedals",
                            Title = "Rose Grip 79",
                            Description = "The right choice for all those looking for lightweight all-terrain pedals! With a weight of only approx. 250 g per pair, this elegant, CNC milled pedal is a durable touring compantion that offers great grip.",
                            Price = 179,
                            Quantity = 550,
                            Sale_Perc = 0,
                            Photo = "/assets/Parts_Pedal_Rose_Pro79.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2024-01-17"),
                            Age_Limit = 16,
                            Rating = 4.9
                        },
                        new Product
                        {
                            Class_Name = "Parts",
                            Sub_Class = "Pedals",
                            Title = "Rose Grip 219",
                            Description = "Large, flat pedals for all-mountain or ATB use. Shoes with sufficient tread will have good grip on the pedals thanks to 10 steel pins. Holes to retrofit reflectors are also included.",
                            Price = 129,
                            Quantity = 550,
                            Sale_Perc = 25,
                            Photo = "/assets/Parts_Pedal_Rose_Pro219.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 4.9
                        },
                        new Product
                        {
                            Class_Name = "Parts",
                            Sub_Class = "Saddle",
                            Title = "Brooks Classic B17",
                            Description = "It has been over 110 years now since it came on the market - the B17 Standard saddle belongs to the classics from Brooks. This saddle is designed for longer, fast-paced tours.",
                            Price = 489,
                            Quantity = 450,
                            Sale_Perc = 25,
                            Photo = "/assets/Parts_Saddle_Brooks_ClassicB17.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2023-02-02"),
                            Age_Limit = 16,
                            Rating = 5
                        },
                        new Product
                        {
                            Class_Name = "Parts",
                            Sub_Class = "Saddle",
                            Title = "Ergon SR Man",
                            Description = "The SR Comp Men road saddle by Ergon is flat, but still comfortably padded. The saddle’s orthopaedic comfort foam with OrthoCell inlays which is lighter and more durable than gel pads evenly distributes pressure over the contact surface for relaxed pedalling.",
                            Price = 519,
                            Quantity = 450,
                            Sale_Perc = 20,
                            Photo = "/assets/Parts_Saddle_Ergon_SRMan.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2024-02-21"),
                            Age_Limit = 16,
                            Rating = 4.4
                        },
                        new Product
                        {
                            Class_Name = "Parts",
                            Sub_Class = "Saddle",
                            Title = "Terry Anatomica Flex",
                            Description = "Additional gel inserts inside the flexible Anatomica Flex Gel Women saddle in the area of the sit bones promise additional comfort on daily commutes and leisure bike tours. In order to best meet the high comfort demands of city and e-bike riders, the Terry Anatomica saddle has been completely revised.",
                            Price = 459,
                            Quantity = 450,
                            Sale_Perc = 0,
                            Photo = "/assets/Parts_Saddle_Terry_AnatomicaFlex.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2023-08-27"),
                            Age_Limit = 16,
                            Rating = 4.5
                        },
                        new Product
                        {
                            Class_Name = "Parts",
                            Sub_Class = "Saddle",
                            Title = "Terry Fisio Gel Max",
                            Description = "The Fisio Gel Max Men Touring Comfort saddle by Terry comes with a 3 cm wider seat with gel padding and shock-absorbing Cellasto damping system for best riding comfort on longer tours. Your back is relieved thanks to a riding position that is only slightly bent.",
                            Price = 489,
                            Quantity = 450,
                            Sale_Perc = 25,
                            Photo = "/assets/Parts_Saddle_Terry_FisioGelMax.png",
                            Color = "Black",
                            ReleaseDate = DateTime.Parse("2023-05-13"),
                            Age_Limit = 16,
                            Rating = 5
                        }

                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
