using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Data
{
    [Key] public long BatchId { get; set; }

    public int Week { get; set; }
    public DateTime Date { get; set; }
    public DateTime DateTime { get; set; }
    public string MachineId { get; set; }
    public int Centimeters { get; set; }
    public int Quantity { get; set; }
    public int Meters { get; set; }
    public string Reason { get; set; }
}