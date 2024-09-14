namespace Coding_Tracker_JVR_Hannes
{
    public class CodingSession
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Duration { get; set; }

        public void CalculateDuration()
        {
            Duration = (EndTime - StartTime).TotalHours;
        }
    }
}
