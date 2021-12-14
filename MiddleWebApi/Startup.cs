using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MiddleWebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ReverseProxy>();

            app.Run(async (context) =>
            {
                


                
                string page = "<strong>tables/{table}/{startDate}/{endDate}/</strong> - <a href='tables/a'>tables/A</a><br>";
                page+= "<strong>rates/{table}/{currency}</strong> - <a href='rates/A/EUR'>rates/A/EUR</a><br>";
                page+= "<strong>tables-middle-A/{startDate}/{endDate}/</strong> - <a href='tables-middle-A'>tables-middle-A</a><br>";
                page+= "<strong>tables-middle-B/{startDate}/{endDate}/</strong> - <a href='tables-middle-B'>tables-middle-B</a><br>";
                page+= "<strong>tables-buy-sell/{startDate}/{endDate}</strong> - <a href='tables-buy-sell'>tables-buy-sell</a><br>";
                page+= "<strong>rates-middle-A/{code}/{startDate}/{endDate}/</strong> - <a href='rates-middle-A/EUR'>rates-middle-A/EUR</a><br>";
                page+= "<strong>rates-middle-B/{code}/{startDate}/{endDate}/</strong> - <a href='rates-middle-B/EUR'>rates-middle-B/EUR</a><br>";
                page+= "<strong>rates-buy-sell/{code}/{startDate}/{endDate}/</strong> - <a href='rates-buy-sell/EUR'>rates-buy-sell/EUR</a><br>";
                
                await context.Response.WriteAsync(page);
            });
        }
    }
}
