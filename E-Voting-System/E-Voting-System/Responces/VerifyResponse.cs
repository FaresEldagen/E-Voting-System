namespace E_Voting_System.Responces
{
    public class VerifyResponse
    {
        public bool success { get; set; }
        public bool is_same_person { get; set; }
        public string id_number_arabic { get; set; }
        public string id_number_english { get; set; }
        public double similarity { get; set; }
        public double threshold { get; set; }
        public string message { get; set; }
    }
}
