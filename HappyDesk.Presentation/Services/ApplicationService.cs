using System.IO;
using HappyDesk.Domain.Interfaces;
using Application = System.Windows.Application;

namespace HappyDesk.Presentation.Services;

public class ApplicationService:IApplicationService
{
    public object GetIcon(string path)
    {
        return new Icon(Application.GetResourceStream(new Uri(path))!.Stream);
    }
}