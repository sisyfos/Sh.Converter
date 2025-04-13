// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Sh.Converter.ConsoleApp.Options;
using Microsoft.Extensions.Configuration;
using Sh.Converter.ConsoleApp.Models;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<SettingsOptions>(builder.Configuration.GetSection(SettingsOptions.Settings));
builder.Services.Configure<RelationOptions>(options =>
{
    options.Relations = builder.Configuration.GetSection(RelationOptions.Relation).Get<List<Relation>>()!;
});

builder.Services.AddHostedService<Sh.Converter.ConsoleApp.Application>();

var host = builder.Build();

await host.StartAsync();
