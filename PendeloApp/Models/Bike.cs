namespace PendeloApp.Models
{
    public class Bike
    {
        public int Id { get; set; }
        public string Marke { get; set; }
        public string Modell { get; set; }
        public string Farbe { get; set; }
        public string Gewicht { get; set; }
        public string Gang { get; set; }
        public string Beschreibung { get; set; }
        public double Preis { get; set; }

        //User ID for the user who created the bike
        public string UserId { get; set; }
    }
}
