using Microsoft.EntityFrameworkCore;
using Antigolpe.Data;
using System.Windows.Forms;

namespace Antigolpe.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        using (var db = new AppDbContext())
            db.Database.Migrate();

        Application.Run(new MainForm());
    }
}
