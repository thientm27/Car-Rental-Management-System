// See https://aka.ms/new-console-template for more information

using CarRenting.Repositories.Repo;
using DotNetEnv;

DotNetEnv.Env.TraversePath().Load();

Console.WriteLine("Hello, World!");
CustomerRepo customerRepo = new CustomerRepo();

var result = await customerRepo.LoginAsync("EmilyJohnson@FUCarRenting.org", "@1");
Console.WriteLine(result.Email);
Console.WriteLine(result.Telephone);
Console.WriteLine(result.CustomerBirthday);