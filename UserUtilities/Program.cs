using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using tfemshoes.Domain.Context;
using tfemshoes.Domain.Service;
using Microsoft.Extensions.Logging;

using IHost host = Host.CreateDefaultBuilder(args).Build();
var configBuilder = new ConfigurationBuilder();
configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
configBuilder.AddUserSecrets<Program>();
var config = configBuilder.Build();

var connection = "Server={0};Database={1};User Id={2};Password={3};";
connection = String.Format(connection,
    config["DatabaseServerName"],
    config["DatabaseName"],
    config["DatabaseUserId"],
    config["DatabasePassword"]);

DbContextOptionsBuilder<TFemShoesContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<TFemShoesContext>();
dbContextOptionsBuilder.UseSqlServer(connection, x => x.UseNetTopologySuite());
TFemShoesContext context = new TFemShoesContext(dbContextOptionsBuilder.Options);

var dummy = LoggerFactory.Create((ILoggingBuilder builder) =>
{

}).CreateLogger<UserService>();
IUserService userService = new UserService(context, dummy);
userService.Register("username", "<>");
