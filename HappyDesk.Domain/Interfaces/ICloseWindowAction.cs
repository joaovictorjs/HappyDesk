namespace HappyDesk.Domain.Interfaces;

public interface ICloseWindowAction
{
    Action<bool?> ExecuteClose { get; set; }
}