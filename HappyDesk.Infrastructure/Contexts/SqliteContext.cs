using Microsoft.EntityFrameworkCore;

namespace HappyDesk.Infrastructure.Contexts;

public class SqliteContext(DbContextOptions<SqliteContext> options) :DbContext(options);