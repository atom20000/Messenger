using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/Home", async context => { await context.Response.WriteAsync("<h1>Tipo Dom, eto her ne poddergivaet russkie</h1>"); });
            });
            lifetime.ApplicationStopped.Register( () => {
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), Program.config["User_directory"], "nickname.json"), JsonConvert.SerializeObject(Program.NickName, Formatting.Indented));
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), Program.config["User_directory"], "loginid.json"), JsonConvert.SerializeObject(Program.LoginID, Formatting.Indented));
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], "chatsid.json"), JsonConvert.SerializeObject(Program.ChatsID, Formatting.Indented));
            });
            lifetime.ApplicationStopping.Register(() => {
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), Program.config["User_directory"], "nickname.json"), JsonConvert.SerializeObject(Program.NickName, Formatting.Indented));
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), Program.config["User_directory"], "loginid.json"), JsonConvert.SerializeObject(Program.LoginID, Formatting.Indented));
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], "chatsid.json"), JsonConvert.SerializeObject(Program.ChatsID, Formatting.Indented));
            });
            app.Run(async (context) => {
                await context.Response.WriteAsync("<h1>What is fuck? What are you doing?</h1>");
            });
        }
    }
}
